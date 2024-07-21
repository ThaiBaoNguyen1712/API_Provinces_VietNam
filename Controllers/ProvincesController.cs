using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using API_Province_VietNam.Models;

[Route("api/[controller]")]
[ApiController]
public class ProvincesController : ControllerBase
{
    private readonly ProvincesVietNamContext _context;

    public ProvincesController(ProvincesVietNamContext context)
    {
        _context = context;
    }

    // GET: api/Provinces
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Province>>> GetProvinces()
    {
        return await _context.Provinces.ToListAsync();
    }

    // GET: api/Provinces/{code}
    [HttpGet("{code}")]
    public async Task<ActionResult<Province>> GetProvince(string code)
    {
        var province = await _context.Provinces.FirstOrDefaultAsync(x => x.Code == code);

        if (province == null)
        {
            return NotFound();
        }

        return province;
    }

    // POST: api/Provinces
    [HttpPost]
    public async Task<ActionResult<Province>> CreateProvince(Province province)
    {
        _context.Provinces.Add(province);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProvince), new { code = province.Code }, province);
    }

    // PUT: api/Provinces/{code}
    [HttpPut("{code}")]
    public async Task<IActionResult> UpdateProvince(string code, Province province)
    {
        if (code != province.Code)
        {
            return BadRequest();
        }

        _context.Entry(province).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProvinceExists(code))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Provinces/{code}
    [HttpDelete("{code}")]
    public async Task<IActionResult> DeleteProvince(string code)
    {
        var province = await _context.Provinces.FirstOrDefaultAsync(x => x.Code == code);
        if (province == null)
        {
            return NotFound();
        }

        _context.Provinces.Remove(province);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ProvinceExists(string code)
    {
        return _context.Provinces.Any(e => e.Code == code);
    }
}
