# Estágio de build: SDK do .NET 8 para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de projeto (.csproj) e restaura as dependências.
# O CONTEXTO AGORA É A PASTA ONDE O DOCKERFILE ESTÁ (~/Projeto/UniScoreBackend).
# O arquivo .csproj está dentro da subpasta UniScoreBackend/
COPY ["UniScoreBackend/UniScore.API.csproj", "UniScoreBackend/"]
RUN dotnet restore "UniScoreBackend/UniScore.API.csproj"

# Copia o restante dos arquivos do projeto.
# ATENÇÃO: Se o seu código-fonte estiver DENTRO de UniScoreBackend/, você pode precisar ajustar.
# O '.' aqui copia tudo do CONTEXTO (que é ~/Projeto/UniScoreBackend) para o WORKDIR (/src)
COPY . .

# Vai para o diretório do projeto onde o build será executado.
# Como o .csproj está em UniScoreBackend/, o WORKDIR deve ser /src/UniScoreBackend.
WORKDIR "/src/UniScoreBackend"
RUN dotnet build "UniScore.API.csproj" -c Release -o /app/build

# Estágio de publicação: Runtime do .NET 8 para executar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app

# Copia os arquivos da aplicação construída (que estão em /app/build do estágio 'build')
# para a pasta de trabalho atual (/app) no estágio 'publish'.
COPY --from=build /app/build .

# Define o ponto de entrada da aplicação.
ENTRYPOINT ["dotnet", "UniScore.API.dll"]