using MyVoby.Banking.Domain.Entities;

namespace MyVoby.Banking.Domain.Interfaces.Services;

public interface IAccountService
{
    IEnumerable<Account> GetAccounts();
}