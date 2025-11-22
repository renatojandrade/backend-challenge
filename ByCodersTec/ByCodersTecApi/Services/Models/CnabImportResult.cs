namespace ByCodersTecApi.Services.Models;

public class CnabImportResult
{
    public int LinesRead { get; set; }
    public int TransactionsImported { get; set; }
    public int StoresCreated { get; set; }
    public int LinesSkipped { get; set; }
}
