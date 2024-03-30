<h1 align="center">ESHOP AspNetCore WebAPI</h1>

> This is an application was built using essential features as a store, organizing by microservices
---
## ABOUT THE PROJECT
This project was built as a learning to understand the concept of the **Clean Architecture** together with 
Entity FrameworkCore as ORM and MediatR as CQRS library. The intend purpose was to build a simple store application,
where you can store and manage the products as an administrator, and able to order, online payment as the client.
<br/>

The application be splitting by three services, such as:
- **Product sync service**:  Managing the product items
- **Order service**: Managing orders
- **Payment service**: Managing payments
### Workflow
> *Description*
>
> 
> 

![image](https://github.com/luuthuong/e-shop-microservices/assets/86012214/accb66fd-a595-4092-a286-ab5df3316dca)

### Functions cover
- [x] AspNetCore 8 WebAPI (using **Minimal API**)
- [x] SwaggerUI 
- [x] API Versioning
- [x] Onion architecture
- [ ] Authentication
- [x] Testing
- [x] Caching
- [x] Logging
- [x] Performance
- [x] Mapping (AutoMapper)
- [x] Background Jobs
- [x] Exception Handling
- [x] Code First Approach
- [x] Dependency injection
- [x] CRQS with MediatR library
- [x] MediatR pipelines for: Caching, Validation, Logging
- [x] Repository pattern
- [x] Option pattern
- [x] Dockerized
- [ ] CI/CD
- [ ] Kubernetes

## How to run the project
**With Docker:**
This will create a container with the application and a container with a SQL Server database.
Make sure that you have Docker installed on your machine & running.
1. Clone the repository
2. In root directory run `docker compose up`

**Without Docker:**
1. Clone the repository
2. In root directory run `dotnet restore`
3. In root directory run `dotnet build`
4. In root directory run
   - Product service 
   ```shell
    dotnet run --project .\src\ProductSyncService\ProductSyncService.API
   ```
    - Order service
   ```shell
    dotnet run --project .\src\OrderService\OrderService.API
   ```
    - Payment service
   ```shell
    dotnet run --project .\src\PaymentService\PaymentService.API
   ```