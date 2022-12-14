FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Com.Sanduk.Api/Com.Sanduk.Api.csproj", "Com.Sanduk.Api/"]
RUN dotnet restore "Com.Sanduk.Api/Com.Sanduk.Api.csproj"
COPY . .
WORKDIR "/src/Com.Sanduk.Api"
RUN dotnet build "Com.Sanduk.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Com.Sanduk.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Com.Sanduk.Api.dll"]
