#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Phonebook.Contact.API/Phonebook.Contact.API.csproj", "Phonebook.Contact.API/"]
COPY ["Phonebook.Contact.Infrastracture/Phonebook.Contact.Infrastracture.csproj", "Phonebook.Contact.Infrastracture/"]
COPY ["Phonebook.Contact.Domain/Phonebook.Contact.Domain.csproj", "Phonebook.Contact.Domain/"]
COPY ["Phonebook.Shared/Phonebook.Shared.csproj", "Phonebook.Shared/"]
RUN dotnet restore "Phonebook.Contact.API/Phonebook.Contact.API.csproj"
COPY . .
WORKDIR "/src/Phonebook.Contact.API"
RUN dotnet build "Phonebook.Contact.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Phonebook.Contact.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Phonebook.Contact.API.dll"]