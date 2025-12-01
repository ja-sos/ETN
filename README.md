# ETN

ETN is an ASP.NET Core Web API (targeting .NET 9) designed to manage products within categories. It provides endpoints for querying, adding, and updating product data, with support for filtering and structured results. The solution follows a clean architecture approach and includes unit tests with an in-memory database for isolated testing.

---

## 🧱 Project Structure

```
ETN/
├── src/
│   ├── ETN.API/             # Entry point Web API project (controllers, Program.cs)
│   ├── ETN.Application/     # Application layer (contracts, result wrappers)
│   ├── ETN.Domain/          # Domain models (Product, Category)
│   └── ETN.Infrastructure/  # Data access layer (EF Core services, DTOs)
├── test/
│   └── ETN.API.Tests/       # Unit tests using InMemory EF Core
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/)
- A local or containerized database server instance (if not using InMemory for dev)

### Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/ja-sos/ETN.git
   cd ETN
   ```

2. Restore dependencies and build:
   ```bash
   dotnet restore
   dotnet build
   ```

3. Run the API:
   ```bash
   dotnet run --project src/ETN.API
   ```

4. Navigate to Swagger UI:
   ```
   https://localhost:<port>/swagger
   ```

---

## 📡 API Endpoints

| Method | Endpoint                  | Description                                 |
|--------|---------------------------|---------------------------------------------|
| POST   | `/Data/products`          | Returns a filtered list of products         |
| PUT    | `/Data/products/update`   | Updates a product and returns the result    |

> Both endpoints expect DTOs in the request body. See Swagger for schema definitions.

---

## 🧪 Testing

Unit tests are located in `test/ETN.API.Tests` and use an in-memory EF Core database for isolation.

To run tests:

```bash
dotnet test
```

- `DbFicture.cs` sets up and disposes the in-memory database context for clean test runs.
- `DataControllerTests.cs` contains tests for the product endpoints.

---

## 🧰 Technologies Used

- ASP.NET Core Web API (.NET 9)
- Entity Framework Core
- Swagger / Swashbuckle
- xUnit for testing
- InMemory EF Core provider
