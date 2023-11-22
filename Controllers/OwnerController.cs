using demoWebApi.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoWebApi.Controllers;
[ApiController]
[Route("api")]
public class OwnerController(WpmDbContext wpmDbContext) : ControllerBase
{
    private readonly WpmDbContext wpmDbContext = wpmDbContext;

    [HttpGet("pets/{id}/owners")]
    public async Task<IActionResult> GetOwnersByPet(int id)
    {
        var owners = await wpmDbContext.Pets.Include(p => p.Owners)
        .Where(p => p.Id == id)
        .SelectMany(p => p.Owners)
        .Select(p => new { p.Id, p.Name })
        .ToListAsync();
        return owners != null ? Ok(owners) : NotFound(id);
    }
}