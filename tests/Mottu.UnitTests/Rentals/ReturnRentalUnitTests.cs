using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Rentals
{
    public class ReturnRentalUnitTests
    {
        private readonly Mock<IRentalService> _rentalServiceMock;
        private readonly Mock<ILogger<RentalController>> _loggerMock;
        private readonly RentalController _controller;

        public ReturnRentalUnitTests()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _loggerMock = new Mock<ILogger<RentalController>>();
            _controller = new RentalController(_rentalServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ReturnRental_InvalidId_ReturnsNotFound()
        {
            // Arrange
            string rentalId = "invalid-id";
            var returnRentalDto = new ReturnRentalDto { DataDevolucao = DateTime.UtcNow };
            _rentalServiceMock.Setup(s => s.ReturnRentalAsync(rentalId, returnRentalDto))
                .ThrowsAsync(new KeyNotFoundException("Locação não encontrada"));

            // Act
            var result = await _controller.ReturnRental(rentalId, returnRentalDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(notFoundResult.Value);
            Assert.Equal("Locação não encontrada", messageException.Mensagem);
        }

        [Fact]
        public async Task ReturnRental_ValidId_ReturnsOk()
        {
            // Arrange
            var rentalId = "rental-123";
            var returnRentalDto = new ReturnRentalDto { DataDevolucao = DateTime.UtcNow };

            // Act
            var result = await _controller.ReturnRental(rentalId, returnRentalDto);

            // Assert
            var statusCodeResult = Assert.IsType<OkObjectResult>(result);
            _rentalServiceMock.Verify(s => s.ReturnRentalAsync(rentalId, returnRentalDto), Times.Once);
        }
    }
}