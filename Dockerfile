FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG PROJECT=WebApi
ARG CONFIG=Release
WORKDIR /workspace

COPY ["src/DataAccess/DataAccess.csproj", "src/DataAccess/"]
COPY ["src/Models/Models.csproj", "src/Models/"]
COPY ["src/Utility/Utility.csproj", "src/Utility/"]
COPY ["src/WebApi/WebApi.csproj", "src/WebApi/"]

COPY ["DanCartBack.sln", "./"]
RUN dotnet restore "DanCartBack.sln"

COPY src/ src/

RUN dotnet publish "src/${PROJECT}/${PROJECT}.csproj" -c "${CONFIG}" -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
ENV ASPNETCORE_URLS=http://+:80 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

WORKDIR /app
COPY --from=build /app/publish/ .
EXPOSE 80
ENTRYPOINT ["dotnet", "WebApi.dll"]