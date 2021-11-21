using Microsoft.EntityFrameworkCore;
using MyVoby.Banking.Domain.Entities;

namespace MyVoby.Banking.Data.Context;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
}