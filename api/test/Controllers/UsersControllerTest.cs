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
    public void CreateUser_returns_uprocessable_entity_status_code_when_request_is_invalid() {
      // Arrange
      var request = Mock.Of<CreateUserRequest>();
      Mock.Get(request).SetupGet(r => r.IsValid).Returns(false);
      // Act
      var result = controller.CreateUser(request);
      // Assert
      Assert.That(result, Is.InstanceOf<ObjectResult>());
      Assert.That((result as ObjectResult).StatusCode,
        Is.EqualTo(StatusCodes.Status422UnprocessableEntity));
    }

    [Test]
    public void CreateUser_returns_created_user_record_when_request_is_valid() {
      // Arrange
      var request = Mock.Of<CreateUserRequest>();
      Mock.Get(request).SetupGet(r => r.IsValid).Returns(true);
      var record = Mock.Of<UserRecord>();
      Mock.Get(database).Setup(d => d.CreateUser(request)).Returns(record);
      // Act
      var result = controller.CreateUser(request);
      // Assert
      Assert.That(result, Is.InstanceOf<CreatedResult>());
      Assert.That((result as CreatedResult).Value, Is.SameAs(record));
    }

    [Test]
    public void CreateUser_returns_conflict_status_code_when_email_is_already_exist() {
      // Arrage
      Mock.Get(database).Setup(d => d.HasMailAddress("existent email")).Returns(true);
      // Act
      var result = controller.CreateUser(new CreateUserRequest{
        Name = "name",
        Mail = "existent email",
        Salary = 1,
        Expenses = 1
      });
      // Assert
      Assert.That(result, Is.InstanceOf<ObjectResult>());
      Assert.That((result as ObjectResult).StatusCode,
        Is.EqualTo(StatusCodes.Status409Conflict));
    }

    [Test]
    public void CreateUser_returns_created_user_record_when_email_does_not_exists() {
      // Arrage
      var request = new CreateUserRequest{
        Name = "name",
        Mail = "new email",
        Salary = 1,
        Expenses = 1
      };
      Mock.Get(database).Setup(d => d.HasMailAddress(It.IsAny<string>())).Returns(true);
      Mock.Get(database).Setup(d => d.HasMailAddress("new email")).Returns(false);
      var record = Mock.Of<UserRecord>();
      Mock.Get(database).Setup(d => d.CreateUser(request)).Returns(record);
      // Act
      var result = controller.CreateUser(request);
      // Assert
      Assert.That(result, Is.InstanceOf<CreatedResult>());
      Assert.That((result as CreatedResult).Value, Is.SameAs(record));
    }

    [Test]
    public void CreateAccount_returns_not_found_status_code_when_specified_user_id_does_not_exist() {
      // Arrange
      Mock.Get(database).Setup(d => d.GetUserById(It.IsAny<int>())).Returns<UserRecord>(null);
      var request = Mock.Of<CreateAccountRequest>();
      Mock.Get(request).SetupGet(r => r.IsValid).Returns(true);
      // Act
      var result = controller.CreateAccount(0, request);
      // Assert
      Assert.That(result, Is.InstanceOf<ObjectResult>());
      Assert.That((result as ObjectResult).StatusCode,
        Is.EqualTo(StatusCodes.Status404NotFound));
    }

    [Test]
    public void CreateAccount_returns_uprocessable_entity_status_code_when_user_can_not_create_account() {
      // Arrange
      var user = Mock.Of<UserRecord>();
      Mock.Get(user).SetupGet(u => u.CanCreateAccount).Returns(false);
      Mock.Get(database).Setup(d => d.GetUserById(It.IsAny<int>())).Returns(user);
      // Act
      var result = controller.CreateAccount(0, Mock.Of<CreateAccountRequest>());
      // Assert
      Assert.That(result, Is.InstanceOf<ObjectResult>());
      Assert.That((result as ObjectResult).StatusCode,
        Is.EqualTo(StatusCodes.Status422UnprocessableEntity));
    }

    [Test]
    public void CreateAccount_returns_created_account_record_when_user_can_create_account() {
      // Arrange
      var user = Mock.Of<UserRecord>();
      Mock.Get(user).SetupGet(u => u.CanCreateAccount).Returns(true);
      Mock.Get(database).Setup(d => d.GetUserById(It.IsAny<int>())).Returns(user);
      var account = Mock.Of<AccountRecord>();
      Mock.Get(database).Setup(d => d.CreateAccount(It.IsAny<CreateAccountRequest>()))
        .Returns(account);
      var request = Mock.Of<CreateAccountRequest>();
      Mock.Get(request).SetupGet(r => r.IsValid).Returns(true);
      // Act
      var result = controller.CreateAccount(0, request);
      // Assert
      Assert.That(result, Is.InstanceOf<CreatedResult>());
      Assert.That((result as CreatedResult).Value, Is.SameAs(account));
    }

    [Test]
    public void CreateAccount_returns_uprocessable_entity_status_code_when_request_is_invalid() {
      // Arrange
      var request = Mock.Of<CreateAccountRequest>();
      Mock.Get(request).SetupGet(r => r.IsValid).Returns(false);
      var user = Mock.Of<UserRecord>();
      Mock.Get(user).SetupGet(u => u.CanCreateAccount).Returns(true);
      Mock.Get(database).Setup(d => d.GetUserById(It.IsAny<int>())).Returns(user);
      // Act
      var result = controller.CreateAccount(0, request);
      // Assert
      // Assert
      Assert.That(result, Is.InstanceOf<ObjectResult>());
      Assert.That((result as ObjectResult).StatusCode,
        Is.EqualTo(StatusCodes.Status422UnprocessableEntity));
    }
  }
}
