namespace MyVoby.Banking.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public string AccountType { get; set; }
    public decimal AccountBalance { get; set; }
}