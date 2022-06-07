cd..
sqlcmd -i "./scripts/DropAll.sql"
dotnet ef migrations remove
dotnet ef migrations add Init
dotnet ef database update
pause