using ByCodersTecApi.Domain.Entities;

namespace ByCodersTecApi.Infrastructure.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction, CancellationToken ct = default);
    Task AddRangeAsync(IEnumerable<Transaction> transactions, CancellationToken ct = default);
}
