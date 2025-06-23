# Project Evaluation API

API RESTful para sistema de avaliação de projetos acadêmicos, desenvolvida em ASP.NET Core 8.0 com Entity Framework Core, MySQL e autenticação JWT.

## Funcionalidades

- **Sistema de Autenticação JWT**: Login seguro com tokens
- **Gerenciamento de Usuários**: CRUD completo para pessoas e tipos de usuário
- **Gerenciamento de Eventos**: Criação e administração de eventos acadêmicos
- **Gerenciamento de Projetos**: Cadastro de projetos com informações complementares
- **Sistema de Avaliação**: Avaliação de projetos por critérios específicos
- **Cálculo de Notas**: Cálculo automático de notas finais
- **Documentação Swagger**: Interface interativa para teste da API
- **Docker Support**: Containerização completa da aplicação

## Tecnologias Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- MySQL 8.0
- JWT Authentication
- Swagger/OpenAPI
- Docker & Docker Compose
- phpMyAdmin (para administração do banco)

## Estrutura do Banco de Dados

### Tabelas Principais
- **Pessoa**: Usuários do sistema (avaliadores, professores, etc.)
- **TipoUsuario**: Tipos/papéis de usuário (Admin, Professor, Avaliador, etc.)
- **Evento**: Eventos acadêmicos (feiras, mostras, hackathons)
- **Projeto**: Projetos a serem avaliados
- **InformacoesComplementares**: Informações adicionais dos projetos
- **Avaliacao**: Avaliações realizadas por pessoas em projetos
- **CriterioAvaliacao**: Critérios de avaliação por evento
- **Nota**: Notas atribuídas por critério

### Tabelas de Relacionamento
- **Pessoa_TipoUsuario**: Relacionamento N:N entre pessoas e tipos de usuário
- **Evento_Projeto**: Relacionamento N:N entre eventos e projetos
- **Nota**: Relacionamento entre critérios e avaliações

## Endpoints da API

### Autenticação
- `POST /api/auth/login` - Login do usuário
- `POST /api/auth/register` - Registro de novo usuário

### Pessoas
- `GET /api/pessoas` - Lista todas as pessoas
- `GET /api/pessoas/{id}` - Obtém uma pessoa específica
- `POST /api/pessoas` - Cria uma nova pessoa
- `PUT /api/pessoas/{id}` - Atualiza uma pessoa
- `DELETE /api/pessoas/{id}` - Remove uma pessoa

### Tipos de Usuário
- `GET /api/tiposusuario` - Lista todos os tipos de usuário
- `GET /api/tiposusuario/{id}` - Obtém um tipo específico
- `POST /api/tiposusuario` - Cria um novo tipo
- `PUT /api/tiposusuario/{id}` - Atualiza um tipo
- `DELETE /api/tiposusuario/{id}` - Remove um tipo

### Eventos
- `GET /api/eventos` - Lista todos os eventos
- `GET /api/eventos/{id}` - Obtém um evento específico
- `POST /api/eventos` - Cria um novo evento
- `PUT /api/eventos/{id}` - Atualiza um evento
- `DELETE /api/eventos/{id}` - Remove um evento
- `POST /api/eventos/{eventoId}/projetos/{projetoId}` - Adiciona projeto ao evento
- `DELETE /api/eventos/{eventoId}/projetos/{projetoId}` - Remove projeto do evento

### Projetos
- `GET /api/projetos` - Lista todos os projetos
- `GET /api/projetos/{id}` - Obtém um projeto específico
- `POST /api/projetos` - Cria um novo projeto
- `PUT /api/projetos/{id}` - Atualiza um projeto
- `DELETE /api/projetos/{id}` - Remove um projeto
- `POST /api/projetos/{id}/calcular-nota` - Calcula nota final do projeto

### Avaliações
- `GET /api/avaliacoes` - Lista todas as avaliações
- `GET /api/avaliacoes/{id}` - Obtém uma avaliação específica
- `POST /api/avaliacoes` - Cria uma nova avaliação
- `PUT /api/avaliacoes/{id}` - Atualiza uma avaliação
- `DELETE /api/avaliacoes/{id}` - Remove uma avaliação
- `GET /api/avaliacoes/pessoa/{pessoaId}` - Avaliações por pessoa
- `GET /api/avaliacoes/projeto/{projetoId}` - Avaliações por projeto

## Como Executar

### Pré-requisitos
- Docker
- Docker Compose

### Executando com Docker Compose

1. Clone o repositório
2. Execute o comando:
\`\`\`bash
docker-compose up -d
\`\`\`

### Serviços Disponíveis

- **API**: http://localhost:8080
- **Swagger UI**: http://localhost:8080 (interface de documentação)
- **phpMyAdmin**: http://localhost:8081 (administração do banco)
- **MySQL**: localhost:3306

### Credenciais do Banco

- **Host**: mysql (dentro do container) / localhost (externo)
- **Porta**: 3306
- **Database**: ProjectEvaluationDb
- **Usuário**: root
- **Senha**: rootpassword

### Usuários Padrão

- **Admin**: `admin` / `admin123`
- **Professor**: `joao.silva` / `admin123`
- **Avaliador**: `maria.santos` / `admin123`

## Exemplos de Uso

### 1. Login
\`\`\`json
POST /api/auth/login
{
  "usuario": "admin",
  "senha": "admin123"
}
\`\`\`

### 2. Criando um Evento
\`\`\`json
POST /api/eventos
Authorization: Bearer {token}
{
  "nomeEvento": "Feira de Tecnologia 2024",
  "descricaoEvento": "Apresentação de projetos tecnológicos",
  "local": "Auditório Central",
  "dataInicio": "2024-09-15T08:00:00",
  "dataFim": "2024-09-15T18:00:00",
  "criteriosAvaliacao": [
    "Inovação",
    "Aplicabilidade",
    "Apresentação"
  ]
}
\`\`\`

### 3. Criando um Projeto
\`\`\`json
POST /api/projetos
Authorization: Bearer {token}
{
  "nomeProjeto": "Sistema de Biblioteca Digital",
  "descricaoProjeto": "Sistema web para gerenciamento de biblioteca",
  "integrantes": "João Silva, Maria Santos",
  "turma": "INFO-2024",
  "informacoesComplementares": [
    "Desenvolvido em React e Node.js",
    "Banco de dados PostgreSQL"
  ]
}
\`\`\`

### 4. Criando uma Avaliação
\`\`\`json
POST /api/avaliacoes
Authorization: Bearer {token}
{
  "idPessoa": 2,
  "idProjeto": 1,
  "notas": [
    {
      "idCriterio": 1,
      "notaProjeto": 8.5
    },
    {
      "idCriterio": 2,
      "notaProjeto": 9.0
    }
  ]
}
\`\`\`

## Desenvolvimento

### Estrutura do Projeto
\`\`\`
ProjectEvaluationApi/
├── Controllers/          # Controladores da API
├── Models/              # Modelos de dados
├── DTOs/                # Data Transfer Objects
├── Data/                # Contexto do Entity Framework
├── Services/            # Serviços (Autenticação, etc.)
├── Dockerfile           # Configuração do Docker
├── docker-compose.yml   # Orquestração dos serviços
├── init.sql            # Script de inicialização do banco
└── README.md           # Documentação
\`\`\`

### Comandos Úteis

\`\`\`bash
# Subir os serviços
docker-compose up -d

# Ver logs da API
docker-compose logs -f api

# Parar os serviços
docker-compose down

# Rebuild da API
docker-compose up --build api
\`\`\`

## Fluxo de Trabalho

1. **Configuração Inicial**:
   - Criar tipos de usuário (Admin, Professor, Avaliador)
   - Cadastrar pessoas e associar aos tipos

2. **Criação de Eventos**:
   - Definir evento com critérios de avaliação
   - Associar projetos ao evento

3. **Avaliação**:
   - Avaliadores fazem login
   - Criam avaliações para projetos
   - Atribuem notas por critério

4. **Resultados**:
   - Sistema calcula notas finais automaticamente
   - Relatórios disponíveis via API

## Segurança

- Autenticação JWT obrigatória (exceto login/register)
- Senhas hasheadas com SHA256
- Validação de dados em todas as operações
- Controle de acesso por roles

## Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature
3. Commit suas mudanças
4. Push para a branch
5. Abra um Pull Request

## Licença

Este projeto está sob a licença MIT.
