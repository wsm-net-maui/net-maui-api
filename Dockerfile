FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["net-maui-api.csproj", "./"]
COPY ["Wsm.Aplication/Wsm.Aplication.csproj", "Wsm.Aplication/"]
COPY ["Wsm.Domain/Wsm.Domain.csproj", "Wsm.Domain/"]
COPY ["Wsm.Infra.Core/Wsm.Infra.Core.csproj", "Wsm.Infra.Core/"]
COPY ["Wsm.Infra.Estrutura/Wsm.Infra.Estrutura.csproj", "Wsm.Infra.Estrutura/"]

RUN dotnet restore "net-maui-api.csproj"

COPY . .
RUN dotnet publish "net-maui-api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "net-maui-api.dll"]
