# VTA Coding Challenge

## Description
VTA Coding challenge involving http requests, database usage with Entity Framework Core, and database access and serialization using .NET Core.

**All previous commits** (Apr 13 - Apr 18) were on this repository: 
[philip-nguyen/vta-coding/challenge](https://github.com/philip-nguyen/vta-coding-challenge/commits/main)

I refactored to a new repository due to the former being a console project and needed to be a web project hence the creation of this repo with a web server template project.

## Installation

Install .NET SDK 
- `install .NET SDK 6.0`

### Check if .SDK is installed

- `dotnet --list-sdks`
- `dotnet --list-runtimes`


### Install Entity Framework Core (ORM)

- `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`

### Create the database

```bash
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef database update
```
`dotnet ef database update` utilizes the migrations that are present in the root directory.

## Run the application
with API Key: `59af72683221a1734f637eae7a7e8d9b` and format `json`
- `dotnet dev-certs https --trust` (Trust the HTTPS dev certificate by running this. This command does not work on Linux.)
- `dotnet run json 59af72683221a1734f637eae7a7e8d9b` OR just default `dotnet run` (I have it run by default with api key and format hardcoded)