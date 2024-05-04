# etl_test
 
# Steps to run this project:

1. Clone repo
2. Create appsettings.json file by example in root directory
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "MSSQL_CS": "Server=127.0.0.1,1433;Database=db_name;User=sa;Password=passwordStr0ng!11;TrustServerCertificate=true;"
  }
}
```
3. Create docker-compose.yml file by example in root directory
```
services:
  mssql:
    image: mcr.microsoft.com/mssql/server:latest
    ports:
      - 1433:1433
    environment:
      SA_PASSWORD: "passwordStr0ng!11"
      MSSQL_DB_NAME: "db_name"
      ACCEPT_EULA: "Y"

```
4. Run docker compose up 
5. Start .net application

(for convenience, I did not include them in gitignor)