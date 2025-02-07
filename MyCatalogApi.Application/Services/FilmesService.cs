using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Application.ResultPattern;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.Entities;


namespace MyCatalogApi.Application.Services
{
    public class FilmesService : IFilmesService
    {
        private readonly ILogger<FilmesService> _logger;
        private readonly IFilmeRepository _filmeRepository;

        public FilmesService(ILogger<FilmesService> logger, IFilmeRepository filmeRepository)
        {
            _logger = logger;
            _filmeRepository = filmeRepository;
        }

        public async Task<Result<List<Filme>>> FilmesDaMarvelPorUsuarioId(int userId)
        {
            _logger.LogInformation($"[FilmesService.FilmesDaMarvelPorUsuarioId] Início do processo para buscar filmes da Marvel para o usuário com ID {userId}");

            List<Filme> filmes = new List<Filme>();
            try
            {
                filmes = await _filmeRepository.FilmesDoUsuario(userId);

                if (filmes == null || !filmes.Any())
                {
                    _logger.LogWarning($"[FilmesService.FilmesDaMarvelPorUsuarioId] Nenhum filme da Marvel encontrado para o usuário com ID {userId}");
                    return Result<List<Filme>>.Failure($"Nenhum filme da Marvel encontrado para o usuário com ID {userId}", filmes ?? new List<Filme>());
                }

                _logger.LogInformation($"[FilmesService.FilmesDaMarvelPorUsuarioId] {filmes.Count} filmes encontrados para o usuário com ID {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[FilmesService.FilmesDaMarvelPorUsuarioId] Erro ao buscar filmes da Marvel para o usuário com ID {userId}");
                return Result<List<Filme>>.Failure($"Erro ao buscar filmes da Marvel para o usuário com ID {userId}: {ex.Message}", filmes);
            }

            return Result<List<Filme>>.Success(filmes);
        }


        public async Task<Result<bool>> UploadFilmesCsv(IFormFile file, int userId)
        {
            _logger.LogInformation($"[FilmesService.UploadFilmesCsv] Começo processo de processar filmes vistos por userId{userId}");
            bool inseridoBanco;
            try
            {
                var resultFilmes = Helpers.CsvHelper.ProcessarFilmeCsv(file);
                if (!resultFilmes.IsSuccess)
                {
                    string errorMessage = resultFilmes?.ErrorMessage ?? string.Empty;
                    return new Result<bool>(false, errorMessage, false);
                }
                inseridoBanco = await _filmeRepository.InserirFilmesVistosUsuario(resultFilmes?.Value, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"[FilmesService.UploadFilmesCsv] Error: {ex.Message}", ex);
                return new Result<bool>(false, ex.Message, false);
            }
            _logger.LogInformation($"[PokemonService.ProcessarPokemonsCapturadosCsv] Fim do processo de processar Csv filmes vistos para o usuário: {userId}, resultado: {inseridoBanco}");
            return inseridoBanco ? Result<bool>.Success(true) : Result<bool>.Failure($"Erro ao Inserir no banco de dados para o user {userId}.", false);
        }

        public async Task<Result<List<Filme>>> FiltrarFilmes(FilmeFilter filter)
        {
            _logger.LogInformation($"[FilmesService.FiltrarFilmes] Início do processo para filtrar filmes com os filtros: {filter}");

            List<Filme> filmes = new List<Filme>();
            try
            {
                // A lógica de filtro pode ser aplicada aqui utilizando os parâmetros do 'filter'
                filmes = await _filmeRepository.FiltrarFilmes(filter);

                if (filmes == null || !filmes.Any())
                {
                    _logger.LogWarning($"[FilmesService.FiltrarFilmes] Nenhum filme encontrado com os filtros fornecidos");
                    return Result<List<Filme>>.Failure("Nenhum filme encontrado.", filmes ?? new List<Filme>());
                }

                _logger.LogInformation($"[FilmesService.FiltrarFilmes] {filmes.Count} filmes encontrados com os filtros fornecidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[FilmesService.FiltrarFilmes] Erro ao filtrar filmes");
                return Result<List<Filme>>.Failure($"Erro ao filtrar filmes: {ex.Message}", filmes);
            }

            return Result<List<Filme>>.Success(filmes);
        }

    }
}
