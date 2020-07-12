using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ZipPay.Api;
using ZipPay.Api.Controllers;
using ZipPay.Api.Models;

namespace ZipPay.Test.Controllers {
  public class UsersControllerTest {
    UsersController controller;
    Database database;

    [SetUp]
    public void BeforeEachTest() {
      database = Mock.Of<Database>();
      controller = new UsersController(database);
    }

    [Test]
    public void Create_returns_uprocessable_entity_status_code_when_request_is_invalid() {
      // Arrange
      var request = Mock.Of<CreateUserRequest>();
      Mock.Get(request).SetupGet(r => r.IsValid).Returns(false);
      // Act
      var result = controller.Create(request) as StatusCodeResult;
      // Assert
      Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status422UnprocessableEntity));
    }

    [Test]
    public void Create_returns_created_user_record_when_request_is_valid() {
      // Arrange
      var request = Mock.Of<CreateUserRequest>();
      Mock.Get(request).SetupGet(r => r.IsValid).Returns(true);
      var record = new UserRecord();
      Mock.Get(database).Setup(d => d.CreateUser(request)).Returns(record);
      // Act
      var result = controller.Create(request) as CreatedResult;
      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Value, Is.SameAs(record));
    }

    [Test]
    public void Create_returns_conflict_status_code_when_email_is_already_exist() {
      // Arrage
      Mock.Get(database).Setup(d => d.HasMailAddress("existent email")).Returns(true);
      // Act
      var result = controller.Create(new CreateUserRequest{
        Name = "name",
        Mail = "existent email"
      }) as StatusCodeResult;
      // Assert
      Assert.That(result.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
    }

    [Test]
    public void Create_returns_created_user_record_when_email_does_not_exists() {
      // Arrage
      var request = new CreateUserRequest{
        Name = "name",
        Mail = "new email"
      };
      Mock.Get(database).Setup(d => d.HasMailAddress(It.IsAny<string>())).Returns(true);
      Mock.Get(database).Setup(d => d.HasMailAddress("new email")).Returns(false);
      var record = new UserRecord();
      Mock.Get(database).Setup(d => d.CreateUser(request)).Returns(record);
      // Act
      var result = controller.Create(request) as CreatedResult;
      // Assert
      Assert.That(result, Is.Not.Null);
      Assert.That(result.Value, Is.SameAs(record));
    }
  }
}
