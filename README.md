Dependencies: 
PM > Install-Package Microsoft.EntityFrameworkCore
PM > Install-Package Microsoft.EntityFrameworkCore.SqlServer
PM > Install-Package Microsoft.EntityFrameworkCore.Tools
PM > Update-Database

Database Connection: 
Change the 'ConnectionStrings' in appsettings.json

Execution:
PM > dotnet build
PM > dotnet run
