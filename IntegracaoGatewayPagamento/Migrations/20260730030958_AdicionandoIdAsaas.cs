using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegracaoGatewayPagamento.Migrations
{
    /// <inheritdoc />
    public partial class AdicionandoIdAsaas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "idAsaas",
                table: "Cobrancas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "idAsaas",
                table: "Cobrancas");
        }
    }
}
