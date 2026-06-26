using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;

namespace IntegracaoGatewayPagamento.Repositories
{
    public class MainRepository  : IMainRepository
    {
        private readonly AppDbContext _context;
        
        public MainRepository(AppDbContext context)
        {
            _context = context;
        }

        //SALVAR CLIENTE NO BANCO DE DADOS
        public async Task<ClienteDTO?> CadastrarCliente(Cliente cliente)
        {
            try
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                
                var clienteDTO = new ClienteDTO
                {
                    name = cliente.Name,
                    cpfCnpj = cliente.CpfCnpj,
                };
                
                return clienteDTO;
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao cadastrar cliente", e);
            }
        }
        
    }
    
}