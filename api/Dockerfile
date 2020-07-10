FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-image

WORKDIR /app

COPY main/api.csproj main/
RUN dotnet restore main/api.csproj

COPY test/api-test.csproj test/
RUN dotnet restore test/api-test.csproj

COPY main main
RUN dotnet build main/api.csproj

COPY test test
RUN dotnet test test/api-test.csproj

RUN dotnet publish main/api.csproj --configuration Release --output artifacts --no-restore 

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /app

COPY --from=build-image /app/artifacts .

EXPOSE 80

ENTRYPOINT [ "dotnet", "api.dll" ]
