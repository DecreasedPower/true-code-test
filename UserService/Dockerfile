# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем решение и csproj
COPY UserService.sln ./
COPY UserService/UserService.csproj ./UserService/

# Восстанавливаем зависимости
RUN dotnet restore UserService.sln

# Копируем оставшиеся исходники и публикуем
COPY UserService/ ./UserService/
WORKDIR /src/UserService
RUN dotnet publish -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "UserService.dll"]