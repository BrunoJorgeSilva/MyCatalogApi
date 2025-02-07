using MyCatalogApi.Application.ResultPattern;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.DTOs.OutputObjects;
using MyCatalogApi.Domain.Entities;

namespace MyCatalogApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result<string>> AdicionarUsuario(NovoUserDto user);
        Task<Result<UserCatalogoCompletoOutPutDto>> CatalogoDoUsuario(int userId);
        Task<Result<List<User>>> TodosOsUsuarios();
        Task<Result<List<User>>> BuscarUsuariosPorFiltro(int? userId, string? nome, string? apelido);

    }
}
