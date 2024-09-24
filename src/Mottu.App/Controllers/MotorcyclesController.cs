using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.App.Controllers
{
    [ApiController]
    [Route("motos")]
    [Produces("application/json")]
    [Tags("motos")]
    public class MotorcyclesController(IMotorcycleService motorcycleService,
                                       ILogger<MotorcyclesController> logger) : ControllerBase
    {
        private readonly IMotorcycleService _motorcycleService = motorcycleService;
        private readonly ILogger<MotorcyclesController> _logger = logger;

        /// <summary>
        /// Cadastrar uma nova moto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateMotorcycleAsync([FromBody] MotorcycleDto motorcycle)
        {
            _logger.LogInformation("CreateMotorcycle initiated. Id: {Identificador}", motorcycle.Identificador);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateMotorcycle: Invalid model state. Id: {Identificador}", motorcycle.Identificador);
                return BadRequest(new MessageException());
            }

            try
            {
                await _motorcycleService.CreateMotorcycleAsync(motorcycle);
                _logger.LogInformation("CreateMotorcycle: Motorcycle with ID {Identificador} created successfully.", motorcycle.Identificador);
                return StatusCode(201); // No content, according to Swagger
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in creation motorcycle with Id: {Identificador}", motorcycle.Identificador ?? "No Id provided");
                return BadRequest(new MessageException());
            }
        }

        /// <summary>
        /// Consultar motos existentes
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMotorcyclesAsync([FromQuery] string? placa)
        {
            _logger.LogInformation("GetMotorcycles: Retrieving motorcycles. Plate: {Placa}", placa ?? "No plate provided");

            try
            {
                var motorcycles = await _motorcycleService.GetMotorcyclesAsync(placa);
                _logger.LogInformation("GetMotorcycles: {Count} motorcycle(s) found for Plate: {Placa}", motorcycles.Count(), placa ?? "No plate provided");
                return Ok(motorcycles);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "GetMotorcycles: Resource not found. Plate: {Placa}", placa ?? "No plate provided");
                return NotFound(new MessageException(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving motorcycles for Plate: {Placa}", placa ?? "No plate provided");
                return BadRequest(new MessageException());
            }
        }

        /// <summary>
        /// Modificar a placa de uma moto
        /// </summary>
        [HttpPut("{id}/placa")]
        public async Task<IActionResult> UpdateMotorcycleAsync(string id, [FromBody] string placa)
        {
            _logger.LogInformation("UpdateMotorcycle: Starting plate update. Id: {Id}, New Plate: {Placa}", id, placa);

            try
            {
                await _motorcycleService.UpdateMotorcyclePlacaAsync(id, placa);
                _logger.LogInformation("UpdateMotorcycle: Plate updated successfully. Id: {Id}, New Plate: {Placa}", id, placa);
                return Ok(new MessageDto("Placa modificada com sucesso"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating motorcycle plate. Id: {Id}, New Plate: {Placa}", id, placa);
                return BadRequest(new MessageException(ex.Message));
            }
        }

        /// <summary>
        /// Consultar motos existentes por id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorcycleByIdAsync(string id)
        {
            _logger.LogInformation("GetMotorcycleById initiated for ID {Id}", id);

            try
            {
                var motorcycle = await _motorcycleService.GetMotorcycleByIdAsync(id);
                _logger.LogInformation("GetMotorcycleById: Motorcycle with ID {Id} retrieved successfully.", id);
                return Ok(motorcycle);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "GetMotorcycleById: Motorcycle with ID {Id} not found.", id);
                return NotFound(new MessageException(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMotorcycleById: An error occurred while retrieving the motorcycle with ID {Id}.", id);
                return BadRequest(new { mensagem = "Request mal formada" });
            }
        }

        /// <summary>
        /// Remover uma moto
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotorcycleAsync(string id)
        {
            _logger.LogInformation("DeleteMotorcycle initiated for ID {Id}", id);

            try
            {
                await _motorcycleService.DeleteMotorcycleAsync(id);
                _logger.LogInformation("DeleteMotorcycle: Motorcycle with ID {Id} removed successfully.", id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteMotorcycle: An error occurred while deleting the motorcycle with ID {Id}.", id);
                return BadRequest(new MessageException());
            }
        }
    }
}
