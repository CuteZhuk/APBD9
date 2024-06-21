using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly TripContext _context;

        public TripsController(TripContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var totalTrips = await _context.Trips.CountAsync();
            var totalPages = (int)System.Math.Ceiling(totalTrips / (double)pageSize);

            var trips = await _context.Trips
                .Include(t => t.CountryTrips)
                .ThenInclude(ct => ct.IdCountryNavigation)
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    t.Name,
                    t.Description,
                    t.DateFrom,
                    t.DateTo,
                    t.MaxPeople,
                    Countries = t.CountryTrips.Select(ct => new { ct.IdCountryNavigation.Name }),
                    Clients = t.ClientTrips.Select(ct => new { ct.IdClientNavigation.FirstName, ct.IdClientNavigation.LastName })
                })
                .ToListAsync();

            return Ok(new
            {
                pageNum = page,
                pageSize = pageSize,
                allPages = totalPages,
                trips
            });
        }
    }
}

