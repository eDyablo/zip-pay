using Microsoft.AspNetCore.Mvc;
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
    public string Get() {
      return database.Version;
    }

    [HttpPost]
    public string Post([FromBody] CreateAccountRequest request) {
      var answer = new StringBuilder();
      answer.AppendLine($"name: {request.Name}");
      answer.AppendLine($"user: {request.User}");
      return answer.ToString();
    }
  }
}
