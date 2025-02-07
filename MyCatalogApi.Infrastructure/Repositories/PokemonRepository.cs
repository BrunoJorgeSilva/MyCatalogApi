using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Domain.Entities;
using Microsoft.Extensions.Logging;
using Dapper;
using Npgsql;
using System.Data;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Infrastructure.Helper;

namespace MyCatalogApi.Infrastructure.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly ILogger<PokemonRepository> _logger;
        private readonly IDbConnection _dbConnection;

        public PokemonRepository(ILogger<PokemonRepository> logger, IDbConnection dbConnection)
        {
            _logger = logger;
            _dbConnection = dbConnection;
        }

        public async Task<bool> InserirCapturados(List<int> resultPokemons, int userId)
        {
            try
            {
                var parameters = resultPokemons.Select(pokemonId => new { UserId = userId, PokemonId = pokemonId }).ToList();

                var sql = @"INSERT INTO UserPokemon (UserId, IdExternoPokemon)
                            VALUES (@UserId, @PokemonId)";

                var rowsAffected = await _dbConnection.ExecuteAsync(sql, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao inserir pokemons capturados: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Pokemon>> PokemonsPorUsuarioId(int userId)
        {
            try
            {
                var sql = @"SELECT p.idexternoPokemon as PokemonId, p.name, p.hp, p.ataque, p.defesa, p.AtaqueEspecial, p.defesaespecial, p.velocidade
                            FROM Pokemon p
                            JOIN UserPokemon pc ON p.IdExternoPokemon = pc.IdExternoPokemon
                            WHERE pc.userid = @UserId";

                var pokemons = await _dbConnection.QueryAsync<Pokemon>(sql, new { UserId = userId });

                return pokemons.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar pokemons do usuário {userId}: {ex.Message}");
                return new List<Pokemon>();
            }
        }
        public async Task<List<Pokemon>> FiltrarPokemons(PokemonFilter filter)
        {
            try
            {
                var whereClause = QueryHelper.BuildWhereClausePokemon(filter);
                var sql = $@"SELECT IdExternoPokemon as PokemonId, Name, HP, Ataque, Defesa, AtaqueEspecial, DefesaEspecial, Velocidade FROM Pokemon {whereClause}";

                var pokemons = await _dbConnection.QueryAsync<Pokemon>(sql);

                return pokemons.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao filtrar pokemons: {ex.Message}");
                return new List<Pokemon>();
            }
        }
    }
}
