using challange_bikeRental.Models;
using challange_bikeRental.Repositories.Bikes;
using challange_bikeRental.Repositories.RentedMotorcycles;
using MongoDB.Driver;

namespace challange_bikeRental.Services.Bikes
{
    public class BikeService
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly IRentedMotorcycleRepository _rentedMotorcycleRepository;

        public BikeService(IBikeRepository bikeRepository, IRentedMotorcycleRepository rentedMotorcycleRepository)
        {
            _bikeRepository = bikeRepository;
            _rentedMotorcycleRepository = rentedMotorcycleRepository;
        }

        public async Task<List<Bike>> GetAllBikesAsync() => await _bikeRepository.GetAllAsync();

        public async Task<Bike?> GetBikeByIdAsync(string identificador) => await _bikeRepository.GetByIdAsync(identificador);
        public async Task<Bike?> GetBikeByPlacaAsync(string placa)
        {
            return await _bikeRepository.GetBikeByPlacaAsync(placa);
        }

        public async Task AddBikeAsync(Bike bike)
        {
            try
            {
                await _bikeRepository.CreateAsync(bike);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new InvalidOperationException("Já existe uma bike com essa placa.");
            }
        }

        public async Task UpdatePlacaAsync(string identificador, string novaPlaca)
        {
            var bike = await _bikeRepository.GetByIdAsync(identificador);
            if (bike == null)
            {
                throw new KeyNotFoundException("Bike não encontrada.");
            }
            var existingBike = await _bikeRepository.GetBikeByPlacaAsync(novaPlaca);
            if (existingBike != null && existingBike.Id != identificador)
            {
                throw new InvalidOperationException("Já existe uma bike com essa placa.");
            }

            bike.Plate = novaPlaca;
            await _bikeRepository.UpdateBikeAsync(bike);
        }

        public async Task DeleteBikeAsync(string identificador)
        {
            // Verifica se a moto está alugada antes de deletar
            var rentedBike = await _rentedMotorcycleRepository.GetRentedByMotocycleAsync(identificador);
            if (rentedBike != null)
            {
                throw new InvalidOperationException("Esta moto está associada a um contrato de locação ativo e não pode ser deletada.");
            }
            // Se não estiver alugada, procede com a deleção
            await _bikeRepository.DeleteAsync(identificador);
        }
    }
}
