using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Services.Interface
{
    public interface ICobrancaService
    {
        Task<Cliente?> VerificarCliente(Guid idCliente); //VERIFICAR EXISTENCIA DO CLIENTE
        Task<string> CriarCobranca(CobrancaInputDTO cobrancaInput, Guid IdCliente, string customer); //CADASTRAR PAGAMENTO - MANIPULAÇÃO DE PREÇO
        Task<string> CriarCobrancaFix(CobrancaInputFixDTO cobrancaInput, Guid IdCliente, string customer); //CADASTRAR PAGAMENTO - MANIPULAÇÃO DE PREÇO [FIX]
        Task<Cobranca?> VerificarCobranca(String idAsaas); //VERIFICAR EXISTENCIA DA COBRANCA
        Task<Cobranca?> VerificarCobrancaFix(String idAsaas, Webhook idempotencia); //VERIFICAR EXISTENCIA DA COBRANCA [FIX]
        Task<Cobranca?> UpdateCobranca(Cobranca cobranca); //ATUALIZAR INFORMAÇÕES DA COBRANCA
        Task<Webhook?> AdicionarWebhook(String idEventAsaas); //ADICIONAR IDEMPOTENCIA DO WEBHOOK
        Task<Boolean> UpdateDados(Cobranca cobranca, Webhook idempotencia); //ADICIONAR DATA DE PAGAMENTO E ALTERAR STATUS PARA CONCLUIDO
    }
    
}