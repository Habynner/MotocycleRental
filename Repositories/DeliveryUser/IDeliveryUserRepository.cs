using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;

namespace challange_bikeRental.Repositories.DeliveryUser
{
    public interface IDeliveryUserRepository
    {
        Task CreateUserAsync(User users);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(string id);
        Task UpdateUserCnhAsync(UpdateCnhDto updateDto);
        Task<User?> GetUserByCnpjAsync(string cnpj);
        Task<User?> GetUserByCnhAsync(string numeroCnh);
    }
}