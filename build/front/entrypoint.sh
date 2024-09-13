#!/bin/sh

# Replace variables in nginx.conf
envsubst '$DOMAIN_NAME' < /etc/nginx/nginx.conf.template > /etc/nginx/nginx.conf
mkdir /etc/letsencrypt/live/$DOMAIN_NAME
mv /etc/letsencrypt/live/domain/* /etc/letsencrypt/live/$DOMAIN_NAME

# Start Nginx and Certbot
nginx
certbot --nginx -d $DOMAIN_NAME --non-interactive --agree-tos -m $EMAIL
nginx -s reload

# Export ASPNETCORE_URLS environment variable
export ASPNETCORE_URLS=http://localhost:8080

# Start the ASP.NET application
dotnet DogFriendly.Web.dll