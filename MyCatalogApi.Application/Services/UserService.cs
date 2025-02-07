using Microsoft.Extensions.Logging;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Application.ResultPattern;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.DTOs.OutputObjects;
using MyCatalogApi.Domain.Entities;

namespace MyCatalogApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IUserRepository _userRepository;
        private readonly IPokemonService _pokemonService;
        private readonly IFilmesService _filmesService;

        public UserService(ILogger<UserService> logger,
                           IUserRepository userRepository,
                           IFilmesService filmesService,
                           IPokemonService pokemonService)
        {
            _logger = logger;   
            _userRepository = userRepository;
            _filmesService = filmesService;
            _pokemonService = pokemonService;
        }


        public async Task<Result<string>> AdicionarUsuario(NovoUserDto user)
        {
            _logger.LogInformation($"[UserService.AdicionarUsuario] Vai adicionar o usuário de nome: {user.Name}, apelido: {user.Apelido}");
            int usuarioAdicionado = 0;
            try
            {
                usuarioAdicionado = await _userRepository.AdicionarUsuario(user);
                _logger.LogInformation($"[UserService.AdicionarUsuario] Resultado para o nome {user.Name}, apelido {user.Apelido} IdCriado: {usuarioAdicionado}");
                if (usuarioAdicionado <= 0) 
                {
                    return Result<string>.Failure($"Houve um problema para adicionar o usuario.", "Erro ao criar seu usuário.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UserService.AdicionarUsuario] Error: {ex.Message}", ex);
                return Result<string>.Failure($"Houve um problema para adicionar o usuario: {ex.Message}", "Erro ao criar seu usuário.");
            }
            return Result<string>.Success($"Usuário criado com sucesso, seu Id de usuário agora é: {usuarioAdicionado}");

        }

        public async Task<Result<UserCatalogoCompletoOutPutDto>> CatalogoDoUsuario(int userId)
        {
            _logger.LogInformation($"[UserService.CatalogoDoUsuario] Vai procurar o catálogo completo do usuário: {userId}");
            UserCatalogoCompletoOutPutDto userCatalogoCompleto = new UserCatalogoCompletoOutPutDto();
            try
            {
                var usuario = await BuscaUsuarioPorId(userId);
                if (usuario == null || !usuario.IsSuccess)
                {
                    return Result<UserCatalogoCompletoOutPutDto>.Failure($"Usuário não encontrado pelo ID: {userId}", new UserCatalogoCompletoOutPutDto());
                }
                userCatalogoCompleto = UserCatalogoCompletoOutPutDto.PreencheDadosUsuario(usuario?.Value);
                var pokemonsDoUsuario = await _pokemonService.PokemonsCapturadorPorUsuarioId(userId);
                var filmesDoUsuario = await _filmesService.FilmesDaMarvelPorUsuarioId(userId);
                userCatalogoCompleto = UserCatalogoCompletoOutPutDto.PreenchePokemonsEFilmes(userCatalogoCompleto,
                                                                                             pokemonsDoUsuario.Value,
                                                                                             filmesDoUsuario.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UserService.CatalogoDoUsuario] Error: {ex.Message}", ex);
                return Result<UserCatalogoCompletoOutPutDto>.Failure($"Usuário não encontrado pelo ID: {userId}", new UserCatalogoCompletoOutPutDto());
            }
            return Result<UserCatalogoCompletoOutPutDto>.Success(userCatalogoCompleto);
        }
        public async Task<Result<User>> BuscaUsuarioPorId(int userId)
        {
            _logger.LogInformation($"[UserService.BuscaUsuarioPorId] Vai buscar o usuário pelo Id: {userId}");
            var user = new User();

            try
            {
                user = await _userRepository.UsuarioPorId(userId);
                if (user == null)
                {
                    return Result<User>.Failure($"Usuário não encontrado pelo ID: {userId}", new User());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UserService.AdicionarUsuario] Error: {ex.Message}", ex);
                return Result<User>.Failure($"Falha na pesquisa para o ID: {userId}, Erro: {ex.Message}", new User());
            }
            return Result<User>.Success(user);
        }

        public async Task<Result<List<User>>> TodosOsUsuarios()
        {
            _logger.LogInformation($"[UserService.TodosOsUsuarios] Vai Procurar todos os usuários da base cadastrados.");
            var todosOsUsuariosDaBase = new List<User>();

            try
            {
                todosOsUsuariosDaBase = await _userRepository.TodosOsUsuarios();
                if (todosOsUsuariosDaBase.Any())
                {
                    return Result<List<User>>.Success(todosOsUsuariosDaBase);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PokemonService.TodosOsUsuarios] Error: {ex.Message}", ex);
                return Result<List<User>>.Failure($"Houve um problema para adicionar o usuario: {ex.Message}", todosOsUsuariosDaBase);
            }
            return Result<List<User>>.Failure($"Não achei usuários cadastrados na base no momento", todosOsUsuariosDaBase);
        }

        public async Task<Result<List<User>>> BuscarUsuariosPorFiltro(int? userId, string? nome, string? apelido)
        {
            _logger.LogInformation($"[UserService.BuscarUsuariosPorFiltro] Filtro - UserId: {userId}, Nome: {nome}, Apelido: {apelido}");

            try
            {
                var usuarios = await _userRepository.BuscarUsuariosPorFiltro(userId, nome, apelido);
                if (usuarios.Any())
                {
                    return Result<List<User>>.Success(usuarios);
                }
                return Result<List<User>>.Failure("Nenhum usuário encontrado com os filtros fornecidos", usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[UserService.BuscarUsuariosPorFiltro] Error: {ex.Message}", ex);
                return Result<List<User>>.Failure($"Erro ao buscar usuários: {ex.Message}", new List<User>());
            }
        }

    }
}
