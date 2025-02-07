using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using MyCatalogApi.Application.Services;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Application.ResultPattern;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.Entities;
using MyCatalogApi.Domain.ValueObjects;
using MyCatalogApi.Application.Helpers;

namespace MyCatalogApi.Tests.Services
{
    public class FilmesServiceTests
    {
        private readonly Mock<ILogger<FilmesService>> _mockLogger;
        private readonly Mock<IFilmeRepository> _mockFilmeRepository;
        private readonly FilmesService _filmesService;

        public FilmesServiceTests()
        {
            _mockLogger = new Mock<ILogger<FilmesService>>();
            _mockFilmeRepository = new Mock<IFilmeRepository>();
            _filmesService = new FilmesService(_mockLogger.Object, _mockFilmeRepository.Object);
        }

        [Fact]
        public async Task FilmesDaMarvelPorUsuarioId_DeveRetornarFilmes_QuandoExistiremFilmes()
        {
            // Arrange
            int userId = 1;
            var filmes = new List<Filme> { new Filme { FilmeId = 1, Titulo = "Iron Man" }, new Filme { FilmeId = 2, Titulo = "Thor" } };
            _mockFilmeRepository.Setup(repo => repo.FilmesDoUsuario(userId)).ReturnsAsync(filmes);

            // Act
            var result = await _filmesService.FilmesDaMarvelPorUsuarioId(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Início do processo")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public async Task FilmesDaMarvelPorUsuarioId_DeveRetornarFalha_QuandoNaoExistiremFilmes()
        {
            // Arrange
            _mockFilmeRepository.Setup(repo => repo.FilmesDoUsuario(It.IsAny<int>()))
                .ReturnsAsync(new List<Filme>());  

            int userId = 999;

            // Act
            var result = await _filmesService.FilmesDaMarvelPorUsuarioId(userId);

            // Assert
            Assert.False(result.IsSuccess);
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Nenhum filme da Marvel encontrado")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task UploadFilmesCsv_DeveRetornarFalha_QuandoInsercaoNoBancoFalhar()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            int userId = 1;
            var csvContent = "Titulo,Diretor,TempoDeFilme,Ano\nIron Man,Jon Favreau,120,2008";
            var csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);
            var memoryStream = new MemoryStream(csvBytes);

            fileMock.Setup(f => f.OpenReadStream()).Returns(memoryStream);
            fileMock.Setup(f => f.Length).Returns(csvBytes.Length);

            var filmes = new List<FilmeCsvVO> { new FilmeCsvVO { Titulo = "Iron Man" } };
            _mockFilmeRepository.Setup(repo => repo.InserirFilmesVistosUsuario(filmes, userId)).ReturnsAsync(false);

            // Act
            var result = await _filmesService.UploadFilmesCsv(fileMock.Object, userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.False(result.Value);
        }

        [Fact]
        public async Task FiltrarFilmes_DeveRetornarFilmes_QuandoFiltragemForBemSucedida()
        {
            // Arrange
            var filter = new FilmeFilter { Titulo = "Iron" };
            var filmes = new List<Filme> { new Filme { Titulo = "Iron Man" } };
            _mockFilmeRepository.Setup(repo => repo.FiltrarFilmes(filter)).ReturnsAsync(filmes);

            // Act
            var result = await _filmesService.FiltrarFilmes(filter);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
            Assert.Equal("Iron Man", result.Value.First().Titulo);
        }

        [Fact]
        public async Task FiltrarFilmes_DeveRetornarFalha_QuandoNenhumFilmeForEncontrado()
        {
            // Arrange
            var filter = new FilmeFilter { Titulo = "Inexistente" };
            _mockFilmeRepository.Setup(repo => repo.FiltrarFilmes(filter)).ReturnsAsync(new List<Filme>());

            // Act
            var result = await _filmesService.FiltrarFilmes(filter);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Empty(result.Value);
        }
    }
}
