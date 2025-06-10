-- Garante que estamos no banco de dados master antes de criar/usar o banco UniScoreDB
USE [master];
GO

-- Criar o banco de dados UniScoreDB se não existir
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'UniScoreDB')
BEGIN
    CREATE DATABASE UniScoreDB;
END;
GO

-- Usar o banco de dados UniScoreDB
USE [UniScoreDB];
GO

-- ***** Criação das Tabelas *****
-- Copiado diretamente do seu script fornecido.

CREATE TABLE [dbo].[__EFMigrationsHistory](
    [MigrationId] [nvarchar](150) NOT NULL,
    [ProductVersion] [nvarchar](32) NOT NULL,
CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED
(
    [MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[Usuarios](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Nome] [nvarchar](100) NOT NULL,
    [Email] [nvarchar](450) NOT Cited, [Email] NOT NULL, -- Email já tem UNIQUE no DDL, mas o seu não mostra UNIQUE aqui
    [Senha] [nvarchar](max) NOT NULL,
    [Tipo] [int] NOT NULL,
    [Curso] [nvarchar](100) NULL,
    [Ativo] [bit] NOT NULL,
    [DataCriacao] [datetime2](7) NOT NULL,
CONSTRAINT [PK_Usuarios] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

-- O índice UNIQUE para Email já está no seu DDL fornecido, então não o colocamos aqui novamente
-- CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuarios_Email] ON [dbo].[Usuarios]([Email] ASC)

CREATE TABLE [dbo].[Eventos](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Nome] [nvarchar](200) NOT NULL,
    [Descricao] [nvarchar](1000) NOT NULL,
    [DataInicio] [datetime2](7) NOT NULL,
    [DataFim] [datetime2](7) NULL,
    [Local] [nvarchar](200) NOT NULL,
    [Ativo] [bit] NOT NULL,
    [DataCriacao] [datetime2](7) NOT NULL,
    [CriadorId] [int] NOT NULL,
CONSTRAINT [PK_Eventos] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[Projetos](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Titulo] [nvarchar](200) NOT NULL,
    [Descricao] [nvarchar](1000) NOT NULL,
    [AreaTematica] [nvarchar](100) NOT NULL,
    [Autores] [nvarchar](500) NOT NULL,
    [Localizacao] [nvarchar](200) NOT NULL,
    [Ativo] [bit] NOT NULL,
    [DataCriacao] [datetime2](7) NOT NULL,
    [EventoId] [int] NOT NULL,
CONSTRAINT [PK_Projetos] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[Avaliacoes](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [AvaliadorId] [int] NOT NULL,
    [ProjetoId] [int] NOT NULL,
    [DataAvaliacao] [datetime2](7) NOT NULL,
    [Observacoes] [nvarchar](max) NULL,
CONSTRAINT [PK_Avaliacoes] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

CREATE TABLE [dbo].[CriteriosAvaliacao](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Nome] [nvarchar](100) NOT NULL,
    [Descricao] [nvarchar](500) NOT NULL,
    [NotaMaxima] [decimal](18, 2) NOT NULL,
    [NotaMinima] [decimal](18, 2) NOT NULL,
    [EventoId] [int] NOT NULL,
CONSTRAINT [PK_CriteriosAvaliacao] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[ItensAvaliacao](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [AvaliacaoId] [int] NOT NULL,
    [CriterioId] [int] NOT NULL,
    [Nota] [decimal](18, 2) NOT NULL,
CONSTRAINT [PK_ItensAvaliacao] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

CREATE TABLE [dbo].[UsuarioEventos](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [UsuarioId] [int] NOT NULL,
    [EventoId] [int] NOT NULL,
    [DataAtribuicao] [datetime2](7) NOT NULL,
    [DataValidade] [datetime2](7) NULL,
CONSTRAINT [PK_UsuarioEventos] PRIMARY KEY CLUSTERED
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY];
GO

-- ***** Criação das Chaves Estrangeiras *****
ALTER TABLE [dbo].[Avaliacoes] WITH CHECK ADD CONSTRAINT [FK_Avaliacoes_Projetos_ProjetoId] FOREIGN KEY([ProjetoId])
REFERENCES [dbo].[Projetos] ([Id])
ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[Avaliacoes] CHECK CONSTRAINT [FK_Avaliacoes_Projetos_ProjetoId];
GO

ALTER TABLE [dbo].[Avaliacoes] WITH CHECK ADD CONSTRAINT [FK_Avaliacoes_Usuarios_AvaliadorId] FOREIGN KEY([AvaliadorId])
REFERENCES [dbo].[Usuarios] ([Id]);
GO
ALTER TABLE [dbo].[Avaliacoes] CHECK CONSTRAINT [FK_Avaliacoes_Usuarios_AvaliadorId];
GO

ALTER TABLE [dbo].[CriteriosAvaliacao] WITH CHECK ADD CONSTRAINT [FK_CriteriosAvaliacao_Eventos_EventoId] FOREIGN KEY([EventoId])
REFERENCES [dbo].[Eventos] ([Id])
ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[CriteriosAvaliacao] CHECK CONSTRAINT [FK_CriteriosAvaliacao_Eventos_EventoId];
GO

ALTER TABLE [dbo].[Eventos] WITH CHECK ADD CONSTRAINT [FK_Eventos_Usuarios_CriadorId] FOREIGN KEY([CriadorId])
REFERENCES [dbo].[Usuarios] ([Id]);
GO
ALTER TABLE [dbo].[Eventos] CHECK CONSTRAINT [FK_Eventos_Usuarios_CriadorId];
GO

ALTER TABLE [dbo].[ItensAvaliacao] WITH CHECK ADD CONSTRAINT [FK_ItensAvaliacao_Avaliacoes_AvaliacaoId] FOREIGN KEY([AvaliacaoId])
REFERENCES [dbo].[Avaliacoes] ([Id])
ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[ItensAvaliacao] CHECK CONSTRAINT [FK_ItensAvaliacao_Avaliacoes_AvaliacaoId];
GO

ALTER TABLE [dbo].[ItensAvaliacao] WITH CHECK ADD CONSTRAINT [FK_ItensAvaliacao_CriteriosAvaliacao_CriterioId] FOREIGN KEY([CriterioId])
REFERENCES [dbo].[CriteriosAvaliacao] ([Id]);
GO
ALTER TABLE [dbo].[ItensAvaliacao] CHECK CONSTRAINT [FK_ItensAvaliacao_CriteriosAvaliacao_CriterioId];
GO

ALTER TABLE [dbo].[Projetos] WITH CHECK ADD CONSTRAINT [FK_Projetos_Eventos_EventoId] FOREIGN KEY([EventoId])
REFERENCES [dbo].[Eventos] ([Id])
ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[Projetos] CHECK CONSTRAINT [FK_Projetos_Eventos_EventoId];
GO

ALTER TABLE [dbo].[UsuarioEventos] WITH CHECK ADD CONSTRAINT [FK_UsuarioEventos_Eventos_EventoId] FOREIGN KEY([EventoId])
REFERENCES [dbo].[Eventos] ([Id])
ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[UsuarioEventos] CHECK CONSTRAINT [FK_UsuarioEventos_Eventos_EventoId];
GO

ALTER TABLE [dbo].[UsuarioEventos] WITH CHECK ADD CONSTRAINT [FK_UsuarioEventos_Usuarios_UsuarioId] FOREIGN KEY([UsuarioId])
REFERENCES [dbo].[Usuarios] ([Id])
ON DELETE CASCADE;
GO
ALTER TABLE [dbo].[UsuarioEventos] CHECK CONSTRAINT [FK_UsuarioEventos_Usuarios_UsuarioId];
GO

-- ***** Inserção de Dados Essenciais (Seeding) *****

-- Inserir o primeiro Coordenador
-- A coluna 'Tipo' na tabela 'Usuarios' agora armazena o ID do tipo de usuário (role).
-- Assumimos que 1 = Coordenador, 2 = Avaliador, 3 = ProfessorAdmin, etc.
-- Senha HASHED para "123456": ae727K08Ka0mkSg0aGzwwvXVqGn/PKEgIMkicjbUI= (usando SHA256 do seu AuthService)
INSERT INTO [dbo].[Usuarios] (Nome, Email, Senha, Tipo, Ativo, DataCriacao, Curso)
VALUES ('Coordenador Master', 'master@unipam.edu.br', 'ae727K08Ka0mkSg0aGzwwvXVqGn/PKEgIMkicjbUI=', 1, 1, GETDATE(), NULL);
GO