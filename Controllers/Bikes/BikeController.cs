using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Services.Bikes;
using Microsoft.AspNetCore.Mvc;

namespace challange_bikeRental.Controllers.Bikes
{

    /// <summary>
    /// Controller for managing bike-related operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BikeController : ControllerBase
    {
        private readonly BikeService _bikeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BikeController"/> class with the specified <see cref="BikeService"/>.
        /// </summary>
        /// <param name="bikeService">The service used to manage bikes.</param>
        public BikeController(BikeService bikeService)
        {
            _bikeService = bikeService;
        }

        /// <summary>
        /// Gets all bikes or a specific bike by its plate if the 'placa' query parameter is provided.
        /// </summary>
        /// <param name="placa">The plate of the bike to search for (optional).</param>
        /// <returns>A list of bikes or a single bike if 'placa' is specified.</returns>
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

        /// <summary>
        /// Gets a bike by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the bike to retrieve.</param>
        /// <returns>The bike with the specified identifier, or NotFound if it does not exist.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Bike>> GetById(string id)
        {
            var bike = await _bikeService.GetBikeByIdAsync(id);
            if (bike == null) return NotFound();
            return Ok(bike);
        }


        /// <summary>
        /// Creates a new bike.
        /// </summary>
        /// <param name="bike">The bike to create.</param>
        /// <returns>A response indicating the result of the creation operation.</returns>
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

        /// <summary>
        /// Updates the plate of a bike by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the bike to update.</param>
        /// <param name="placaObj">The object containing the new plate value.</param>
        /// <returns>NoContent if successful, NotFound if the bike does not exist, or Conflict if the plate is duplicated.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlaca(string id, [FromBody] UpdatePlacaDto placaObj)
        {
            try
            {
                await _bikeService.UpdatePlacaAsync(id, placaObj.Plate);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                // Retorna 404 se o identificador não existir
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                // Retorna 409 se houver duplicidade na placa
                return Conflict(new { message = ex.Message });
            }
        }


        /// <summary>
        /// Deletes a bike by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the bike to delete.</param>
        /// <returns>NoContent if successful, NotFound if the bike does not exist, or BadRequest on error.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var existingBike = await _bikeService.GetBikeByIdAsync(id);
                if (existingBike == null) return NotFound();
                await _bikeService.DeleteBikeAsync(id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
