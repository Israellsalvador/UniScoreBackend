# 🚀 Guia de Instalação - UniScore Backend

## ✅ Pré-requisitos

1. **.NET 8 SDK** - [Download aqui](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **MySQL Server 8.0+** - [Download aqui](https://dev.mysql.com/downloads/mysql/)
3. **Visual Studio Code** ou **Visual Studio 2022**

## 📁 Estrutura dos Arquivos

Após baixar, você terá esta estrutura:
\`\`\`
UniScore.API/
├── Controllers/
├── Data/
├── DTOs/
├── Models/
├── Repositories/
├── Services/
├── Program.cs
├── UniScore.API.csproj
├── appsettings.json
└── appsettings.Development.json
\`\`\`

## 🔧 Passo a Passo

### 1. Configurar o Banco de Dados

**1.1. Criar o banco no MySQL:**
```sql
CREATE DATABASE uniscore CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;