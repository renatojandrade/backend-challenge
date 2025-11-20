using ByCodersTecApi.Domain.Enums;

namespace ByCodersTecApi.Domain.Extensions;

public static class TransactionTypeExtensions
{
    public static int GetTransactionSign(this TransactionType type)
    {
        return type switch
        {
            TransactionType.Boleto
                or TransactionType.Financing
                or TransactionType.Rent => -1,
            _ => 1
        };
    }
}
