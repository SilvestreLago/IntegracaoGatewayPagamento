using System.ComponentModel.DataAnnotations.Schema;

namespace IntegracaoGatewayPagamento.DTO;

public class Cobranca
{
    public Guid id { get; set; }
    public Guid idCliente { get; set; }
    public Guid idProduto { get; set; }
    public String idAsaas { get; set; }
    public required int quantidade { get; set; }
    public required double value { get; set; }
    public required string billingType { get; set; }
    public required DateOnly dueDate { get; set; }
    public required DateOnly? paymentDate { get; set; }
    public required string invoiceUrl { get; set; }
    [ForeignKey("idCliente")]
    public Cliente cliente { get; set; }
    [ForeignKey("idProduto")]
    public Produto produto { get; set; }
}