using System;
using System.Configuration;
using Microsoft.Data.Sqlite;
namespace Blog.Tests.Setup;

public class DatabaseFixture : IDisposable
{
    public void Dispose()
    {
        using SqliteConnection con = new(Config.DbTestConnectionString);
        con.Open();
        using var command = new SqliteCommand(@"
DELETE FROM Posts;
", con);
        command.ExecuteNonQuery();
    }
}
