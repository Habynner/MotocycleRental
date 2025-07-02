using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Services.RentedMotorcycles;
using Microsoft.AspNetCore.Mvc;

namespace challange_bikeRental.Controllers.RentedMotorcycles
{
    /// <summary>
    /// Controller for managing rented motorcycles.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RentedMotorcycleController : ControllerBase
    {
        private readonly RentedMotorcycleService _rentedMotorcycleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentedMotorcycleController"/> class.
        /// </summary>
        /// <param name="rentedMotorcycleService">The service for managing rented motorcycles.</param>
        public RentedMotorcycleController(RentedMotorcycleService rentedMotorcycleService)
        {
            _rentedMotorcycleService = rentedMotorcycleService;
        }

        /// <summary>
        /// Creates a new motorcycle rental.
        /// </summary>
        /// <param name="rental">The rental information to create.</param>
        /// <returns>The created rental information with a location header.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] RentedBikes rental)
        {
            try
            {
                await _rentedMotorcycleService.CreateRentalAsync(rental);
                return CreatedAtAction(nameof(GetRentalById), new { id = rental.Id }, rental);
            }
            catch (Exception ex)
            {
                // Pode ser refinado para tratar exceções específicas
                return BadRequest(new { message = ex.Message });
            }
        }

        // Método para consultar um aluguel pelo id, só para o CreatedAtAction funcionar bem
        /// <summary>
        /// Retrieves a rental by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the rental.</param>
        /// <returns>The rental information if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalById(string id)
        {
            var bike = await _rentedMotorcycleService.GetBikeByIdAsync(id);
            if (bike == null) return NotFound();
            return Ok(bike);
        }

        /// <summary>
        /// Updates the rental information for a motorcycle by its ID.
        /// </summary>
        /// <param name="id">The ID of the rental to update.</param>
        /// <param name="rentalDto">The updated rental data.</param>
        /// <returns>NoContent if successful, NotFound if the rental does not exist, or Conflict if there is a duplicate plate.</returns>
        [HttpPut("{id}/devolucao")]
        public async Task<IActionResult> UpdatePlaca(string id, [FromBody] UpdateRentedMotocycleDto rentalDto)
        {
            try
            {
                rentalDto.Id = id; // Define o ID do aluguel a ser atualizado
                await _rentedMotorcycleService.UpdateRentedAsync(rentalDto);
                return NoContent(); // Retorna 204 (Sem Conteúdo) se for bem-sucedido
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // Retorna 404 se o identificador não existir
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message }); // Retorna 409 se houver duplicidade na placa
            }
        }
    }
}
