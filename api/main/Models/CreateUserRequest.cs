namespace ZipPay.Api.Models {
  public class CreateUserRequest {
    public string Name { get; set; }

    public string Mail { get; set; }

    public float Salary { get; set; }

    public float Expenses { get; set; }

    public virtual bool IsValid { get {
      return !string.IsNullOrWhiteSpace(Name)
        && !string.IsNullOrWhiteSpace(Mail)
        && Salary >= 0
        && Expenses >= 0;
    } }
  }
}
