using FiapConnect.Application.Services;
using FiapConnect.Domain.Entities;
using FiapConnect.Domain.Exceptions;
using FiapConnect.Domain.Interfaces;
using Moq;

namespace FiapConnect.UnitTests.Application.Services;

public class AuditoriaServiceTests
{
    [Fact]
    public async Task ObterPorIdAsync_QuandoIdNaoExiste_LancaRecursoNaoEncontradoException()
    {
        // Arrange
        var repositoryMock = new Mock<IAuditoriaRepository>();
        repositoryMock
            .Setup(r => r.ObterPorIdAsync("inexistente"))
            .ReturnsAsync((Auditoria?)null);

        var service = new AuditoriaService(repositoryMock.Object);

        // Act
        Func<Task> acao = () => service.ObterPorIdAsync("inexistente");

        // Assert
        await Assert.ThrowsAsync<RecursoNaoEncontradoException>(acao);
    }

    [Fact]
    public async Task ObterPorIdAsync_QuandoIdExiste_RetornaResponse()
    {
        // Arrange
        var repositoryMock = new Mock<IAuditoriaRepository>();
        var auditoria = new Auditoria
        {
            Id = "abc",
            TabelaAfetada = "USUARIO",
            IdRegistro = 1,
            TipoOperacao = "INSERT",
            RmUsuario = "560384"
        };
        repositoryMock
            .Setup(r => r.ObterPorIdAsync("abc"))
            .ReturnsAsync(auditoria);

        var service = new AuditoriaService(repositoryMock.Object);

        // Act
        var resultado = await service.ObterPorIdAsync("abc");

        // Assert
        Assert.Equal("abc", resultado.Id);
        Assert.Equal("USUARIO", resultado.TabelaAfetada);
    }

    [Fact]
    public async Task RegistrarInternoAsync_QuandoChamado_DelegaAoRepository()
    {
        // Arrange
        var repositoryMock = new Mock<IAuditoriaRepository>();
        repositoryMock
            .Setup(r => r.RegistrarAsync(It.IsAny<Auditoria>()))
            .ReturnsAsync((Auditoria a) => a);

        var service = new AuditoriaService(repositoryMock.Object);

        // Act
        await service.RegistrarInternoAsync("tabela", 1, "INSERT", "560384", null, null);

        // Assert
        repositoryMock.Verify(r => r.RegistrarAsync(It.IsAny<Auditoria>()), Times.Once);
    }
}