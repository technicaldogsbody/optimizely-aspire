# Andy Blyth's Optimizely on Aspire Demo

## Implemented an empty opti project with dotnet aspire in C# 8.0

This project is very basic and is more focussed on using Aspire with Optimizely, the key thing to know is the AppHost project should be the start up project and it has two launch settings, one to scaffold the DB and create an admin used, and one to run the website up. The `scaffold` one should only need to be run once (unless the DB files are deleted, corrupt, etc. or you want a fresh database), after that you should only run the `web` launch setting.