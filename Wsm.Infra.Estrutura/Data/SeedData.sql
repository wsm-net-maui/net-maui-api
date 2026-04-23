-- Script de Seed Data para Banco de Dados de Controle de Estoque
-- Criado em: 08/04/2026

USE ControleEstoqueDb_Dev;
GO

-- Limpar dados existentes (CUIDADO: Apenas para desenvolvimento!)
DELETE FROM MovimentacoesEstoque;
DELETE FROM Produtos;
DELETE FROM Categorias;
DELETE FROM Usuarios;
GO

-- 1. USUÁRIOS
DECLARE @AdminId UNIQUEIDENTIFIER = NEWID();
DECLARE @OperadorId UNIQUEIDENTIFIER = NEWID();

INSERT INTO Usuarios (Id, Nome, Email, SenhaHash, Cargo, CriadoEm, Ativo)
VALUES 
    (@AdminId, 'Administrador', 'admin@email.com', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Administrador', GETDATE(), 1),
    -- Senha: Senha@123 (SHA256 hash)
    (@OperadorId, 'João Silva', 'joao@email.com', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=', 'Operador', GETDATE(), 1);
GO

-- 2. CATEGORIAS
DECLARE @CategoriaEletronicos UNIQUEIDENTIFIER = NEWID();
DECLARE @CategoriaInformatica UNIQUEIDENTIFIER = NEWID();
DECLARE @CategoriaEscritorio UNIQUEIDENTIFIER = NEWID();

INSERT INTO Categorias (Id, Nome, Descricao, CriadoEm, Ativo)
VALUES 
    (@CategoriaEletronicos, 'Eletrônicos', 'Produtos eletrônicos em geral', GETDATE(), 1),
    (@CategoriaInformatica, 'Informática', 'Produtos de informática e tecnologia', GETDATE(), 1),
    (@CategoriaEscritorio, 'Escritório', 'Material de escritório e papelaria', GETDATE(), 1);
GO

-- 3. PRODUTOS
DECLARE @ProdutoNotebook UNIQUEIDENTIFIER = NEWID();
DECLARE @ProdutoMouse UNIQUEIDENTIFIER = NEWID();
DECLARE @ProdutoTeclado UNIQUEIDENTIFIER = NEWID();
DECLARE @ProdutoMonitor UNIQUEIDENTIFIER = NEWID();

-- Obter IDs das categorias (já inseridas)
DECLARE @CatInfo UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Informática');
DECLARE @CatEletro UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Categorias WHERE Nome = 'Eletrônicos');

INSERT INTO Produtos (Id, Nome, Descricao, CodigoBarras, PrecoCompra, PrecoVenda, QuantidadeEstoque, EstoqueMinimo, CategoriaId, CriadoEm, Ativo)
VALUES 
    (@ProdutoNotebook, 'Notebook Dell Inspiron 15', 'Notebook Intel Core i5, 8GB RAM, 256GB SSD', '7891234567890', 2500.00, 3200.00, 10, 5, @CatInfo, GETDATE(), 1),
    (@ProdutoMouse, 'Mouse Logitech MX Master 3', 'Mouse sem fio ergonômico', '7891234567891', 250.00, 350.00, 25, 10, @CatInfo, GETDATE(), 1),
    (@ProdutoTeclado, 'Teclado Mecânico Keychron K2', 'Teclado mecânico wireless', '7891234567892', 400.00, 550.00, 15, 8, @CatInfo, GETDATE(), 1),
    (@ProdutoMonitor, 'Monitor LG 27"', 'Monitor Full HD 27 polegadas', '7891234567893', 800.00, 1100.00, 8, 3, @CatEletro, GETDATE(), 1);
GO

-- 4. MOVIMENTAÇÕES DE ESTOQUE
DECLARE @AdminId2 UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Usuarios WHERE Email = 'admin@email.com');
DECLARE @ProdNotebook UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Produtos WHERE Nome LIKE 'Notebook%');
DECLARE @ProdMouse UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM Produtos WHERE Nome LIKE 'Mouse%');

INSERT INTO MovimentacoesEstoque (Id, ProdutoId, Tipo, Quantidade, Observacao, UsuarioId, CriadoEm, Ativo)
VALUES 
    (NEWID(), @ProdNotebook, 0, 10, 'Entrada inicial de estoque', @AdminId2, DATEADD(DAY, -7, GETDATE()), 1),
    (NEWID(), @ProdMouse, 0, 25, 'Entrada inicial de estoque', @AdminId2, DATEADD(DAY, -7, GETDATE()), 1),
    (NEWID(), @ProdNotebook, 1, 2, 'Venda para cliente', @AdminId2, DATEADD(DAY, -3, GETDATE()), 1),
    (NEWID(), @ProdMouse, 1, 5, 'Venda para cliente', @AdminId2, DATEADD(DAY, -2, GETDATE()), 1);
GO

-- Verificar dados inseridos
SELECT 'Usuários' AS Tabela, COUNT(*) AS Total FROM Usuarios
UNION ALL
SELECT 'Categorias', COUNT(*) FROM Categorias
UNION ALL
SELECT 'Produtos', COUNT(*) FROM Produtos
UNION ALL
SELECT 'Movimentações', COUNT(*) FROM MovimentacoesEstoque;
GO

PRINT 'Seed data inserido com sucesso!';
GO
