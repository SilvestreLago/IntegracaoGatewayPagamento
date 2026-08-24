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
        
        //ADICIONAR IDEMPOTENCIA DO WEBHOOK
        public async Task<Webhook?> AdicionarWebhook(Webhook webhook)
        {
            try
            {
                //SALVAR NO BANCO
                _context.Add(webhook);
                await _context.SaveChangesAsync();
                return webhook;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao salvar o webhook", e);
                return null;
            }
        }

        //ADICIONAR DATA DE PAGAMENTO E ALTERAR STATUS PARA CONCLUIDO
        public async Task<Webhook?> UpdateDados(Cobranca cobranca, Webhook webhook)
        {
            try
            {
                //SALVAR NO BANCO
                _context.Update(cobranca);
                _context.Update(webhook);
                await _context.SaveChangesAsync();
                return webhook;
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao salvar os status", e);
            }
        }

        //REMOVER O REGISTRO DO WEBHOOK
        public async Task<Boolean?> DeleteWebhook(Webhook idempotencia)
        {
            try
            {
                _context.Webhooks.Remove(idempotencia);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao remover o registro do webhook", e);
            }
        }

        //VERIFICAR O STATUS DO WEBHOOK
        public async Task<Webhook?> VerificarWebhook(String idEventAsaas)
        {
            try
            {
                return await _context.Webhooks.FirstOrDefaultAsync(c => c.idEventAsaas == idEventAsaas);
            }
            catch (Exception e)
            {
                throw new InternalServerErrorException("Erro ao buscar o registro do webhook", e);
            }
        }
        
        //BUSCAR COBRANCAS
        public async Task<List<CobrancaViewDTO>> BuscarCobrancas()
        {
            try
            {
                return await _context.Cobrancas
                    .AsNoTracking()
                    .OrderBy(c => c.dueDate)
                    .Select(c => new CobrancaViewDTO(
                        c.cliente.Name,
                        c.produto.Nome,
                        c.value,
                        c.quantidade,
                        c.paymentDate,
                        c.dueDate,
                        c.invoiceUrl
                        ))
                    .ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
}