# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish ArkWorker/Ark.Worker.csproj -c release -o out

# Stage 2: Copy the application to a new container
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["/app/Ark.Worker"]
