FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SocialApp.Media/SocialApp.Media.csproj", "SocialApp.Media/"]
RUN dotnet restore "SocialApp.Media/SocialApp.Media.csproj"
COPY . .
WORKDIR "/src/SocialApp.Media"
RUN dotnet build "SocialApp.Media.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SocialApp.Media.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

VOLUME ["/app/images"]

ENTRYPOINT ["dotnet", "SocialApp.Media.dll"]
