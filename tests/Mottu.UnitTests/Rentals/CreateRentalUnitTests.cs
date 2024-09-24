using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Rentals
{
    public class CreateRentalUnitTests
    {
        private readonly Mock<IRentalService> _rentalServiceMock;
        private readonly Mock<ILogger<RentalController>> _loggerMock;
        private readonly RentalController _controller;

        public CreateRentalUnitTests()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _loggerMock = new Mock<ILogger<RentalController>>();
            _controller = new RentalController(_rentalServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateRental_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var rentalDto = new CreateRentalDto();
            _controller.ModelState.AddModelError("MotoId", "MotoId is required");

            // Act
            var result = await _controller.CreateRental(rentalDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(badRequestResult.Value);
            Assert.Equal("Dados inválidos", messageException.Mensagem);
        }

        [Fact]
        public async Task CreateRental_ValidRental_CreatesRental()
        {
            // Arrange
            var rentalDto = new CreateRentalDto { MotoId = "moto-123", EntregadorId = "entregador-123", DataInicio = DateTime.UtcNow.AddDays(1), DataTermino = DateTime.UtcNow.AddDays(5) };

            // Act
            var result = await _controller.CreateRental(rentalDto);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, statusCodeResult.StatusCode);
            _rentalServiceMock.Verify(s => s.CreateRentalAsync(rentalDto), Times.Once);
        }
    }
}