using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using API_Province_VietNam.Models;

public class AdministrativeDataController : ControllerBase
{
    private readonly ProvincesVietNamContext _context;
    private readonly IWebHostEnvironment _environment;
    public AdministrativeDataController(ProvincesVietNamContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportToJson()
    {
        var provinces = await _context.Provinces
            .Select(p => new
            {
                code = p.Code,
                name = p.Name,
                name_en = p.NameEn,
                full_name = p.FullName,
                full_name_en = p.FullNameEn,
                code_name = p.CodeName,
                administrative_unit_id = p.AdministrativeUnitId,
                administrative_region_id = p.AdministrativeRegionId,
                districts = p.Districts.Select(d => new
                {
                    code = d.Code,
                    name = d.Name,
                    name_en = d.NameEn,
                    full_name = d.FullName,
                    full_name_en = d.FullNameEn,
                    code_name = d.CodeName,
                    province_code = d.ProvinceCode,
                    administrative_unit_id = d.AdministrativeUnitId,
                    wards = d.Wards.Select(w => new
                    {
                        code = w.Code,
                        name = w.Name,
                        name_en = w.NameEn,
                        full_name = w.FullName,
                        full_name_en = w.FullNameEn,
                        code_name = w.CodeName,
                        district_code = w.DistrictCode,
                        administrative_unit_id = w.AdministrativeUnitId
                    }).ToList()
                }).ToList()
            })
            .ToListAsync();

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string jsonString = JsonSerializer.Serialize(provinces, options);

        string wwwrootPath = _environment.WebRootPath;
        if (string.IsNullOrEmpty(wwwrootPath))
        {
            wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            Directory.CreateDirectory(wwwrootPath);
        }

        string filePath = Path.Combine(wwwrootPath, "administrative_data.json");
        await System.IO.File.WriteAllTextAsync(filePath, jsonString);

        return Ok("JSON file has been created successfully at " + filePath);
    }


    [HttpGet("provinces")]
    public async Task<IActionResult> GetProvinces()
    {
        var provinces = await _context.Provinces
            .Select(p => new { code = p.Code, name = p.Name, full_name = p.FullName })
            .ToListAsync();
        return Ok(provinces);
    }

    [HttpGet("provinces/{code}")]
    public async Task<IActionResult> GetProvince(string code)
    {
        var province = await _context.Provinces
            .Where(p => p.Code == code)
            .Select(p => new
            {
                code = p.Code,
                name = p.Name,
                full_name = p.FullName,
                districts = p.Districts.Select(d => new { code = d.Code, name = d.Name, full_name = d.FullName })
            })
            .FirstOrDefaultAsync();

        if (province == null)
            return NotFound();

        return Ok(province);
    }

    [HttpGet("districts/{code}")]
    public async Task<IActionResult> GetDistrict(string code)
    {
        var district = await _context.Districts
            .Where(d => d.Code == code)
            .Select(d => new
            {
                code = d.Code,
                name = d.Name,
                full_name = d.FullName,
                wards = d.Wards.Select(w => new { code = w.Code, name = w.Name, full_name = w.FullName })
            })
            .FirstOrDefaultAsync();

        if (district == null)
            return NotFound();

        return Ok(district);
    }
}