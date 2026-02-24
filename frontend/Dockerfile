# Use an official Node.js runtime as a parent image
FROM node:22-alpine

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Accept API URL as a build argument (set via docker-compose args or CI/CD)
ARG VITE_API_BASE_URL=http://localhost:5027/api
ENV VITE_API_BASE_URL=${VITE_API_BASE_URL}

# Build the application
RUN npm run build

# Expose port 3000
EXPOSE 3000

# Start the web server
CMD ["npm", "run", "preview", "--", "--host", "0.0.0.0", "--port", "3000"]
