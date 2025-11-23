#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#
#COPY . .
#RUN dotnet restore ./hotel_backend.sln
#RUN dotnet publish -c Release -o /app/out
#
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
#WORKDIR /app
#COPY --from=build /app/out .
#ENTRYPOINT ["dotnet", "hotel_backend.dll"]



# syntax=docker/dockerfile:1
################################
# build stage
################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем только слои, нужные для dotnet restore (ускоряет кэширование)
COPY ["hotel_backend.sln", "./"]
COPY ["hotel_backend.csproj", "./"]

# Восстанавливаем зависимости
RUN dotnet restore "hotel_backend.sln"

# Копируем весь код в образ
COPY . .

# Публикуем релизную сборку
RUN dotnet publish "hotel_backend.csproj" -c Release -o /app/out

################################
# runtime stage
################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Переносим опубликованные артефакты
COPY --from=build /app/out ./

# укажите порт если нужно
EXPOSE 8080

# Запуск; замените имя DLL если отличается
ENTRYPOINT ["dotnet", "hotel_backend.dll"]

