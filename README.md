# StorefrontSAM
This is the serverless application model for my [MVVM Storefront Project](https://github.com/basicn86/StorefrontProject).

## Technologies
There's about three AWS technologies used in this project: API Gateway, AWS Lambda, and AWS DynamoDB. The API Gateway allows access from the internet to my application, with AWS Lambda performing the necessary compute functions, and DynamoDB storing business data. The entire application runs serverless.

## Methods

### api/products
- GET: Running GET will return a list of products
- POST: Running POST with no body will delete all the existing products and fill up DynamoDB with new products

### api/orders
- GET: Running GET will return a list of orders
- POST: Running POST will place an order
- PUT: Running PUT will update an existing order
- DELETE: Running DELETE will delete an order
