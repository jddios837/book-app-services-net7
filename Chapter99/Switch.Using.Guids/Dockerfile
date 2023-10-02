FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Chapter99/Switch.Using.Guids/Switch.Using.Guids.csproj", "Chapter99/Switch.Using.Guids/"]
RUN dotnet restore "Chapter99/Switch.Using.Guids/Switch.Using.Guids.csproj"
COPY . .
WORKDIR "/src/Chapter99/Switch.Using.Guids"
RUN dotnet build "Switch.Using.Guids.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Switch.Using.Guids.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Switch.Using.Guids.dll"]
