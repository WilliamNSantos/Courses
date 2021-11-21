using MyVoby.Banking.Domain.Entities;

namespace MyVoby.Banking.Domain.Interfaces
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAccounts();
    }
}