using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegracaoGatewayPagamento.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarProdutoEmCobranca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "idProduto",
                table: "Cobrancas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Cobrancas_idProduto",
                table: "Cobrancas",
                column: "idProduto");

            migrationBuilder.AddForeignKey(
                name: "FK_Cobrancas_Produtos_idProduto",
                table: "Cobrancas",
                column: "idProduto",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cobrancas_Produtos_idProduto",
                table: "Cobrancas");

            migrationBuilder.DropIndex(
                name: "IX_Cobrancas_idProduto",
                table: "Cobrancas");

            migrationBuilder.DropColumn(
                name: "idProduto",
                table: "Cobrancas");
        }
    }
}
