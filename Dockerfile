FROM mcr.microsoft.com/dotnet/sdk:8.0.404 AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 5176

# Ensure we listen on any IP Address 
ENV DOTNET_URLS=http://+:8000

FROM mcr.microsoft.com/dotnet/sdk:8.0.404 AS build
WORKDIR /src
RUN mkdir -p src/Refs
COPY ["Api/GoogleCustomSearchService.Api.Domain/GoogleCustomSearchService.Api.Domain.csproj", "Api/Domain"]
COPY ["Api/GoogleCustomSearchService.Api.WebApplication/GoogleCustomSearchService.Api.WebApplication.csproj", "Api/WebApplication"]
COPY ["Refs/Utilities.dll", "Refs/Utilities"]
COPY . .
WORKDIR "/src"
RUN dotnet restore "Api/GoogleCustomSearchService.Api.WebApplication/GoogleCustomSearchService.Api.WebApplication.csproj"
RUN dotnet build "Api/GoogleCustomSearchService.Api.WebApplication/GoogleCustomSearchService.Api.WebApplication.csproj" -c Release -o /app/build -v diag

FROM build AS publish
RUN dotnet publish "Api/GoogleCustomSearchService.Api.WebApplication/GoogleCustomSearchService.Api.WebApplication.csproj" -c Release -o /app/publish /p:UseAppHost=false -v diag

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "GoogleCustomSearchService.Api.WebApplication.dll" ]
