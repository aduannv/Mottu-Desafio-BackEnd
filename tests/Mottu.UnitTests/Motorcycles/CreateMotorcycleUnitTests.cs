using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Motorcycles
{
    public class CreateMotorcycleUnitTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly Mock<ILogger<MotorcyclesController>> _loggerMock;
        private readonly MotorcyclesController _controller;

        public CreateMotorcycleUnitTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _loggerMock = new Mock<ILogger<MotorcyclesController>>();
            _controller = new MotorcyclesController(_motorcycleServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateMotorcycle_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var motorcycleDto = new MotorcycleDto();

            _controller.ModelState.AddModelError("Placa", "Placa is required");

            // Act
            var result = await _controller.CreateMotorcycleAsync(motorcycleDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(badRequestResult.Value);
            Assert.Equal("Dados inválidos", messageException.Mensagem);
        }

        [Fact]
        public async Task CreateMotorcycle_MotorcycleAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var motorcycleDto = new MotorcycleDto { Identificador = "id123", Placa = "XYZ123" };
            _motorcycleServiceMock.Setup(s => s.CreateMotorcycleAsync(motorcycleDto))
                .ThrowsAsync(new Exception("Moto já cadastrada com este identificador."));

            // Act
            var result = await _controller.CreateMotorcycleAsync(motorcycleDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(badRequestResult.Value);
            Assert.Equal("Dados inválidos", messageException.Mensagem);
        }

        [Fact]
        public async Task CreateMotorcycle_ValidMotorcycle_CreatesMotorcycle()
        {
            // Arrange
            var motorcycleDto = new MotorcycleDto { Identificador = "id123", Placa = "XYZ123" };

            // Act
            var result = await _controller.CreateMotorcycleAsync(motorcycleDto);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
            _motorcycleServiceMock.Verify(s => s.CreateMotorcycleAsync(motorcycleDto), Times.Once);
        }
    }
}