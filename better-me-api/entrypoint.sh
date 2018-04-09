#!/bin/bash

set -e
run_cmd="dotnet run --server.urls http://*:80"

dotnet ef migrations add InitialCreate;
dotnet ef database update;

>&2 echo "MySQL Server is up - executing command"
exec $run_cmd