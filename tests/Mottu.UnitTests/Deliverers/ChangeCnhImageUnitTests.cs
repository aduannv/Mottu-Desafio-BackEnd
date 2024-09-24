using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Mottu.App.Controllers;
using Mottu.Application.Exceptions;
using Mottu.Domain.Dtos;
using Mottu.Domain.Interfaces;

namespace Mottu.UnitTests.Deliverers
{
    public class ChangeCnhImageUnitTests
    {
        private readonly Mock<IDelivererService> _delivererServiceMock;
        private readonly Mock<ILogger<DeliverersController>> _loggerMock;
        private readonly DeliverersController _controller;

        public ChangeCnhImageUnitTests()
        {
            _delivererServiceMock = new Mock<IDelivererService>();
            _loggerMock = new Mock<ILogger<DeliverersController>>();
            _controller = new DeliverersController(_delivererServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task ChangeCnhImage_Returns201_WhenValidData()
        {
            // Arrange
            var id = "id123";
            var imagemCnhDto = new ImagemCnhDto { ImagemCnh = "Base64EncodedImageString" };

            // Act
            var result = await _controller.ChangeCnhImage(id, imagemCnhDto);

            // Assert
            var actionResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(201, actionResult.StatusCode);
            _delivererServiceMock.Verify(s => s.ChangeCnhImage(id, imagemCnhDto.ImagemCnh), Times.Once);
        }

        [Fact]
        public async Task ChangeCnhImage_Returns400_WhenModelStateIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("ImagemCnh", "Required");

            var id = "id123";
            var imagemCnhDto = new ImagemCnhDto { ImagemCnh = "" };

            // Act
            var result = await _controller.ChangeCnhImage(id, imagemCnhDto);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, actionResult.StatusCode);
            _delivererServiceMock.Verify(s => s.ChangeCnhImage(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task ChangeCnhImage_Returns400_WhenServiceThrowsException()
        {
            // Arrange
            var id = "id123";
            var imagemCnhDto = new ImagemCnhDto { ImagemCnh = "Base64EncodedImageString" };

            _delivererServiceMock
                .Setup(s => s.ChangeCnhImage(id, imagemCnhDto.ImagemCnh))
                .ThrowsAsync(new Exception("Error occurred"));

            // Act
            var result = await _controller.ChangeCnhImage(id, imagemCnhDto);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            var messageException = Assert.IsType<MessageException>(actionResult.Value);
            Assert.Equal("Dados inválidos", messageException.Mensagem);
            _delivererServiceMock.Verify(s => s.ChangeCnhImage(id, imagemCnhDto.ImagemCnh), Times.Once);
        }
    }
}
