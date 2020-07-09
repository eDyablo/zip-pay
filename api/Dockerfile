FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-image

WORKDIR /application

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish --configuration Release --output artifacts --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

WORKDIR /application
COPY --from=build-image /application/artifacts .

EXPOSE 80

ENTRYPOINT [ "dotnet", "api.dll" ]
