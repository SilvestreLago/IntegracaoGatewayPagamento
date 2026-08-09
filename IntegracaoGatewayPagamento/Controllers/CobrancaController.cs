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
        
        //CADASTRAR PAGAMENTO - MANIPULAÇÃO DE PREÇO [FIX]
        [HttpPost("manipulacaoPrecoFix")]
        public async Task<IActionResult> manipulacaoPrecoFix([FromBody] CobrancaInputFixDTO cobrancaInputFix)
        {
            //VERIFICAR A EXISTÊNCIA DO CLIENTE
            var clienteCadastrado = await _cobrancaService.VerificarCliente(cobrancaInputFix.idCliente);
            if (clienteCadastrado == null)  return BadRequest($"Cliente não possui cadastro.");
            
            //CRIAR COBRANÇA NO ASAAS
            var cobranca = await _cobrancaService.CriarCobrancaFix(cobrancaInputFix, clienteCadastrado.Id, clienteCadastrado.Customer);
            if (cobranca == null) return BadRequest($"Não foi possível gerar a cobrança no ASAAS.");
            
            //RETORNA O LINK DA COBRANCA
            return Ok($"Link da cobranca: {cobranca}");
        }
        
        //ADICIONAR DATA DE PAGAMENTO - VALIDAÇÃO DE WEBHOOK
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook([FromBody] WebhookDTO req)
        {
            //VERIFICAR A EXISTÊNCIA DA COBRANCA
            var cobranca = await _cobrancaService.VerificarCobranca(req.payment.id);
            if (cobranca == null) return BadRequest($"Cobrança não encontrada.");
            
            //ADICIONAR DATA DE PAGAMENTO
            cobranca.paymentDate = DateOnly.FromDateTime(DateTime.Now);
            _cobrancaService.UpdateCobranca(cobranca);
            
            return Ok("Pagamento atualizado com sucesso.");
        }
        
        //ADICIONAR DATA DE PAGAMENTO[FIX] - VALIDAÇÃO DE WEBHOOK
        [HttpPost("webhookFix")]
        public async Task<IActionResult> WebhookFix([FromHeader (Name = "asaas-access-token")] string? token, [FromBody] WebhookDTO req)
        {
            //VERIFICA SE O TOKEN É VALIDO
            var _asaasToken = Environment.GetEnvironmentVariable("ASAAS_TOKEN");
            if(string.IsNullOrEmpty(token) || token != _asaasToken) return Unauthorized("Token de acesso inválido.");
            
            //VERIFICAR A EXISTÊNCIA DA COBRANCA
            var cobranca = await _cobrancaService.VerificarCobranca(req.payment.id);
            if (cobranca == null) return BadRequest($"Cobrança não encontrada.");
            
            //ADICIONAR DATA DE PAGAMENTO
            cobranca.paymentDate = DateOnly.FromDateTime(DateTime.Now);
            _cobrancaService.UpdateCobranca(cobranca);
            
            return Ok("Pagamento atualizado com sucesso.");
        }
    }
}