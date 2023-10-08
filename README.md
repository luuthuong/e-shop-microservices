# E-shop 
> **Building a tech store application using microservices**
****

## Workflow Demo

![image](https://github.com/luuthuong/e-shop-microservices/assets/86012214/f8ff06f4-ca9c-4dec-81bd-8fc61d718a2f)

## Scripts:

#### Add migration
```shell
dotnet ef migrations add "script name" -p src/PaymentService/Infrastructure -s src/PaymentService/API
dotnet ef migrations add "script name" -p src/OrderService/Infrastructure -s src/OrderService/API
dotnet ef migrations add "script name" -p src/CustomerService/Infrastructure -s src/CustomerService/API
```

#### Update database:
```shell
make update-payment-db
make update:order-db
make update-customer-db
```

## Run

#### Docker.
```shell
docker compose up --build
```
