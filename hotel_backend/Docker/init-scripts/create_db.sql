SELECT 'CREATE DATABASE "MyHotel" OWNER postgres'
    WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'MyHotel')
LIMIT 1;
CREATE DATABASE "MyHotel" OWNER postgres;
