IF NOT "%1"=="" (
    IF "%1"=="-rm" (
        docker stack rm better-me

        FOR /f "tokens=*" %%i IN ('docker ps -aq') DO docker rm -f %%i
        FOR /f "tokens=*" %%i IN ('docker volume ls -q') DO docker volume rm %%i

        docker rmi better-me-api:latest
        docker rmi better-me-web:latest
    )
)

docker build ./better-me-api/ -t better-me-api:latest
docker build ./better-me-web/ -t better-me-web:latest
docker swarm init
docker stack deploy --compose-file better-me-stack.yml better-me
pause