using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using demoWebApi.Models;
using demoWebApi.DAL;

namespace demoWebApi.Controllers;
[ApiController]
[Route("api")]
public class SpeciesController(WpmDbContext wpmDbContext) : ControllerBase
{
    private readonly WpmDbContext wpmDbContext = wpmDbContext;

    [HttpGet("species")]
    public async Task<ActionResult<IEnumerable<SpeciesViewModel>>> GetAllSpecies()
    {
        var allSpecies = await wpmDbContext.Species
        .Select(s => new SpeciesViewModel(s.Id, s.Name)).ToListAsync();
        return Ok(allSpecies);
    }
}