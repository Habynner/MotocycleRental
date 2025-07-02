using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;

namespace challange_bikeRental.Repositories.RentedMotorcycles
{
    /// <summary>
    /// Defines methods for managing rented motorcycles in the repository.
    /// </summary>
    public interface IRentedMotorcycleRepository
    {
        /// <summary>
        /// Creates a new rental record for a rented motorcycle.
        /// </summary>
        /// <param name="rental">The rental information to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateRentalAsync(RentedBikes rental);
        /// <summary>
        /// Updates the user with the rented motorcycle information.
        /// </summary>
        /// <param name="entregadorId">The ID of the user (entregador) to update.</param>
        /// <param name="motoId">The ID of the motorcycle being rented.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateUserWithRentedMotorcycleAsync(string entregadorId, string motoId);
        /// <summary>
        /// Retrieves a rented bike record by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the rented bike.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the rented bike if found; otherwise, null.</returns>
        Task<RentedBikes?> GetByIdAsync(string id);
        /// <summary>
        /// Retrieves a rented bike record by the motorcycle's unique identifier.
        /// </summary>
        /// <param name="motocycleId">The unique identifier of the motorcycle.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the rented bike if found; otherwise, null.</returns>
        Task<RentedBikes?> GetRentedByMotocycleAsync(string motocycleId);
        /// <summary>
        /// Updates the information of a rented motorcycle.
        /// </summary>
        /// <param name="updateDto">The DTO containing updated rental information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateRentedAsync(UpdateRentedMotocycleDto updateDto);
    }
}
