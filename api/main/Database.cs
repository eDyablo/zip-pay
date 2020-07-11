using Npgsql;
using System;
using System.Collections.Generic;
using ZipPay.Api.Models;

namespace ZipPay.Api {
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

    private object ExecuteScalar(string clause) {
      using var command = new NpgsqlCommand(clause, connection);
      return command.ExecuteScalar();
    }

    private void ExecuteNonQuery(string clause) {
      using var command = new NpgsqlCommand(clause, connection);
      command.ExecuteNonQuery();
    }

    public void CreateUser(CreateUserRequest request) {
      using var command = new NpgsqlCommand(
        @"INSERT INTO users(name, email, salary, expenses) VALUES(@name, @email, @salary, @expenses)",
        connection);
      command.Parameters.AddWithValue("name", request.Name);
      command.Parameters.AddWithValue("email", request.Mail);
      command.Parameters.AddWithValue("salary", request.Salary);
      command.Parameters.AddWithValue("expenses", request.Expenses);
      command.Prepare();
      command.ExecuteNonQuery();
    }

    public IEnumerable<UserRecord> GetAllUsers() {
      using var command = new NpgsqlCommand(
        "SELECT id, name, email, salary, expenses FROM users", connection);
      using var reader = command.ExecuteReader();
      while (reader.Read()) {
        yield return new UserRecord {
          Id = reader.GetInt32(0),
          Name = reader.GetString(1),
          Mail = reader.GetString(2),
          Salary = reader.GetFloat(3),
          Expenses = reader.GetFloat(4)
        };
      }
    }
  }
}
