# Integração com Gateway de Pagamentos

Repositório dedicado ao laboratório de pesquisa sobre falhas na integração com gateways de pagamentos. O projeto implementa uma API em ASP.NET Core que se integra ao gateway [Asaas](https://www.asaas.com/) para cadastro de clientes, produtos e geração de cobranças, servindo como ambiente controlado para estudo de vulnerabilidades comuns nesse tipo de integração — como manipulação de preço no lado do cliente e validação inadequada de webhooks — e suas respectivas correções.

## 🎯 Objetivo

Este é um projeto de laboratório/pesquisa. A API expõe endpoints "vulneráveis" e suas versões "corrigidas" (sufixo `Fix`) lado a lado, permitindo comparar na prática:

- O que acontece quando o valor de uma cobrança é definido a partir de dados enviados pelo cliente, em vez de ser recalculado no servidor.
- O que acontece quando um endpoint de webhook não valida a origem da requisição.

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
├── Entities/         # Modelos de domínio (Cliente, Produto, Cobranca)
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
| `POST` | `/api/webhook` | Recebe notificações de pagamento do Asaas sem validação de origem (cenário vulnerável) |
| `POST` | `/api/webhookFix` | Recebe notificações de pagamento validando o header `asaas-access-token` (cenário corrigido) |

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