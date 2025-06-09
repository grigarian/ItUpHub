FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["GrowSphere.Web/GrowSphere.Web.csproj", "GrowSphere.Web/"]
COPY ["GrowSphere.Application/GrowSphere.Application.csproj", "GrowSphere.Application/"]
COPY ["GrowSphere.Core/GrowSphere.Domain.csproj", "GrowSphere.Core/"]

RUN dotnet restore "./GrowSphere.Web/GrowSphere.Web.csproj"
COPY . .
WORKDIR "/src/GrowSphere.Web"
RUN dotnet build "./GrowSphere.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GrowSphere.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrowSphere.Web.dll"]