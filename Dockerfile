
# Utiliza a imagem do .NET 8 para construir a aplicacao
FROM mcr.microsoft.com/dotnet/sdk:8.0

# Define a pasta de trabalho dentro da imagem
WORKDIR /uraniaquest

# Copia os arquivos do projeto para a imagem
COPY app/ .

# Compila e prepara a aplicacao para execucao
RUN dotnet publish UraniaQuest.Solucoes.csproj -c Release -o /publicacao

# Define a pasta onde a aplicacao foi publicada
WORKDIR /publicacao

# Executa o UraniaQuest quando o container iniciar
ENTRYPOINT ["dotnet", "UraniaQuest.Solucoes.dll"]
