using demoWebApi.DAL;
using demoWebApi.Domain;
using demoWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoWebApi.Controllers;
[ApiController]
[Route("api")]
public class BreedController(WpmDbContext wpmDbContext) : ControllerBase
{
    private readonly WpmDbContext wpmDbContext = wpmDbContext;

    [HttpGet("breeds")]
    public async Task<ActionResult<IEnumerable<BreedViewModel>>> GetAllBreeds()
    {
        var allBreeds = await wpmDbContext.Breeds.Include(b => b.Species)
        .Select(b => new BreedViewModel(b.Id, b.Name, b.Species.Name)).ToListAsync();
        return Ok(allBreeds);
    }

    [HttpGet("species/{id}/breeds")]
    public async Task<ActionResult<IEnumerable<BreedViewModel>>> GetBreedsBySpecies(int id)
    {
        var breeds = await wpmDbContext.Breeds.Include(b => b.Species)
        .Where(b => b.SpeciesId == id)
        .Select(b => new BreedViewModel(b.Id, b.Name, b.Species.Name)).ToListAsync();
        return breeds.Count != 0 ? Ok(breeds) : NotFound();
    }

    [HttpPost("species/{speciesId}/breeds")]
    public async Task<IActionResult> CreateBreed(int speciesId, BreedModel breedModel)
    {
        var newBreed = new Breed()
        {
            Name = breedModel.Name,
            IdealMaxWeight = breedModel.IdealMaxWeight,
            SpeciesId = speciesId
        };

        wpmDbContext.Breeds.Add(newBreed);
        var result = await wpmDbContext.SaveChangesAsync();
        return result == 1 ? Ok(newBreed.Id) : BadRequest();
    }

    [HttpPut("breeds/{breedId}")]
    public async Task<IActionResult> UpdateBreed(int breedId, BreedModel breedModel)
    {
        var breed = wpmDbContext.Breeds.First(b => b.Id == breedId);
        breed.Name = breedModel.Name;
        breed.IdealMaxWeight = breedModel.IdealMaxWeight;
        var result = await wpmDbContext.SaveChangesAsync();
        return result == 1 ? Ok() : BadRequest();
    }
}