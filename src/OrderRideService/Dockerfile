FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OrderRideService/OrderRideService.csproj", "OrderRideService/"]
RUN dotnet restore "OrderRideService/OrderRideService.csproj"
COPY . .
WORKDIR "/src/OrderRideService"
RUN dotnet build "OrderRideService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderRideService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderRideService.dll"]
