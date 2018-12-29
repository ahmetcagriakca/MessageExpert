#Publish sdk version
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

#Build sdk version
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
#Project restore
RUN dotnet restore MessageExpert.Api/MessageExpert.Api.csproj
WORKDIR /src/MessageExpert.Api
#Build before release
RUN dotnet build --no-restore -c Release -o /app

WORKDIR /src/MessageExpert.Api
FROM build AS publish
RUN dotnet publish --no-restore -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MessageExpert.Api.dll"]
