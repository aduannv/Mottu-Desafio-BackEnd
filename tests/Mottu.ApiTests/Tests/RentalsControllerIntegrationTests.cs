using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Mottu.Domain.Dtos;
using Mottu.Domain.Enums;
using System.Net;
using System.Net.Http.Json;

namespace Mottu.ApiTests.Tests;
public class RentalsControllerIntegrationTests(WebApplicationFactory<Program> factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateRental_Returns201()
    {
        // Arrange
        var delivererDto = new DelivererDto
        {
            Identificador = "entregador123",
            Cnpj = "12345678000195",
            NumeroCnh = "12345678900",
            Nome = "Entregador",
            TipoCnh = "A"
        };

        await _client.PostAsJsonAsync("/entregadores", delivererDto);

        var motorcycleDto = new MotorcycleDto
        {
            Identificador = "moto123",
            Placa = "XYZ123",
            Ano = 2024,
            Modelo = "YBR"
        };

        await _client.PostAsJsonAsync("/motos", motorcycleDto);

        var rentalDto = new CreateRentalDto
        {
            MotoId = "moto123",
            EntregadorId = "entregador123",
            DataInicio = DateTime.UtcNow.AddDays(1),
            DataTermino = DateTime.UtcNow.AddDays(5),
            DataPrevisaoTermino = DateTime.UtcNow.AddDays(5),
            Plano = (int)EPlanoLocacao.SeteDias
        };

        // Act
        var response = await _client.PostAsJsonAsync("/locacao", rentalDto);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
