FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /

# Copy everything
COPY . ./

# Build the solution
RUN dotnet build -c Release ./src

# Publish backend API
FROM build AS publish
RUN dotnet publish ./src/ViMusic.API/ -c Release --no-build -o out/backend-api

# Build backend API image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS backend-api
ENV ASPNETCORE_URLS=http://*:80
EXPOSE 80
WORKDIR /src
COPY --from=publish out/backend-api .
ENTRYPOINT ["dotnet", "ViMusic.API.dll"]