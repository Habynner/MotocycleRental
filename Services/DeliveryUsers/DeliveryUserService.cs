using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Repositories.DeliveryUser;
using MongoDB.Driver;

namespace challange_bikeRental.Services.DeliveryUsers
{
    /// <summary>
    /// Provides services for managing delivery users, including CRUD operations and CNH image handling.
    /// </summary>
    public class DeliveryUserService
    {
        private readonly IDeliveryUserRepository _deliveryUserRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryUserService"/> class with the specified delivery user repository.
        /// </summary>
        /// <param name="deliveryUserRepository">The repository used for delivery user operations.</param>
        public DeliveryUserService(IDeliveryUserRepository deliveryUserRepository)
        {
            _deliveryUserRepository = deliveryUserRepository;
        }

        /// <summary>
        /// Retrieves all delivery users asynchronously.
        /// </summary>
        /// <returns>A list of all users.</returns>
        public async Task<List<User>> GetAllUsersAsync() => await _deliveryUserRepository.GetAllUsersAsync();

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="identificador">The unique identifier of the user to retrieve.</param>
        /// <returns>The user with the specified identifier, or null if not found.</returns>
        public async Task<User?> GetUserByIdAsync(string identificador) => await _deliveryUserRepository.GetUserByIdAsync(identificador);

        /// <summary>
        /// Retrieves a user by their CNH (driver's license number).
        /// </summary>
        /// <param name="cnh">The CNH of the user to retrieve.</param>
        /// <returns>The user with the specified CNH, or null if not found.</returns>
        public async Task<User?> GetUserByCnhAsync(string cnh)
        {
            return await _deliveryUserRepository.GetUserByCnhAsync(cnh);
        }

        /// <summary>
        /// Retrieves a user by their CNPJ (Cadastro Nacional da Pessoa Jurídica).
        /// </summary>
        /// <param name="cnpj">The CNPJ of the user to retrieve.</param>
        /// <returns>The user with the specified CNPJ, or null if not found.</returns>
        public async Task<User?> GetUserByCnpjAsync(string cnpj)
        {
            return await _deliveryUserRepository.GetUserByCnpjAsync(cnpj);
        }

        /// <summary>
        /// Creates a new delivery user in the repository.
        /// </summary>
        /// <param name="user">The user entity to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateUserAsync(User user)
        {
            try
            {
                await _deliveryUserRepository.CreateUserAsync(user);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new InvalidOperationException("Já existe um usuário com esta CNPJ ou CNH.");
            }
        }

        /// <summary>
        /// Updates the user's CNH (driver's license image) using the provided DTO.
        /// </summary>
        /// <param name="updateDto">DTO containing the user ID and the new license image in base64 format.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateUserAsync(UpdateCnhDto updateDto)
        {
            if (updateDto.Id == null)
            {
                throw new ArgumentNullException(nameof(updateDto.Id), "O identificador do usuário não pode ser nulo.");
            }

            var user = await _deliveryUserRepository.GetUserByIdAsync(updateDto.Id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            user.LicenseImage = updateDto.LicenseImage;
            await _deliveryUserRepository.UpdateUserCnhAsync(updateDto);
        }
        /// <summary>
        /// Updates the CNH (driver's license image) for a delivery user.
        /// </summary>
        /// <param name="updateDto">DTO containing the user ID and the new license image in base64 format.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateCnhAsync(UpdateCnhDto updateDto)
        {
            if (updateDto.Id == null)
            {
                throw new ArgumentNullException(nameof(updateDto.Id), "O identificador do usuário não pode ser nulo.");
            }
            // Busca o usuário pelo identificador
            var user = await _deliveryUserRepository.GetUserByIdAsync(updateDto.Id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            // Decodifique e valide a imagem
            var imagePath = SaveImageToLocalDisk(updateDto.LicenseImage, updateDto.Id);

            // Atualize apenas o campo da imagem no repositório
            var updateData = new UpdateCnhDto
            {
                Id = updateDto.Id,
                LicenseImage = imagePath
            };

            await _deliveryUserRepository.UpdateUserCnhAsync(updateData);
        }

        // Método auxiliar para salvar a imagem no disco local
        private string SaveImageToLocalDisk(string base64Image, string identificador)
        {
            var bytes = Convert.FromBase64String(base64Image);
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, $"{identificador}.png");
            File.WriteAllBytes(filePath, bytes);

            return filePath;
        }


    }
}
