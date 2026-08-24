namespace IntegracaoGatewayPagamento.DTO;

public record CobrancaViewDTO(
    string NomeCliente,
    string NomeProduto,
    double ValorCobranca,
    int Quantidade,
    DateOnly? DataPagamento,
    DateOnly? DataVencimento,
    string url
    );