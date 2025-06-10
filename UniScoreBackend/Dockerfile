# Estágio de build: SDK do .NET 8 para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia o arquivo de projeto e restaura as dependências
COPY ["UniScore.API/UniScore.API.csproj", "UniScore.API/"]
RUN dotnet restore "UniScore.API/UniScore.API.csproj"

# Copia o restante dos arquivos do projeto
COPY . .

# Vai para o diretório do projeto e constrói a aplicação
WORKDIR "/src/UniScore.API"
RUN dotnet build "UniScore.API.csproj" -c Release -o /app/build

# Estágio de publicação: Runtime do .NET 8 para executar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS publish
WORKDIR /app

# Copia os arquivos da aplicação construída
COPY --from=build /app/build .

# Define o ponto de entrada da aplicação
# Use o nome do arquivo .dll gerado (geralmente o nome do projeto .dll)
ENTRYPOINT ["dotnet", "UniScore.API.dll"]