using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegracaoGatewayPagamento.Repositories
{
    public class ProdutoRepository  : IProdutoRepository
    {
        private readonly AppDbContext _context;
        
        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        //SALVAR PRODUTO NO BANCO DE DADOS
        public async Task<Produto?> CadastrarProduto(Produto produto)
        {
            try
            {
                _context.Add(produto);
                await _context.SaveChangesAsync();
                
                return produto;
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao cadastrar produto", e);
            }
        }
        
    }
    
}