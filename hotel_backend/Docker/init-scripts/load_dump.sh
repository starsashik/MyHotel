#!/bin/bash
set -e

echo "Creating database MyHotel..."
psql -U postgres -c "CREATE DATABASE \"MyHotel\";"

echo "Restoring dump..."
pg_restore -U postgres -d MyHotel /docker-entrypoint-initdb.d/dump.dump

echo "Done."
