# StoreAssignment2

Full stack-applikation för Inlämningsuppgift 2 i C# II.

## Teknologier

- ASP.NET Core Web API
- Blazor
- Entity Framework Core
- SQL Server LocalDB
- xUnit
- NSubstitute

## Projekt

Lösningen innehåller tre projekt:

- StoreApi - Web API och backend
- StoreFrontend - Blazor frontend
- StoreApi.Tests - enhetstester

## Funktionalitet

Applikationen har CRUD-funktionalitet för produkter:

- Visa produkter
- Skapa produkter
- Uppdatera produkter
- Ta bort produkter
- Välja kategori

Projektet innehåller två entiteter:

- Product
- Category

En Category kan ha flera Products.

## Backend

Backend använder:

- Controllers
- Services
- DTOs
- Interfaces
- Dependency Injection
- Repository Pattern
- Generic Repository
- Entity Framework Core
- async/await

Controllers använder service-interfaces via Dependency Injection.

## Databas

Projektet använder SQL Server LocalDB.

Databasnamn: StoreAssignment2Db

Entity Framework Core migrations används för att skapa databasen.

## Starta API

Kommando:

dotnet run --project .\StoreApi\StoreApi.csproj

API kör på:

http://localhost:5166

## Starta frontend

Kommando:

dotnet run --project .\StoreFrontend\StoreFrontend.csproj

Frontend kör på:

http://localhost:5090

Products-sidan:

http://localhost:5090/products

## Tester

Projektet innehåller 8 enhetstester för ProductService.

Tester använder:

- xUnit
- NSubstitute
- Arrange-Act-Assert

Repository-interfaces mockas med NSubstitute.

Kör tester med:

dotnet test
