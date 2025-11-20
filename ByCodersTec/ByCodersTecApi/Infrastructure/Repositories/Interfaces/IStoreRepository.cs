using ByCodersTecApi.Domain.Entities;

namespace ByCodersTecApi.Infrastructure.Repositories.Interfaces;

public interface IStoreRepository
{
    Task<Store?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Store?> GetByNameOwnerAsync(string name, string owner, CancellationToken ct = default);
    Task AddAsync(Store store, CancellationToken ct = default);
}
