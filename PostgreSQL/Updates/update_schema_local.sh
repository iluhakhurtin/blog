#!/bin/bash

echo ""
echo ""
echo "****"
echo "This is SCHEMA update for LOCAL Blog server"
echo "****"

dbUpdatesPath="../DbUpdates_Build/Mac/netcoreapp2.0/DbUpdates.dll";
dbNameTemplate="Blog";
versionFieldName="SchemaVersion";
updatesFolderPath="./Schema"

# Read Server address
echo -n Server:
read server
echo

# Read User Id
echo -n User Id:
read userId
echo

# Read Password
echo -n Password: 
read -s password
echo

connectionString="Server=$server;Port=5432;Database=postgres;User Id=$userId;Password=$password;"

dotnet "$dbUpdatesPath" -cs "$connectionString" -vfn "$versionFieldName" -db "$dbNameTemplate" -uf "$updatesFolderPath";
