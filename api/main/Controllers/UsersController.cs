using Microsoft.AspNetCore.Mvc;
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
    public string Get() {
      return database.Version;
    }

    [HttpPost]
    public string Post([FromBody]UserRecord record) {
      var answer = new StringBuilder();
      answer.AppendLine($"name:             {record.Name}");
      answer.AppendLine($"mail:             {record.Mail}");
      answer.AppendLine($"monthly salary:   {record.Salary}");
      answer.AppendLine($"monthly expenses: {record.Expenses}");
      return answer.ToString();
    }
  }
}
