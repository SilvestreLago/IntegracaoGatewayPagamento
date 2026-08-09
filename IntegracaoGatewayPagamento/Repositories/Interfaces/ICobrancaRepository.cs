using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Repositories.Interfaces
{
    public interface ICobrancaRepository
    {
        Task<Cliente?> VerificarCliente(Guid idCliente); //VERIFICAR EXISTENCIA DO CLIENTE
        Task<string> SalvarCobranca(Cobranca cobranca); //SALVAR COBRANCA NO BANCO DE DADOS
        Task<Cobranca?> VerificarCobranca(String idAsaas); //VERIFICAR EXISTENCIA DA COBRANCA
        Task<Cobranca?> UpdateCobrancaWebhook(Cobranca cobranca);
    }
    
}