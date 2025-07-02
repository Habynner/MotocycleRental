using challange_bikeRental.Models;

namespace challange_bikeRental.Repositories.Logs
{
    /// <summary>
    /// Defines methods for managing rented motorcycles in the repository.
    /// </summary>
    public interface ILogsMotorcycleCreatedRepository
    {
        /// <summary>
        /// Creates a new log record for a new motorcycle created.
        /// </summary>
        /// <param name="log">The rental information to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateLogAsync(LogsMotorcycleCreated log);
    }
}
