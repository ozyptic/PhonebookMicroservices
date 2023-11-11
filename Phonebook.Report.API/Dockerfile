#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Phonebook.Report.API/Phonebook.Report.API.csproj", "Phonebook.Report.API/"]
COPY ["EventBus.Base/EventBus.Base.csproj", "EventBus.Base/"]
COPY ["EventBus.Factory/EventBus.Factory.csproj", "EventBus.Factory/"]
COPY ["EventBus.RabbitMQ/EventBus.RabbitMQ.csproj", "EventBus.RabbitMQ/"]
COPY ["Phonebook.Report.Infrastructure/Phonebook.Report.Infrastructure.csproj", "Phonebook.Report.Infrastructure/"]
COPY ["Phonebook.Report.Domain/Phonebook.Report.Domain.csproj", "Phonebook.Report.Domain/"]
COPY ["Phonebook.Shared/Phonebook.Shared.csproj", "Phonebook.Shared/"]
RUN dotnet restore "Phonebook.Report.API/Phonebook.Report.API.csproj"
COPY . .
WORKDIR "/src/Phonebook.Report.API"
RUN dotnet build "Phonebook.Report.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Phonebook.Report.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Phonebook.Report.API.dll"]