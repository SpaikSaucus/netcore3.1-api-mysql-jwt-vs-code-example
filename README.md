# netcore3.1-api-mysql-jwt-vs-code-example
Netcore 3.1 api example with MySQL, JWT and swagger (using VS Code)

## Table of Contents
- [Getting started](#getting-started)
- [.NET Core](#net-core)
  - [Debugging](#debugging)
  - [How to generate a new NET Core web api? (step by step)](#how-to-generate-a-new-net-core-web-api-step-by-step)
- [JWT](#JWT)
- [MySQL](#mysql-with-net-core)
- [Swagger](#swagger)
- [Tips](#tips)
  - [Environment configuration](#environment-configuration)
  - [Generate model from database](#generate-model-from-database)
  - [Other Provider DB](#other-provider-db)
  - [Disable telemetry](#disable-telemetry)
- [Postman Collection](#postman-collection)
- [Troubleshooting](#troubleshooting)
  - [IntelliSense not working](#intelliSense-not-working)
- [License](#license)

## Getting Started

* Download repository
* Install VS Code IDE :point_right: [download](https://code.visualstudio.com/download)
* Install SDK 3.1 :point_right: [download](https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.1.302-windows-x64-installer)
* Install extension C# of microsoft (powered by OmniSharp).
  https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp

* create new database: 
	```sql
	create database netcore_api_example;
	```
* create table "user" and insert admin:
	```sql
	use netcore_api_example;

	CREATE TABLE `user` (
		`guid` varchar(36) not null,
		`username` varchar(45) not null,
		`password` varchar(45) not null,
		`email` varchar(100) not null,
		`created` datetime not null,
		PRIMARY KEY (`guid`)
	);

	INSERT INTO `user` (`guid`, `username`, `password`, `email`, `created`) VALUES
	(uuid(), 'admin', '1234', 'admin@test.com', now());
	```
* open VS Code and open folder ***/scr/api*** where store the project.
* configure credentials database in project [Startup.cs - line 34](src/Api/Startup.cs)


* execute command in terminal:
	```bash
	dotnet run
	```
* and enjoy!
 
Recommended Pages:
* [Tutorial .NET Core, first web api](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio)
* [Tutorial VS Code, Working with C#](https://code.visualstudio.com/docs/languages/csharp)


## .NetCore

### Debugging
---
In the IDE we press "F5" or go to the menu of "Run" → Start Debugging
* we chose the .NET Core option
* we load the "program" variable with "${workspaceFolder}/bin/Debug/net5.0/Api.dll".

We press "F5" again or go to the menu of "Run" → Start Debugging
* an error message will appear telling us if we want to "Configure Task", selecting it will tell us if we want to "Create task.json file from template NET Core"
* then we will choose the ".NET Core" option

We press "F5" again or go to the menu of "Run" → Start Debugging And enter https://localhost:5001/swagger

### How to generate a new NET Core web api? (step by step)
---
To generate a new webApi, follow these steps:
  
- > Location example: _C:\myFolderNewWebApi>_
	```bash
	dotnet new webapi -o Api
	```
- next command:
	```bash
	cd api
	```
- >Location: _C:\myFolderNewWebApi\api>_

- entity framework:
	```bash
	dotnet add package MySql.Data.EntityFrameworkCore
	```
- swagger:
	```bash
	dotnet add package Swashbuckle.AspNetCore --version 5.6.3
	```
- JToken and JObject and JsonConvert:
	```bash
	dotnet add package Newtonsoft.Json --version 13.0.1
	```
- check build successful
	```bash
	dotnet build
	```

## JWT

Documentation Inprogress...

## MYSQL with NET Core
https://dev.mysql.com/downloads/installer/
Select:
-Connector .net
-MySQL Server
-Worbench

Details:
* https://www.c-sharpcorner.com/article/how-to-connect-mysql-with-asp-net-core/

## Swagger
  https://localhost:5001/swagger/index.html


## Tips

### Environment configuration
---
Inside the "Properties" folder we have the "launchSettings.json" file where we have the environment configuration in "environmentVariables" → "Development" and that we can replace with environment necessary.

### Generate model from database
---
In case you want to generate the model from the database, you can use the following scaffolding:
* https://dev.mysql.com/doc/connector-net/en/connector-net-entityframework-core-scaffold-example.html

### Other Provider DB
---
In case you want to use another provider other than MySql, check the following link.
  https://docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli


### Disable telemetry
---
https://docs.microsoft.com/en-us/dotnet/core/tools/telemetry


## Postman Collection
Download and import this [collection](postman/NetCoreExample.postman_collection.json).


## Troubleshoot
### IntelliSense not working
---
Solution: Downgrade extension vs code C# OmniSharp

Sometimes some vs code extension could stop working. In those cases we can install an old version of the extension to check if it works correctly with another version.

To do that we go to the extension and on the uninstall button, press on the arrow to choose "install another version", we will choose a previous version to validate that it works.

## License

Is licensed under [The MIT License](LICENSE).
