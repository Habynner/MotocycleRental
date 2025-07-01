using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Repositories.DeliveryUser;
using MongoDB.Driver;

namespace challange_bikeRental.Services.DeliveryUsers
{
    public class DeliveryUserService
    {
        private readonly IDeliveryUserRepository _deliveryUserRepository;

        public DeliveryUserService(IDeliveryUserRepository deliveryUserRepository)
        {
            _deliveryUserRepository = deliveryUserRepository;
        }

        public async Task<List<User>> GetAllUsersAsync() => await _deliveryUserRepository.GetAllUsersAsync();

        public async Task<User?> GetUserByIdAsync(string identificador) => await _deliveryUserRepository.GetUserByIdAsync(identificador);

        public async Task<User?> GetUserByCnhAsync(string cnh)
        {
            return await _deliveryUserRepository.GetUserByCnhAsync(cnh);
        }

        public async Task<User?> GetUserByCnpjAsync(string cnpj)
        {
            return await _deliveryUserRepository.GetUserByCnpjAsync(cnpj);
        }

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

        public async Task UpdateUserAsync(UpdateCnhDto updateDto)
        {
            var user = await _deliveryUserRepository.GetUserByIdAsync(updateDto.Id);
            if (user == null)
            {
                throw new KeyNotFoundException("Usuário não encontrado.");
            }

            user.LicenseImage = updateDto.LicenseImage;
            await _deliveryUserRepository.UpdateUserCnhAsync(updateDto);
        }
        public async Task UpdateCnhAsync(UpdateCnhDto updateDto)
        {
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
