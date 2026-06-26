using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IntegracaoGatewayPagamento.Controllers
{
    [ApiController]
    [Route("api")]
    public class CobrancaController : ControllerBase
    {
        private readonly ICobrancaService _cobrancaService;
        public CobrancaController(ICobrancaService cobrancaService)
        {
            _cobrancaService = cobrancaService;
        }
        
        //CADASTRAR PAGAMENTO - MANIPULAÇÃO DE PREÇO
        [HttpPost("manipulacaoPreco")]
        public async Task<IActionResult> manipulacaoPreco([FromBody] CobrancaInputDTO cobrancaInput)
        {
            //VERIFICAR A EXISTÊNCIA DO CLIENTE
            var clienteCadastrado = await _cobrancaService.VerificarCliente(cobrancaInput.idCliente);
            if (clienteCadastrado == null)  return BadRequest($"Cliente não possui cadastro.");
            
            //CRIAR COBRANÇA NO ASAAS
            var cobranca = await _cobrancaService.CriarCobranca(cobrancaInput, clienteCadastrado.Id, clienteCadastrado.Customer);
            if (cobranca == null) return BadRequest($"Não foi possível gerar a cobrança no ASAAS.");
            
            //RETORNA O LINK DA COBRANCA
            return Ok($"Link da cobranca: {cobranca}");
        }
    }
}