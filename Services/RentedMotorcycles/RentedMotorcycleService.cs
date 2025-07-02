using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Repositories.DeliveryUser;
using challange_bikeRental.Repositories.RentedMotorcycles;
using challange_bikeRental.Utils.Attributes;

namespace challange_bikeRental.Services.RentedMotorcycles
{
    /// <summary>
    /// Service class for handling rented motorcycle operations.
    /// </summary>
    public class RentedMotorcycleService
    {
        private readonly IRentedMotorcycleRepository _rentedMotorcycleRepository;
        private readonly IDeliveryUserRepository _deliveryUserRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentedMotorcycleService"/> class.
        /// </summary>
        /// <param name="rentedMotorcycleRepository">The repository for rented motorcycles.</param>
        /// <param name="deliveryUserRepository">The repository for delivery users.</param>
        public RentedMotorcycleService(IRentedMotorcycleRepository rentedMotorcycleRepository, IDeliveryUserRepository deliveryUserRepository)
        {
            _rentedMotorcycleRepository = rentedMotorcycleRepository;
            _deliveryUserRepository = deliveryUserRepository;
        }

        /// <summary>
        /// Creates a new rental for a motorcycle.
        /// </summary>
        /// <param name="rental">The rental information to create.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task CreateRentalAsync(RentedBikes rental)
        {
            // Validação: O início da locação deve ser o primeiro dia após a data de criação
            var today = DateTime.UtcNow.Date;
            var expectedStartDate = today.AddDays(1);

            if (rental.StartDate.Date != expectedStartDate) throw new InvalidOperationException($"A data de início deve ser {expectedStartDate:yyyy-MM-dd}.");

            // Validação: Somente entregadores habilitados na categoria A podem efetuar uma locação
            var entregador = await _deliveryUserRepository.GetUserByIdAsync(rental.DeliveryUser);
            if (entregador == null) throw new KeyNotFoundException("Entregador não encontrado.");

            if (!entregador.LicenseType.Contains("A")) throw new InvalidOperationException("Somente entregadores habilitados na categoria A podem efetuar uma locação.");

            // Validação: A moto deve estar disponível para locação
            var rentedBike = await _rentedMotorcycleRepository.GetRentedByMotocycleAsync(rental.MotocycleId);
            if (rentedBike != null) throw new KeyNotFoundException("Esta moto já está alugada.");

            // Criando lógica para conta de valor do aluguel.
            rental.DailyRate = RentalPlanExtensions.GetDailyCost(rental.Plan);

            // Cria o registro do aluguel
            await _rentedMotorcycleRepository.CreateRentalAsync(rental);

            // Atualiza o usuário com a moto alugada
            await _rentedMotorcycleRepository.UpdateUserWithRentedMotorcycleAsync(rental.DeliveryUser, rental.MotocycleId);
        }
        /// <summary>
        /// Retrieves a rented bike by its identifier.
        /// </summary>
        /// <param name="identificador">The identifier of the rented bike.</param>
        /// <returns>The rented bike if found; otherwise, null.</returns>
        public async Task<RentedBikes?> GetBikeByIdAsync(string identificador) => await _rentedMotorcycleRepository.GetByIdAsync(identificador);

        /// <summary>
        /// Updates the information of a rented motorcycle.
        /// </summary>
        /// <param name="updateDto">The DTO containing updated rental information.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateRentedAsync(UpdateRentedMotocycleDto updateDto)
        {
            if (string.IsNullOrEmpty(updateDto.Id))
            {
                throw new ArgumentNullException(nameof(updateDto.Id), "O identificador do aluguel não pode ser nulo.");
            }

            var rental = await _rentedMotorcycleRepository.GetByIdAsync(updateDto.Id);
            if (rental == null)
            {
                throw new KeyNotFoundException("Aluguel não encontrado.");
            }

            rental.ReturnDate = updateDto.ReturnDate;
            await _rentedMotorcycleRepository.UpdateRentedAsync(updateDto);
        }
    }
}
