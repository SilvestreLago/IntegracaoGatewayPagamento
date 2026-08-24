using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Services.Interface
{
    public interface IProdutoService
    {
        Task<Produto?> CadastrarProduto(ProdutoDTO produto); //CADASTRAR PRODUTO
        Task<List<Produto?>> BuscarProdutos(); //BUSCAR PRODUTOS
    }
    
}