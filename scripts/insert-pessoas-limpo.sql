-- ============================================================
-- SCRIPT: Inserir Apenas as Pessoas (VERSAO LIMPA)
-- BANCO: SQLite (Carona Alvinegra)
-- USO: HeidiSQL / DB Browser / sqlite3
-- NOTA: Usa INSERT OR REPLACE para evitar erro de ID duplicado
-- ============================================================

-- =====================
-- VAN 1 - Rota: Cascatinha
-- =====================
INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000001-0000-4000-8000-000000000001', 'Pedrinho Santos',  '(24) 99901-0001', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000002-0000-4000-8000-000000000002', 'Guaraci Costa',   '(24) 99901-0002', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000003-0000-4000-8000-000000000003', 'Anne',            '(24) 99901-0003', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000004-0000-4000-8000-000000000004', 'Ana Luiza',       '(24) 99901-0004', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000005-0000-4000-8000-000000000005', 'Leo Mussel',      '(24) 99901-0005', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000006-0000-4000-8000-000000000006', 'Gabi Mussel',     '(24) 99901-0006', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000007-0000-4000-8000-000000000007', 'Mae Mussel',      '(24) 99901-0007', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000008-0000-4000-8000-000000000008', 'Guilherme Almeida','(24) 99901-0008', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000009-0000-4000-8000-000000000009', 'Murillo Almeida', '(24) 99901-0009', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000010-0000-4000-8000-000000000010', 'Roberta Almeida', '(24) 99901-0010', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000011-0000-4000-8000-000000000011', 'Isabela Carvalho','(24) 99901-0011', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000012-0000-4000-8000-000000000012', 'Leo Silva',       '(24) 99901-0012', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000013-0000-4000-8000-000000000013', 'Anna Silva',      '(24) 99901-0013', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000014-0000-4000-8000-000000000014', 'Lysa Silva',      '(24) 99901-0014', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000015-0000-4000-8000-000000000015', 'Amanda Soares',   '(24) 99901-0015', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Cascatinha';

-- =====================
-- VAN 2 - Rota: Bingen
-- =====================
INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000016-0000-4000-8000-000000000016', 'Tio Barbinha',    '(24) 99902-0001', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000017-0000-4000-8000-000000000017', 'Joao Bernardo',   '(24) 99902-0002', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000018-0000-4000-8000-000000000018', 'Emily',           '(24) 99902-0003', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000019-0000-4000-8000-000000000019', 'Marcela Diniz',   '(24) 99902-0004', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000020-0000-4000-8000-000000000020', 'Ester',           '(24) 99902-0005', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000021-0000-4000-8000-000000000021', 'Gabriella Reis',  '(24) 99902-0006', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000022-0000-4000-8000-000000000022', 'Daniel Rempto',   '(24) 99902-0007', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000023-0000-4000-8000-000000000023', 'Renan Costa',     '(24) 99902-0008', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000024-0000-4000-8000-000000000024', 'Sr(a) Isabelle Costa','(24) 99902-0009', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000025-0000-4000-8000-000000000025', 'Nicolle',         '(24) 99902-0010', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Bingen';

-- =====================
-- VAN 3 - Rota: Quitandinha
-- =====================
INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000026-0000-4000-8000-000000000026', 'Dadock',          '(24) 99903-0001', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000027-0000-4000-8000-000000000027', 'Guilherme Franca', '(24) 99903-0002', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000028-0000-4000-8000-000000000028', 'Fonseca',         '(24) 99903-0003', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000029-0000-4000-8000-000000000029', 'Vitinho Lacerda', '(24) 99903-0004', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000030-0000-4000-8000-000000000030', 'Isaac Lacerda',   '(24) 99903-0005', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000031-0000-4000-8000-000000000031', 'Monique Palma',   '(24) 99903-0006', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000032-0000-4000-8000-000000000032', 'Aline Palma',     '(24) 99903-0007', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000033-0000-4000-8000-000000000033', 'Miguel Palma',    '(24) 99903-0008', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000034-0000-4000-8000-000000000034', 'Tia Valeria Gama','(24) 99903-0009', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000035-0000-4000-8000-000000000035', 'Marcia Gama',     '(24) 99903-0010', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000036-0000-4000-8000-000000000036', 'Luis Gustavo',    '(24) 99903-0011', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000037-0000-4000-8000-000000000037', 'Cassiano',        '(24) 99903-0012', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000038-0000-4000-8000-000000000038', 'Giovani Carvalho','(24) 99903-0013', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000039-0000-4000-8000-000000000039', 'Giovana Nicolay', '(24) 99903-0014', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Quitandinha';

-- =====================
-- SPIN (14:00-14:10) - Rota: Centro
-- =====================
INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000040-0000-4000-8000-000000000040', 'Jones',           '(24) 99904-0001', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Centro';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000041-0000-4000-8000-000000000041', 'Lucas Barbosa',   '(24) 99904-0002', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Centro';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000042-0000-4000-8000-000000000042', 'Luizeto',         '(24) 99904-0003', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Centro';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000043-0000-4000-8000-000000000043', 'Cesar Vallejo',   '(24) 99904-0004', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Centro';

INSERT OR REPLACE INTO Usuarios (Id, Nome, Telefone, RotaPreferencialId, GrupoId, CriadoEm, AtualizadoEm)
SELECT 'b1000044-0000-4000-8000-000000000044', 'Rian Praxedes',   '(24) 99904-0005', r.Id, NULL, datetime('now'), datetime('now')
FROM Rotas r WHERE r.Nome = 'Centro';
