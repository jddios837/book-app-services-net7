FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Chapter12/Northwind.Grpc.Client.Mvc/Northwind.Grpc.Client.Mvc.csproj", "Chapter12/Northwind.Grpc.Client.Mvc/"]
RUN dotnet restore "Chapter12/Northwind.Grpc.Client.Mvc/Northwind.Grpc.Client.Mvc.csproj"
COPY . .
WORKDIR "/src/Chapter12/Northwind.Grpc.Client.Mvc"
RUN dotnet build "Northwind.Grpc.Client.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Northwind.Grpc.Client.Mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Northwind.Grpc.Client.Mvc.dll"]
