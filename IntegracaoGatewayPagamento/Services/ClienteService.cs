using System.Text;
using System.Text.Json;
using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.IdentityModel.Tokens;

namespace IntegracaoGatewayPagamento.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly HttpClient _httpClient;

        public ClienteService(HttpClient httpClient, IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
            _httpClient = httpClient;
        }

        //CADASTRAR CLIENTE LOCALMENTE
        public async Task<ClienteDTO?> CadastrarLocal(Cliente cliente)
        {
            //VALIDAÇÃO DOS DADOS
            cliente.Customer = cliente.Customer.Trim();
            if (cliente.Customer.IsNullOrEmpty()) return null;
            
            //SALVAR NO BANCO DE DADOS
            var clienteCadastro = await _clienteRepository.CadastrarCliente(cliente);

            return clienteCadastro;
            
        }

        //CADASTRAR CLIENTE NO ASAAS
        public async Task<Cliente?> CadastrarAsaas(ClienteDTO cliente)
        {
            //VALIDAÇÃO DOS DADOS   
            cliente.cpfCnpj = cliente.cpfCnpj.Trim();
            if(cliente.name.IsNullOrEmpty()) return null;
            
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
            var resposta = JsonSerializer.Deserialize<ClienteAsaasResponseDTO>(await response.Content.ReadAsStringAsync());

            //CRIA O CLIENTE COM TODOS OS DADOS
            var clienteAsaas = new Cliente
            {
                Id = new Guid(),
                Name = cliente.name,
                CpfCnpj = cliente.cpfCnpj,
                Customer = resposta.id
            }; 
            
            return clienteAsaas;
        }
        
        //VERIFICAR EXISTENCIA DE CADASTRO DO CLIENTE
        public Task<Cliente?> VerificarCadastro(string cpfCnpj)
        {
            //VALIDAÇÃO DOS PARAMETROS PASSADOS
            cpfCnpj = cpfCnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if(cpfCnpj.IsNullOrEmpty()) return null;
            
            //BUSCAR NO BANCO DE DADOS
            var verificar = _clienteRepository.VerificarCadastro(cpfCnpj);
            return verificar;
        }
        
        //BUSCAR CLIENTES
        public async Task<List<Cliente?>> BuscarClientes()
        {
            //BUSCAR CLIENTES
            var clientes = await _clienteRepository.BuscarClientes();
            if (clientes == null) return null;

            return clientes;
        }
    }
    
}