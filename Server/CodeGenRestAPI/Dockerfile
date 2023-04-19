#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=development

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CodeGenRestAPI/CodeGenRestAPI.csproj", "CodeGenRestAPI/"]
COPY ["CodeGenEngine/CodeGenEngine.csproj", "CodeGenEngine/"]
COPY ["Converts/Converts.csproj", "Converts/"]
COPY ["ExtModel/ExtModel.csproj", "ExtModel/"]
COPY ["CodeGenRestAPI/Configuration/Languages/", "CodeGenRestAPI/Configuration/Languages/"]
RUN dotnet restore "CodeGenRestAPI/CodeGenRestAPI.csproj"
COPY . .
WORKDIR "/src/CodeGenRestAPI"
RUN dotnet build "CodeGenRestAPI.csproj" -c Development -o /app/build

FROM build AS publish
RUN dotnet publish "CodeGenRestAPI.csproj" -c Development -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeGenRestAPI.dll"]