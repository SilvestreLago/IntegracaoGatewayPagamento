using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Repositories.Interfaces
{
    public interface ICobrancaRepository
    {
        Task<Cliente?> VerificarCliente(Guid idCliente); //VERIFICAR EXISTENCIA DO CLIENTE
        Task<string> SalvarCobranca(Cobranca cobranca); //SALVAR COBRANCA NO BANCO DE DADOS
        Task<Cobranca?> VerificarCobranca(String idAsaas); //VERIFICAR EXISTENCIA DA COBRANCA
        Task<Cobranca?> UpdateCobrancaWebhook(Cobranca cobranca); //ATUALIZAR INFORMAÇÕES DA COBRANCA
        Task<Webhook?> AdicionarWebhook(Webhook webhook); //ADICIONAR IDEMPOTENCIA DO WEBHOOK
        Task<Webhook?> UpdateDados(Cobranca cobranca, Webhook webhook); //ADICIONAR DATA DE PAGAMENTO E ALTERAR STATUS PARA CONCLUIDO
        Task<Boolean?> DeleteWebhook(Webhook idempotencia); //REMOVER O REGISTRO DO WEBHOOK
        Task<Webhook?> VerificarWebhook(String idEventAsaas); //VERIFICAR O STATUS DO WEBHOOK
    }
    
}