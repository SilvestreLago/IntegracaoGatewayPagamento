using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IntegracaoGatewayPagamento.Controllers
{
    [ApiController]
    [Route("api")]
    public class MainController : ControllerBase
    {
        private readonly IMainService _mainService;
        public MainController(IMainService mainService)
        {
            _mainService = mainService;
        }
        
        //CADASTRAR CLIENTE NO ASAAS
        [HttpPost("cadastrarCliente")]
        public async Task<IActionResult> cadastrarCliente([FromBody] ClienteDTO cliente)
        {
            //CADASTRAR CLIENTE LOCALMENTE
            var cadastroLocal = await _mainService.CadastrarLocal(cliente);
            if (cadastroLocal == null) return BadRequest("Não foi possível cadastrar o cliente localmente.");
            
            //CADASTRAR CLIENTE NO ASAAS
            var cadastroAsaas = await _mainService.CadastrarAsaas(cliente);
            if (cadastroAsaas == null) return BadRequest("Não foi possível cadastrar o cliente no ASAAS.");
            
            return Ok($"Cliente {cadastroLocal.name} cadastrado com sucesso!");
        }
    }
}