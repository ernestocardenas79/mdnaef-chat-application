#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 3005
EXPOSE 3006

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
#COPY ["unoschat.client/nuget.config", "unoschat.client/"]
COPY ["UNOSChat.ConversationAPI/UNOSChat.ConversationAPI.csproj", "UNOSChat.ConversationAPI/"]
#COPY ["unoschat.client/unoschat.client.esproj", "unoschat.client/"]
RUN dotnet restore "./UNOSChat.ConversationAPI/UNOSChat.ConversationAPI.csproj"
COPY . .
WORKDIR "/src/UNOSChat.ConversationAPI"
RUN dotnet build "./UNOSChat.ConversationAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UNOSChat.ConversationAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UNOSChat.ConversationAPI.dll"]