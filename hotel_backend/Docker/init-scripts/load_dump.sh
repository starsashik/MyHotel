#!/bin/bash

echo "Restoring db from dump..."

pg_restore -U postgres -d MyHotel /docker-entrypoint-initdb.d/dump.dump

echo "Restore completed"
