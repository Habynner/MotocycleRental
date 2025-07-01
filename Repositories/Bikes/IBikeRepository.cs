using challange_bikeRental.Models;

namespace challange_bikeRental.Repositories.Bikes
{
    public interface IBikeRepository
    {
        Task<List<Bike>> GetAllAsync();
        Task<Bike?> GetByIdAsync(string id);
        Task<Bike?> GetBikeByPlacaAsync(string placa);
        Task CreateAsync(Bike bike);
        Task UpdateBikeAsync(Bike bike);
        Task DeleteAsync(string id);
    }
}