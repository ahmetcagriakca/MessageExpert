# MessageExpert
Messaging application 

## Appsettings 
Database configuration managed on DatabaseConfig, you can choose database provider like postgre or mssql

 "DatabaseConfig": {
    "Provider": "Postgre",
    "ConnectionString": "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=MessageExpert.Services.ApiDb;Pooling=true;"
  },

#### update database command

dotnet ef database update --project src/Services/API/MessageExpert.Data/MessageExpert.Data.csproj --startup-project src/Services/API/MessageExpert.Api/MessageExpert.Api.csproj

### add migrations

dotnet ef migrations add [migration_name] --project src/Services/API/MessageExpert.Data/MessageExpert.Data.csproj --startup-project src/Services/API/MessageExpert.Api/MessageExpert.Api.csproj

## Linux 

#### Install git

sudo apt-get update
sudo apt-get upgrade
sudo apt-get install git

#### Install Docker

sudo apt install docker.io
sudo apt install docker-compose

### clone to project

git clone https://github.com/ahmetcagriakca/MessageExpert.git

sudo mkdir /environments
cd MessageExpert
sudo cp src/Services/API/MessageExpert.Api/appsettings.json /environments

nano for editting text
nano docker-compose.yml