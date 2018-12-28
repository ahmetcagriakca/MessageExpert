
# Docker
## Api docker up 

### retrieving content from  parent directory

docker build -f src\Services\API\MessageExpert.Api\Dockerfile src\Services\API\.

#### -p attaching to port. 

docker run -p 90:80 mexpert/api

#### remove all docker containers

docker rm $( docker ps -q) -f

#### run docker with volume

docker run -p 90:80 -v c:/environments/appsettings.json:/app/appsettings.json mexpert/api

#### connect to docker container

docker exec -it 21f61c5df5ef sh

### restart docker container

docker container restart [Container_Id]

#### create docker volume

docker volume create --driver local \
    --opt type=nfs \
    --opt o=addr=192.168.1.1,rw \
    --opt device=:/path/to/dir \



docker volume create -d local -o type=bind -o device=/environments  --name test
docker volume create -d local -o type=nfs -o device=c:\environments --name test1
docker volume create -d local -o type=nfs -o device=c:\environments -o o=addr=192.168.1.1 --name test2
docker volume create -d local -o type=bind -o device=c:\environments --name test3

### add volume to docker container
docker run -p 92:80 -v test1:/app mexpert/api
docker run -p 92:80 -v test2:/app mexpert/api
docker run -p 92:80 -v test3:/app mexpert/api

# Working with docker compose

### docker compose up 

docker-compose up --build

### docker compose build service 

docker-compose build core-api

### docker compose restart service

docker-compose restart core-api