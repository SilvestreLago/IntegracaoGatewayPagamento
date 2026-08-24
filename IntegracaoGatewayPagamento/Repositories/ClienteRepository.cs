using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegracaoGatewayPagamento.Repositories
{
    public class ClienteRepository  : IClienteRepository
    {
        private readonly AppDbContext _context;
        
        public ClienteRepository(AppDbContext context)
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
        
        //VERIFICAR A EXISTENCIA DO CLIENTE NO BANCO DE DADOS
        public async Task<Cliente?> VerificarCadastro(string cpfCnpj)
        {
            try
            {
                //BUSCANDO BASEADO NO CAMPO/COLUNA CPFCNPJ
                return await _context.Clientes.FirstOrDefaultAsync(c => c.CpfCnpj == cpfCnpj);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao verificar a existencia do cliente", e);
            }
        }
        
        //BUSCAR CLIENTES
        public async Task<List<Cliente>> BuscarClientes()
        {
            try
            {
                //BUSCANDO TODOS OS CLIENTES
                var clientes = _context.Clientes.ToList();
                return clientes;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
    }
    
}