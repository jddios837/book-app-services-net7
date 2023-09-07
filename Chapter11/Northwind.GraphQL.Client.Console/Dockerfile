FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Chapter11/Northwind.GraphQL.Client.Console/Northwind.GraphQL.Client.Console.csproj", "Chapter11/Northwind.GraphQL.Client.Console/"]
RUN dotnet restore "Chapter11/Northwind.GraphQL.Client.Console/Northwind.GraphQL.Client.Console.csproj"
COPY . .
WORKDIR "/src/Chapter11/Northwind.GraphQL.Client.Console"
RUN dotnet build "Northwind.GraphQL.Client.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Northwind.GraphQL.Client.Console.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Northwind.GraphQL.Client.Console.dll"]
