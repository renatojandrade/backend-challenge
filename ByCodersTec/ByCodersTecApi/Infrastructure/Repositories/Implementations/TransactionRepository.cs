using ByCodersTecApi.Domain.Entities;
using ByCodersTecApi.Infrastructure.Data;
using ByCodersTecApi.Infrastructure.Repositories.Interfaces;

namespace ByCodersTecApi.Infrastructure.Repositories.Implementations;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _db;

    public TransactionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
    {
        await _db.Transactions.AddAsync(transaction, ct);
    }

    public async Task AddRangeAsync(IEnumerable<Transaction> transactions, CancellationToken ct = default)
    {
        await _db.Transactions.AddRangeAsync(transactions, ct);
    }
}
