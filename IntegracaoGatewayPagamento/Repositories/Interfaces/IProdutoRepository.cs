using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> CadastrarProduto(Produto produto); //SALVAR PRODUTO NO BANCO DE DADOS
    }
    
}