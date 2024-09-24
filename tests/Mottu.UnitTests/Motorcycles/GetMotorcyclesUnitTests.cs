using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Motorcycles
{
    public class GetMotorcyclesUnitTests
    {
        private readonly Mock<IMotorcycleService> _motorcycleServiceMock;
        private readonly Mock<ILogger<MotorcyclesController>> _loggerMock;
        private readonly MotorcyclesController _controller;

        public GetMotorcyclesUnitTests()
        {
            _motorcycleServiceMock = new Mock<IMotorcycleService>();
            _loggerMock = new Mock<ILogger<MotorcyclesController>>();
            _controller = new MotorcyclesController(_motorcycleServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetMotorcycles_ValidRequest_ReturnsOk()
        {
            // Arrange
            var motorcycles = new List<MotorcycleDto>
            {
                new() { Identificador = "id123", Placa = "XYZ123" },
                new() { Identificador = "id456", Placa = "ABC456" }
            };
            _motorcycleServiceMock.Setup(s => s.GetMotorcyclesAsync(null))
                .ReturnsAsync(motorcycles);

            // Act
            var result = await _controller.GetMotorcyclesAsync(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMotorcycles = Assert.IsType<List<MotorcycleDto>>(okResult.Value);
            Assert.Equal(2, returnedMotorcycles.Count);
        }

        [Fact]
        public async Task GetMotorcycles_ResourceNotFound_ReturnsNotFound()
        {
            // Arrange
            var placa = "XYZ123";
            _motorcycleServiceMock.Setup(s => s.GetMotorcyclesAsync(placa))
                .ThrowsAsync(new KeyNotFoundException("Moto não encontrada"));

            // Act
            var result = await _controller.GetMotorcyclesAsync(placa);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(notFoundResult.Value);
            Assert.Equal("Moto não encontrada", messageException.Mensagem);
        }

        [Fact]
        public async Task GetMotorcycles_InternalServerError_ReturnsBadRequest()
        {
            // Arrange
            var placa = "XYZ123";
            _motorcycleServiceMock.Setup(s => s.GetMotorcyclesAsync(placa))
                .ThrowsAsync(new Exception("Erro genérico"));

            // Act
            var result = await _controller.GetMotorcyclesAsync(placa);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(badRequestResult.Value);

            // Verifica se a mensagem padrão de erro é "Dados inválidos"
            Assert.Equal("Dados inválidos", messageException.Mensagem);
        }

        [Fact]
        public async Task GetMotorcycles_NoPlateProvided_ReturnsOk()
        {
            // Arrange
            var motorcycles = new List<MotorcycleDto>
    {
        new MotorcycleDto { Identificador = "id123", Placa = "XYZ123" },
        new MotorcycleDto { Identificador = "id456", Placa = "ABC456" }
    };
            _motorcycleServiceMock.Setup(s => s.GetMotorcyclesAsync(null))
                .ReturnsAsync(motorcycles);

            // Act
            var result = await _controller.GetMotorcyclesAsync(null);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedMotorcycles = Assert.IsType<List<MotorcycleDto>>(okResult.Value);
            Assert.Equal(2, returnedMotorcycles.Count);
        }
    }
}