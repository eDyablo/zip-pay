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
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult Create([FromBody]CreateUserRequest request) {
      if (!request.IsValid) {
        return StatusCode(StatusCodes.Status422UnprocessableEntity);
      }
      if (database.HasMailAddress(request.Mail)) {
        return StatusCode(StatusCodes.Status409Conflict);
      }
      var record = database.CreateUser(request);
      return Created("users", record);
    }

    [HttpGet("{id}/accounts")]
    public IEnumerable<AccountRecord> GetAccounts(int id) {
      return database.GetUserAccounts(id);
    }

    [HttpPost("{id}/accounts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult CreateAccount(int id, [FromBody]CreateAccountRequest request) {
      request.UserId = id;
      database.CreateAccount(request);
      return Created("accounts", request);
    }
  }
}
