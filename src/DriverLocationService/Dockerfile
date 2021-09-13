FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DriverLocationService/DriverLocationService.csproj", "DriverLocationService/"]
RUN dotnet restore "DriverLocationService/DriverLocationService.csproj"
COPY . .
WORKDIR "/src/DriverLocationService"
RUN dotnet build "DriverLocationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DriverLocationService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DriverLocationService.dll"]
