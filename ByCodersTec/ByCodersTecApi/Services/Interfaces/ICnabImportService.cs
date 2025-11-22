using ByCodersTecApi.Services.Models;
using Microsoft.AspNetCore.Http;

namespace ByCodersTecApi.Services.Interfaces;

public interface ICnabImportService
{
    Task<CnabImportResult> ImportAsync(IFormFile file, CancellationToken ct = default);
}
