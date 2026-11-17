using AgroSync.Api.Data;
using AgroSync.Api.Dtos;
using AgroSync.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSync.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeiturasController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeiturasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LeituraSoloResponse>>> GetAll([FromQuery] int? areaId)
    {
        var query = _context.Leituras.AsNoTracking();

        if (areaId.HasValue)
            query = query.Where(l => l.AreaId == areaId.Value);

        var leituras = await query.OrderBy(l => l.Id).ToListAsync();

        return Ok(leituras.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LeituraSoloResponse>> GetById(int id)
    {
        var leitura = await _context.Leituras
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == id);

        if (leitura is null)
            return NotFound(new { mensagem = $"Leitura {id} não encontrada." });

        return Ok(ToResponse(leitura));
    }

    private static LeituraSoloResponse ToResponse(LeituraSolo l) =>
        new(l.Id, l.AreaId, l.UmidadePercentual, l.DataHora, l.Origem, l.DispositivoId);
}