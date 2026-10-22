# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY CardPlatformDemo.csproj ./
RUN dotnet restore CardPlatformDemo.csproj
COPY . .
RUN dotnet publish CardPlatformDemo.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Render provides the PORT env var; Program.cs binds to it (defaults to 8080).
ENV PORT=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "CardPlatformDemo.dll"]
