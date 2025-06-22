# Используем официальный SDK-образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем решение и проекты
COPY . ./

# Восстанавливаем зависимости
RUN dotnet restore ./UserService/UserService.csproj

# Собираем проект
RUN dotnet publish ./UserService/UserService.csproj -c Release -o /app/publish

# Используем runtime-образ для запуска
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Копируем опубликованные файлы из стадии сборки
COPY --from=build /app/publish .

# Задаем точку входа
ENTRYPOINT ["dotnet", "UserService.dll"]