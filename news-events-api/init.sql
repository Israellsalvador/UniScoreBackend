-- Criação das tabelas conforme especificação original
CREATE TABLE IF NOT EXISTS TipoUsuario (
  IdTipoUsuario tinyint(3) NOT NULL AUTO_INCREMENT, 
  PapelUsuario  varchar(50) NOT NULL, 
  PRIMARY KEY (IdTipoUsuario)
);

CREATE TABLE IF NOT EXISTS Pessoa (
  IdPessoa   int(10) NOT NULL AUTO_INCREMENT, 
  NomePessoa varchar(255) NOT NULL, 
  Usuario    varchar(50) NOT NULL UNIQUE, 
  Senha      varchar(128) NOT NULL, 
  PRIMARY KEY (IdPessoa)
);

CREATE TABLE IF NOT EXISTS Pessoa_TipoUsuario (
  Id            int(10) NOT NULL AUTO_INCREMENT, 
  IdPessoa      int(10) NOT NULL, 
  IdTipoUsuario tinyint(3) NOT NULL, 
  PRIMARY KEY (Id, IdPessoa, IdTipoUsuario),
  FOREIGN KEY (IdPessoa) REFERENCES Pessoa (IdPessoa),
  FOREIGN KEY (IdTipoUsuario) REFERENCES TipoUsuario (IdTipoUsuario)
);

CREATE TABLE IF NOT EXISTS Evento (
  IdEvento        int(10) NOT NULL AUTO_INCREMENT, 
  NomeEvento      varchar(255) NOT NULL, 
  DescricaoEvento text NOT NULL, 
  Local           varchar(255) NOT NULL, 
  DataInicio      datetime NOT NULL, 
  DataFim         datetime NULL, 
  PRIMARY KEY (IdEvento)
);

CREATE TABLE IF NOT EXISTS Projeto (
  IdProjeto        int(10) NOT NULL AUTO_INCREMENT, 
  NomeProjeto      varchar(255) NOT NULL, 
  DescricaoProjeto varchar(500) NOT NULL, 
  Integrantes      varchar(500) NOT NULL, 
  Turma            varchar(100) NOT NULL, 
  NotaFinal        float, 
  PRIMARY KEY (IdProjeto)
);

CREATE TABLE IF NOT EXISTS InformacoesComplementares (
  IdComplemento          int(10) NOT NULL AUTO_INCREMENT, 
  InformacaoComplementar text NOT NULL, 
  IdProjeto              int(10) NOT NULL, 
  PRIMARY KEY (IdComplemento),
  FOREIGN KEY (IdProjeto) REFERENCES Projeto (IdProjeto)
);

CREATE TABLE IF NOT EXISTS Avaliacao (
  IdAvaliacao int(10) NOT NULL AUTO_INCREMENT, 
  IdPessoa    int(10) NOT NULL, 
  IdProjeto   int(10) NOT NULL, 
  PRIMARY KEY (IdAvaliacao),
  FOREIGN KEY (IdPessoa) REFERENCES Pessoa (IdPessoa),
  FOREIGN KEY (IdProjeto) REFERENCES Projeto (IdProjeto)
);

CREATE TABLE IF NOT EXISTS CriterioAvaliacao (
  IdCriterio   int(10) NOT NULL AUTO_INCREMENT, 
  IdEvento     int(10) NOT NULL, 
  NomeCriterio varchar(255) NOT NULL, 
  PRIMARY KEY (IdCriterio),
  FOREIGN KEY (IdEvento) REFERENCES Evento (IdEvento)
);

CREATE TABLE IF NOT EXISTS Nota (
  Id          int(10) NOT NULL AUTO_INCREMENT, 
  IdCriterio  int(10) NOT NULL, 
  IdAvaliacao int(10) NOT NULL, 
  NotaProjeto float NOT NULL, 
  PRIMARY KEY (Id, IdCriterio, IdAvaliacao),
  FOREIGN KEY (IdCriterio) REFERENCES CriterioAvaliacao (IdCriterio),
  FOREIGN KEY (IdAvaliacao) REFERENCES Avaliacao (IdAvaliacao)
);

CREATE TABLE IF NOT EXISTS Evento_Projeto (
  Id        int(10) NOT NULL AUTO_INCREMENT, 
  IdEvento  int(10) NOT NULL, 
  IdProjeto int(10) NOT NULL, 
  PRIMARY KEY (Id, IdEvento, IdProjeto),
  FOREIGN KEY (IdEvento) REFERENCES Evento (IdEvento),
  FOREIGN KEY (IdProjeto) REFERENCES Projeto (IdProjeto)
);

-- Inserção de dados iniciais
INSERT INTO TipoUsuario (PapelUsuario) VALUES 
('Administrador'),
('Professor'),
('Avaliador'),
('Coordenador');

-- Inserir usuário administrador padrão (senha: admin123)
INSERT INTO Pessoa (NomePessoa, Usuario, Senha) VALUES 
('Administrador do Sistema', 'admin', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg='),
('Professor João Silva', 'joao.silva', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg='),
('Avaliador Maria Santos', 'maria.santos', 'jGl25bVBBBW96Qi9Te4V37Fnqchz/Eu4qB9vKrRIqRg=');

-- Associar tipos de usuário
INSERT INTO Pessoa_TipoUsuario (IdPessoa, IdTipoUsuario) VALUES 
(1, 1), -- Admin é Administrador
(2, 2), -- João é Professor
(2, 3), -- João também é Avaliador
(3, 3); -- Maria é Avaliadora

-- Dados de exemplo para Eventos
INSERT INTO Evento (NomeEvento, DescricaoEvento, Local, DataInicio, DataFim) VALUES 
('Feira de Ciências 2024', 'Apresentação de projetos científicos dos alunos', 'Auditório Principal', '2024-06-15 08:00:00', '2024-06-15 18:00:00'),
('Mostra de TCC', 'Apresentação dos Trabalhos de Conclusão de Curso', 'Laboratório de Informática', '2024-07-10 14:00:00', '2024-07-10 17:00:00'),
('Hackathon Estudantil', 'Competição de desenvolvimento de software', 'Sala de Projetos', '2024-08-20 09:00:00', '2024-08-22 18:00:00');

-- Dados de exemplo para Projetos
INSERT INTO Projeto (NomeProjeto, DescricaoProjeto, Integrantes, Turma, NotaFinal) VALUES 
('Sistema de Gestão Escolar', 'Desenvolvimento de um sistema web para gestão de escolas', 'Ana Silva, Carlos Santos, Maria Oliveira', 'INFO-2024A', NULL),
('App de Monitoramento Ambiental', 'Aplicativo mobile para monitoramento da qualidade do ar', 'João Pedro, Fernanda Costa', 'INFO-2024B', NULL),
('Robô Seguidor de Linha', 'Robô autônomo capaz de seguir uma linha pré-definida', 'Roberto Lima, Patricia Souza, Diego Ferreira', 'MECA-2024A', NULL);

-- Informações complementares dos projetos
INSERT INTO InformacoesComplementares (InformacaoComplementar, IdProjeto) VALUES 
('Projeto desenvolvido utilizando React.js e Node.js', 1),
('Banco de dados MySQL com mais de 50 tabelas', 1),
('Interface responsiva e acessível', 1),
('Aplicativo desenvolvido em React Native', 2),
('Integração com sensores IoT', 2),
('Robô construído com Arduino e sensores ultrassônicos', 3),
('Algoritmo de controle PID implementado', 3);

-- Critérios de avaliação para os eventos
INSERT INTO CriterioAvaliacao (IdEvento, NomeCriterio) VALUES 
(1, 'Inovação e Criatividade'),
(1, 'Aplicabilidade Prática'),
(1, 'Qualidade da Apresentação'),
(1, 'Fundamentação Teórica'),
(2, 'Metodologia Científica'),
(2, 'Relevância do Tema'),
(2, 'Qualidade da Escrita'),
(2, 'Apresentação Oral'),
(3, 'Funcionalidade do Software'),
(3, 'Interface do Usuário'),
(3, 'Código Limpo e Documentação'),
(3, 'Trabalho em Equipe');

-- Associar projetos aos eventos
INSERT INTO Evento_Projeto (IdEvento, IdProjeto) VALUES 
(1, 1), -- Sistema de Gestão na Feira de Ciências
(1, 2), -- App Ambiental na Feira de Ciências
(2, 1), -- Sistema de Gestão na Mostra de TCC
(3, 1), -- Sistema de Gestão no Hackathon
(3, 2); -- App Ambiental no Hackathon

-- Avaliações de exemplo
INSERT INTO Avaliacao (IdPessoa, IdProjeto) VALUES 
(2, 1), -- João avalia Sistema de Gestão
(2, 2), -- João avalia App Ambiental
(3, 1), -- Maria avalia Sistema de Gestão
(3, 3); -- Maria avalia Robô Seguidor

-- Notas de exemplo (escala 0-10)
INSERT INTO Nota (IdCriterio, IdAvaliacao, NotaProjeto) VALUES 
-- João avaliando Sistema de Gestão (Avaliação 1)
(1, 1, 8.5), -- Inovação e Criatividade
(2, 1, 9.0), -- Aplicabilidade Prática
(3, 1, 7.5), -- Qualidade da Apresentação
(4, 1, 8.0), -- Fundamentação Teórica

-- João avaliando App Ambiental (Avaliação 2)
(1, 2, 9.0), -- Inovação e Criatividade
(2, 2, 8.5), -- Aplicabilidade Prática
(3, 2, 8.0), -- Qualidade da Apresentação
(4, 2, 7.5), -- Fundamentação Teórica

-- Maria avaliando Sistema de Gestão (Avaliação 3)
(1, 3, 8.0), -- Inovação e Criatividade
(2, 3, 8.5), -- Aplicabilidade Prática
(3, 3, 8.5), -- Qualidade da Apresentação
(4, 3, 8.0), -- Fundamentação Teórica

-- Maria avaliando Robô Seguidor (Avaliação 4)
(1, 4, 7.5), -- Inovação e Criatividade
(2, 4, 8.0), -- Aplicabilidade Prática
(3, 4, 7.0), -- Qualidade da Apresentação
(4, 4, 7.5); -- Fundamentação Teórica
