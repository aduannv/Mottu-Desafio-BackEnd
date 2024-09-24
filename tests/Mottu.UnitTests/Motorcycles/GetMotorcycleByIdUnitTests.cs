using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Motorcycles
{
    public class GetMotorcycleByIdUnitTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly Mock<ILogger<MotorcyclesController>> _loggerMock;
        private readonly MotorcyclesController _controller;

        public GetMotorcycleByIdUnitTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _loggerMock = new Mock<ILogger<MotorcyclesController>>();
            _controller = new MotorcyclesController(_motorcycleServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetMotorcycleByIdAsync_MotorcycleExists_ReturnsOk()
        {
            // Arrange
            var id = "id123";
            var motorcycleDto = new MotorcycleDto { Identificador = id, Placa = "XYZ123" };

            _motorcycleServiceMock.Setup(s => s.GetMotorcycleByIdAsync(id))
                .ReturnsAsync(motorcycleDto);

            // Act
            var result = await _controller.GetMotorcycleByIdAsync(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMotorcycle = Assert.IsType<MotorcycleDto>(okResult.Value);
            Assert.Equal(id, returnedMotorcycle.Identificador);

            _motorcycleServiceMock.Verify(s => s.GetMotorcycleByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetMotorcycleByIdAsync_MotorcycleDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var id = "id123";

            _motorcycleServiceMock.Setup(s => s.GetMotorcycleByIdAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Moto não encontrada"));

            // Act
            var result = await _controller.GetMotorcycleByIdAsync(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(notFoundResult.Value);
            Assert.Equal("Moto não encontrada", messageException.Mensagem);

            _motorcycleServiceMock.Verify(s => s.GetMotorcycleByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetMotorcycleByIdAsync_UnexpectedError_ReturnsBadRequest()
        {
            // Arrange
            var id = "id123";

            _motorcycleServiceMock.Setup(s => s.GetMotorcycleByIdAsync(id))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.GetMotorcycleByIdAsync(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = badRequestResult.Value.GetType().GetProperty("mensagem")?.GetValue(badRequestResult.Value, null);
            Assert.Equal("Request mal formada", response);

            _motorcycleServiceMock.Verify(s => s.GetMotorcycleByIdAsync(id), Times.Once);
        }
    }
}