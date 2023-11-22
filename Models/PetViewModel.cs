namespace demoWebApi.Models;

public record PetViewModel(int Id, string Name, int? Age, decimal? Weight, string BreedName);