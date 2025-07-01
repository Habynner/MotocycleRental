using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;

namespace challange_bikeRental.Repositories.RentedMotorcycles
{
    public interface IRentedMotorcycleRepository
    {
        Task CreateRentalAsync(RentedBikes rental);
        Task UpdateUserWithRentedMotorcycleAsync(string entregadorId, string motoId);
        Task<RentedBikes?> GetByIdAsync(string id);
        Task<RentedBikes?> GetRentedByMotocycleAsync(string motocycleId);
        Task UpdateRentedAsync(UpdateRentedMotocycleDto updateDto);
    }
}
