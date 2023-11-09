# Use the official .NET SDK as a parent image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the .csproj and restore dependencies
COPY *.sln .
COPY TavisApi/*.csproj ./TavisApi/
COPY TavisApi.Tests/*.csproj ./TavisApi.Tests/
RUN dotnet restore

# Copy the remaining source code
COPY . .

#build the applicaton
RUN dotnet build -c Release -o /app/build

#publish the application
RUN dotnet publish -c Release -o /app/publish

# build the runtime image
FROM  mcr.microsoft.com/dotnet/aspnet:6.0 As runtime
WORKDIR /app

# copy the published application from the build image
COPY --from=build /app/publish .

# Set the ASP.NET Core environment to Development
ENV ASPNETCORE_ENVIRONMENT Development

# Expose port 80 for the application
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "TavisApi.dll"]
