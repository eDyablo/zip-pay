namespace ZipPay.Api.Models {
  public class UserRecord {
    public int Id { get; set; }

    public string Name { get; set; }

    public string Mail { get; set; }

    public float Salary { get; set; }

    public float Expenses { get; set; }

    public virtual bool CanCreateAccount { get {
      return Salary - Expenses >= 1000;
    } }
  }
}
