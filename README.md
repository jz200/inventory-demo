# Inventory Control System - Demo Version
This is a demo version of a real world application that allows business to 
track and manage product inventories across multiple warehouse locations. 
It is built with ASP.NET Core 2.0 and SQL Server 2017.

## Architecture Overview
This is a multi-layer, multi-project solution. 
* Data Access Layer. The data project contains 
entity classes, database context class, migrations and data services. 
The other two projects gain access to database 
through data services in this project.
* User Interface Layer.  The UI project is built with ASP.NET 
Core MVC. This is the interface for regular users 
who have restricted access to entities.
* Admin Layer. The admin project is built with ASP.Net Core Razor Pages.
This is for admin users who are authorized 
to perform CRUD (Create, Read, Update, Delete) 
operations on all entities.

## Technologies & Tools
* C# 7
* ASP.NET Core 2.0 MVC & Razor Pages
* ASP.NET Core Identity
* Entity Framework Core
* Custom Tag Helper
* Remote Validation
* Visual Studio 2017
* SQL Server 2017
* Bootstrap 3.3.7
* jQuery 2.2
* HTML 5
* CSS 3
* GIT


## Functionality Overview
### Regular User Interface
* Search products in stock by product category, 
style, item number and warehouse location.
* CRUD operations on product inventories only.

### Admin User Interface
* CRUD operations on all business entities including
inventories, product items, categories, styles, and 
warehouse locations.
* Complete user management including role authorization.


## Getting Started

### Prepare Development Environment
* Install Microsoft Visual Studio.  Go to 
[Visual Studio Download Page](https://visualstudio.microsoft.com/downloads/). 
The community edition is free and has all features used in this demo application.
* Install Microsoft SQL Server.  Go to 
[Sql Server Downloads](https://www.microsoft.com/en-us/sql-server/sql-server-downloads).
The SQL Server 2017 Developer is a full-featured
free edition.
* Install version control tool Git. Go to 
[git-scm.com](https://git-scm.com/downloads) to 
download an installer for your operating system.
* Install GitHub Extension in visual studio. This step is optional.

### Download Code from GitHub
* Run `git clone https://github.com/jz200/inventory-demo.git` 
in command prompt to clone the solution. <br/>
or
* Use Team Explorer in visual studio to 
clone the solution if you have GitHub extension installed.

### Set up Database
* Modify the database connection string in configuration file appsetting.json if needed. 
The demo database resides in a named instance of SQL server named MSSQLSERVER01. 
You can remove the instance name if you 
plan to use the default instance of SQL server.

    `"DefaultConnection": "Server=(local)\\MSSQLSERVER01;Database=BBMDemo;Trusted_Connection=True;MultipleActiveResultSets=true"`
* Open Package Manager Console 
(Tools -> Nuget Package Manager -> Package Manager Console)
and make sure default project is set to the data project.
As migration data are already generated in Migrations folder, 
you can simply run `update-database` to create database.

* Connect to database with SQL server management studio (SSMS).
You will find seven ASP.NET Identity related tables (tables prefixed with AspNet) and 
one migration history table in dbo schema. The inventory control tables 
all reside in Production schema.

* Add seed data. There is a query file 
named `addTestData.sql` in data project. 
Execute the queries on SSMS to add necessary test
data.

### Run Applications
* Run UI application. In visual studio, right click UI project and
click View in Browser. You can access the site for
regular user at [https://localhost:44312](https://localhost:44312).
* Run admin application. In visual studio, right click admin project and
click View in Browser. You can access the site for
admin user at [https://localhost:44377](https://localhost:44377).




