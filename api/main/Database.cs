using Npgsql;
using System;

namespace api
{
  public class Database : IDisposable {
    NpgsqlConnection connection;
    public readonly string Version;

    public Database(string connectionString) {
      connection = new NpgsqlConnection(connectionString);
      connection.Open();
      Version = ExecuteScalar("SELECT version()").ToString();

      ExecuteNonQuery(@"
        |CREATE TABLE IF NOT EXISTS users(
        | id SERIAL PRIMARY KEY,
        | name VARCHAR(255),
        | email VARCHAR(255),
        | salary real,
        | expenses real
        |)".StripMargin());

      ExecuteNonQuery(@"
        |CREATE TABLE IF NOT EXISTS accounts(
        | id SERIAL PRIMARY KEY,
        | user_id INTEGER REFERENCES users(id),
        | name VARCHAR(255)
        |)".StripMargin());
    }

    public void Dispose() {
      connection.Close();
    }

    private object ExecuteScalar(string command) {
      return new NpgsqlCommand(command, connection).ExecuteScalar();
    }

    private void ExecuteNonQuery(string command) {
      new NpgsqlCommand(command, connection).ExecuteNonQuery();
    }
  }
}
