using challange_bikeRental.Models;
using challange_bikeRental.Models.DTOs;
using challange_bikeRental.Services.DeliveryUsers;
using Microsoft.AspNetCore.Mvc;

namespace challange_bikeRental.Controllers.DeliveryUsers
{
    [ApiController]
    [Route("[controller]")]
    public class DeliveryUserController : ControllerBase
    {
        private readonly DeliveryUserService _deliveryUserService;

        public DeliveryUserController(DeliveryUserService deliveryUserService)
        {
            _deliveryUserService = deliveryUserService;
        }

        // Endpoint para cadastrar um novo entregador
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
        [HttpPost("{identificador}/imagem-cnh")]
        public async Task<IActionResult> UploadCnhImage(string identificador, [FromBody] UpdateCnhDto dto)
        {
            try
            {
                dto.Id = identificador;
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
