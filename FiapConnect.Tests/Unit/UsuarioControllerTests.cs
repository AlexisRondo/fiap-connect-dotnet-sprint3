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
    /// Testes unitários do UsuarioController seguindo o padrão AAA
    /// (Arrange - Act - Assert)
    /// </summary>
    public class UsuarioControllerTests
    {
        private readonly Mock<ILogger<UsuarioController>> _loggerMock;
        private readonly UsuarioController _controller;

        public UsuarioControllerTests()
        {
            _loggerMock = new Mock<ILogger<UsuarioController>>();
            _controller = new UsuarioController(_loggerMock.Object);
        }

        [Fact]
        public void GetUsuarios_QuandoExistemUsuarios_RetornaOkComLista()
        {
            // Arrange
            // Controller já possui usuários pré-carregados em memória

            // Act
            var resultado = _controller.GetUsuarios();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var usuarios = Assert.IsAssignableFrom<IEnumerable<UsuarioDTO>>(okResult.Value);
            Assert.NotEmpty(usuarios);
        }

        [Fact]
        public void GetUsuario_QuandoIdExiste_RetornaOkComUsuario()
        {
            // Arrange
            var idExistente = 1;

            // Act
            var resultado = _controller.GetUsuario(idExistente);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var usuario = Assert.IsType<UsuarioDTO>(okResult.Value);
            Assert.Equal(idExistente, usuario.IdUsuario);
        }

        [Fact]
        public void CreateUsuario_QuandoDadosValidos_RetornaCreated()
        {
            // Arrange
            var novoUsuario = new Usuario
            {
                RM = "RM99999",
                NomeCompleto = "Teste Unitário",
                EmailInstitucional = "teste.unitario@fiap.com.br",
                Curso = "ADS",
                Periodo = "Noturno",
                Turma = "2TDSPS",
                StatusBusca = true
            };

            // Act
            var resultado = _controller.CreateUsuario(novoUsuario);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(resultado.Result);
            var usuarioCriado = Assert.IsType<UsuarioDTO>(createdResult.Value);
            Assert.Equal("RM99999", usuarioCriado.RM);
        }

        [Fact]
        public void CreateUsuario_QuandoEmailInvalido_RetornaBadRequest()
        {
            // Arrange
            var usuarioEmailInvalido = new Usuario
            {
                RM = "RM88888",
                NomeCompleto = "Email Inválido",
                EmailInstitucional = "email@gmail.com",
                Curso = "ADS"
            };

            // Act
            var resultado = _controller.CreateUsuario(usuarioEmailInvalido);

            // Assert
            Assert.IsType<BadRequestObjectResult>(resultado.Result);
        }
    }
}
