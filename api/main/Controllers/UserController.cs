using Microsoft.AspNetCore.Mvc;

namespace api
{
  [ApiController]
  [Route("[controller]")]
  public class UserController : ControllerBase {
    Database database;

    public UserController(Database database) {
      this.database = database;
    }

    [HttpGet]
    public string Get() {
      return database.Version;
    }
  }
}
