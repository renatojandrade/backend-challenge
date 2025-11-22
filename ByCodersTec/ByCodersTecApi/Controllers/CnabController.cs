using ByCodersTecApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ByCodersTecApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CnabController : ControllerBase
{
    private readonly ICnabImportService _cnabImportService;

    public CnabController(ICnabImportService cnabImportService)
    {
        _cnabImportService = cnabImportService;
    }

    [HttpPost("import")]
    public async Task<IActionResult> Import([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is required.");

        var result = await _cnabImportService.ImportAsync(file, ct);

        return Ok(result);
    }
}
