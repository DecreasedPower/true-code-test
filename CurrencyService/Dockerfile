# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем решение и csproj
COPY CurrencyService.sln ./
COPY CurrencyService/CurrencyService.csproj ./CurrencyService/

# Восстанавливаем зависимости
RUN dotnet restore CurrencyService.sln

# Копируем оставшиеся исходники и публикуем
COPY CurrencyService/ ./CurrencyService/
WORKDIR /src/CurrencyService
RUN dotnet publish -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "CurrencyService.dll"]