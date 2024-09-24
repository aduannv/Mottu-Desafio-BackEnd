using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Motorcycles
{
    public class DeleteMotorcycleUnitTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly Mock<ILogger<MotorcyclesController>> _loggerMock;
        private readonly MotorcyclesController _controller;

        public DeleteMotorcycleUnitTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _loggerMock = new Mock<ILogger<MotorcyclesController>>();
            _controller = new MotorcyclesController(_motorcycleServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task DeleteMotorcycleAsync_ValidId_ReturnsOk()
        {
            // Arrange
            var id = "id123";

            _motorcycleServiceMock.Setup(s => s.DeleteMotorcycleAsync(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteMotorcycleAsync(id);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);

            _motorcycleServiceMock.Verify(s => s.DeleteMotorcycleAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteMotorcycleAsync_UnexpectedError_ReturnsBadRequest()
        {
            // Arrange
            var id = "id123";

            _motorcycleServiceMock.Setup(s => s.DeleteMotorcycleAsync(id))
                .ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _controller.DeleteMotorcycleAsync(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<MessageException>(badRequestResult.Value);

            _motorcycleServiceMock.Verify(s => s.DeleteMotorcycleAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteMotorcycleAsync_MotorcycleNotFound_ReturnsBadRequest()
        {
            // Arrange
            var id = "id123";

            _motorcycleServiceMock.Setup(s => s.DeleteMotorcycleAsync(id))
                .ThrowsAsync(new KeyNotFoundException("Dados inválidos"));

            // Act
            var result = await _controller.DeleteMotorcycleAsync(id);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(badRequestResult.Value);

            _motorcycleServiceMock.Verify(s => s.DeleteMotorcycleAsync(id), Times.Once);
        }
    }
}