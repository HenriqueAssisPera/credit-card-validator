FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/CreditCardValidator/CreditCardValidator.csproj", "src/CreditCardValidator/"]
RUN dotnet restore "src/CreditCardValidator/CreditCardValidator.csproj"
COPY . .
WORKDIR "/src/src/CreditCardValidator"
RUN dotnet build "CreditCardValidator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CreditCardValidator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CreditCardValidator.dll"]