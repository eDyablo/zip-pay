using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using ZipPay.Api.Models;

namespace ZipPay.Api.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class AccountsController : ControllerBase {
    Database database;

    public AccountsController(Database database) {
      this.database = database;
    }

    [HttpGet]
    public IEnumerable<AccountRecord> Get() {
      return database.GetAllAccounts();
    }
  }
}
