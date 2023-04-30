FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

RUN mkdir Client
RUN mkdir Server
RUN mkdir Shared

# Copy csproj and restore as distinct layers
WORKDIR /app/Shared
RUN echo "Copying Shared Project File"
COPY Shared/*.csproj .
WORKDIR /app/Client
RUN echo "Copying client Project File"
COPY Client/*.csproj .
WORKDIR /app/Server
RUN echo "Copying Server Project File"
COPY Server/*.csproj .
RUN dotnet restore

# Copy everything else and build
RUN echo "Copying all files"
WORKDIR /app
COPY . ./
WORKDIR /app/Server
RUN echo "Beginning build"
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine-amd64

WORKDIR /app
COPY --from=build-env /app/Server/out .
ENTRYPOINT ["dotnet", "d20web.Server.dll"]