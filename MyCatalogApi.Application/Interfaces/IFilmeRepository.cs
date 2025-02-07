using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.Entities;
using MyCatalogApi.Domain.ValueObjects;

namespace MyCatalogApi.Application.Interfaces
{
    public interface IFilmeRepository
    {
        Task<bool> InserirFilmesVistosUsuario(List<FilmeCsvVO>? value, int userId);
        Task<List<Filme>> FilmesDoUsuario(int userId);
        Task<List<Filme>> FiltrarFilmes(FilmeFilter filter);
    }
}
