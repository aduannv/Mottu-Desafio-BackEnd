using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.App.Controllers
{
    [ApiController]
    [Route("locacao")]
    [Produces("application/json")]
    [Tags("locacao")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        private readonly ILogger<RentalController> _logger;

        public RentalController(IRentalService rentalService, ILogger<RentalController> logger)
        {
            _rentalService = rentalService;
            _logger = logger;
        }

        /// <summary>
        /// Alugar uma moto
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalDto rentalDto)
        {
            _logger.LogInformation("CreateRental: Attempting to create rental with motorcycle identifier {MotoId} and deliverer {EntregadorId}.", rentalDto.MotoId, rentalDto.EntregadorId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateRental: Invalid data provided for motorcycle identifier {MotoId} and deliverer {EntregadorId}.", rentalDto.MotoId, rentalDto.EntregadorId);
                return BadRequest(new MessageException());
            }

            try
            {
                await _rentalService.CreateRentalAsync(rentalDto);
                _logger.LogInformation("CreateRental: Rental created successfully with motorcycle identifier {MotoId} and deliverer {EntregadorId}.", rentalDto.MotoId, rentalDto.EntregadorId);
                return StatusCode(201);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "CreateRental: Invalid argument provided for motorcycle identifier {MotoId} and deliverer {EntregadorId}.", rentalDto.MotoId, rentalDto.EntregadorId);
                return BadRequest(new MessageException());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateRental: Error creating rental with motorcycle identifier {MotoId} and deliverer {EntregadorId}.", rentalDto.MotoId, rentalDto.EntregadorId);
                return BadRequest(new MessageException());
            }
        }

        /// <summary>
        /// Consultar locação por id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRental(string id)
        {
            _logger.LogInformation("GetRental: Attempting to retrieve rental with ID {Id}.", id);

            try
            {
                var rental = await _rentalService.GetRentalAsync(id);
                _logger.LogInformation("GetRental: Rental retrieved successfully with ID {Id}.", id);
                return Ok(rental);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "GetRental: Rental not found for ID {Id}.", id);
                return NotFound(new MessageException(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetRental: Error retrieving rental with ID {Id}.", id);
                return BadRequest(new MessageException());
            }
        }

        /// <summary>
        /// Informar data de devolução e calcular valor
        /// </summary>
        [HttpPost("{id}/devolucao")]
        public async Task<IActionResult> ReturnRental(string id, [FromBody] ReturnRentalDto returnRental)
        {
            _logger.LogInformation("ReturnRental: Attempting to record return date for rental with ID {Id}.", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ReturnRental: Invalid data provided for rental ID {Id}.", id);
                return BadRequest(new MessageException());
            }

            try
            {
                await _rentalService.ReturnRentalAsync(id, returnRental);
                _logger.LogInformation("ReturnRental: Return date recorded successfully for rental ID {Id}.", id);
                return Ok(new MessageDto("Data de devolução informada com sucesso"));
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "ReturnRental: Rental not found for ID {Id}.", id);
                return NotFound(new MessageException(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReturnRental: Error recording return date for rental ID {Id}.", id);
                return BadRequest(new MessageException(ex.Message));
            }
        }
    }
}
