FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore src/EVM.ProjectManagement.API/EVM.ProjectManagement.API.csproj
RUN dotnet publish src/EVM.ProjectManagement.API/EVM.ProjectManagement.API.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EVM.ProjectManagement.API.dll"]
