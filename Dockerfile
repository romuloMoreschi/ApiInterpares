# NuGet restore
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY *.sln .
COPY ApiInterpares/*.csproj ApiInterpares/
RUN dotnet restore
COPY . .

# testing
FROM build AS testing
WORKDIR /src/ApiInterpares
RUN dotnet build

# publish
FROM build AS publish
WORKDIR /src/ApiInterpares
RUN dotnet publish -c Release -o /src/publish

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS runtime
WORKDIR /app
COPY --from=publish /src/publish .
# ENTRYPOINT [ "dotnet" ,  "ApiInterpares.dll" ]
# heroku uses the following
CMD ASPNETCORE_URLS=http://*:$PORT dotnet ApiInterpares.dll