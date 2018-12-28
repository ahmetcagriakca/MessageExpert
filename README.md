# MessageExpert
Messaging application 

## Appsettings 
Database configuration managed on DatabaseConfig, you can choose database provider like postgre or mssql

 "DatabaseConfig": {
    "Provider": "Postgre",
    "ConnectionString": "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=MessageExpert.Services.ApiDb;Pooling=true;"
  },

update database command
dotnet ef database update --project src/Services/API/MessageExpert.Data/MessageExpert.Data.csproj --startup-project src/Services/API/MessageExpert.Api/MessageExpert.Api.csproj
