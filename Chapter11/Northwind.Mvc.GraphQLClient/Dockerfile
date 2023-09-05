FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Chapter11/Northwind.Mvc.GraphQLClient/Northwind.Mvc.GraphQLClient.csproj", "Chapter11/Northwind.Mvc.GraphQLClient/"]
RUN dotnet restore "Chapter11/Northwind.Mvc.GraphQLClient/Northwind.Mvc.GraphQLClient.csproj"
COPY . .
WORKDIR "/src/Chapter11/Northwind.Mvc.GraphQLClient"
RUN dotnet build "Northwind.Mvc.GraphQLClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Northwind.Mvc.GraphQLClient.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Northwind.Mvc.GraphQLClient.dll"]
