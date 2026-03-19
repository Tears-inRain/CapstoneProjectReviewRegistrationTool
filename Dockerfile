FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["CapstoneReview.API/CapstoneReview.API.csproj", "CapstoneReview.API/"]
COPY ["CapstoneReview.Service/CapstoneReview.Service.csproj", "CapstoneReview.Service/"]
COPY ["CapstoneReview.Repository/CapstoneReview.Repository.csproj", "CapstoneReview.Repository/"]
RUN dotnet restore "CapstoneReview.API/CapstoneReview.API.csproj"
COPY . .
WORKDIR "/src/CapstoneReview.API"
RUN dotnet build "CapstoneReview.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CapstoneReview.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY init_db_mock.sql /app/
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "CapstoneReview.API.dll"]
