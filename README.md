# etl_test
 
# Steps to run this project:

1. Clone repo
2. Create appsettings.json file by example 

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

3. Run docker compose up in root directory

