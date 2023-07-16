FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/Group/src/Group.Api/Group.Api.csproj", "Group.Api/"]
RUN dotnet restore "src/Services/Group/src/Group.Api/Group.Api.csproj"
COPY . .
WORKDIR "/src/Group.Api"
RUN dotnet build "Group.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Group.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Group.Api.dll"]
