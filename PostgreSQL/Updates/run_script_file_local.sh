#!/bin/bash

echo ""
echo ""
echo "****"
echo "This is Clients script run update for LOCAL Sana Live server"
echo "****"

read -p "Enter sql script file: " fileName

dbUpdatesPath="../../DbUpdates_Build/Mac/netcoreapp2.0/DbUpdates.dll";
dbNameTemplate="Client";

# Read Server address
echo -n Server:
read server
echo

# Read User Id
echo -n User Id:
read userId
echo

# Read Password
# Password for beta: lskuscs1d80k
echo -n Password: 
read -s password
echo

connectionString="Server=$server;Port=5432;Database=postgres;User Id=$userId;Password=$password;"

dotnet "$dbUpdatesPath" -cs "$connectionString" -rs "$fileName" -db "$dbNameTemplate";