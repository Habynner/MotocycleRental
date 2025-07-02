using challange_bikeRental.Models;
using challange_bikeRental.Repositories.Bikes;
using challange_bikeRental.Repositories.RentedMotorcycles;
using challange_bikeRental.Utils.Producers;
using MongoDB.Driver;

namespace challange_bikeRental.Services.Bikes
{
    /// <summary>
    /// Provides services for managing bikes, including CRUD operations and integration with RabbitMQ.
    /// </summary>
    public class BikeService
    {
        private readonly IBikeRepository _bikeRepository;
        private readonly IRentedMotorcycleRepository _rentedMotorcycleRepository;
        private readonly RabbitMqProducer _rabbitMqProducer;

        /// <summary>
        /// Initializes a new instance of the <see cref="BikeService"/> class.
        /// </summary>
        /// <param name="bikeRepository">The bike repository.</param>
        /// <param name="rentedMotorcycleRepository">The rented motorcycle repository.</param>
        /// <param name="rabbitMqProducer">The RabbitMQ producer for event publishing.</param>
        public BikeService(IBikeRepository bikeRepository, IRentedMotorcycleRepository rentedMotorcycleRepository, RabbitMqProducer rabbitMqProducer)
        {
            _bikeRepository = bikeRepository;
            _rentedMotorcycleRepository = rentedMotorcycleRepository;
            _rabbitMqProducer = rabbitMqProducer;
        }

        /// <summary>
        /// Retrieves all bikes asynchronously.
        /// </summary>
        /// <returns>A list of all bikes.</returns>
        public async Task<List<Bike>> GetAllBikesAsync() => await _bikeRepository.GetAllAsync();

        /// <summary>
        /// Retrieves a bike by its identifier asynchronously.
        /// </summary>
        /// <param name="identificador">The unique identifier of the bike.</param>
        /// <returns>The bike with the specified identifier, or null if not found.</returns>
        public async Task<Bike?> GetBikeByIdAsync(string identificador) => await _bikeRepository.GetByIdAsync(identificador);
        /// <summary>
        /// Retrieves a bike by its license plate asynchronously.
        /// </summary>
        /// <param name="placa">The license plate of the bike.</param>
        /// <returns>The bike with the specified license plate, or null if not found.</returns>
        public async Task<Bike?> GetBikeByPlacaAsync(string placa)
        {
            return await _bikeRepository.GetBikeByPlacaAsync(placa);
        }

        /// <summary>
        /// Adds a new bike asynchronously and publishes an event to RabbitMQ.
        /// </summary>
        /// <param name="bike">The bike to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task AddBikeAsync(Bike bike)
        {
            try
            {
                await _bikeRepository.CreateAsync(bike);
                // Publica o evento de moto cadastrada
                _rabbitMqProducer.Publish(bike);
            }
            catch (MongoWriteException ex) when (ex.WriteError.Category == ServerErrorCategory.DuplicateKey)
            {
                throw new InvalidOperationException("Já existe uma bike com essa placa.");
            }
        }

        /// <summary>
        /// Updates the license plate of a bike asynchronously.
        /// </summary>
        /// <param name="identificador">The unique identifier of the bike.</param>
        /// <param name="novaPlaca">The new license plate to assign to the bike.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Deletes a bike asynchronously if it is not associated with an active rental contract.
        /// </summary>
        /// <param name="identificador">The unique identifier of the bike to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the bike is associated with an active rental contract.</exception>
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
