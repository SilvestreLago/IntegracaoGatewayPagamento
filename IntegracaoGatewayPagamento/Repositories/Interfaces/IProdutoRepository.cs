using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Repositories.Interfaces
{
    public interface IProdutoRepository
    {
        Task<Produto?> CadastrarProduto(Produto produto); //SALVAR PRODUTO NO BANCO DE DADOS
        Task<double?> BuscarValorProduto(Guid idProduto); //BUSCAR O VALOR DE UM PRODUTO NO BANCO DE DADOS
    }
    
}