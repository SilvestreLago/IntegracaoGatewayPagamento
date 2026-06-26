using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.IdentityModel.Tokens;

namespace IntegracaoGatewayPagamento.Services
{
    public class MainService : IMainService
    {
        private readonly IMainRepository _mainRepository;

        public MainService(IMainRepository mainRepository)
        {
            _mainRepository = mainRepository;
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
        public Task<ClienteDTO?> CadastrarAsaas(ClienteDTO cliente)
        {
            throw new NotImplementedException();
        }
        
    }
    
}