using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using FiapConnect.Controllers;
using FiapConnect.Models;
using FiapConnect.DTOs;
using Xunit;

namespace FiapConnect.Tests.Unit
{
    /// <summary>
    /// Testes unitários do GrupoController seguindo o padrão AAA
    /// (Arrange - Act - Assert)
    /// </summary>
    public class GrupoControllerTests
    {
        private readonly Mock<ILogger<GrupoController>> _loggerMock;
        private readonly GrupoController _controller;

        public GrupoControllerTests()
        {
            _loggerMock = new Mock<ILogger<GrupoController>>();
            _controller = new GrupoController(_loggerMock.Object);
        }

        [Fact]
        public void GetGrupos_QuandoExistemGrupos_RetornaOkComLista()
        {
            // Arrange
            // Controller já possui grupos pré-carregados em memória

            // Act
            var resultado = _controller.GetGrupos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var grupos = Assert.IsAssignableFrom<IEnumerable<GrupoDTO>>(okResult.Value);
            Assert.NotEmpty(grupos);
        }

        [Fact]
        public void CreateGrupo_QuandoDadosValidos_RetornaCreated()
        {
            // Arrange
            var novoGrupo = new Grupo
            {
                NomeGrupo = "Grupo Teste Sprint 3",
                DescricaoProjeto = "Projeto para demonstração dos testes",
                DisciplinaTema = "Mobile",
                MaxIntegrantes = 3
            };

            // Act
            var resultado = _controller.CreateGrupo(novoGrupo);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            var grupoCriado = Assert.IsType<GrupoDTO>(createdResult.Value);
            Assert.Equal("ABERTO", grupoCriado.StatusGrupo);
        }

        [Fact]
        public void CreateGrupo_QuandoMaxIntegrantesInvalido_RetornaBadRequest()
        {
            // Arrange
            var grupoInvalido = new Grupo
            {
                NomeGrupo = "Grupo Inválido",
                DescricaoProjeto = "Teste",
                DisciplinaTema = "Mobile",
                MaxIntegrantes = 10
            };

            // Act
            var resultado = _controller.CreateGrupo(grupoInvalido);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }
    }
}
