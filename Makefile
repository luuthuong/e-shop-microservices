build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch --project Domain/
start:
	cd Domain/ && dotnet run
