using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using ZipPay.Api.Models;

namespace ZipPay.Api.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class UsersController : ControllerBase {
    Database database;

    public UsersController(Database database) {
      this.database = database;
    }

    [HttpGet]
    public IEnumerable<UserRecord> Get() {
      return database.GetAllUsers();
    }

    [HttpGet("{id}")]
    public ActionResult<UserRecord> Get(int id) {
      return Ok(database.GetUserById(id));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post([FromBody]CreateUserRequest request) {
      database.CreateUser(request);
      return Created("users", request);
    }
  }
}
