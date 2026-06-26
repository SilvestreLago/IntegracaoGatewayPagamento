using System.Text;
using System.Text.Json;
using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.IdentityModel.Tokens;

namespace IntegracaoGatewayPagamento.Services
{
    public class MainService : IMainService
    {
        private readonly IMainRepository _mainRepository;
        private readonly HttpClient _httpClient;

        public MainService(HttpClient httpClient, IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
            _httpClient = httpClient;
        }

        //CADASTRAR CLIENTE LOCALMENTE
        public async Task<ClienteDTO?> CadastrarLocal(ClienteDTO cliente)
        {
            //VALIDAÇÃO DOS PARAMETROS PASSADOS
            cliente.cpfCnpj = cliente.cpfCnpj.Trim();
            cliente.cpfCnpj = cliente.cpfCnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if(cliente == null || cliente.name.IsNullOrEmpty() || cliente.cpfCnpj.IsNullOrEmpty()) return null;
            
            //CRIAR O CLIENTE
            var clienteLocal = new Cliente
            {
                Id = Guid.NewGuid(),
                Name = cliente.name,
                CpfCnpj = cliente.cpfCnpj,
            };
            
            //SALVAR NO BANCO DE DADOS
            var clienteCadastro = await _mainRepository.CadastrarCliente(clienteLocal);

            return clienteCadastro;
            
        }

        //CADASTRAR CLIENTE NO ASAAS
        public async Task<ClienteDTO?> CadastrarAsaas(ClienteDTO cliente)
        {
            //CRIA O JSON
            var jsonContetnt = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");

            //FAZ A REQUISIÇÃO PARA CRIAR O CLIENTE NO ASAAS
            var response = await _httpClient.PostAsync("v3/customers", jsonContetnt);

            //VERIFICA SE O CLIENTE FOI CRIADO COM SUCESSO
            if (!response.IsSuccessStatusCode)
            {
                var erroJson = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {(int)response.StatusCode}: {erroJson}", null, response.StatusCode);
            }
            
            //RETORNA A RESPOSTA
            var resposta = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<ClienteDTO>(resposta);
        }
        
    }
    
}