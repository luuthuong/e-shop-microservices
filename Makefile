build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
watch:
	dotnet watch --project API/
start:
	dotnet run --project API/
update-db:
	dotnet ef database update --project ./Infrastructure -s API/
