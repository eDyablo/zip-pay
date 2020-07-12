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
    public IEnumerable<UserRecord> GetUsers() {
      return database.GetAllUsers();
    }

    [HttpGet("{id}")]
    public ActionResult<UserRecord> GetUser(int id) {
      return Ok(database.GetUserById(id));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult CreateUser([FromBody]CreateUserRequest request) {
      if (!request.IsValid) {
        return StatusCode(StatusCodes.Status422UnprocessableEntity,
          new { reason = "specified user data is invalid", data = request });
      }
      if (database.HasMailAddress(request.Mail)) {
        return StatusCode(StatusCodes.Status409Conflict,
          new { reason = "specified user mail is already registered", mail = request.Mail });
      }
      return Created("users", database.CreateUser(request));
    }

    [HttpGet("{id}/accounts")]
    public IEnumerable<AccountRecord> GetAccounts(int id) {
      return database.GetUserAccounts(id);
    }

    [HttpPost("{id}/accounts")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public IActionResult CreateAccount(int id, [FromBody]CreateAccountRequest request) {
      request.UserId = id;
      if (!request.IsValid) {
        return StatusCode(StatusCodes.Status422UnprocessableEntity,
          new { reason = "specified account data is not valid", data = request });
      }
      var user = database.GetUserById(id);
      if (user == null) {
        return StatusCode(StatusCodes.Status404NotFound,
          new { reason = "specified user id is not found", id = id });
      }
      if (!user.CanCreateAccount) {
        return StatusCode(StatusCodes.Status422UnprocessableEntity,
          new { reason = "can not create account for the user", user = user });
      }
      return Created("accounts", database.CreateAccount(request));
    }
  }
}
