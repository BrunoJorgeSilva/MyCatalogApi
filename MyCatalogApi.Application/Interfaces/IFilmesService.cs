using Microsoft.AspNetCore.Http;
using MyCatalogApi.Application.ResultPattern;
using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.Entities;

namespace MyCatalogApi.Application.Interfaces
{
    public interface IFilmesService
    {
        Task<Result<List<Filme>>> FilmesDaMarvelPorUsuarioId(int userId);
        Task<Result<bool>> UploadFilmesCsv(IFormFile file, int userId);
        Task<Result<List<Filme>>> FiltrarFilmes(FilmeFilter filter);
    }
}
