namespace ZipPay.Api.Models {
  public class CreateAccountRequest {
    public string Name { get; set; }

    public int UserId { get; set; }

    public virtual bool IsValid { get {
      return !string.IsNullOrWhiteSpace(Name)
        && UserId != 0;
    } }
  }
}
