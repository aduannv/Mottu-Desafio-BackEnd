using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Mottu.Domain.Dtos;
using System.Net;
using System.Net.Http.Json;

namespace Mottu.ApiTests.Tests
{
    public class DeliverersControllerIntegrationTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
    {
        [Fact]
        public async Task CreateDeliverer_Returns201()
        {
            // Arrange
            var delivererDto = new DelivererDto
            {
                Identificador = "deliv123",
                Cnpj = "12345678000195",
                NumeroCnh = "12345678900",
                Nome = "Entregador",
                TipoCnh = "A"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/entregadores", delivererDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task ChangeCnhImage_Returns201()
        {
            // Arrange
            var delivererDto = new DelivererDto
            {
                Identificador = "deliv123",
                Cnpj = "12345678000195",
                NumeroCnh = "12345678900",
                Nome = "Entregador",
                TipoCnh = "A"
            };

            await _client.PostAsJsonAsync("/entregadores", delivererDto);
            var imagemCnhDto = new ImagemCnhDto
            {
                ImagemCnh = "iVBORw0KGgoAAAANSUhEUgAAAeAAAAHgCAMAAABKCk6nAAAABGdBTUEAALGPC/xhBQAAACBjSFJNAAB6JgAAgIQAAPoAAACA6AAAdTAAAOpgAAA6mAAAF3CculE8AAAA/1BMVEUAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAaBwBoGgCfKAC3LgC2LgBnGgAZBgAcBwD8PwD/QACeKAA8DwDqOwDtOwBCEQBAEAD6PwD5PgA/EAAbBwDsOwDrOwCcJwCbJwAXBgD7PwAWBgBlGQBjGQCaJwCZJgCzLQCyLQCYJgBiGQDpOgCdJwBmGgC1LQAYBgD///8cojVtAAAAK3RSTlMAHGq21/Ibm/uaGk9Ob/1uUPOenB/8Hm25t9va9PXcbB2d8UyYGWm01rNoQVVekgAAAAFiS0dEVOQDiKUAAAAHdElNRQfoBhIHOzfC0wVJAAACJUlEQVR42u3aCVITURCAYQQmLMHBLeACohERbYwSMqAIhkWQRRHw/nextFSU4QTN9x3h76qpfvPewAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA19mNwaHhohgeGmxokVBjZDR+Gx0b1yObZhGx9Lrz5m1nuRtRTCiSy80yVjq96pfe6kqUk5pkcquMtXfVX+/Xo2yqkmi9uh0fNqp/bH6MwqqVx53ob1X/2e7GXV2yuNeKneqS3WhNKZPEZHzqXR7w3n5MK5PE/fhc1RzEA2WSeBiH9QEfxSNlkpiJ4/qAj2NGmSRmrxrwFwNO43Ec1Qf81Sc60ZJ1UB/wiSUr0TFpf++KY9KcMklMtWK3vkS3niiTxdNY2q79qhzRJY2flw2bLhsSa5ax9u1ivlvrUbryz7VnldE//XPhv9OP0o/oZCaKiO7Z6vn56dn3iPYzRbIZH7t4dDf/XI+EGgsvFtvtxZevvKkEAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAuN5+AJQhV/XqpdETAAAAJXRFWHRkYXRlOmNyZWF0ZQAyMDI0LTA2LTE4VDA3OjU5OjU1KzAwOjAw070K/gAAACV0RVh0ZGF0ZTptb2RpZnkAMjAyNC0wNi0xOFQwNzo1OTo1NSswMDowMKLgskIAAAAASUVORK5CYII="
            };

            // Act
            var response = await _client.PostAsJsonAsync("/entregadores/deliv123/cnh", imagemCnhDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateDeliverer_InvalidData_Returns400()
        {
            // Arrange
            var delivererDto = new DelivererDto(); // Invalid DTO (e.g., missing required fields)

            // Act
            var response = await _client.PostAsJsonAsync("/entregadores", delivererDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ChangeCnhImage_InvalidData_Returns400()
        {
            // Arrange
            var delivererDto = new DelivererDto { Identificador = "deliv123", Cnpj = "12345678000195", NumeroCnh = "12345678900", Nome = "Entregador" };
            await _client.PostAsJsonAsync("/entregadores", delivererDto);
            var imagemCnhDto = new ImagemCnhDto(); // Invalid DTO

            // Act
            var response = await _client.PostAsJsonAsync("/entregadores/deliv123/cnh", imagemCnhDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateDeliverer_ExceptionThrown_Returns400()
        {
            // Arrange
            var delivererDto = new DelivererDto
            {
                Identificador = "deliv123",
                Cnpj = "12345678000195",
                NumeroCnh = "12345678900",
                Nome = "Entregador",
                TipoCnh = "A"
            };

            await _client.PostAsJsonAsync("/entregadores", delivererDto);
            var imagemCnhDto = new ImagemCnhDto { ImagemCnh = "base64image" };

            // Simulate an error by modifying the mock service to throw an exception
            // This part would typically involve a specific setup in your test environment or a dedicated test for exceptions.

            // Act
            var response = await _client.PostAsJsonAsync("/entregadores/deliv123/cnh", imagemCnhDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
