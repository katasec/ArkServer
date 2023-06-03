# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish ArkWorker/Ark.Worker.csproj -c release -o out && \
    wget https://get.pulumi.com/releases/sdk/pulumi-v3.69.0-linux-x64.tar.gz && \
    tar -xzvf pulumi-v3.69.0-linux-x64.tar.gz --directory /tmp/ 

# Stage 2: Copy the application to a new container
FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY --from=build /app/out .
COPY --from=build /tmp/pulumi/pulumi /usr/local/bin/pulumi
ENTRYPOINT ["/app/Ark.Worker"]
