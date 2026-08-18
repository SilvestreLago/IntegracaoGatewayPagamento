using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IntegracaoGatewayPagamento.DTO;

[Index(nameof(idEventAsaas), IsUnique = true)]
public class Webhook
{
    public Guid id { get; set; }
    [Required]
    public string idEventAsaas { get; set; }
    public string status  { get; set; }
}