using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Services.RentedMotorcycles;
using Microsoft.AspNetCore.Mvc;

namespace challange_bikeRental.Controllers.RentedMotorcycles
{
    [ApiController]
    [Route("[controller]")]
    public class RentedMotorcycleController : ControllerBase
    {
        private readonly RentedMotorcycleService _rentedMotorcycleService;

        public RentedMotorcycleController(RentedMotorcycleService rentedMotorcycleService)
        {
            _rentedMotorcycleService = rentedMotorcycleService;
        }

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRentalById(string id)
        {
            var bike = await _rentedMotorcycleService.GetBikeByIdAsync(id);
            if (bike == null) return NotFound();
            return Ok(bike);
        }

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
