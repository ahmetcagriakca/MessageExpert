# MessageExpert
Messaging application 

## Appsettings 
Database configuration managed on DatabaseConfig, you can choose database provider like postgre or mssql

 "DatabaseConfig": {
    "Provider": "Postgre",
    "ConnectionString": "User ID=postgres;Password=postgres;Host=localhost;Port=5432;Database=MessageExpert.Services.ApiDb;Pooling=true;"
  },

#### update database command

> set environment before update database

> windows

$env:DATABASE_CONNECTION_STRING='User ID=postgres;Password=test1234;Host=localhost;Port=5433;Database=MessageExpert.Services.Api.TestDb;Pooling=true;'

$env:DATABASE_CONNECTION_STRING='User ID=postgres;Password=test1231;Host=localhost;Port=5434;Database=MessageExpert.Services.Api.ProdDb;Pooling=true;'

> linux

export set for global 

export DATABASE_CONNECTION_STRING='User ID=postgres;Password=test1234;Host=localhost;Port=5433;Database=MessageExpert.Services.Api.TestDb;Pooling=true;'

set for this session

DATABASE_CONNECTION_STRING='User ID=postgres;Password=test1231;Host=localhost;Port=5434;Database=MessageExpert.Services.Api.ProdDb;Pooling=true;' &&
dotnet ef database update --project src/Services/API/MessageExpert.Data/MessageExpert.Data.csproj --startup-project src/Services/API/MessageExpert.Api/MessageExpert.Api.csproj

remove environment variable
unset DATABASE_CONNECTION_STRING

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

### Install Dotnet 

wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb

sudo dpkg -i packages-microsoft-prod.deb

sudo add-apt-repository universe

sudo apt-get install apt-transport-https

sudo apt-get update

sudo apt-get install dotnet-sdk-2.2

### create environments folder

sudo mkdir /environments

sudo mkdir /environments/test

sudo mkdir /environments/prod

cd MessageExpert

### copy docker settings to environments

sudo cp src/Services/API/MessageExpert.Api/appsettings.Docker.json /environments

sudo cp src/Services/API/MessageExpert.Api/appsettings.json /environments/test

sudo cp src/Services/API/MessageExpert.Api/appsettings.Docker.json /environments/prod

### editting file

nano docker-compose.yml

### after at all running docker on linux

docker-compose up --build

### firewall add port

connect to gcloud linux

gcloud compute --project "PROJECT_NAME" ssh --zone "us-east1-b" "instance-1"

gcloud allow port 

gcloud compute --project "PROJECT_NAME" firewall-rules create  test-api-rule --allow tcp:81

gcloud compute --project "PROJECT_NAME" firewall-rules create  prod-api-rule --allow tcp:82

allow firewall 

ufw allow 81

ufw allow 82

#Git

### clone to project

git clone https://github.com/ahmetcagriakca/MessageExpert.git

### reset branch to head

> clean local change

git clean -fd

> fetch all change

git fetch --all

> reset local to last change and remove local commit

git reset --hard origin/master