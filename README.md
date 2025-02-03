# MongoDB.Abstracts

## Overview

The MongoDB Abstracts library defines abstract base classes for repository pattern.

[![Build status](https://github.com/loresoft/MongoDB.Abstracts/workflows/Build/badge.svg)](https://github.com/loresoft/MongoDB.Abstracts/actions)

[![NuGet Version](https://img.shields.io/nuget/v/MongoDB.Abstracts.svg?style=flat-square)](https://www.nuget.org/packages/MongoDB.Abstracts/)   

[![Coverage Status](https://coveralls.io/repos/github/loresoft/MongoDB.Abstracts/badge.svg?branch=master)](https://coveralls.io/github/loresoft/MongoDB.Abstracts?branch=master)

## Download

The MongoDB.Abstracts library is available on nuget.org via package name `MongoDB.Abstracts`.

To install MongoDB.Abstracts, run the following command in the Package Manager Console

    PM> Install-Package MongoDB.Abstracts
    
More information about NuGet package available at
<https://nuget.org/packages/MongoDB.Abstracts>


### Features

* interface for generic MongoDB queries; `IMongoQuery<TEntity, TKey>`
* interface for generic MongoDB repository; `IMongoRepository<TEntity, TKey>`
* base class for generic MongoDB queries; `MongoQuery<TEntity, TKey>`
* base class for generic MongoDB repository; `MongoRepository<TEntity, TKey>`
* interface for generic MongoDB entity; `IMongoEntity`
* base class for generic MongoDB entity; `MongoEntity`
* interface for generic MongoDB entity repository; `IMongoEntityRepository<TEntity>`
* base class for generic MongoDB entity repository; `MongoEntityRepostiory<TEntity>`

### Configuration

Register with dependency injection

```c#
services.AddMongoDB("mongodb://localhost:27017/UnitTesting");
```

Register using a connection name from the appsettings.json

```c#
services.AddMongoDB("UnitTesting");
```

```json
{
  "ConnectionStrings": {
    "UnitTesting": "mongodb://localhost:27017/UnitTesting"
  }
}
```

### Usage

Find an entity by key

```c#
// dependency inject
var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();

// find by key
var role = await roleRepo.FindAsync("67a0dc52fa5ebe49f293a374");
```

Find one entity with query

```c#
// dependency inject
var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();

// find one by query expression
var role = await roleRepo.FindOneAsync(r => r.Name.StartsWith("Admin"))
```

Find many with query

```c#
// dependency inject
var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();

// find one by query expression
var roles = await roleRepo.FindAllAsync(r => r.Name.StartsWith("Admin"))
```

Use `IQueryable`

```c#
// dependency inject
var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();

// Use IQueryable
var roles = roleRepo.All()
    .Where(r => r.IsActive)
    .ToList();
```

Insert entity

```c#
// dependency inject
var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();

var role = new Role { Name = "CreateReadRole" };

var createdRole = await roleRepo.InsertAsync(role);
```

Update entity

```c#
// dependency inject
var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();

// find by key
var role = await roleRepo.FindAsync("67a0dc52fa5ebe49f293a374");

// make changes
role.Name = "UpdateRole";

var updateRole = await roleRepo.UpdateAsync(role);
```

Delete entity by key

```c#
// dependency inject
var roleRepo = Services.GetRequiredService<IMongoEntityRepository<Role>>();

// items deleted
var count = await roleRepo.DeleteAsync("67a0dc52fa5ebe49f293a374");
```

