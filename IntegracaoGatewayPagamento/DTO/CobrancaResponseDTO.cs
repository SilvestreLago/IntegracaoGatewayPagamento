using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracaoGatewayPagamento.DTO;

public class CobrancaResponseDTO
{
    public string id { get; set; }
    public required double value { get; set; }
    public required string billingType { get; set; }
    public required DateOnly dueDate { get; set; }
    public required string invoiceUrl { get; set; }
}