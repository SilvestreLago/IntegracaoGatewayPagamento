using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Services.Interface
{
    public interface ICobrancaService
    {
        Task<Cliente?> VerificarCliente(Guid idCliente); //VERIFICAR EXISTENCIA DO CLIENTE
        Task<string> CriarCobranca(CobrancaInputDTO cobrancaInput, Guid IdCliente, string customer); //CRIAR A COBRANCA
    }
    
}