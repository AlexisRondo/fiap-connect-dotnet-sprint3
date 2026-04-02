using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace FiapConnect.Tests.Integration
{
    /// <summary>
    /// Testes de integração validando o fluxo completo HTTP da API
    /// Utiliza WebApplicationFactory para subir a API em memória
    /// </summary>
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetUsuarios_RetornaStatusOk()
        {
            // Arrange
            var url = "/api/usuario";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateUsuario_QuandoDadosValidos_RetornaCreated()
        {
            // Arrange
            var novoUsuario = new
            {
                RM = "RM77777",
                NomeCompleto = "Teste Integração",
                EmailInstitucional = "teste.integracao@fiap.com.br",
                Curso = "ADS",
                Periodo = "Noturno",
                Turma = "2TDSPS",
                StatusBusca = true
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/usuario", novoUsuario);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task HealthCheck_RetornaStatusOk()
        {
            // Arrange
            var url = "/health/simple";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}