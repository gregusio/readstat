# Use the official .NET SDK image for building and running EF Core migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY . ./
RUN dotnet restore

# Build the project
RUN dotnet build -c Release -o out

# Use the official ASP.NET Core runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy the published application and entrypoint script
COPY --from=build /app/out ./

# Expose the port your application will run on
EXPOSE 5027

CMD ["dotnet", "Backend.dll"]