using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Mottu.Domain.Dtos;
using System.Net;
using System.Net.Http.Json;

namespace Mottu.ApiTests.Tests
{
    public class ControllerIntegrationTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
    {
        [Fact]
        public async Task CreateMotorcycle_Returns201()
        {
            // Arrange
            var motorcycleDto = new MotorcycleDto { Identificador = "id123", Placa = "XYZ123", Ano = 2024, Modelo = "YBR" };

            // Act
            var response = await _client.PostAsJsonAsync("/motos", motorcycleDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetAllMotorcycles_ReturnsMotorcycles()
        {
            // Arrange
            var motorcycleDto1 = new MotorcycleDto { Identificador = "id123", Placa = "XYZ123", Ano = 2024, Modelo = "YBR" };
            var motorcycleDto2 = new MotorcycleDto { Identificador = "id456", Placa = "ABC789", Ano = 2025, Modelo = "Honda" };

            // Seed database
            await _client.PostAsJsonAsync("/motos", motorcycleDto1);
            await _client.PostAsJsonAsync("/motos", motorcycleDto2);

            // Act
            var response = await _client.GetAsync("/motos");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<MotorcycleDto>>();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }


        [Fact]
        public async Task UpdateMotorcycle_ReturnsSuccess()
        {
            // Arrange
            var id = "id123";
            var initialMotorcycleDto = new MotorcycleDto { Identificador = id, Placa = "XYZ123", Ano = 2024, Modelo = "YBR" };
            await _client.PostAsJsonAsync("/motos", initialMotorcycleDto);

            string updatePlate = "ABC789";

            // Act
            var response = await _client.PutAsJsonAsync($"/motos/{id}/placa", updatePlate);

            // Assert
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<MessageDto>();
            Assert.Equal("Placa modificada com sucesso", result?.Mensagem);


            var getResponse = await _client.GetAsync($"/motos/{id}");
            getResponse.EnsureSuccessStatusCode();
            var resultGet = await getResponse.Content.ReadFromJsonAsync<MotorcycleDto>();
            Assert.Equal(initialMotorcycleDto.Identificador, resultGet?.Identificador);
            Assert.Equal(updatePlate, resultGet?.Placa);
            Assert.Equal(initialMotorcycleDto.Ano, resultGet?.Ano);
            Assert.Equal(initialMotorcycleDto.Modelo, resultGet?.Modelo);
        }


        [Fact]
        public async Task GetMotorcycleById_ReturnsMotorcycle()
        {
            // Arrange
            var id = "id123";
            var motorcycleDto = new MotorcycleDto { Identificador = id, Placa = "XYZ123", Ano = 2024, Modelo = "YBR" };

            // Seed the in-memory database
            await _client.PostAsJsonAsync("/motos", motorcycleDto);

            // Act
            var response = await _client.GetAsync($"/motos/{id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<MotorcycleDto>();
            Assert.Equal(motorcycleDto.Identificador, result?.Identificador);
            Assert.Equal(motorcycleDto.Placa, result?.Placa);
        }

        [Fact]
        public async Task DeleteMotorcycle_ReturnsNoContent()
        {
            // Arrange
            var id = "id123";
            var motorcycleDto = new MotorcycleDto { Identificador = id, Placa = "XYZ123", Ano = 2024, Modelo = "YBR" };

            // Seed database
            await _client.PostAsJsonAsync("/motos", motorcycleDto);

            // Act
            var deleteResponse = await _client.DeleteAsync($"/motos/{id}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync($"/motos/{id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

    }
}