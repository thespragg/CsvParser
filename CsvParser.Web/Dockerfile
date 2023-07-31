FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CsvParser.Web/CsvParser.Web.csproj", "CsvParser.Web/"]
RUN dotnet restore "CsvParser.Web/CsvParser.Web.csproj"
COPY . .
WORKDIR "/src/CsvParser.Web"
RUN dotnet build "CsvParser.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CsvParser.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CsvParser.Web.dll"]
