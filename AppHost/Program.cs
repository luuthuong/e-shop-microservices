using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<ApiGateway_Ocelot>("api-gateway");
builder.AddProject<CustomerService_API>("customer-service");
builder.AddProject<OrderService_API>("order-service");
builder.AddProject<PaymentService_API>("payment-service");
builder.AddProject<ProductSyncService_API>("inventory-service");

builder.Build().Run();