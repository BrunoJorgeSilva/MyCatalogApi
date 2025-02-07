using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Application.Services;
using MyCatalogApi.Domain.Entities;
using MyCatalogApi.Domain.DTOs.EntryObjects;

namespace MyCatalogApi.Tests
{
    public class PokemonServiceTests
    {
        private readonly Mock<ILogger<PokemonService>> _loggerMock;
        private readonly Mock<IPokemonRepository> _pokemonRepositoryMock;
        private readonly PokemonService _pokemonService;

        public PokemonServiceTests()
        {
            _loggerMock = new Mock<ILogger<PokemonService>>();
            _pokemonRepositoryMock = new Mock<IPokemonRepository>();
            _pokemonService = new PokemonService(_loggerMock.Object, _pokemonRepositoryMock.Object);
        }

        [Fact]
        public async Task PokemonsCapturadorPorUsuarioId_ReturnsSuccess_WhenPokemonsAreFound()
        {
            // Arrange
            int userId = 1;
            var pokemonList = new List<Pokemon>
            {
                new Pokemon { Id = 1, Name = "Pikachu"},
                new Pokemon { Id = 2, Name = "Bulbasaur"}
            };
            _pokemonRepositoryMock.Setup(repo => repo.PokemonsPorUsuarioId(userId))
                                  .ReturnsAsync(pokemonList);

            // Act
            var result = await _pokemonService.PokemonsCapturadorPorUsuarioId(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);
            
            _loggerMock.Verify(logger => logger.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.AtLeastOnce);
        }



        [Fact]
        public async Task PokemonsCapturadorPorUsuarioId_ReturnsFailure_WhenNoPokemonsAreFound()
        {
            // Arrange
            int userId = 1;
            _pokemonRepositoryMock.Setup(repo => repo.PokemonsPorUsuarioId(userId))
                                  .ReturnsAsync(new List<Pokemon>());

            // Act
            var result = await _pokemonService.PokemonsCapturadorPorUsuarioId(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Pokemons não encontrados para o usuário", result.ErrorMessage);
        }

        [Fact]
        public async Task ProcessarPokemonsCapturadosCsv_ReturnsFailure_WhenErrorOccursDuringCsvProcessing()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            int userId = 1;
            _pokemonRepositoryMock.Setup(repo => repo.InserirCapturados(It.IsAny<List<int>>(), userId))
                                  .ReturnsAsync(false);

            // Act
            var result = await _pokemonService.ProcessarPokemonsCapturadosCsv(mockFile.Object, userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Não foi possível converter o csv, revise", result.ErrorMessage); 
        }

        [Fact]
        public async Task FiltrarPokemons_ReturnsSuccess_WhenPokemonsAreFiltered()
        {
            // Arrange
            var filter = new PokemonFilter { Name = "Pikachu" };
            var pokemonList = new List<Pokemon>
            {
                new Pokemon { Id = 1, Name = "Pikachu" }
            };

            _pokemonRepositoryMock.Setup(repo => repo.FiltrarPokemons(filter))
                                  .ReturnsAsync(pokemonList);

            // Act
            var result = await _pokemonService.FiltrarPokemons(filter);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Value);
        }

        [Fact]
        public async Task FiltrarPokemons_ReturnsFailure_WhenNoPokemonsAreFiltered()
        {
            // Arrange
            var filter = new PokemonFilter { Name = "Unknown" };
            _pokemonRepositoryMock.Setup(repo => repo.FiltrarPokemons(filter))
                                  .ReturnsAsync(new List<Pokemon>());

            // Act
            var result = await _pokemonService.FiltrarPokemons(filter);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Nenhum pokemon encontrado com os critérios fornecidos", result.ErrorMessage);
        }
    }
}
