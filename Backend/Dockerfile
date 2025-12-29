# Use the official .NET SDK image for building and running EF Core migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the project files and restore dependencies
COPY . ./
RUN dotnet restore

# Build the project
RUN dotnet build -c Release -o out

# Use the official .NET SDK for runtime (needed for EF Core tools)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app

# Copy the published application and source files
COPY --from=build /app/out ./
COPY --from=build /app ./source

# Expose the port your application will run on
EXPOSE 5027

# Create entrypoint script
RUN echo '#!/bin/bash\n\
set -e\n\
echo "Waiting for SQL Server..."\n\
sleep 15\n\
echo "Running migrations..."\n\
cd /app/source\n\
dotnet ef database update --no-build || echo "Migration failed, continuing..."\n\
cd /app\n\
echo "Starting application..."\n\
exec dotnet Backend.dll' > /app/entrypoint.sh && chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]