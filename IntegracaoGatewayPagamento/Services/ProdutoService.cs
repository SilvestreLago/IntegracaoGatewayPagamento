using System.Text;
using System.Text.Json;
using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.IdentityModel.Tokens;

namespace IntegracaoGatewayPagamento.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoService(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        //CADASTRAR PRODUTO
        public async Task<Produto?>CadastrarProduto(ProdutoDTO produto)
        {
            //VALIDAÇÃO DOS DADOS
            if(produto.Nome.IsNullOrEmpty() || produto.Preco <= 5) return null;
         
            //CRIAR NOVO PRODUTO
            var prod = new Produto
            {
                Id = new Guid(),
                Nome = produto.Nome,
                Preco = produto.Preco,
            };
            
            //SALVAR NO BANCO DE DADOS
            var produtoCadastro = await _produtoRepository.CadastrarProduto(prod);

            return produtoCadastro;
            
        }
    }
    
}