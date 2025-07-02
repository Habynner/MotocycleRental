using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Services.DeliveryUsers;
using Microsoft.AspNetCore.Mvc;

namespace challange_bikeRental.Controllers.DeliveryUsers
{
    /// <summary>
    /// Controller responsible for handling delivery user related operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DeliveryUserController : ControllerBase
    {
        private readonly DeliveryUserService _deliveryUserService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryUserController"/> class.
        /// </summary>
        /// <param name="deliveryUserService">The delivery user service to be used by this controller.</param>
        public DeliveryUserController(DeliveryUserService deliveryUserService)
        {
            _deliveryUserService = deliveryUserService;
        }

        /// <summary>
        /// Registers a new delivery user.
        /// </summary>
        /// <param name="user">The user to be registered.</param>
        /// <returns>An ActionResult indicating the result of the operation.</returns>
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] User user)
        {
            try
            {
                await _deliveryUserService.CreateUserAsync(user);
                return CreatedAtAction(nameof(CreateUser), new { identificador = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Uploads the CNH image for a delivery user.
        /// </summary>
        /// <param name="id">The identifier of the delivery user.</param>
        /// <param name="dto">The DTO containing the CNH image data.</param>
        /// <returns>An IActionResult indicating the result of the operation.</returns>
        [HttpPost("{id}/imagem-cnh")]
        public async Task<IActionResult> UploadCnhImage(string id, [FromBody] UpdateCnhDto dto)
        {
            try
            {
                dto.Id = id;
                await _deliveryUserService.UpdateCnhAsync(dto);
                return Ok(new { message = "Imagem enviada com sucesso!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // 404 - Não encontrado
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message }); // 400 - Erro de validação
            }
        }


    }
}
