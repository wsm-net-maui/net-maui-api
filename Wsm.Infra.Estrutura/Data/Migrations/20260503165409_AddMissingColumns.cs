using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wsm.Infra.Estrutura.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                SET @dbname = DATABASE();
                SET @tablename = 'ClientePerfis';
                SET @columnname = 'Observacoes';
                SET @preparedStatement = (SELECT IF(
                  (
                    SELECT COUNT(*) FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = @tablename AND COLUMN_NAME = @columnname
                  ) > 0,
                  'SELECT 1',
                  'ALTER TABLE `ClientePerfis` ADD COLUMN `Observacoes` varchar(1000) NULL'
                ));
                PREPARE stmt FROM @preparedStatement;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @dbname = DATABASE();
                SET @tablename = 'FuncionarioPerfis';
                SET @columnname = 'Descricao';
                SET @preparedStatement = (SELECT IF(
                  (
                    SELECT COUNT(*) FROM information_schema.COLUMNS
                    WHERE TABLE_SCHEMA = @dbname AND TABLE_NAME = @tablename AND COLUMN_NAME = @columnname
                  ) > 0,
                  'SELECT 1',
                  'ALTER TABLE `FuncionarioPerfis` ADD COLUMN `Descricao` varchar(500) NULL'
                ));
                PREPARE stmt FROM @preparedStatement;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER TABLE `ClientePerfis` DROP COLUMN `Observacoes`;");
            migrationBuilder.Sql(@"ALTER TABLE `FuncionarioPerfis` DROP COLUMN `Descricao`;");
        }
    }
}
