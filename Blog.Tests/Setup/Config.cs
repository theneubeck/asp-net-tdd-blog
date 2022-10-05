using System.Text.Json;

namespace Blog.Tests.Setup;

public static class Config
{
    public static string DbTestCollectionName = "DbTestCollection";

    public static string DbTestConnectionString =
        "Data Source=test.db";
    public static JsonSerializerOptions JsonSerializerOptions => new() { PropertyNameCaseInsensitive = true };
}
