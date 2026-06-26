using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Services.Interface
{
    public interface IMainService
    {
        Task<ClienteDTO?> CadastrarLocal(ClienteDTO cliente); //CADASTRAR CLIENTE LOCALMENTE
        Task<ClienteDTO?> CadastrarAsaas(ClienteDTO cliente); //CADASTRAR CLIENTE NO ASAAS
    }
    
}