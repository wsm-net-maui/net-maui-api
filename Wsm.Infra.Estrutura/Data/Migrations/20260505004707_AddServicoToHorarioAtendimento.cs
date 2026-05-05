using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wsm.Infra.Estrutura.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddServicoToHorarioAtendimento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServicoId",
                table: "HorariosAtendimento",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_HorariosAtendimento_ServicoId",
                table: "HorariosAtendimento",
                column: "ServicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_HorariosAtendimento_Servicos_ServicoId",
                table: "HorariosAtendimento",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HorariosAtendimento_Servicos_ServicoId",
                table: "HorariosAtendimento");

            migrationBuilder.DropIndex(
                name: "IX_HorariosAtendimento_ServicoId",
                table: "HorariosAtendimento");

            migrationBuilder.DropColumn(
                name: "ServicoId",
                table: "HorariosAtendimento");
        }
    }
}
