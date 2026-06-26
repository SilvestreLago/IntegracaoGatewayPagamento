using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Repositories.Interfaces
{
    public interface IMainRepository
    {
        Task<ClienteDTO> CadastrarCliente(Cliente cliente); //SALVAR CLIENTE NO BANCO DE DADOS
    }
    
}