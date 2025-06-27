# Используем официальный SDK-образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080

# Копируем решение и проекты
COPY . ./

# Восстанавливаем зависимости
RUN dotnet restore ./FinanceService/FinanceService.csproj

# Собираем проект
RUN dotnet publish ./FinanceService/FinanceService.csproj -c Release -o /app/publish

# Используем runtime-образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем опубликованные файлы из стадии сборки
COPY --from=build /app/publish .

# Задаем точку входа
ENTRYPOINT ["dotnet", "FinanceService.dll"]