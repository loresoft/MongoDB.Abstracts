# MongoDB.Abstracts

## Overview

The MongoDB Abstracts library defines abstract base classes for repository pattern.

[![Build status](https://ci.appveyor.com/api/projects/status/4linx1kr2kqlohpg?svg=true)](https://ci.appveyor.com/project/LoreSoft/mongodb-abstracts)   

[![NuGet Version](https://img.shields.io/nuget/v/MongoDB.Abstracts.svg?style=flat-square)](https://www.nuget.org/packages/MongoDB.Abstracts/)   

[![Coverage Status](https://coveralls.io/repos/github/loresoft/MongoDB.Abstracts/badge.svg?branch=master)](https://coveralls.io/github/loresoft/MongoDB.Abstracts?branch=master)

## Download

The MongoDB.Abstracts library is available on nuget.org via package name `MongoDB.Abstracts`.

To install MongoDB.Abstracts, run the following command in the Package Manager Console

    PM> Install-Package MongoDB.Abstracts
    
More information about NuGet package available at
<https://nuget.org/packages/MongoDB.Abstracts>

## Development Builds

Development builds are available on the myget.org feed.  A development build is promoted to the main NuGet feed when it's determined to be stable. 

In your Package Manager settings add the following package source for development builds:
<http://www.myget.org/F/loresoft/>

### Features

* interface for generic MongoDB queries; `IMongoQuery<TEntity, TKey>`
* interface for generic MongoDB repository; `IMongoRepository<TEntity, TKey>`
* base class for generic MongoDB queries; `MongoQuery<TEntity, TKey>`
* base class for generic MongoDB repository; `MongoRepository<TEntity, TKey>`
* interface for generic MongoDB entity; `IMongoEntity`
* base class for generic MongoDB entity; `MongoEntity`
* interface for generic MongoDB entity repository; `IMongoEntityRepository<TEntity>`
* base class for generic MongoDB entity repository; `MongoEntityRepostiory<TEntity>`
