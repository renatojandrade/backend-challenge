namespace ByCodersTecApi.Domain.Entities;

public class Store
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;

    public ICollection<Transaction> Transactions { get; set; } = [];
}
