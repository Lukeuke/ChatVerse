FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/File/src/File.Api/File.Api.csproj", "File.Api/"]
RUN dotnet restore "src/Services/File/src/File.Api/File.Api.csproj"
COPY . .
WORKDIR "/src/File.Api"
RUN dotnet build "File.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "File.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "File.Api.dll"]
