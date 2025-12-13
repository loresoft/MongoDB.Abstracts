# MongoDB.Abstracts

A modern .NET library that provides abstract base classes and interfaces for implementing the repository pattern with MongoDB. Simplify your MongoDB data access layer with strongly-typed repositories, automatic auditing, and dependency injection support.

[![Build status](https://github.com/loresoft/MongoDB.Abstracts/workflows/Build/badge.svg)](https://github.com/loresoft/MongoDB.Abstracts/actions)
[![NuGet Version](https://img.shields.io/nuget/v/MongoDB.Abstracts.svg?style=flat-square)](https://www.nuget.org/packages/MongoDB.Abstracts/)
[![Coverage Status](https://coveralls.io/repos/github/loresoft/MongoDB.Abstracts/badge.svg?branch=master)](https://coveralls.io/github/loresoft/MongoDB.Abstracts?branch=master)

## Features

- **Generic Repository Pattern**: Type-safe repository interfaces and base classes for MongoDB
- **Automatic Auditing**: Built-in support for tracking entity creation and modification timestamps
- **Dependency Injection**: Seamless integration with .NET's dependency injection container
- **Multi-Database Support**: Connection discrimination for working with multiple MongoDB databases
- **LINQ Support**: Full IQueryable support for complex queries
- **Async/Await**: Complete async operation support for modern applications
- **Custom Repository Extensions**: Easy override points for specialized repository behavior

## Installation

### Package Manager Console
```powershell
Install-Package MongoDB.Abstracts
```

### .NET CLI
```bash
dotnet add package MongoDB.Abstracts
```

### PackageReference
```xml
<PackageReference Include="MongoDB.Abstracts" />
```

More information available at [nuget.org/packages/MongoDB.Abstracts](https://nuget.org/packages/MongoDB.Abstracts)

## Core Interfaces and Classes

### Entity Interfaces
- **`IMongoEntity`** - Base interface for MongoDB entities with Id, Created, and Updated properties
- **`MongoEntity`** - Base class implementing IMongoEntity with common MongoDB entity behavior

### Repository Interfaces
- **`IMongoQuery<TEntity, TKey>`** - Interface for generic MongoDB query operations
- **`IMongoRepository<TEntity, TKey>`** - Interface for generic MongoDB repository operations
- **`IMongoEntityRepository<TEntity>`** - Specialized repository interface for IMongoEntity types
- **`IMongoEntityRepository<TDiscriminator, TEntity>`** - Multi-database discriminator support

### Repository Implementations
- **`MongoQuery<TEntity, TKey>`** - Base class for MongoDB query operations
- **`MongoRepository<TEntity, TKey>`** - Base class for MongoDB repository operations
- **`MongoEntityRepository<TEntity>`** - Entity-specific repository implementation

## Configuration

### Basic Setup

Register MongoDB repositories with dependency injection using a connection string:

```csharp
// Direct connection string
services.AddMongoRepository("mongodb://localhost:27017/MyDatabase");

// Connection string from configuration
services.AddMongoRepository("MyDatabase");
```

**appsettings.json:**
```json
{
  "ConnectionStrings": {
    "MyDatabase": "mongodb://localhost:27017/MyDatabase"
  }
}
```

### Advanced Configuration

#### Configure MongoDB Client Settings

You can customize the MongoDB client settings, including logging configuration:

```csharp
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Configuration;

services.AddMongoRepository(
    nameOrConnectionString: "MyDatabase",
    configuration: (provider, settings) =>
    {
        // Enable MongoDB driver logging
        var loggerFactory = provider.GetService<ILoggerFactory>();
        settings.LoggingSettings = new LoggingSettings(loggerFactory);
        
        // Configure other client settings
        settings.ConnectTimeout = TimeSpan.FromSeconds(30);
        settings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);
        settings.MaxConnectionPoolSize = 100;
    }
);
```

#### Specify Database Name

Override the database name from the connection string:

```csharp
services.AddMongoRepository(
    nameOrConnectionString: "mongodb://localhost:27017/DefaultDb",
    databaseName: "ActualDatabase"
);
```

### Multi-Database Configuration with Discriminators

For applications requiring multiple MongoDB connections, use discriminator types to distinguish between different database contexts:

#### 1. Define Discriminator Types
```csharp
public readonly struct ProductsConnection;
public readonly struct InventoryConnection;
public readonly struct UsersConnection;
```

#### 2. Register Multiple Connections
```csharp
services.AddMongoRepository<ProductsConnection>("ProductsConnection");
services.AddMongoRepository<InventoryConnection>("InventoryConnection");
services.AddMongoRepository<UsersConnection>("UsersConnection");
```

#### 3. Configuration
**appsettings.json:**
```json
{
  "ConnectionStrings": {
    "ProductsConnection": "mongodb://localhost:27017/Products",
    "InventoryConnection": "mongodb://localhost:27017/Inventory",
    "UsersConnection": "mongodb://localhost:27017/Users"
  }
}
```

#### 4. Dependency Injection
```csharp
public class ProductService
{
    private readonly IMongoEntityRepository<ProductsConnection, Product> _repository;

    public ProductService(IMongoEntityRepository<ProductsConnection, Product> repository)
    {
        _repository = repository;
    }
    
    public async Task<Product> GetProductAsync(string id)
    {
        return await _repository.FindAsync(id);
    }
}

public class InventoryService
{
    private readonly IMongoEntityRepository<InventoryConnection, Inventory> _repository;

    public InventoryService(IMongoEntityRepository<InventoryConnection, Inventory> repository)
    {
        _repository = repository;
    }
    
    public async Task<List<Inventory>> GetLowStockItemsAsync()
    {
        return await _repository.FindAllAsync(i => i.Quantity < 10);
    }
}
```

## Usage Examples

### Define Your Entity

```csharp
public class Product : MongoEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
```

### Basic CRUD Operations

#### Create (Insert)
```csharp
var productRepo = serviceProvider.GetRequiredService<IMongoEntityRepository<Product>>();

var product = new Product 
{ 
    Name = "Gaming Laptop",
    Description = "High-performance gaming laptop",
    Price = 1299.99m,
    Stock = 5,
    Category = "Electronics"
};

var createdProduct = await productRepo.InsertAsync(product);
Console.WriteLine($"Created product with ID: {createdProduct.Id}");
```

#### Read (Find)
```csharp
// Find by ID
var product = await productRepo.FindAsync("67a0dc52fa5ebe49f293a374");

// Find single entity with query
var expensiveProduct = await productRepo.FindOneAsync(p => p.Price > 1000);

// Find multiple entities
var electronicsProducts = await productRepo.FindAllAsync(p => p.Category == "Electronics");

// Find with LINQ
var activeProducts = productRepo.All()
    .Where(p => p.IsActive && p.Stock > 0)
    .OrderBy(p => p.Name)
    .ToListAsync();
```

#### Update
```csharp
// Find and update
var product = await productRepo.FindAsync("67a0dc52fa5ebe49f293a374");
if (product != null)
{
    product.Price = 1199.99m;
    product.Stock = 3;
    
    var updatedProduct = await productRepo.UpdateAsync(product);
    Console.WriteLine($"Updated product: {updatedProduct.Name}");
}
```

#### Delete
```csharp
// Delete by ID
var deletedCount = await productRepo.DeleteAsync("67a0dc52fa5ebe49f293a374");
Console.WriteLine($"Deleted {deletedCount} product(s)");

// Delete with query
var deletedInactiveCount = await productRepo.DeleteAllAsync(p => !p.IsActive);
Console.WriteLine($"Deleted {deletedInactiveCount} inactive product(s)");
```

### Advanced Querying

#### Complex LINQ Queries
```csharp
var productRepo = serviceProvider.GetRequiredService<IMongoEntityRepository<Product>>();

// Complex filtering with pagination
var expensiveElectronics = await productRepo.All()
    .Where(p => p.Category == "Electronics" && p.Price > 500)
    .Where(p => p.IsActive && p.Stock > 0)
    .OrderByDescending(p => p.Price)
    .ThenBy(p => p.Name)
    .Skip(0)
    .Take(10)
    .ToListAsync();

// Aggregation-style queries
var categoryStats = productRepo.All()
    .Where(p => p.IsActive)
    .GroupBy(p => p.Category)
    .Select(g => new 
    {
        Category = g.Key,
        Count = g.Count(),
        AveragePrice = g.Average(p => p.Price),
        TotalValue = g.Sum(p => p.Price * p.Stock)
    })
    .ToList();
```

#### Using MongoDB Filters
```csharp
using MongoDB.Driver;

// Using MongoDB.Driver filters for advanced scenarios
var filter = Builders<Product>.Filter.And(
    Builders<Product>.Filter.Eq(p => p.Category, "Electronics"),
    Builders<Product>.Filter.Gte(p => p.Price, 100),
    Builders<Product>.Filter.Lte(p => p.Price, 1000)
);

var products = await productRepo.FindAllAsync(filter);
```

## Custom Repository Implementation

Create specialized repositories by inheriting from the base classes:

```csharp
public class ProductRepository : MongoEntityRepository<Product>
{
    public ProductRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase)
    {
    }

    // Custom business logic before insert
    protected override void BeforeInsert(Product entity)
    {
        base.BeforeInsert(entity);
        
        // Auto-generate SKU
        entity.SKU = GenerateSKU(entity.Category, entity.Name);
        
        // Validate business rules
        ValidateProductRules(entity);
    }

    // Custom business logic before update
    protected override void BeforeUpdate(Product entity)
    {
        base.BeforeUpdate(entity);
        
        // Re-validate business rules
        ValidateProductRules(entity);
        
        // Update search tags
        entity.SearchTags = GenerateSearchTags(entity);
    }

    // Ensure custom indexes
    protected override void EnsureIndexes(IMongoCollection<Product> mongoCollection)
    {
        base.EnsureIndexes(mongoCollection);

        // Create compound index for category and price
        mongoCollection.Indexes.CreateOne(
            new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys
                    .Ascending(p => p.Category)
                    .Descending(p => p.Price),
                new CreateIndexOptions { Name = "category_price_idx" }
            )
        );

        // Create text index for search
        mongoCollection.Indexes.CreateOne(
            new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Text(p => p.Name).Text(p => p.Description),
                new CreateIndexOptions { Name = "text_search_idx" }
            )
        );

        // Create unique index on SKU
        mongoCollection.Indexes.CreateOne(
            new CreateIndexModel<Product>(
                Builders<Product>.IndexKeys.Ascending(p => p.SKU),
                new CreateIndexOptions { Unique = true, Name = "sku_unique_idx" }
            )
        );
    }

    // Custom repository methods
    public async Task<List<Product>> FindByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await FindAllAsync(p => p.Price >= minPrice && p.Price <= maxPrice && p.IsActive);
    }

    public async Task<Product?> FindBySKUAsync(string sku)
    {
        return await FindOneAsync(p => p.SKU == sku);
    }

    public async Task<bool> IsStockAvailableAsync(string productId, int requestedQuantity)
    {
        var product = await FindAsync(productId);
        return product?.Stock >= requestedQuantity;
    }

    private string GenerateSKU(string category, string name)
    {
        // Implementation for SKU generation
        var categoryCode = category.ToUpperInvariant().Substring(0, Math.Min(3, category.Length));
        var nameCode = string.Concat(name.Where(char.IsLetterOrDigit)).ToUpperInvariant();
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return $"{categoryCode}-{nameCode}-{timestamp}";
    }

    private void ValidateProductRules(Product entity)
    {
        if (entity.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero");
            
        if (string.IsNullOrWhiteSpace(entity.Name))
            throw new ArgumentException("Product name is required");
            
        if (entity.Stock < 0)
            throw new ArgumentException("Product stock cannot be negative");
    }

    private List<string> GenerateSearchTags(Product entity)
    {
        var tags = new List<string>();
        
        // Add category tags
        tags.Add(entity.Category.ToLowerInvariant());
        
        // Add name words
        tags.AddRange(entity.Name.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(w => w.ToLowerInvariant()));
        
        // Add price range tags
        if (entity.Price < 50) tags.Add("budget");
        else if (entity.Price > 1000) tags.Add("premium");
        else tags.Add("mid-range");
        
        return tags.Distinct().ToList();
    }
}
```

## Entity Auditing

The `IMongoEntity` interface automatically provides auditing capabilities:

```csharp
public class AuditableProduct : MongoEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    // Additional audit fields (beyond base Created/Updated)
    public string CreatedBy { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
}

public class AuditableProductRepository : MongoEntityRepository<AuditableProduct>
{
    private readonly ICurrentUserService _currentUserService;

    public AuditableProductRepository(
        IMongoDatabase mongoDatabase, 
        ICurrentUserService currentUserService) 
        : base(mongoDatabase)
    {
        _currentUserService = currentUserService;
    }

    protected override void BeforeInsert(AuditableProduct entity)
    {
        base.BeforeInsert(entity); // Sets Created and Updated timestamps
        
        entity.CreatedBy = _currentUserService.GetCurrentUserId();
    }

    protected override void BeforeUpdate(AuditableProduct entity)
    {
        base.BeforeUpdate(entity); // Updates the Updated timestamp
        
        entity.UpdatedBy = _currentUserService.GetCurrentUserId();
    }
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
