using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegracaoGatewayPagamento.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIdCobrancaWebhook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Webhooks_Cobrancas_idCobranca",
                table: "Webhooks");

            migrationBuilder.DropIndex(
                name: "IX_Webhooks_idCobranca",
                table: "Webhooks");

            migrationBuilder.DropColumn(
                name: "idCobranca",
                table: "Webhooks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "idCobranca",
                table: "Webhooks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
    }
}
