#  Deployment Guide - Studio Al Amal Backend

> **Note:** This guide is intended for a real client environment.  
> All sensitive secrets (database credentials, JWT keys) are replaced with placeholders.  
> **Do not use these placeholders in production** — replace them with your actual secrets securely.

---

## Required Environment Variables

Set these environment variables in your production environment for all services.

### All Services
# Database Connection
ConnectionStrings__DefaultConnection="Server=YOUR_PRODUCTION_SERVER;Database=StudioAlAmalDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"

# JWT Configuration
Jwt__Key="YOUR_64_CHARACTER_SECRET_KEY_HERE"
Jwt__Issuer="StudioAlAmal"
Jwt__Audience="StudioAlAmalUsers"

# ASP.NET Core Environment
ASPNETCORE_ENVIRONMENT="Production"
ASPNETCORE_URLS="https://+:443;http://+:80"
Setting Environment Variables
Windows (PowerShell):

powershell
Copier le code
$env:ConnectionStrings__DefaultConnection="Server=YOUR_PRODUCTION_SERVER;Database=StudioAlAmalDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
$env:Jwt__Key="YOUR_64_CHARACTER_SECRET_KEY_HERE"
Linux/Mac:


Copier le code
export ConnectionStrings__DefaultConnection="Server=YOUR_PRODUCTION_SERVER;Database=StudioAlAmalDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
export Jwt__Key="YOUR_64_CHARACTER_SECRET_KEY_HERE"
Azure App Service:

Go to Configuration → Application Settings

Add each variable as a new setting with your actual production secrets

Docker:

dockerfile
Copier le code
ENV ConnectionStrings__DefaultConnection="Server=YOUR_PRODUCTION_SERVER;Database=StudioAlAmalDB;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
ENV Jwt__Key="YOUR_64_CHARACTER_SECRET_KEY_HERE"