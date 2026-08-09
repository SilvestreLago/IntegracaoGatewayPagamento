namespace IntegracaoGatewayPagamento.DTO
{
    public class WebhookDTO
    {
        public required string id { get; set; }
        public string? @event { get; set; }
        public string? dateCreated { get; set; }
        public required Payment payment { get; set; }
    }

    public class Payment
    {
        public required string id { get; set; }
        public string? customer { get; set; }
        public decimal? value { get; set; }
        public decimal? netValue { get; set; }
        public string? description { get; set; }
        public string? externalReference { get; set; }
        public string? billingType { get; set; }
        public int? installmentNumber {get; set;}
        public string? creditDate { get; set; }
        public string? status { get; set; }
    }
}