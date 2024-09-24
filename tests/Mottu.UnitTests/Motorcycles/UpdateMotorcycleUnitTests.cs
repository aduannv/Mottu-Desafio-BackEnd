using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Motorcycles
{
    public class UpdateMotorcycleUnitTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly Mock<ILogger<MotorcyclesController>> _loggerMock;
        private readonly MotorcyclesController _controller;

        public UpdateMotorcycleUnitTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _loggerMock = new Mock<ILogger<MotorcyclesController>>();
            _controller = new MotorcyclesController(_motorcycleServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task UpdateMotorcycleAsync_ValidIdAndPlaca_ReturnsOk()
        {
            // Arrange
            var id = "id123";
            var newPlaca = "XYZ123";

            // Act
            var result = await _controller.UpdateMotorcycleAsync(id, newPlaca);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var messageObject = Assert.IsType<MessageDto>(okResult.Value);
            Assert.Equal("Placa modificada com sucesso", messageObject.Mensagem);
            _motorcycleServiceMock.Verify(s => s.UpdateMotorcyclePlacaAsync(id, newPlaca), Times.Once);
        }

        [Fact]
        public async Task UpdateMotorcycleAsync_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var id = "invalidId";
            var newPlaca = "XYZ123";

            _motorcycleServiceMock.Setup(s => s.UpdateMotorcyclePlacaAsync(id, newPlaca))
                .ThrowsAsync(new KeyNotFoundException("Dados inválidos"));

            // Act
            var result = await _controller.UpdateMotorcycleAsync(id, newPlaca);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(badRequestResult.Value);
            Assert.Equal("Dados inválidos", messageException.Mensagem);
            _motorcycleServiceMock.Verify(s => s.UpdateMotorcyclePlacaAsync(id, newPlaca), Times.Once);
        }

        [Fact]
        public async Task UpdateMotorcycleAsync_Exception_ReturnsBadRequest()
        {
            // Arrange
            var id = "id123";
            var newPlaca = "XYZ123";

            _motorcycleServiceMock.Setup(s => s.UpdateMotorcyclePlacaAsync(id, newPlaca))
                .ThrowsAsync(new Exception("Erro ao atualizar a placa"));

            // Act
            var result = await _controller.UpdateMotorcycleAsync(id, newPlaca);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(badRequestResult.Value);
            Assert.Equal("Erro ao atualizar a placa", messageException.Mensagem);
            _motorcycleServiceMock.Verify(s => s.UpdateMotorcyclePlacaAsync(id, newPlaca), Times.Once);
        }
    }
}