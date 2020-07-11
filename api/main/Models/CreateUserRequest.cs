namespace ZipPay.Api.Models {
  public class CreateUserRequest {
    public string Name { get; set; }

    public string Mail { get; set; }

    public float Salary { get; set; }

    public float Expenses { get; set; }
  }
}
