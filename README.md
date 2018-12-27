# MessageExpert
Messaging application 

# Docker
## Api docker up 

retrieving content from  parent directory
docker build -f src\Services\API\MessageExpert.Api\Dockerfile src\Services\API\.

-p attaching to port. 
docker run -p 90:80 mexpert/api

remove all docker containers
docker rm $( docker ps -q) -f
