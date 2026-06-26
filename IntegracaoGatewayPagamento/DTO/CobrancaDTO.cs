namespace IntegracaoGatewayPagamento.DTO;

public class CobrancaDTO
{
    public required string customer { get; set; }
    public required string billingType { get; set; }
    public required double value { get; set; }
    public required DateOnly dueDate { get; set; }
}