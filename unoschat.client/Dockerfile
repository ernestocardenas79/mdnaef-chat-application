# Use an official Node.js runtime as a parent image
FROM node:18.20.0-buster AS build

# Set the working directory
WORKDIR /app

# Copy the package.json and package-lock.json
COPY unoschat.client/package*.json ./

# Install dependencies
RUN npm install --force

COPY ["unoschat.client/", "./"]

# Build the Angular application
RUN npx ng build

# Use an Nginx image to serve the Angular application
FROM nginx:stable-alpine

# Copy the built Angular files from the previous stage to the Nginx public directory
COPY --from=build /app/dist/unoschat.client/browser /usr/share/nginx/html

# Expose the port Nginx will use
EXPOSE 80

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]

