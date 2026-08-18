using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegracaoGatewayPagamento.Migrations
{
    /// <inheritdoc />
    public partial class CriarTabelaWebhook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Webhooks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idCobranca = table.Column<Guid>(type: "uuid", nullable: false),
                    idEventAsaas = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Webhooks", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Webhooks_idEventAsaas",
                table: "Webhooks",
                column: "idEventAsaas",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Webhooks");
        }
    }
}
