#!/usr/bin/env bash

URL=https://team9apim.azure-api.net/api/GetProducts/api/GetProducts

[ $APIMKEY ] || echo "Export your API Management key first: 'export APIMKEY=value'" && exit 1

for i in `seq 10`
do
    for i in `seq 10`
    do
        curl -si -o /dev/null -w "%{http_code}\n" $URL \
            -H "Ocp-Apim-Subscription-Key: $APIMKEY" &
    done
done
