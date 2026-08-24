using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IntegracaoGatewayPagamento.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        public ProdutoController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }
        
        //CADASTRAR PRODUTO
        [HttpPost("cadastrarProduto")]
        public async Task<IActionResult> cadastrarProduto([FromBody] ProdutoDTO produto)
        {
            //CADASTRAR PRODUTO
            var cadastroLocal = await _produtoService.CadastrarProduto(produto);
            if (cadastroLocal == null) return BadRequest("Não foi possível cadastrar o produto.");
            
            return Ok($"Produto {cadastroLocal.Nome} cadastrado com sucesso.");
        }
        
        //BUSCAR PRODUTO
        [HttpGet("buscarProdutos")]
        public async Task<IActionResult> buscarProdutos()
        {
            //BUSCAR PRODUTO
            var produtos = await _produtoService.BuscarProdutos();
            if (produtos == null) return BadRequest("Não foi possível encontrar os produtos");
            
            return Ok(produtos);
        }
    }
}