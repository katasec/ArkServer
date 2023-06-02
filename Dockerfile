#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish --no-self-contained -c Release -o out -r linux-x64

# Build runtime image
FROM mcr.microsoft.com/dotnet/nightly/aspnet:7.0-jammy-chiseled
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["/App/Ark.Server"]