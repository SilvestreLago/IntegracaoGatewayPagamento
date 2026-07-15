using IntegracaoGatewayPagamento.DTO;

namespace IntegracaoGatewayPagamento.Services.Interface
{
    public interface ICobrancaService
    {
        Task<Cliente?> VerificarCliente(Guid idCliente); //VERIFICAR EXISTENCIA DO CLIENTE
        Task<string> CriarCobranca(CobrancaInputDTO cobrancaInput, Guid IdCliente, string customer); //CADASTRAR PAGAMENTO - MANIPULAÇÃO DE PREÇO
        Task<string> CriarCobrancaFix(CobrancaInputFixDTO cobrancaInput, Guid IdCliente, string customer); //CADASTRAR PAGAMENTO - MANIPULAÇÃO DE PREÇO [FIX]
    }
    
}