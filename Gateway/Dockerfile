FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Gateway.sln ./
COPY Gateway/Gateway.csproj ./Gateway/

RUN dotnet restore Gateway.sln

COPY Gateway/ ./Gateway/
WORKDIR Gateway
RUN dotnet publish -c Release -o /app/publish

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./
ENTRYPOINT ["dotnet", "Gateway.dll"]