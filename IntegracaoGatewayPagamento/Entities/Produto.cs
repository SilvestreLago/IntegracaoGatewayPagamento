namespace IntegracaoGatewayPagamento.DTO;

public class Produto
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public required double Preco { get; set; }
}