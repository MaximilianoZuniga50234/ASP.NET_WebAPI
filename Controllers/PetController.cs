using demoWebApi.DAL;
using demoWebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace demoWebApi.Controllers;
[ApiController]
[Route("api")]
public class PetController(WpmDbContext wpmDbContext) : ControllerBase
{
    private readonly WpmDbContext wpmDbContext = wpmDbContext;

    [HttpGet("pets")]
    public async Task<ActionResult<IEnumerable<PetViewModel>>> GetAllPets()
    {
        var allPets = await wpmDbContext.Pets.Include(p => p.Breed)
        .Select(p => new PetViewModel(p.Id, p.Name, p.Age, p.Weight, p.Breed.Name)).ToListAsync();
        return Ok(allPets);
    }

    [HttpGet("breeds/{id}/pets")]
    public async Task<ActionResult<IEnumerable<PetViewModel>>> GetPetsByBreeds(int id)
    {
        var pets = await wpmDbContext.Pets.Include(p => p.Breed)
        .Where(p => p.BreedId == id)
        .Select(p => new PetViewModel(p.Id, p.Name, p.Age, p.Weight, p.Breed.Name)).ToListAsync();
        return pets.Count != 0 ? Ok(pets) : NotFound();
    }
}