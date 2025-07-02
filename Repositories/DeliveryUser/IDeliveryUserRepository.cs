using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;

namespace challange_bikeRental.Repositories.DeliveryUser
{
    /// <summary>
    /// Defines methods for managing delivery user data in the repository.
    /// </summary>
    public interface IDeliveryUserRepository
    {
        /// <summary>
        /// Asynchronously creates a new delivery user in the repository.
        /// </summary>
        /// <param name="users">The user entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateUserAsync(User users);
        /// <summary>
        /// Asynchronously retrieves all delivery users from the repository.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, containing a list of users.</returns>
        Task<List<User>> GetAllUsersAsync();
        /// <summary>
        /// Asynchronously retrieves a delivery user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>A task representing the asynchronous operation, containing the user if found; otherwise, null.</returns>
        Task<User?> GetUserByIdAsync(string id);
        /// <summary>
        /// Asynchronously updates the CNH information of a delivery user.
        /// </summary>
        /// <param name="updateDto">The DTO containing updated CNH information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task UpdateUserCnhAsync(UpdateCnhDto updateDto);
        /// <summary>
        /// Asynchronously retrieves a delivery user by their CNPJ.
        /// </summary>
        /// <param name="cnpj">The CNPJ of the user.</param>
        /// <returns>A task representing the asynchronous operation, containing the user if found; otherwise, null.</returns>
        Task<User?> GetUserByCnpjAsync(string cnpj);
        /// <summary>
        /// Asynchronously retrieves a delivery user by their CNH number.
        /// </summary>
        /// <param name="numeroCnh">The CNH number of the user.</param>
        /// <returns>A task representing the asynchronous operation, containing the user if found; otherwise, null.</returns>
        Task<User?> GetUserByCnhAsync(string numeroCnh);
    }
}