using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.Entities;
using System.Data;
using Dapper;
using MyCatalogApi.Infrastructure.Helper;

namespace MyCatalogApi.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public UserRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<int> AdicionarUsuario(NovoUserDto user)
        {
            var query = @"
                INSERT INTO ""User"" (Nome, Apelido)
                VALUES (@Nome, @Apelido)
                RETURNING UserId;";

            var parameters = new
            {
                Nome = user.Name,
                Apelido = user.Apelido
            };

            return await _dbConnection.ExecuteScalarAsync<int>(query, parameters);
        }

        public async Task<List<User>> TodosOsUsuarios()
        {
            var query = "SELECT UserId, Nome, Apelido FROM \"User\";";

            return (await _dbConnection.QueryAsync<User>(query)).ToList();
        }
        public async Task<User?> UsuarioPorId(int userId)
        {
            var query = "SELECT UserId, Nome, Apelido FROM \"User\" WHERE UserId = @userId;";

            var usuario = await _dbConnection.QueryFirstOrDefaultAsync<User>(query, new { userId });

            return usuario;
        }

        public async Task<List<User>> BuscarUsuariosPorFiltro(int? userId, string? nome, string? apelido)
        {
            var whereClause = QueryHelper.BuildWhereClauseUser(userId, nome, apelido);
            var query = $"SELECT * FROM \"User\" {whereClause}";
            var usuarios = await _dbConnection.QueryAsync<User>(query);
            return usuarios.ToList();
        }

    }
}
