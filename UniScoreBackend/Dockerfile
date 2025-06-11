# Estágio de build: SDK do .NET 8 para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de projeto (.csproj) e restaura as dependências.
# Como UniScore.API.csproj está diretamente na raiz do contexto (UniScoreBackend/),
# ele é copiado para a raiz do WORKDIR (/src).
COPY ["UniScore.API.csproj", "./"]
RUN dotnet restore "UniScore.API.csproj"

# Copia o restante dos arquivos do projeto da raiz do contexto (UniScoreBackend/)
# para a raiz do WORKDIR atual (/src).
COPY . .

# O WORKDIR já é /src, que é a raiz do seu projeto dentro do contêiner.
# Constrói a aplicação diretamente nesse diretório.
RUN dotnet build "UniScore.API.csproj" -c Release -o /app/build

# Estágio de publicação: Runtime do .NET 8 para executar a aplicação
# Usa uma imagem menor, apenas com o runtime necessário para executar a aplicação.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app

# Copia os arquivos da aplicação construída (que estão em /app/build do estágio 'build')
# para a pasta de trabalho atual (/app) no estágio 'publish'.
COPY --from=build /app/build .

# Define o ponto de entrada da aplicação.
# O nome do arquivo .dll é geralmente o nome do projeto (UniScore.API.dll).
ENTRYPOINT ["dotnet", "UniScore.API.dll"]