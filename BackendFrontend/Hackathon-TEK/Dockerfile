FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY ["Hackathon-TEK/Hackathon-TEK.csproj", "Hackathon-TEK/"]
RUN dotnet restore "Hackathon-TEK/Hackathon-TEK.csproj"

COPY . .
WORKDIR "/src/Hackathon-TEK"
RUN dotnet build "Hackathon-TEK.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Hackathon-TEK.csproj" -c Release -o /app

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

WORKDIR "/app/wwwroot"
RUN npm install

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Hackathon-TEK.dll"]