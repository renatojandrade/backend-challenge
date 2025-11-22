using ByCodersTecApi.Domain.Enums;
using ByCodersTecApi.Domain.Extensions;

namespace ByCodersTecApi.Domain.Entities;

public class Transaction
{
    public long Id { get; set; }
    public TransactionType Type { get; set; }
    public DateTime OccurredAt { get; set; }
    public decimal Value { get; set; }
    public string Cpf { get; set; } = string.Empty;
    public string Card { get; set; } = string.Empty;

    public int StoreId { get; set; }
    public Store? Store { get; set; }
}
