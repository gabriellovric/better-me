#!/bin/bash

if [[ ! -z $1 ]]; then
    if [[$1 == "-rm"]]; then
        docker stack rm better-me
        docker rm $(docker ps -aq)
        docker volume rm $(docker volume rm -q)
        docker rmi better-me-api:latest
        docker rmi better-me-web:latest
    fi
fi

docker build ./better-me-api/ -t better-me-api:latest
docker build ./better-me-web/ -t better-me-web:latest
docker swarm init
docker stack deploy --compose-file better-me-stack.yml better-me
read -p "Press enter to continue"