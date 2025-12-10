#!/bin/bash
set -e

echo "Creating database MyHotel..."
psql -U postgres -c "CREATE DATABASE \"MyHotel\";" || echo "Database might already exist, continuing..."

if [ ! -f /docker-entrypoint-initdb.d/dump.dump ]; then
    echo "ERROR: Dump file not found at /docker-entrypoint-initdb.d/dump.dump"
    exit 1
fi

echo "Restoring dump..."
pg_restore -U postgres -d MyHotel /docker-entrypoint-initdb.d/dump.dump || echo "Warning: pg_restore failed, but continuing..."

echo "Done."
