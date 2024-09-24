using Microsoft.AspNetCore.Mvc;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.App.Controllers
{
    [ApiController]
    [Route("entregadores")]
    [Produces("application/json")]
    [Tags("entregadores")]
    public class DeliverersController(IDelivererService delivererService,
                                      ILogger<DeliverersController> logger) : ControllerBase
    {
        private readonly IDelivererService _delivererService = delivererService;
        private readonly ILogger<DeliverersController> _logger = logger;

        /// <summary>
        /// Cadastrar entregador
        /// </summary>
        /// <param name="delivererDto">Dados do entregador</param>
        /// <response code="201"></response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost]
        public async Task<IActionResult> CreateDeliverer([FromBody] DelivererDto delivererDto)
        {
            _logger.LogInformation("CreateDeliverer: Attempting to register deliverer with identifier {Identifier}.", delivererDto.Identificador);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("CreateDeliverer: Invalid data provided.");
                return BadRequest(new MessageException());
            }

            try
            {
                _logger.LogInformation("CreateDeliverer: Attempting to register deliverer with identifier {Identifier}.", delivererDto.Identificador);
                await _delivererService.CreateDelivererAsync(delivererDto);
                _logger.LogInformation("CreateDeliverer: Deliverer with identifier {Identifier} registered successfully.", delivererDto.Identificador);

                return StatusCode(201); // No content, according to swagger
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateDeliverer: Error registering deliverer with identifier {Identifier}.", delivererDto.Identificador);
                return BadRequest(new MessageException());
            }
        }

        /// <summary>
        /// Enviar foto da CNH
        /// </summary>
        /// <param name="id">Identificador do entregador</param>
        /// <param name="imagemCnhDto">Imagem da CNH</param>
        /// <response code="201"></response>
        /// <response code="400">Dados inválidos</response>
        [HttpPost("{id}/cnh")]
        public async Task<IActionResult> ChangeCnhImage(string id, [FromBody] ImagemCnhDto imagemCnhDto)
        {
            _logger.LogInformation("ChangeCnhImage: Attempting to change CNH image for deliverer {Id}.", id);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ChangeCnhImage: Invalid data provided for deliverer {Id}.", id);
                return BadRequest(new MessageException());
            }

            try
            {
                await _delivererService.ChangeCnhImage(id, imagemCnhDto.ImagemCnh);
                _logger.LogInformation("ChangeCnhImage: CNH image changed successfully for deliverer {Id}.", id);

                return StatusCode(201); // No content, according to swagger
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "ChangeCnhImage: Error changing CNH image for deliverer {Id} because format is not a valid base64.", id);
                return BadRequest(new MessageException());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ChangeCnhImage: Error changing CNH image for deliverer {Id}.", id);
                return BadRequest(new MessageException());
            }
        }
    }
}
