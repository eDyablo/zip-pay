using NUnit.Framework;
using ZipPay.Api.Models;

namespace ZipPay.Test.Models {
  public class UserRecordTest {
    [TestCase(0, 0, false)]
    [TestCase(999, 0, false)]
    [TestCase(1000, 0, true)]
    [TestCase(1001, 0, true)]
    [TestCase(0, 1000, false)]
    [TestCase(1000, 1000, false)]
    [TestCase(1000, 1001, false)]
    public void Can_create_account_when_salary_exeeds_expenses_by_specific_amount(
      float salary, float expenses, bool canCreate) {
      var record = new UserRecord {
        Salary = salary,
        Expenses = expenses
      };
      Assert.That(record.CanCreateAccount, Is.EqualTo(canCreate));
    }
  }
}
