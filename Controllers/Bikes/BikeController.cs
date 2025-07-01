using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Services.Bikes;
using Microsoft.AspNetCore.Mvc;

namespace challange_bikeRental.Controllers.Bikes
{
    [ApiController]
    [Route("[controller]")]
    public class BikeController : ControllerBase
    {
        private readonly BikeService _bikeService;

        public BikeController(BikeService bikeService)
        {
            _bikeService = bikeService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Bike>>> GetAll([FromQuery] string? placa)
        {
            if (!string.IsNullOrEmpty(placa))
            {
                var bike = await _bikeService.GetBikeByPlacaAsync(placa);
                if (bike == null)
                {
                    return NotFound(new { message = "Nenhuma bike encontrada com essa placa." });
                }
                return Ok(new List<Bike> { bike });
            }

            var bikes = await _bikeService.GetAllBikesAsync();
            return Ok(bikes);
        }

        [HttpGet("{identificador}")]
        public async Task<ActionResult<Bike>> GetById(string identificador)
        {
            var bike = await _bikeService.GetBikeByIdAsync(identificador);
            if (bike == null) return NotFound();
            return Ok(bike);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Bike bike)
        {
            try
            {
                await _bikeService.AddBikeAsync(bike);

                // Apontar para o método "GetAll" com query param "placa"
                return CreatedAtAction(nameof(GetAll), new { bike.Plate }, bike);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{identificador}")]
        public async Task<IActionResult> UpdatePlaca(string identificador, [FromBody] UpdatePlacaDto placaObj)
        {
            try
            {
                await _bikeService.UpdatePlacaAsync(identificador, placaObj.Plate);
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


        [HttpDelete("{identificador}")]
        public async Task<ActionResult> Delete(string identificador)
        {
            try
            {
                var existingBike = await _bikeService.GetBikeByIdAsync(identificador);
                if (existingBike == null) return NotFound();
                await _bikeService.DeleteBikeAsync(identificador);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
