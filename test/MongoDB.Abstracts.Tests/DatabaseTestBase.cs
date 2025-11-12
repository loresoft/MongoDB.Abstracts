using System;

using XUnit.Hosting;

namespace MongoDB.Abstracts.Tests;

[Collection(DatabaseCollection.CollectionName)]
public abstract class DatabaseTestBase(DatabaseFixture databaseFixture) : TestHostBase<DatabaseFixture>(databaseFixture)
{
}
