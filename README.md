# E-shop 
> **Building a tech store application using microservices**
****

# Function cover fundamental
## API Level
- Authentication
- Testing
- Caching
- Logging
- Performance
- Monitoring
- Mapping
- Background Jobs
- Exception Handling
- Code First Approach
- Dependency injection

## Library for
- Logging
- Caching
- Mappings
- Monitoring
- Unit testing
- Background jobs
- Realtime communication

## Advanced
- Dockerized
- Kubernetes
- CI/CD
- Microservices
- Cloud services
- Design patterns
- Design principle
- Clean architecture

# Workflow Demo
> _Description_:
> 
> ~~todo~~


![image](https://github.com/luuthuong/e-shop-microservices/assets/86012214/accb66fd-a595-4092-a286-ab5df3316dca)

# Scripts:

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
