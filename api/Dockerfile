FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-image

WORKDIR /application

COPY main/api.csproj main/
RUN dotnet restore main/api.csproj

COPY test/api-test.csproj test/
RUN dotnet restore test/api-test.csproj

COPY main main
RUN dotnet build main/api.csproj --no-restore

COPY test test
RUN dotnet test test/api-test.csproj --no-restore

RUN dotnet publish main/api.csproj --configuration Release --output artifacts --no-restore 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /application
COPY --from=build-image /application/artifacts .

EXPOSE 80

ENTRYPOINT [ "dotnet", "api.dll" ]
