#!/usr/bin/env bash

func_base=team9prodfunc.azurewebsites.net

body=$(cat  << EOF
  {
      "userId": "6dd3bb49-a5be-41ca-9dac-3b995450f2db",
      "productId": "4c25613a-a3c2-4ef3-8e02-9c335eb23204",
      "locationName": "Sample ice cream shop",
      "rating": 5,
      "userNotes": "Ice cream is good for you!"
  }
EOF
)

curl -o /dev/null -is https://$func_base/api/CreateRating \
    -w "%{http_code}\n" \
    -H "Content-Type: application/json" \
    -H "x-functions-key: $FUNCTIONKEY" \
    --data "$body"


curl -o /dev/null -is https://$func_base/api/getRating/6dd3bb49-a5be-41ca-9dac-3b995450f2db/ac73b154-7c69-4989-a6a2-7f9a275805c7 \
    -w "%{http_code}\n" \
    -H "x-functions-key: $FUNCTIONKEY"

curl -o /dev/null -is https://$func_base/api/getratings/6dd3bb49-a5be-41ca-9dac-3b995450f2db \
    -w "%{http_code}\n" \
    -H "x-functions-key: $FUNCTIONKEY"
