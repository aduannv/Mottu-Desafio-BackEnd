using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Rentals
{
    public class GetRentalUnitTests
    {
        private readonly Mock<IRentalService> _rentalServiceMock;
        private readonly Mock<ILogger<RentalController>> _loggerMock;
        private readonly RentalController _controller;

        public GetRentalUnitTests()
        {
            _rentalServiceMock = new Mock<IRentalService>();
            _loggerMock = new Mock<ILogger<RentalController>>();
            _controller = new RentalController(_rentalServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetRental_InvalidId_ReturnsNotFound()
        {
            // Arrange
            string rentalId = "invalid-id";
            _rentalServiceMock.Setup(s => s.GetRentalAsync(rentalId))
                .ThrowsAsync(new KeyNotFoundException("Locação não encontrada"));

            // Act
            var result = await _controller.GetRental(rentalId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(notFoundResult.Value);
            Assert.Equal("Locação não encontrada", messageException.Mensagem);
        }

        [Fact]
        public async Task GetRental_ValidId_ReturnsRental()
        {
            // Arrange
            var rentalDto = new GetRentalDto { Identificador = "rental-123" };
            _rentalServiceMock.Setup(s => s.GetRentalAsync("rental-123")).ReturnsAsync(rentalDto);

            // Act
            var result = await _controller.GetRental("rental-123");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedRental = Assert.IsType<GetRentalDto>(okResult.Value);
            Assert.Equal(rentalDto.Identificador, returnedRental.Identificador);
        }
    }
}