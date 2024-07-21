using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using API_Province_VietNam.Models;

[Route("api/[controller]")]
[ApiController]
public class WardsController : ControllerBase
{
    private readonly ProvincesVietNamContext _context;

    public WardsController(ProvincesVietNamContext context)
    {
        _context = context;
    }

    // GET: api/Wards
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ward>>> GetWards()
    {
        return await _context.Wards.ToListAsync();
    }

    // GET: api/Wards/{code}
    [HttpGet("{code}")]
    public async Task<ActionResult<Ward>> GetWard(string code)
    {
        var ward = await _context.Wards
            .Include(w => w.AdministrativeUnit)
            .Include(w => w.DistrictCodeNavigation)
            .FirstOrDefaultAsync(w => w.Code == code);

        if (ward == null)
        {
            return NotFound();
        }

        return ward;
    }

    // POST: api/Wards
    [HttpPost]
    public async Task<ActionResult<Ward>> CreateWard(Ward ward)
    {
        _context.Wards.Add(ward);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWard), new { code = ward.Code }, ward);
    }

    // PUT: api/Wards/{code}
    [HttpPut("{code}")]
    public async Task<IActionResult> UpdateWard(string code, Ward ward)
    {
        if (code != ward.Code)
        {
            return BadRequest();
        }

        _context.Entry(ward).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!WardExists(code))
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

    // DELETE: api/Wards/{code}
    [HttpDelete("{code}")]
    public async Task<IActionResult> DeleteWard(string code)
    {
        var ward = await _context.Wards.FirstOrDefaultAsync(w => w.Code == code);
        if (ward == null)
        {
            return NotFound();
        }

        _context.Wards.Remove(ward);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool WardExists(string code)
    {
        return _context.Wards.Any(e => e.Code == code);
    }
}
