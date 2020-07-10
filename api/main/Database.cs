using Npgsql;
using System;
using System.Data.Common;

namespace api
{
  public class Database : IDisposable {
    NpgsqlConnection connection;
    public readonly string Version;

    public Database(string connectionString) {
      connection = new NpgsqlConnection(connectionString);
      connection.Open();
      Version = new NpgsqlCommand("SELECT version()", connection).ExecuteScalar().ToString();
    }

    public void Dispose() {
      connection.Close();
    }
  }
}
