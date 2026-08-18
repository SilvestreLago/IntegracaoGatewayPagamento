using IntegracaoGatewayPagamento.DTO;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    // O construtor recebe 'options' e passa para a classe base (DbContext).
    // É através dessas 'options' que o .NET injeta a string de conexão do .env.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Cobranca> Cobrancas { get; set; }
    public DbSet<Webhook>  Webhooks { get; set; }
}