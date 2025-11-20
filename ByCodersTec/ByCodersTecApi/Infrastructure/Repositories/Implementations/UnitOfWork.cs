using ByCodersTecApi.Infrastructure.Data;
using ByCodersTecApi.Infrastructure.Repositories.Interfaces;

namespace ByCodersTecApi.Infrastructure.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _db;

    public UnitOfWork(AppDbContext db)
    {
        _db = db;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        return _db.SaveChangesAsync(ct);
    }
}
