namespace IntegracaoGatewayPagamento.DTO;

public class Cliente
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string CpfCnpj { get; set; }
    public required string Customer { get; set; }
}