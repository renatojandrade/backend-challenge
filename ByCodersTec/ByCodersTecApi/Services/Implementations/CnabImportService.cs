using System.Globalization;
using ByCodersTecApi.Domain.Entities;
using ByCodersTecApi.Domain.Enums;
using ByCodersTecApi.Infrastructure.Repositories.Interfaces;
using ByCodersTecApi.Services.Interfaces;
using ByCodersTecApi.Services.Models;

namespace ByCodersTecApi.Services.Implementations;

public class CnabImportService : ICnabImportService
{
    private readonly IStoreRepository _storeRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _uow;

    public CnabImportService(
        IStoreRepository storeRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork uow)
    {
        _storeRepository = storeRepository;
        _transactionRepository = transactionRepository;
        _uow = uow;
    }

    public async Task<CnabImportResult> ImportAsync(IFormFile file, CancellationToken ct = default)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty.");

        var result = new CnabImportResult();
        var transactions = new List<Transaction>();
        var storeCache = new Dictionary<string, Store>(StringComparer.OrdinalIgnoreCase);
        var storesCreated = 0;

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        string? line;

        while ((line = await reader.ReadLineAsync()) != null)
        {
            result.LinesRead++;
            if (string.IsNullOrWhiteSpace(line))
            {
                result.LinesSkipped++;
                continue;
            }

            try
            {
                var type = (TransactionType)ParseInt(line.Substring(0, 1));

                var dateStr = line.Substring(1, 8);
                var amountStr = line.Substring(9, 10);
                var cpf = line.Substring(19, 11);
                var card = line.Substring(30, 12);
                var timeStr = line.Substring(42, 6);
                var owner = line.Substring(48, 14).TrimEnd();
                var storeName = line.Substring(62).TrimEnd();

                var occurredAt = ParseOccurredAt(dateStr, timeStr);

                var amount = decimal.Parse(amountStr, NumberStyles.None, CultureInfo.InvariantCulture);
                var normalizedAmount = amount / 100m;

                var storeKey = $"{storeName}|{owner}";

                if (!storeCache.TryGetValue(storeKey, out var store))
                {
                    store = await _storeRepository.GetByNameOwnerAsync(storeName, owner, ct) ?? new Store
                    {
                        Name = storeName,
                        Owner = owner
                    };

                    if (store.Id == 0)
                    {
                        await _storeRepository.AddAsync(store, ct);
                        storesCreated++;
                    }

                    storeCache[storeKey] = store;
                }

                var transaction = new Transaction
                {
                    Type = type,
                    OccurredAt = occurredAt,
                    Value = normalizedAmount,
                    Cpf = cpf,
                    Card = card,
                    Store = store
                };

                transactions.Add(transaction);
            }
            catch(Exception ex)
            {
                result.LinesSkipped++;
            }
        }

        if (transactions.Count > 0)
        {
            await _transactionRepository.AddRangeAsync(transactions, ct);
            await _uow.SaveChangesAsync(ct);
        }

        result.TransactionsImported = transactions.Count;
        result.StoresCreated = storesCreated;

        return result;
    }

    private static int ParseInt(ReadOnlySpan<char> s)
    {
        var val = 0;
        for (int i = 0; i < s.Length; i++)
        {
            val = val * 10 + (s[i] - '0');
        }
        return val;
    }

    private static DateTime ParseOccurredAt(string dateStr, string timeStr)
    {
        var year = int.Parse(dateStr.AsSpan(0, 4), CultureInfo.InvariantCulture);
        var month = int.Parse(dateStr.AsSpan(4, 2), CultureInfo.InvariantCulture);
        var day = int.Parse(dateStr.AsSpan(6, 2), CultureInfo.InvariantCulture);

        var hour = int.Parse(timeStr.AsSpan(0, 2), CultureInfo.InvariantCulture);
        var minute = int.Parse(timeStr.AsSpan(2, 2), CultureInfo.InvariantCulture);
        var second = int.Parse(timeStr.AsSpan(4, 2), CultureInfo.InvariantCulture);

        return new DateTime(year, month, day, hour, minute, second, DateTimeKind.Unspecified);
    }
}
