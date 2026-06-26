using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Services.Interface
{
    public interface IClienteService
    {
        Task<Cliente?> VerificarCadastro(string cpfCnpj); //VERIFICAR EXISTENCIA DE CADASTRO
        Task<ClienteDTO?> CadastrarLocal(Cliente cliente); //CADASTRAR CLIENTE LOCALMENTE
        Task<Cliente?> CadastrarAsaas(ClienteDTO cliente); //CADASTRAR CLIENTE NO ASAAS
    }
    
}