using MyVoby.Banking.Data.Context;
using MyVoby.Banking.Domain.Entities;
using MyVoby.Banking.Domain.Interfaces;

namespace MyVoby.Banking.Data.Repository;

public class AccountRepository : IAccountRepository
{
    private BankingDbContext _db;

    public AccountRepository(BankingDbContext db)
    {
        _db = db;
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _db.Accounts;
    }
}