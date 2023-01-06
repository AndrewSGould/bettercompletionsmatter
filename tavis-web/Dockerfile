### STAGE 1: Build ###
FROM node:18.12.1 AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build

### STAGE 2: Run ###
# nginx state for serving content
FROM nginx:alpine
COPY nginx.conf /etc/nginx/conf.d/default.conf
# Set working directory to nginx asset directory
WORKDIR /usr/share/nginx/html
# Remove default nginx static assets
RUN rm -rf ./*
# Copy static assets from builder stage
COPY --from=build /app/dist/tavis .

EXPOSE 8080
# Containers run nginx with global directives and daemon off
ENTRYPOINT [ "nginx" ]
CMD [ "-g", "daemon off;" ]