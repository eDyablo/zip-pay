using NUnit.Framework;
using ZipPay.Api.Models;

namespace ZipPay.Test.Models {
  public class CreateUserAccountRequestTest {
    [Test]
    public void Newly_created_request_is_not_valid() {
      var request = new CreateUserRequest();
      Assert.That(request.IsValid, Is.False);
    }

    [Test]
    public void Is_valid_when_all_fields_are_properly_specified() {
      var request = new CreateUserRequest {
        Name = "name",
        Mail = "mail",
        Salary = 1,
        Expenses = 1
      };
      Assert.That(request.IsValid, Is.True);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\t")]
    [TestCase(" \t  \t\t")]
    public void Is_not_valid_when_name_is_null_or_empty_or_whitespaces_only(string name) {
      var request = new CreateUserRequest {
        Name = name,
        Mail = "mail",
        Salary = 1,
        Expenses = 1
      };
      Assert.That(request.IsValid, Is.False);
    }

    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\t")]
    [TestCase(" \t  \t\t")]
    public void Is_not_valid_when_mail_is_null_or_empty_or_whitespaces_only(string mail) {
      var request = new CreateUserRequest {
        Name = "name",
        Mail = mail,
        Salary = 1,
        Expenses = 1
      };
      Assert.That(request.IsValid, Is.False);
    }

    [TestCase(0, false)]
    [TestCase(1, true)]
    [TestCase(-1, false)]
    public void IsValid_checks_salary(int salary, bool valid) {
      var request = new CreateUserRequest {
        Name = "name",
        Mail = "mail",
        Salary = salary,
        Expenses = 1
      };
      Assert.That(request.IsValid, Is.EqualTo(valid));
    }

    [TestCase(0, false)]
    [TestCase(1, true)]
    [TestCase(-1, false)]
    public void IsValid_checks_expenses(int expenses, bool valid) {
      var request = new CreateUserRequest {
        Name = "name",
        Mail = "mail",
        Salary = 1,
        Expenses = expenses
      };
      Assert.That(request.IsValid, Is.EqualTo(valid));
    }
  }
}
