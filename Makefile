build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
watch-orderService:
	dotnet watch --project ./src/OrderService/API

watch-customerService:
	dotnet watch --project ./src/CustomerService/API

watch-paymentService:
	dotnet watch --project ./src/PaymentService/API

start-orderService:
	dotnet run --project ./src/OrderService/API

start-customerService:
	dotnet run --project ./src/CustomerService/API

start-paymentService:
	dotnet run --project ./src/PaymentService/API

update-customer-db:
	dotnet ef database update --project ./Infrastructure -s API/
