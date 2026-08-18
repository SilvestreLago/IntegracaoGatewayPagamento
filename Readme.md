# Integração com Gateway de Pagamentos

Repositório dedicado ao laboratório de pesquisa sobre falhas na integração com gateways de pagamentos. O projeto implementa uma API em ASP.NET Core que se integra ao gateway [Asaas](https://www.asaas.com/) para cadastro de clientes, produtos e geração de cobranças, servindo como ambiente controlado para estudo de vulnerabilidades comuns nesse tipo de integração — como manipulação de preço no lado do cliente, validação inadequada de webhooks e ausência de idempotência no processamento de eventos — e suas respectivas correções.

## 🎯 Objetivo

Este é um projeto de laboratório/pesquisa. A API expõe endpoints "vulneráveis" e suas versões "corrigidas" (sufixo `Fix`) lado a lado, permitindo comparar na prática:

- O que acontece quando o valor de uma cobrança é definido a partir de dados enviados pelo cliente, em vez de ser recalculado no servidor.
- O que acontece quando um endpoint de webhook não valida a origem da requisição.
- O que acontece quando um endpoint de webhook não é idempotente e pode reprocessar o mesmo evento múltiplas vezes (por exemplo, em reenvios do Asaas).

## 🛠️ Tecnologias

- **C# / .NET 10** (ASP.NET Core Web API)
- **Entity Framework Core** com **PostgreSQL** (Npgsql)
- **Asaas API** — gateway de pagamentos
- **Swagger / Scalar** — documentação e exploração da API
- **Docker**

## 🏗️ Arquitetura

O projeto segue uma separação em camadas:

```
IntegracaoGatewayPagamento/
├── Controllers/     # Endpoints da API (Cliente, Produto, Cobrança)
├── Services/        # Regras de negócio e comunicação com a API do Asaas
├── Repositories/    # Acesso a dados via Entity Framework Core
├── Entities/         # Modelos de domínio (Cliente, Produto, Cobranca, Webhook)
├── DTO/              # Objetos de transferência de dados (entrada/saída)
├── Exceptions/       # Exceções customizadas
└── Migrations/       # Migrations do Entity Framework Core
```

## 📡 Endpoints principais

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/cadastrarCliente` | Cadastra um cliente localmente e no Asaas |
| `POST` | `/api/manipulacaoPrecosQuantidade` | Cadastra um produto (nome e preço) |
| `POST` | `/api/manipulacaoPreco` | Gera uma cobrança a partir do valor enviado na requisição (cenário vulnerável) |
| `POST` | `/api/manipulacaoPrecoFix` | Gera uma cobrança recalculando o valor no servidor a partir do produto e da quantidade (cenário corrigido) |
| `POST` | `/api/webhook` | Recebe notificações de pagamento do Asaas sem validação de origem nem controle de idempotência (cenário vulnerável) |
| `POST` | `/api/webhookFix` | Recebe notificações de pagamento validando o header `asaas-access-token` e garantindo idempotência por evento (cenário corrigido) |

### 🔁 Idempotência do webhook (`webhookFix`)

Gateways de pagamento podem reenviar a mesma notificação de webhook mais de uma vez (timeout, retry, falha de rede etc.). No cenário vulnerável (`/api/webhook`), cada reenvio é processado normalmente, o que pode gerar efeitos colaterais duplicados. No cenário corrigido (`/api/webhookFix`), cada evento recebido do Asaas é controlado por uma tabela `Webhooks`, chaveada pelo `idEventAsaas` (campo `id` do payload) com índice único no banco:

1. Ao receber o evento, é feita uma tentativa de inserir o registro de idempotência com status `PENDENTE`.
2. Se o `idEventAsaas` já existir e ainda estiver `PENDENTE`, a requisição retorna `409 Conflict` — o evento já está sendo processado (proteção contra chamadas concorrentes/duplicadas).
3. Se o `idEventAsaas` já existir com status `CONCLUIDO`, a requisição retorna `200 OK` sem reprocessar — o evento já foi tratado anteriormente.
4. Se a cobrança referenciada não for encontrada, o registro de idempotência recém-criado é removido, permitindo uma nova tentativa futura.
5. Ao concluir o processamento com sucesso, a data de pagamento da cobrança é atualizada e o status do registro de idempotência passa para `CONCLUIDO`.

## ⚙️ Configuração

A aplicação carrega variáveis de ambiente a partir de um arquivo `.env` (não versionado) na raiz do projeto `IntegracaoGatewayPagamento/`:

```env
ASAAS_BASEURL=https://api-sandbox.asaas.com/v3
ASAAS_APIKEY=sua_api_key_do_asaas
ASAAS_TOKEN=token_usado_para_validar_o_webhookFix
DB_CONNECTION_STRING=Host=localhost;Database=integracao_gateway;Username=postgres;Password=sua_senha
```

> Utilize as credenciais do ambiente **sandbox** do Asaas para testes.

## 🚀 Como executar

### Pré-requisitos
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL (local ou via container)
- Conta sandbox no [Asaas](https://www.asaas.com/)

### Localmente

```bash
git clone https://github.com/SilvestreLago/IntegracaoGatewayPagamento.git
cd IntegracaoGatewayPagamento/IntegracaoGatewayPagamento

# crie o arquivo .env com as variáveis descritas acima

dotnet restore
dotnet ef database update
dotnet run
```

A documentação interativa (Swagger) fica disponível em ambiente de desenvolvimento em `/swagger`.

### Via Docker

```bash
docker build -t integracao-gateway-pagamento .
docker run -p 8080:8080 --env-file IntegracaoGatewayPagamento/.env integracao-gateway-pagamento
```

## ⚠️ Aviso

Este repositório tem finalidade **educacional e de pesquisa**. Os endpoints "vulneráveis" foram criados intencionalmente para fins de estudo e **não devem ser utilizados em ambiente de produção**.