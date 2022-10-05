using Xunit;

namespace Blog.Tests.Setup;

[CollectionDefinition(nameof(Config.DbTestCollectionName))]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}

[Collection(nameof(Config.DbTestCollectionName))]
public class IntegrationTestBase : IClassFixture<TestWebApplicationFactory>
{
}
