# VTA Coding Challenge

## Description
VTA Coding challenge involving http requests, database usage with Entity Framework Core, and database access and serialization using .NET Core.

**All previous commits** (Apr 13 - Apr 18) were on this repository: 
[philip-nguyen/vta-coding/challenge](https://github.com/philip-nguyen/vta-coding-challenge/commits/main)

I refactored to a new repository due to the former being a console project and needed to be a web project hence the creation of this repo with a web server template project.

## Installation

- install .NET SDK 6.0

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

### Output of `dotnet run`
![dotnet-output](https://user-images.githubusercontent.com/55335418/234157826-30fc6bad-6969-4efb-a674-5678b4acf4bf.PNG)

### Endpoint at `localhost:5045/real-time/trip-updates`
![real-time-trip-updates-endpoint](https://user-images.githubusercontent.com/55335418/234157976-8bc05dd8-8271-40df-837d-5848727e0489.PNG)

### Database after every command
Deletes expired trip updates and can be verified with the same count of rows in the TripUpdates table as the amount of TripUpdates in the API response.
![marked-delete-expired-trip-updates](https://user-images.githubusercontent.com/55335418/234158950-2071fa1f-425a-4b5e-a51b-981236177c30.jpg)
