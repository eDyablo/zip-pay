using NUnit.Framework;
using System;
using ZipPay.Api.Models;

namespace ZipPay.Test.Models {
  public class CreateAccountRequestTest {
    [TestCase(null, false)]
    [TestCase("name", true)]
    [TestCase("", false)]
    [TestCase(" ", false)]
    [TestCase("\t", false)]
    [TestCase(" \t  \t\t", false)]
    public void IsValid_validates_name(string name, bool valid) {
      var request = new CreateAccountRequest { Name = name, UserId = 1 };
      Assert.That(request.IsValid, Is.EqualTo(valid));
    }

    [TestCase(0, false)]
    [TestCase(Int32.MinValue, true)]
    [TestCase(Int32.MaxValue, true)]
    public void IsValid_validates_user_id(int id, bool valid) {
      var request = new CreateAccountRequest { Name = "name", UserId = id };
      Assert.That(request.IsValid, Is.EqualTo(valid));
    }
  }
}
