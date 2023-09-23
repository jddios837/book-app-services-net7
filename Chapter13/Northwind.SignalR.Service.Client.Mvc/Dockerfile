FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Chapter13/Northwind.SignalR.Service.Client.Mvc/Northwind.SignalR.Service.Client.Mvc.csproj", "Chapter13/Northwind.SignalR.Service.Client.Mvc/"]
RUN dotnet restore "Chapter13/Northwind.SignalR.Service.Client.Mvc/Northwind.SignalR.Service.Client.Mvc.csproj"
COPY . .
WORKDIR "/src/Chapter13/Northwind.SignalR.Service.Client.Mvc"
RUN dotnet build "Northwind.SignalR.Service.Client.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Northwind.SignalR.Service.Client.Mvc.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Northwind.SignalR.Service.Client.Mvc.dll"]
