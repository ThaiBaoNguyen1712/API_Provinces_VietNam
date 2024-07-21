using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using API_Province_VietNam.Models;

[Route("api/[controller]")]
[ApiController]
public class DistrictsController : ControllerBase
{
    private readonly ProvincesVietNamContext _context;

    public DistrictsController(ProvincesVietNamContext context)
    {
        _context = context;
    }

    // GET: api/Districts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<District>>> GetDistricts()
    {
        return await _context.Districts.ToListAsync();
    }

    // GET: api/Districts/{code}
    [HttpGet("{code}")]
    public async Task<ActionResult<District>> GetDistrict(string code)
    {
        var district = await _context.Districts
            .Include(d => d.AdministrativeUnit)
            .Include(d => d.ProvinceCodeNavigation)
            .FirstOrDefaultAsync(d => d.Code == code);

        if (district == null)
        {
            return NotFound();
        }

        return district;
    }

    // POST: api/Districts
    [HttpPost]
    public async Task<ActionResult<District>> CreateDistrict(District district)
    {
        _context.Districts.Add(district);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDistrict), new { code = district.Code }, district);
    }

    // PUT: api/Districts/{code}
    [HttpPut("{code}")]
    public async Task<IActionResult> UpdateDistrict(string code, District district)
    {
        if (code != district.Code)
        {
            return BadRequest();
        }

        _context.Entry(district).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!DistrictExists(code))
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

    // DELETE: api/Districts/{code}
    [HttpDelete("{code}")]
    public async Task<IActionResult> DeleteDistrict(string code)
    {
        var district = await _context.Districts.FirstOrDefaultAsync(d => d.Code == code);
        if (district == null)
        {
            return NotFound();
        }

        _context.Districts.Remove(district);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool DistrictExists(string code)
    {
        return _context.Districts.Any(e => e.Code == code);
    }
}
