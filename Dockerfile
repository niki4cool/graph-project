FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src/Server
COPY src/GraphEditor/GraphEditor/GraphEditor.csproj .
RUN dotnet restore
COPY src/GraphEditor/GraphEditor/ .
RUN dotnet publish --no-restore -c Debug -o /app

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as server
WORKDIR /app
COPY --from=build /app .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "GraphEditor.dll"]

FROM node:lts-alpine as node-build
WORKDIR /src/client
COPY src/graph-editor/package.json .
RUN npm install --omit=dev
COPY src/graph-editor/ .
RUN npm run build

FROM server as final
WORKDIR /app
COPY --from=node-build src/client/build ./react-app