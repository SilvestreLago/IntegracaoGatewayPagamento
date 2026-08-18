using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegracaoGatewayPagamento.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignkeyWebhook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_idCobranca",
                table: "Webhooks",
                column: "idCobranca");

            migrationBuilder.AddForeignKey(
                name: "FK_Webhooks_Cobrancas_idCobranca",
                table: "Webhooks",
                column: "idCobranca",
                principalTable: "Cobrancas",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Webhooks_Cobrancas_idCobranca",
                table: "Webhooks");

            migrationBuilder.DropIndex(
                name: "IX_Webhooks_idCobranca",
                table: "Webhooks");
        }
    }
}
