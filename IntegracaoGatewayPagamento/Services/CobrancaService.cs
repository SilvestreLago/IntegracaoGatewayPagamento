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
        private readonly ICobrancaRepository _cobrancaRepository;
        private readonly HttpClient _httpClient;

        public CobrancaService(HttpClient httpClient, ICobrancaRepository cobrancaRepository)
        {
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
        
        //CRIAR COBRANÇA
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
            var response = await _httpClient.PostAsync("v3/payments",  jsonContent);

            //VERIFICA SE A COBRANÇA FOI CRIADA COM SUCESSO
            if (!response.IsSuccessStatusCode)
            {
                var erroJson = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {(int)response.StatusCode}: {erroJson})", null, response.StatusCode);
            }
            
            //COLETA A RESPOSTA
            var resposta = JsonSerializer.Deserialize<CobrancaResponseDTO>(await response.Content.ReadAsStringAsync());
            
            //CRIA A COBRANCA COM TODOS OS DADOS PARA ARMAZENAR
            var cobrancaAsaas = new Cobranca
            {
                id = new Guid(),
                idCliente = IdCliente,
                idProduto =  cobrancaInput.idProduto,
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
    }
    
}