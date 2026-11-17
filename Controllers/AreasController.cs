using AgroSync.Api.Data;
using AgroSync.Api.Dtos;
using AgroSync.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgroSync.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AreasController : ControllerBase
{
    private readonly AppDbContext _context;

    public AreasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AreaResponse>>> GetAll()
    {
        var areas = await _context.Areas
            .AsNoTracking()
            .OrderBy(a => a.Id)
            .ToListAsync();

        return Ok(areas.Select(ToResponse));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AreaResponse>> GetById(int id)
    {
        var area = await _context.Areas
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (area is null)
            return NotFound(new { mensagem = $"Área {id} não encontrada." });

        return Ok(ToResponse(area));
    }

    private static AreaResponse ToResponse(Area area) =>
        new(area.Id, area.Nome, area.TamanhoHectares, area.Latitude, area.Longitude);
}