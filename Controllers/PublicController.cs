using API_Province_VietNam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Province_VietNam.Controllers
{
    [ApiController]
    [Route("/province")]
    public class PublicController : Controller
    {
        private readonly ProvincesVietNamContext _context;
        public PublicController(ProvincesVietNamContext provincesVietNamContext)
        {
            _context = provincesVietNamContext;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Province>>> GetAll()
        {
            var provinces = await _context.Provinces.Select(m=> new
            {
                m.Code,
                m.Name,
                m.FullName,
            }).ToListAsync();
            return Ok(provinces);
        }
        [HttpGet("district/{provinceCode}")]
        public async Task<IActionResult> GetDistrictByProvinceCode(string provinceCode)
        {
            var district = await _context.Districts
                .Where(x => x.ProvinceCode == provinceCode)
                .Select(m => new
                {
                    m.Code,
                    m.Name,
                    m.FullName,
                })
                .ToListAsync();

            if (district == null)
            {
                return NotFound();
            }

            return Ok(district);
        }
        [HttpGet("ward/{districtCode}")]
        public async Task<ActionResult> GetWardByDistrictCode(string districtCode)
        {
            var ward = await _context.Wards.Where(x=>x.DistrictCode == districtCode)
                .Select(m=> new { m.Code, m.Name, m.FullName }).ToListAsync();

            if (ward == null)
            {
                return NotFound();
            }
            return Ok(ward);
        }

    }
}
