using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wsm.Infra.Estrutura.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `ClientePerfis` (
                    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                    `UsuarioId` char(36) COLLATE ascii_general_ci NOT NULL,
                    `Telefone` varchar(20) CHARACTER SET utf8mb4 NULL,
                    `Observacoes` varchar(1000) CHARACTER SET utf8mb4 NULL,
                    `CriadoEm` datetime(6) NOT NULL,
                    `AtualizadoEm` datetime(6) NULL,
                    `Ativo` tinyint(1) NOT NULL,
                    CONSTRAINT `PK_ClientePerfis` PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_ClientePerfis_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`Id`) ON DELETE RESTRICT
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `FuncionarioPerfis` (
                    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                    `UsuarioId` char(36) COLLATE ascii_general_ci NOT NULL,
                    `Especialidade` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
                    `Descricao` varchar(500) CHARACTER SET utf8mb4 NULL,
                    `CriadoEm` datetime(6) NOT NULL,
                    `AtualizadoEm` datetime(6) NULL,
                    `Ativo` tinyint(1) NOT NULL,
                    CONSTRAINT `PK_FuncionarioPerfis` PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_FuncionarioPerfis_Usuarios_UsuarioId` FOREIGN KEY (`UsuarioId`) REFERENCES `Usuarios` (`Id`) ON DELETE RESTRICT
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `Servicos` (
                    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                    `Nome` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
                    `Descricao` varchar(500) CHARACTER SET utf8mb4 NULL,
                    `Preco` decimal(18,2) NOT NULL,
                    `DuracaoMinutos` int NOT NULL,
                    `CriadoEm` datetime(6) NOT NULL,
                    `AtualizadoEm` datetime(6) NULL,
                    `Ativo` tinyint(1) NOT NULL,
                    CONSTRAINT `PK_Servicos` PRIMARY KEY (`Id`)
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS `HorariosAtendimento` (
                    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                    `FuncionarioPerfilId` char(36) COLLATE ascii_general_ci NOT NULL,
                    `DiaSemana` int NOT NULL,
                    `HoraInicio` time(6) NOT NULL,
                    `HoraFim` time(6) NOT NULL,
                    `IntervaloMinutos` int NOT NULL,
                    `CriadoEm` datetime(6) NOT NULL,
                    `AtualizadoEm` datetime(6) NULL,
                    `Ativo` tinyint(1) NOT NULL,
                    CONSTRAINT `PK_HorariosAtendimento` PRIMARY KEY (`Id`),
                    CONSTRAINT `FK_HorariosAtendimento_FuncionarioPerfis_FuncionarioPerfilId`
                        FOREIGN KEY (`FuncionarioPerfilId`) REFERENCES `FuncionarioPerfis` (`Id`) ON DELETE CASCADE
                ) CHARACTER SET=utf8mb4;
            ");

            migrationBuilder.Sql(@"
                SET @s1 = IF((SELECT COUNT(*) FROM information_schema.STATISTICS WHERE table_schema=DATABASE() AND table_name='ClientePerfis' AND index_name='IX_ClientePerfis_UsuarioId')=0,
                    'CREATE INDEX `IX_ClientePerfis_UsuarioId` ON `ClientePerfis` (`UsuarioId`)', 'SELECT 1');
                PREPARE st FROM @s1; EXECUTE st; DEALLOCATE PREPARE st;
            ");

            migrationBuilder.Sql(@"
                SET @s2 = IF((SELECT COUNT(*) FROM information_schema.STATISTICS WHERE table_schema=DATABASE() AND table_name='FuncionarioPerfis' AND index_name='IX_FuncionarioPerfis_UsuarioId')=0,
                    'CREATE INDEX `IX_FuncionarioPerfis_UsuarioId` ON `FuncionarioPerfis` (`UsuarioId`)', 'SELECT 1');
                PREPARE st FROM @s2; EXECUTE st; DEALLOCATE PREPARE st;
            ");

            migrationBuilder.Sql(@"
                SET @s3 = IF((SELECT COUNT(*) FROM information_schema.STATISTICS WHERE table_schema=DATABASE() AND table_name='HorariosAtendimento' AND index_name='IX_HorariosAtendimento_FuncionarioPerfilId')=0,
                    'CREATE INDEX `IX_HorariosAtendimento_FuncionarioPerfilId` ON `HorariosAtendimento` (`FuncionarioPerfilId`)', 'SELECT 1');
                PREPARE st FROM @s3; EXECUTE st; DEALLOCATE PREPARE st;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientePerfis");

            migrationBuilder.DropTable(
                name: "HorariosAtendimento");

            migrationBuilder.DropTable(
                name: "Servicos");

            migrationBuilder.DropTable(
                name: "FuncionarioPerfis");
        }
    }
}
