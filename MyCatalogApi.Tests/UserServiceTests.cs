using Moq;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Application.ResultPattern;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.DTOs.OutputObjects;
using MyCatalogApi.Domain.Entities;
using MyCatalogApi.Application.Services;
using Microsoft.Extensions.Logging;

namespace MyCatalogApi.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IPokemonService> _mockPokemonService;
        private readonly Mock<IFilmesService> _mockFilmesService;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockLogger = new Mock<ILogger<UserService>>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPokemonService = new Mock<IPokemonService>();
            _mockFilmesService = new Mock<IFilmesService>();
            _userService = new UserService(_mockLogger.Object,
                                           _mockUserRepository.Object,
                                           _mockFilmesService.Object,
                                           _mockPokemonService.Object);
        }

        [Fact]
        public async Task AdicionarUsuario_Success_ReturnsSuccessResult()
        {
            // Arrange
            var newUser = new NovoUserDto { Name = "John", Apelido = "Doe" };
            _mockUserRepository.Setup(repo => repo.AdicionarUsuario(It.IsAny<NovoUserDto>())).ReturnsAsync(1);

            // Act
            var result = await _userService.AdicionarUsuario(newUser);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Usuário criado com sucesso, seu Id de usuário agora é: 1", result.Value);
        }

        [Fact]
        public async Task AdicionarUsuario_Failure_ReturnsFailureResult()
        {
            // Arrange
            var newUser = new NovoUserDto { Name = "John", Apelido = "Doe" };
            _mockUserRepository.Setup(repo => repo.AdicionarUsuario(It.IsAny<NovoUserDto>())).ReturnsAsync(0);

            // Act
            var result = await _userService.AdicionarUsuario(newUser);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Houve um problema para adicionar o usuario.", result.ErrorMessage);
        }

        [Fact]
        public async Task CatalogoDoUsuario_Success_ReturnsUserCatalog()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Nome = "John", Apelido = "Doe" };
            var pokemonList = new List<PokemonDto> { new PokemonDto() };
            var filmeList = new List<Filme> { new Filme() };

            _mockUserRepository.Setup(repo => repo.UsuarioPorId(userId)).ReturnsAsync(user);
            _mockPokemonService.Setup(service => service.PokemonsCapturadorPorUsuarioId(userId))
                               .ReturnsAsync(Result<List<PokemonDto>>.Success(pokemonList));
            _mockFilmesService.Setup(service => service.FilmesDaMarvelPorUsuarioId(userId))
                               .ReturnsAsync(Result<List<Filme>>.Success(filmeList));

            // Act
            var result = await _userService.CatalogoDoUsuario(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value.Pokemons);
            Assert.NotEmpty(result.Value.Filmes);
        }

        [Fact]
        public async Task CatalogoDoUsuario_UserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(repo => repo.UsuarioPorId(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.CatalogoDoUsuario(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não encontrado pelo ID: 1", result.ErrorMessage);
        }

        [Fact]
        public async Task BuscarUsuariosPorFiltro_Success_ReturnsFilteredUsers()
        {
            // Arrange
            var userId = 1;
            var nome = "John";
            var apelido = "Doe";
            var userList = new List<User> { new User { UserId = 1, Nome = "John", Apelido = "Doe" } };

            _mockUserRepository.Setup(repo => repo.BuscarUsuariosPorFiltro(userId, nome, apelido))
                               .ReturnsAsync(userList);

            // Act
            var result = await _userService.BuscarUsuariosPorFiltro(userId, nome, apelido);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal("John", result.Value.First().Nome);
        }

        [Fact]
        public async Task BuscarUsuariosPorFiltro_NoUsersFound_ReturnsFailureResult()
        {
            // Arrange
            var userId = 1;
            var nome = "John";
            var apelido = "Doe";
            _mockUserRepository.Setup(repo => repo.BuscarUsuariosPorFiltro(userId, nome, apelido))
                               .ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.BuscarUsuariosPorFiltro(userId, nome, apelido);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Nenhum usuário encontrado com os filtros fornecidos", result.ErrorMessage);
        }

        [Fact]
        public async Task BuscaUsuarioPorId_Success_ReturnsUser()
        {
            // Arrange
            var userId = 1;
            var user = new User { UserId = userId, Nome = "John", Apelido = "Doe" };
            _mockUserRepository.Setup(repo => repo.UsuarioPorId(userId)).ReturnsAsync(user);

            // Act
            var result = await _userService.BuscaUsuarioPorId(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(userId, result.Value.UserId);
        }

        [Fact]
        public async Task BuscaUsuarioPorId_UserNotFound_ReturnsFailureResult()
        {
            // Arrange
            var userId = 1;
            _mockUserRepository.Setup(repo => repo.UsuarioPorId(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userService.BuscaUsuarioPorId(userId);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Usuário não encontrado pelo ID: 1", result.ErrorMessage);
        }

        [Fact]
        public async Task TodosOsUsuarios_Success_ReturnsAllUsers()
        {
            // Arrange
            var userList = new List<User> { new User { UserId = 1, Nome = "John", Apelido = "Doe" } };
            _mockUserRepository.Setup(repo => repo.TodosOsUsuarios()).ReturnsAsync(userList);

            // Act
            var result = await _userService.TodosOsUsuarios();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Value.Count);
        }

        [Fact]
        public async Task TodosOsUsuarios_NoUsersFound_ReturnsFailureResult()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.TodosOsUsuarios()).ReturnsAsync(new List<User>());

            // Act
            var result = await _userService.TodosOsUsuarios();

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Não achei usuários cadastrados na base no momento", result.ErrorMessage);
        }
    }
}
