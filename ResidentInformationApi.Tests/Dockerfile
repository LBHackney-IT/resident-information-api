FROM mcr.microsoft.com/dotnet/core/sdk:3.1

# disable microsoft telematry
ENV DOTNET_CLI_TELEMETRY_OPTOUT='true'

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./ResidentInformationApi.sln ./
COPY ./ResidentInformationApi/ResidentInformationApi.csproj ./ResidentInformationApi/
COPY ./ResidentInformationApi.Tests/ResidentInformationApi.Tests.csproj ./ResidentInformationApi.Tests/

RUN dotnet restore ./ResidentInformationApi/ResidentInformationApi.csproj
RUN dotnet restore ./ResidentInformationApi.Tests/ResidentInformationApi.Tests.csproj

# Copy everything else and build
COPY . .

RUN dotnet build -c debug -o out ResidentInformationApi.Tests/ResidentInformationApi.Tests.csproj

CMD dotnet test
