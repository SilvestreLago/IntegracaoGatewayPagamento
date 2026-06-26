using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegracaoGatewayPagamento.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarCobrancas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cobrancas",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    idCliente = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: false),
                    quantidade = table.Column<int>(type: "integer", nullable: false),
                    billingType = table.Column<string>(type: "text", nullable: false),
                    dueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    paymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    invoiceUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cobrancas", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cobrancas");
        }
    }
}
