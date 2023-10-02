## E-shop 


## scripts:
### add migration:
- Payment service: `dotnet ef migrations add "script name" -p src/PaymentService/Infrastructure -s src/PaymentService/API`
- Order service: `dotnet ef migrations add "script name" -p src/OrderService/Infrastructure -s src/OrderService/API`
- Customer service: `dotnet ef migrations add "script name" -p src/CustomerService/Infrastructure -s src/CustomerService/API`

### update database:
- Payment service: `make update-payment-db`
- Order service: `make update:order-db`
- Customer service: `make update-customer-db`

