using System.Text.Json;
using System.Text;
using System;
using System.Net.Http;

namespace Blog.Tests;

public static class Utils {
    
    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string ToJson<T>(T obj)
    {
        return JsonSerializer.Serialize<T>(obj, JsonSerializerOptions);
    }
    
    public static StringContent ToJsonStringContent<T>(T obj)
    {
        return new StringContent(JsonSerializer.Serialize<T>(obj, JsonSerializerOptions));
    }

}
