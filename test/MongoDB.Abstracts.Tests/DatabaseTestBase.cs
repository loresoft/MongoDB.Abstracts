using System;

using XUnit.Hosting;

namespace MongoDB.Abstracts.Tests;

[Collection(DatabaseCollection.CollectionName)]
public abstract class DatabaseTestBase : TestHostBase<DatabaseFixture>
{
    protected DatabaseTestBase(ITestOutputHelper output, DatabaseFixture databaseFixture)
    : base(output, databaseFixture)
    {
    }

    public IServiceProvider ServiceProvider => Fixture.Services;
}
