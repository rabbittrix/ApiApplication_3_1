# Install after version 3.x
dotnet tool install --global dotnet-ef

dotnet ef database update (create tables on the database)

dotnet new globaljson --sdk-version 3.0.201 (Created de file)

dotnet ef migrations add UserMigration

dotnet ef migrations remove

# Database
This is confugurated to use MySql and Sql-Server

You can configurate on class: "ContextFactory, ConfigureRepository and MyContext (Database.Migration())".
