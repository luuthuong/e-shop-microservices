# E-shop 
> **Building a tech store application using microservices**
****

## Workflow Demo

```mermaid
graph TB
	A[Start]
    A1([NotifyAddOrder])
    B([AddOrder])
    C{OrderResult}
    
    STORE([Store])
    PAYMENT([Payment])
    D([DoEventAsync])
    E([DoEventAsync])
    F([WhenAllEvents])
    G[UpdateOrder]
    G1([NotifyOrderSuccessfully])
    Exception([Catch TaskCanceledException or Exception])
    H[Refund]
    I[End]

    A --> |"Order command"| A1
    A1 --> |"Notification - Order Received"| B
    B --> |"Response"| C
    C --> |"Success"| D -->|"Store Order Updated"| F
    STORE --> |"Sub store order update event"| D
    C --> |"Success"| E -->|"Payment order updated"| F
    PAYMENT --> |"Sub payment order update event"| E
    F --> |"Merge Result"| G --> |"Order Completed"| G1
    C --> |"Compensate"| H
    D --> |"Failed"| Exception 
    E --> |"Failed"| Exception 
    G --> |"Failed"| Exception 
	Exception --> |"Compensate"| H --> I
    G1 --> I
    H --> I
```


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