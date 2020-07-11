using Microsoft.AspNetCore.Mvc;

namespace api {
  [ApiController]
  [Route("[controller]")]
  public class AccountsController : ControllerBase {
    Database database;

    public AccountsController(Database database) {
      this.database = database;
    }

    [HttpGet]
    public string Get() {
      return database.Version;
    }
  }
}
