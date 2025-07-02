using challange_bikeRental.Models;

namespace challange_bikeRental.Repositories.Bikes
{
    /// <summary>
    /// Interface for bike repository operations.
    /// </summary>
    public interface IBikeRepository
    {
        /// <summary>
        /// Retrieves all bikes asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of bikes.</returns>
        Task<List<Bike>> GetAllAsync();
        /// <summary>
        /// Retrieves a bike by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the bike.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the bike if found; otherwise, null.</returns>
        Task<Bike?> GetByIdAsync(string id);
        /// <summary>
        /// Retrieves a bike by its license plate asynchronously.
        /// </summary>
        /// <param name="placa">The license plate of the bike.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the bike if found; otherwise, null.</returns>
        Task<Bike?> GetBikeByPlacaAsync(string placa);
        /// <summary>
        /// Creates a new bike asynchronously.
        /// </summary>
        /// <param name="bike">The bike to create.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateAsync(Bike bike);
        /// <summary>
        /// Updates an existing bike asynchronously.
        /// </summary>
        /// <param name="bike">The bike to update.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task UpdateBikeAsync(Bike bike);
        /// <summary>
        /// Deletes a bike by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the bike to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(string id);
    }
}