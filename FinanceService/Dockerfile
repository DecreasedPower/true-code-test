# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем решение и csproj
COPY FinanceService.sln ./
COPY FinanceService/FinanceService.csproj ./FinanceService/

# Восстанавливаем зависимости
RUN dotnet restore FinanceService.sln

# Копируем оставшиеся исходники и публикуем
COPY FinanceService/ ./FinanceService/
WORKDIR /src/FinanceService
RUN dotnet publish -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "FinanceService.dll"]