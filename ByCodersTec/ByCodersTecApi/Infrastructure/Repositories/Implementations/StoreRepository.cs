using ByCodersTecApi.Domain.Entities;
using ByCodersTecApi.Infrastructure.Data;
using ByCodersTecApi.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ByCodersTecApi.Infrastructure.Repositories.Implementations;

public class StoreRepository : IStoreRepository
{
    private readonly AppDbContext _db;

    public StoreRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Store store, CancellationToken ct = default)
    {
        await _db.Stores.AddAsync(store, ct);
    }

    public async Task<Store?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.Stores.FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<Store?> GetByNameOwnerAsync(string name, string owner, CancellationToken ct = default)
    {
        return await _db.Stores.FirstOrDefaultAsync(s => s.Name == name && s.Owner == owner, ct);
    }
}
