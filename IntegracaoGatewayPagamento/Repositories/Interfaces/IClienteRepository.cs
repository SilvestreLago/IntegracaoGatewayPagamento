using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        Task<ClienteDTO> CadastrarCliente(Cliente cliente); //SALVAR CLIENTE NO BANCO DE DADOS
        Task<Cliente?> VerificarCadastro(string cpfCnpj); //VERIFICAR EXISTENCIA DO CLIENTE
        Task<List<Cliente?>> BuscarClientes();
    }
    
}