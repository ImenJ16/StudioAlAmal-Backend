Write-Host "Starting StudioAlAmal services..."

Start-Process powershell -ArgumentList "cd Services\AuthService; dotnet watch"
Start-Process powershell -ArgumentList "cd Services\ContentService; dotnet watch"
Start-Process powershell -ArgumentList "cd Services\CommunicationService; dotnet watch"
