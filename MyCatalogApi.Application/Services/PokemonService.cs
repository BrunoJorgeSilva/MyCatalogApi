using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Application.ResultPattern;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.DTOs.OutputObjects;

namespace MyCatalogApi.Application.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly ILogger<PokemonService> _logger;

        private readonly IPokemonRepository _pokemonRepository;
        public PokemonService(ILogger<PokemonService> logger,
                              IPokemonRepository pokemonRepository)
        {
            _logger = logger;
            _pokemonRepository = pokemonRepository;
        }
        public async Task<Result<List<PokemonDto>>> PokemonsCapturadorPorUsuarioId(int userId)
        {
            _logger.LogInformation($"[PokemonService.PokemonsPorUsuarioId] Começo processo de pegar pokemons por userId{userId}");

            try
            {
                var pokemonsEntidade = await _pokemonRepository.PokemonsPorUsuarioId(userId);
                var pokemons = PokemonDto.RetornaListaTratada(pokemonsEntidade);

                _logger.LogInformation($"[PokemonService.PokemonsPorUsuarioId] Achados {pokemons.Count}, para o userId: {userId}");
                return pokemons.Count > 0 ? Result<List<PokemonDto>>.Success(pokemons) :
                                            Result<List<PokemonDto>>.Failure("Pokemons não encontrados para o usuário", pokemons);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PokemonService.PokemonsPorUsuarioId] Error: {ex.Message}", ex);
                return Result<List<PokemonDto>>.Failure($"Houve um problema na consulta: {ex.Message}", new List<PokemonDto>());
            }
        }

        public async Task<Result<bool>> ProcessarPokemonsCapturadosCsv(IFormFile file, int userId)
        {
            _logger.LogInformation($"[PokemonService.ProcessarPokemonsCapturadosCsv] Começo processo de processar pokemons capturados por CSV, para o usuário: {userId}");
            bool inseridoBanco;
            try
            {
                var resultPokemons = Helpers.CsvHelper.ProcessarCapturadosCsv(file);
                if (!resultPokemons.IsSuccess)
                {
                    string errorMessage = resultPokemons?.ErrorMessage ?? string.Empty;
                    return new Result<bool>(false, errorMessage, false);
                }
                inseridoBanco = await _pokemonRepository.InserirCapturados(resultPokemons?.Value, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[PokemonService.ProcessarPokemonsCapturadosCsv] Error: {ex.Message}", ex);
                return new Result<bool>(false, ex.Message, false);
            }
            _logger.LogInformation($"[PokemonService.ProcessarPokemonsCapturadosCsv] Fim do processo de processar Csv pokemons capturados para o usuário: {userId}, resultado: {inseridoBanco}");
            return inseridoBanco ? Result<bool>.Success(true) : Result<bool>.Failure($"Erro ao Inserir no banco de dados para o user {userId}.", false);
        }
        public async Task<Result<List<PokemonDto>>> FiltrarPokemons(PokemonFilter filter)
        {
            _logger.LogInformation($"[PokemonService.FiltrarPokemons] Início do processo de filtro de pokemons com os parâmetros: {filter}");

            try
            {
                var pokemonsEntidade = await _pokemonRepository.FiltrarPokemons(filter);
                if (pokemonsEntidade.Count > 0)
                {
                    var pokemons = PokemonDto.RetornaListaTratada(pokemonsEntidade);
                    _logger.LogInformation($"[PokemonService.FiltrarPokemons] Encontrados {pokemons.Count} pokemons com os filtros fornecidos.");
                    return Result<List<PokemonDto>>.Success(pokemons);
                }
                _logger.LogInformation($"[PokemonService.FiltrarPokemons] Nenhum pokemon encontrado com os filtros: {filter}");
                return Result<List<PokemonDto>>.Failure("Nenhum pokemon encontrado com os critérios fornecidos", new List<PokemonDto>());

            }
            catch (Exception ex)
            {
                _logger.LogError($"[PokemonService.FiltrarPokemons] Erro ao filtrar pokemons com os parâmetros {filter}: {ex.Message}", ex);
                return Result<List<PokemonDto>>.Failure($"Erro ao filtrar pokemons: {ex.Message}", new List<PokemonDto>());
            }
        }

    }
}
