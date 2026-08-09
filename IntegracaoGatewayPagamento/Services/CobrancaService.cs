using System.ComponentModel;
using System.Text;
using System.Text.Json;
using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.IdentityModel.Tokens;

namespace IntegracaoGatewayPagamento.Services
{
    public class CobrancaService : ICobrancaService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICobrancaRepository _cobrancaRepository;
        private readonly HttpClient _httpClient;

        public CobrancaService(HttpClient httpClient, ICobrancaRepository cobrancaRepository,
            IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
            _cobrancaRepository = cobrancaRepository;
            _httpClient = httpClient;
        }

        //VERIFICAR EXISTENCIA DO CLIENTE
        public async Task<Cliente?> VerificarCliente(Guid idCliente)
        {
            //BUSCAR NO BANCO DE DADOS
            var verificar = await _cobrancaRepository.VerificarCliente(idCliente);
            return verificar;
        }

        //CADASTRAR PAGAMENTO - MANIPULAÇÃO DE PREÇO
        public async Task<string> CriarCobranca(CobrancaInputDTO cobrancaInput, Guid IdCliente, string customer)
        {
            //CRIAR A COBRANCA
            var cobranca = new CobrancaDTO
            {
                billingType = cobrancaInput.billingType,
                customer = customer,
                dueDate = cobrancaInput.dueDate,
                value = cobrancaInput.value
            };

            //CRIAR O JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(cobranca), Encoding.UTF8, "application/json");

            //FAZ A REQUISIÇÃO PARA GERAR A COBRANÇA
            var response = await _httpClient.PostAsync("v3/payments", jsonContent);

            //VERIFICA SE A COBRANÇA FOI CRIADA COM SUCESSO
            if (!response.IsSuccessStatusCode)
            {
                var erroJson = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {(int)response.StatusCode}: {erroJson})", null,
                    response.StatusCode);
            }

            //COLETA A RESPOSTA
            var resposta = JsonSerializer.Deserialize<CobrancaResponseDTO>(await response.Content.ReadAsStringAsync());

            //CRIA A COBRANCA COM TODOS OS DADOS PARA ARMAZENAR
            var cobrancaAsaas = new Cobranca
            {
                id = new Guid(),
                idCliente = IdCliente,
                idProduto = cobrancaInput.idProduto,
                idAsaas = resposta.id,
                value = cobrancaInput.value,
                quantidade = cobrancaInput.quantidade,
                billingType = cobrancaInput.billingType,
                dueDate = cobrancaInput.dueDate,
                paymentDate = null,
                invoiceUrl = resposta.invoiceUrl
            };

            //SALVAR COBRANCA NO BANCO DE DADOS
            var cobrancaBanco = await _cobrancaRepository.SalvarCobranca(cobrancaAsaas);

            return cobrancaBanco;
        }

        //CADASTRAR PAGAMENTO - MANIPULAÇÃO DE PREÇO [FIX]
        public async Task<string> CriarCobrancaFix(CobrancaInputFixDTO cobrancaInput, Guid IdCliente, string customer)
        {
            //BUSCAR O VALOR DO PRODUTO COM BASE NO ID
            var valorProduto = _produtoRepository.BuscarValorProduto(cobrancaInput.idProduto);
            
            //VERIFICA SE A QUANTIDADE A SER COMPRADA É MAIOR QUE 0 E SE O PRODUTO FOI ENCONTRADO
            if (cobrancaInput.quantidade <= 0 || valorProduto == null)
            {
                return null;
            }
            
            //CALCULA O VALOR DA COBRANÇA COM BASE NO PREÇO E QUANTIDADE
            var valorFinal = cobrancaInput.quantidade * valorProduto.Result.Value;
            
            //CRIAR A COBRANCA
            var cobranca = new CobrancaDTO
            {
                billingType = cobrancaInput.billingType,
                customer = customer,
                dueDate = cobrancaInput.dueDate,
                value = valorFinal
            };

            //CRIAR O JSON
            var jsonContent = new StringContent(JsonSerializer.Serialize(cobranca), Encoding.UTF8, "application/json");

            //FAZ A REQUISIÇÃO PARA GERAR A COBRANÇA
            var response = await _httpClient.PostAsync("v3/payments", jsonContent);

            //VERIFICA SE A COBRANÇA FOI CRIADA COM SUCESSO
            if (!response.IsSuccessStatusCode)
            {
                var erroJson = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {(int)response.StatusCode}: {erroJson})", null,
                    response.StatusCode);
            }

            //COLETA A RESPOSTA
            var resposta = JsonSerializer.Deserialize<CobrancaResponseDTO>(await response.Content.ReadAsStringAsync());

            //CRIA A COBRANCA COM TODOS OS DADOS PARA ARMAZENAR
            var cobrancaAsaas = new Cobranca
            {
                id = new Guid(),
                idCliente = IdCliente,
                idProduto = cobrancaInput.idProduto,
                idAsaas = resposta.id,
                value = valorFinal,
                quantidade = cobrancaInput.quantidade,
                billingType = cobrancaInput.billingType,
                dueDate = cobrancaInput.dueDate,
                paymentDate = null,
                invoiceUrl = resposta.invoiceUrl
            };

            //SALVAR COBRANCA NO BANCO DE DADOS
            var cobrancaBanco = await _cobrancaRepository.SalvarCobranca(cobrancaAsaas);

            return cobrancaBanco;
        }
        
        //VERIFICAR EXISTENCIA DA COBRANCA
        public async Task<Cobranca?> VerificarCobranca(String idAsaas)
        {
            //BUSCAR NO BANCO DE DADOS
            var verificar = await _cobrancaRepository.VerificarCobranca(idAsaas);
            if (verificar != null) return verificar; 
            return null;
        }
        
        //ATUALIZAR INFORMAÇÕES DA COBRANCA
        public async Task<Cobranca?> UpdateCobranca(Cobranca cobranca)
        {
            //ALTERAR DATA DE PAGAMENTO
            cobranca.paymentDate = DateOnly.FromDateTime(DateTime.Now);
            
            //SALVAR NO BANCO
            var updateBanco = await _cobrancaRepository.UpdateCobrancaWebhook(cobranca);
            return updateBanco;
        }
    }
}