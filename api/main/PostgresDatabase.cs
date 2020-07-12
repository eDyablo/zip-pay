using Npgsql;
using System;
using System.Collections.Generic;
using ZipPay.Api.Models;

namespace ZipPay.Api {
  public class PostgresDatabase : IDisposable, Database {
    NpgsqlConnection connection;
    public readonly string Version;

    public PostgresDatabase(string connectionString) {
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

    public int CreateUser(CreateUserRequest request) {
      using var command = new NpgsqlCommand(@"
        |INSERT INTO users(name, email, salary, expenses)
        |VALUES(@name, @email, @salary, @expenses)
        |RETURNING id".StripMargin(), connection);
      command.Parameters.AddWithValue("name", request.Name);
      command.Parameters.AddWithValue("email", request.Mail);
      command.Parameters.AddWithValue("salary", request.Salary);
      command.Parameters.AddWithValue("expenses", request.Expenses);
      command.Prepare();
      return Convert.ToInt32(command.ExecuteScalar());
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

    public UserRecord GetUserById(int id) {
      using var command = new NpgsqlCommand(
        $"SELECT id, name, email, salary, expenses FROM users WHERE id = {id}", connection);
      using var reader = command.ExecuteReader();
      if (reader.Read()) {
        return new UserRecord {
          Id = reader.GetInt32(0),
          Name = reader.GetString(1),
          Mail = reader.GetString(2),
          Salary = reader.GetFloat(3),
          Expenses = reader.GetFloat(4)
        };
      } else {
        return null;
      }
    }

    public IEnumerable<AccountRecord> GetAllAccounts() {
      using var command = new NpgsqlCommand(
        "SELECT id, user_id, name FROM accounts", connection);
      using var reader = command.ExecuteReader();
      while (reader.Read()) {
        yield return new AccountRecord {
          Id = reader.GetInt32(0),
          UserId = reader.GetInt32(1),
          Name = reader.GetString(2),
        };
      }
    }

    public int CreateAccount(CreateAccountRequest request) {
      using var command = new NpgsqlCommand(@"
        |INSERT INTO accounts(name, user_id)
        |VALUES(@name, @user_id)
        |RETURNING id".StripMargin(), connection);
      command.Parameters.AddWithValue("name", request.Name);
      command.Parameters.AddWithValue("user_id", request.UserId);
      command.Prepare();
      return Convert.ToInt32(command.ExecuteScalar());
    }

    public IEnumerable<AccountRecord> GetUserAccounts(int userId) {
      using var command = new NpgsqlCommand(
        $"SELECT id, user_id, name FROM accounts WHERE user_id = {userId}", connection);
      using var reader = command.ExecuteReader();
      while (reader.Read()) {
        yield return new AccountRecord {
          Id = reader.GetInt32(0),
          UserId = reader.GetInt32(1),
          Name = reader.GetString(2),
        };
      }
    }

    public bool HasMailAddress(string address) {
      using var command = new NpgsqlCommand(
        $"SELECT id FROM users WHERE email = '{address}'", connection);
      using var reader = command.ExecuteReader();
      return reader.Read();
    }
  }
}
