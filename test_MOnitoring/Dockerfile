#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["test_MOnitoring/test_MOnitoring.csproj", "test_MOnitoring/"]
RUN dotnet restore "test_MOnitoring/test_MOnitoring.csproj"
COPY . .
WORKDIR "/src/test_MOnitoring"
RUN dotnet build "test_MOnitoring.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "test_MOnitoring.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "test_MOnitoring.dll"]
