#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Services/Project/Project.Api/Project.Api.csproj", "Services/Project/Project.Api/"]
RUN dotnet restore "Services/Project/Project.Api/Project.Api.csproj"
COPY . .
WORKDIR "/src/Services/Project/Project.Api"
RUN dotnet build "Project.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Project.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Project.Api.dll"]