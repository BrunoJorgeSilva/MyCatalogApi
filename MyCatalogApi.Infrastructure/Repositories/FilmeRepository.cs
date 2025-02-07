using Dapper;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Domain.ValueObjects;
using MyCatalogApi.Domain.Entities;
using System.Data;
using Microsoft.Extensions.Logging;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Infrastructure.Helper;

namespace MyCatalogApi.Infrastructure.Repositories
{
    public class FilmeRepository : IFilmeRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly ILogger<FilmeRepository> _logger;

        public FilmeRepository(IDbConnection dbConnection, ILogger<FilmeRepository> logger)
        {
            _dbConnection = dbConnection;
            _logger = logger;
        }

        public async Task<bool> InserirFilmesVistosUsuario(List<FilmeCsvVO>? filmes, int userId)
        {
            if (filmes == null || !filmes.Any())
            {
                _logger.LogWarning($"[FilmeRepository.InserirFilmesVistosUsuario] Nenhum filme recebido para o usuário {userId}.");
                return false;
            }
            _dbConnection.Open();

            using var transaction = _dbConnection.BeginTransaction();

            try
            {
                foreach (var filme in filmes)
                {
                    var filmeId = await _dbConnection.ExecuteScalarAsync<int?>(
                        @"SELECT FilmeId FROM Filme WHERE Titulo = @Titulo;",
                        new { Titulo = filme.Titulo },
                        transaction
                    );

                    if (filmeId == null)
                    {
                        filmeId = await _dbConnection.ExecuteScalarAsync<int>(
                            @"
                             INSERT INTO Filme (Titulo, Diretor, TempoDeFilme, Ano, AtoresPrincipais, Genero, ClassificacaoIndicativa, 
                                      OrcamentoProducaoMilhao, BilheteriaMundialMilhao, Sinopse, PremiosEIndicacoes, Roteiristas, 
                                      CronologiaNoUniverso, CameosOuParticipacoes, RecepcaoDaCritica, CuriosidadeDeProducao,
                                      TrilhaSonora, EstudioProducao)
                            VALUES (@Titulo, @Diretor, @TempoDeFilme, @Ano, @AtoresPrincipais, @Genero, @ClassificacaoIndicativa,
                            @OrcamentoProducaoMilhao, @BilheteriaMundialMilhao, @Sinopse, @PremiosEIndicacoes, @Roteiristas,
                            @CronologiaNoUniverso, @CameosOuParticipacoes, @RecepcaoDaCritica, @CuriosidadeDeProducao,
                            @TrilhaSonora, @EstudioProducao)
                            RETURNING FilmeId;",
                            filme,
                            transaction
                        );
                    }

                    var userFilmeExists = await _dbConnection.ExecuteScalarAsync<int?>(
                        @"SELECT UserFilmeId FROM UserFilme WHERE UserId = @UserId AND FilmeId = @FilmeId;",
                        new { UserId = userId, FilmeId = filmeId },
                        transaction
                    );

                    if (userFilmeExists == null)
                    {
                        await _dbConnection.ExecuteAsync(
                            @"INSERT INTO UserFilme (UserId, FilmeId) VALUES (@UserId, @FilmeId);",
                            new { UserId = userId, FilmeId = filmeId },
                            transaction
                        );
                    }
                }

                transaction.Commit();
                _logger.LogInformation($"[FilmeRepository.InserirFilmesVistosUsuario] Filmes inseridos com sucesso para o usuário {userId}.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[FilmeRepository.InserirFilmesVistosUsuario] Erro ao inserir filmes para o usuário {userId}: {ex.Message}");
                transaction.Rollback();
                return false;
            }
            finally
            {
                _dbConnection.Close();
            }
        }


        public async Task<List<Filme>> FilmesDoUsuario(int userId)
        {
            try
            {
                var query = @"
                   SELECT f.FilmeId, f.Titulo, f.Diretor, f.TempoDeFilme, f.Ano, f.AtoresPrincipais, f.Genero, f.ClassificacaoIndicativa,
                   f.OrcamentoProducaoMilhao, f.BilheteriaMundialMilhao, f.Sinopse, f.PremiosEIndicacoes, f.Roteiristas,
                   f.CronologiaNoUniverso, f.CameosOuParticipacoes, f.RecepcaoDaCritica, f.CuriosidadeDeProducao, 
                   f.TrilhaSonora, f.EstudioProducao
                   FROM Filme f
                   INNER JOIN UserFilme uf ON f.FilmeId = uf.FilmeId
                   WHERE uf.UserId = @UserId;";

                var filmes = (await _dbConnection.QueryAsync<Filme>(query, new { UserId = userId })).ToList();

                _logger.LogInformation($"[FilmeRepository.FilmesDoUsuario] Retornados {filmes.Count} filmes para o usuário {userId}.");
                return filmes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[FilmeRepository.FilmesDoUsuario] Erro ao buscar filmes para o usuário {userId}: {ex.Message}");
                return new List<Filme>();
            }
        }

        public async Task<List<Filme>> FiltrarFilmes(FilmeFilter filter)
        {
            try
            {
                var whereClause = QueryHelper.BuildWhereClauseFilme(filter);

                var sql = $@"SELECT FilmeId, Titulo, Diretor, TempoDeFilme, Ano, AtoresPrincipais, Genero, ClassificacaoIndicativa,
                           OrcamentoProducaoMilhao, BilheteriaMundialMilhao, Sinopse, PremiosEIndicacoes, Roteiristas,
                           CronologiaNoUniverso, CameosOuParticipacoes, RecepcaoDaCritica, CuriosidadeDeProducao, 
                           TrilhaSonora, EstudioProducao
                           FROM Filme {whereClause}"; 
                
                var filmes = (await _dbConnection.QueryAsync<Filme>(sql)).ToList();

                _logger.LogInformation($"[FilmeRepository.FiltrarFilmes] Retornados {filmes.Count} filmes com os filtros fornecidos.");
                return filmes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[FilmeRepository.FiltrarFilmes] Erro ao filtrar filmes");
                return new List<Filme>();
            }
        }

    }
}
