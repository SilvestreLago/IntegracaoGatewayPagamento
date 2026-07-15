namespace IntegracaoGatewayPagamento.DTO;

public class CobrancaInputFixDTO
{
    public required Guid idCliente { get; set; }
    public required Guid idProduto { get; set; }
    public required string billingType { get; set; }
    public required int quantidade { get; set; }
    public required DateOnly dueDate { get; set; }
}