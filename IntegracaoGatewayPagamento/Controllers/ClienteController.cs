using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace IntegracaoGatewayPagamento.Controllers
{
    [ApiController]
    [Route("api")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }
        
        //CADASTRAR CLIENTE
        [HttpPost("cadastrarCliente")]
        public async Task<IActionResult> cadastrarCliente([FromBody] ClienteDTO cliente)
        {
            //VERIFICAR A EXISTÊNCIA DO CLIENTE
            var clienteCadastrado = await _clienteService.VerificarCadastro(cliente.cpfCnpj);
            if (clienteCadastrado != null)  return Conflict($"Cliente {cliente.name} já possui cadastro.");
            
            //CADASTRAR CLIENTE NO ASAAS
            var cadastroAsaas = await _clienteService.CadastrarAsaas(cliente);
            if (cadastroAsaas == null) return BadRequest("Não foi possível cadastrar o cliente no ASAAS.");
            
            //CADASTRAR CLIENTE LOCALMENTE
            var cadastroLocal = await _clienteService.CadastrarLocal(cadastroAsaas);
            if (cadastroLocal == null) return BadRequest("Não foi possível cadastrar o cliente localmente.");
            
            return Ok($"Cliente {cadastroLocal.name} cadastrado com sucesso!");
        }
    }
}