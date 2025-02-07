using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.Entities;

namespace MyCatalogApi.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AdicionarUsuario(NovoUserDto user);
        Task<List<User>> TodosOsUsuarios();
        Task<User?> UsuarioPorId(int userId);
        Task<List<User>> BuscarUsuariosPorFiltro(int? userId, string? nome, string? apelido);
    }
}
