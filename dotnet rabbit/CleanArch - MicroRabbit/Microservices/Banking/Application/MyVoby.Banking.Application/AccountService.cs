using MyVoby.Banking.Domain.Entities;
using MyVoby.Banking.Domain.Interfaces;
using MyVoby.Banking.Domain.Interfaces.Services;

namespace MyVoby.Banking.Application;

public class AccountService : IAccountService
{
    public IAccountRepository _accountRepository { get; set; }

    public AccountService(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public IEnumerable<Account> GetAccounts()
    {
        return _accountRepository.GetAccounts();
    }
}