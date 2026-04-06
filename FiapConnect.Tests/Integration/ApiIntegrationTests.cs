using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace FiapConnect.Tests.Integration
{
    /// <summary>
    /// Testes de integração validando o fluxo completo HTTP da API.
    /// Utiliza WebAppFixture compartilhado via Collection Fixture,
    /// evitando subir múltiplas instâncias da aplicação em memória.
    /// Padrão de nomenclatura: MetodoTestado_Cenario_ResultadoEsperado
    /// </summary>
    [Collection("IntegrationTests")]
    public class ApiIntegrationTests
    {
        private readonly HttpClient _client;

        public ApiIntegrationTests(WebAppFixture fixture)
        {
            _client = fixture.Client;
        }

        // ── Usuário ───────────────────────────────────────────────────────────

        [Fact]
        public async Task GetUsuarios_QuandoApiDisponivel_RetornaStatusOk()
        {
            // Arrange
            var url = "/api/usuario";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateUsuario_QuandoDadosValidos_RetornaStatusCreated()
        {
            // Arrange
            var novoUsuario = new
            {
                RM = "RM11111",
                NomeCompleto = "Teste Collection Fixture",
                EmailInstitucional = "teste.fixture@fiap.com.br",
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
        public async Task CreateUsuario_QuandoEmailForaDoDominioFiap_RetornaStatusBadRequest()
        {
            // Arrange
            var usuarioEmailInvalido = new
            {
                RM = "RM22222",
                NomeCompleto = "Email Fora do Domínio",
                EmailInstitucional = "usuario@gmail.com",
                Curso = "ADS",
                Periodo = "Noturno",
                Turma = "2TDSPS",
                StatusBusca = true
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/usuario", usuarioEmailInvalido);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        // ── Grupo ─────────────────────────────────────────────────────────────

        [Fact]
        public async Task GetGrupos_QuandoApiDisponivel_RetornaStatusOk()
        {
            // Arrange
            var url = "/api/grupo";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetGruposAbertos_QuandoExistemGruposComVagas_RetornaStatusOk()
        {
            // Arrange
            var url = "/api/grupo/abertos";

            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // ── Health Check ──────────────────────────────────────────────────────

        [Fact]
        public async Task HealthCheckSimple_QuandoApiDisponivel_RetornaStatusOk()
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