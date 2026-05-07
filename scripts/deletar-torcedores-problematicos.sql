-- ============================================================
-- SCRIPT: Deletar torcedores criados via SQL que dao 404
-- BANCO: SQLite (Carona Alvinegra)
-- USO: HeidiSQL / DB Browser / sqlite3
-- ============================================================

-- Deleta todos os usuarios com IDs b0000... (primeira tentativa)
DELETE FROM Usuarios WHERE Id LIKE 'b0000%';

-- Deleta todos os usuarios com IDs b1000... (segunda tentativa)
DELETE FROM Usuarios WHERE Id LIKE 'b1%';

-- Ou, se quiser deletar por nome:
-- DELETE FROM Usuarios WHERE Nome IN (
--     'Pedrinho Santos', 'Guaraci Costa', 'Anne', 'Ana Luiza',
--     'Leo Mussel', 'Gabi Mussel', 'Mae Mussel', 'Guilherme Almeida',
--     'Murillo Almeida', 'Roberta Almeida', 'Isabela Carvalho',
--     'Leo Silva', 'Anna Silva', 'Lysa Silva', 'Amanda Soares',
--     'Tio Barbinha', 'Joao Bernardo', 'Emily', 'Marcela Diniz',
--     'Ester', 'Gabriella Reis', 'Daniel Rempto', 'Renan Costa',
--     'Sr(a) Isabelle Costa', 'Nicolle',
--     'Dadock', 'Guilherme Franca', 'Fonseca', 'Vitinho Lacerda',
--     'Isaac Lacerda', 'Monique Palma', 'Aline Palma', 'Miguel Palma',
--     'Tia Valeria Gama', 'Marcia Gama', 'Luis Gustavo', 'Cassiano',
--     'Giovani Carvalho', 'Giovana Nicolay',
--     'Jones', 'Lucas Barbosa', 'Luizeto', 'Cesar Vallejo', 'Rian Praxedes'
-- );

-- Confira quem sobrou:
SELECT Id, Nome, Telefone FROM Usuarios ORDER BY Nome;
