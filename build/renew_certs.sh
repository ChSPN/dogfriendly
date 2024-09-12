#!/bin/sh
certbot renew --quiet --no-self-upgrade
nginx -s reload