using IntegracaoGatewayPagamento.DTO;
using IntegracaoGatewayPagamento.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IntegracaoGatewayPagamento.Repositories
{
    public class CobrancaRepository  : ICobrancaRepository
    {
        private readonly AppDbContext _context;
        
        public CobrancaRepository(AppDbContext context)
        {
            _context = context;
        }
        
        //VERIFICAR A EXISTENCIA DO CUSTOMER NO BANCO DE DADOS
        public async Task<Cliente?> VerificarCliente(Guid idCliente)
        {
            try
            {
                //BUSCANDO BASEADO NO ID DO CLIENTE
                return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == idCliente);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao verificar a existencia do cliente", e);
            }
        }
        
        //SALVAR COBRANCA NO BANCO DE DADOS
        public async Task<string> SalvarCobranca(Cobranca cobranca)
        {
            try
            {
                _context.Add(cobranca);
                await _context.SaveChangesAsync();
                
                return cobranca.invoiceUrl;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        //VERIFICAR A EXISTENCIA DO IDASAAS NO BANCO DE DADOS
        public async Task<Cobranca?> VerificarCobranca(String idAsaas)
        {
            try
            {
                //BUSCANDO BASEADO NO ID DA COBRANCA
                return await _context.Cobrancas.FirstOrDefaultAsync(c => c.idAsaas == idAsaas);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao verificar a existencia da cobranca", e);
            }
        }
        
        //SALVAR INFORMAÇÕES NO BANCO DE DADOS
        public async Task<Cobranca?> UpdateCobrancaWebhook(Cobranca cobranca)
        {
            try
            {
                //SALVAR NO BANCO
                await _context.SaveChangesAsync();
                return cobranca;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
}