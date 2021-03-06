using Npgsql;
using System;
using System.Collections.Generic;
using ZipPay.Api.Models;

namespace ZipPay.Api {
  public interface Database {
    UserRecord CreateUser(CreateUserRequest request);

    IEnumerable<UserRecord> GetAllUsers();

    UserRecord GetUserById(int id);

    IEnumerable<AccountRecord> GetAllAccounts();

    AccountRecord CreateAccount(CreateAccountRequest request);

    IEnumerable<AccountRecord> GetUserAccounts(int userId);

    bool HasMailAddress(string address);
  }
}
