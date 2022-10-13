FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./

RUN dotnet restore "./DTARServer.csproj"

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build-env /app/out .

# NOTE: copy the appropriate env file depending on the DEPLOYMENT target (CLOUD vs. SQUIRE)
COPY ./${PROJECT}/.env.cloud .env
#COPY ./${PROJECT}/.env.squire .env
ENTRYPOINT ["dotnet", "DTARServer.dll"]

# az login
# az acr login --name iobtassets
# docker build -t dtarserver -f Dockerfile  .
# docker tag dtarserver iobtassets.azurecr.io/dtarserver:v5.4.0.0
# docker push iobtassets.azurecr.io/dtarserver:v5.4.0.0
# docker run -d -p 8400:80 -v /iobt:/app/storage --rm --name dtarserver dtarserver
# docker run -it  dtarserver /bin/bash
# docker exec -it dtarserver bash