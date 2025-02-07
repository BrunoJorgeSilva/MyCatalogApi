using MyCatalogApi.Application.ResultPattern;
using Microsoft.AspNetCore.Http;
using MyCatalogApi.Domain.DTOs.OutputObjects;
using MyCatalogApi.Domain.DTOs.EntryObjects;

namespace MyCatalogApi.Application.Interfaces
{
    public interface IPokemonService
    {
        Task<Result<List<PokemonDto>>> PokemonsCapturadorPorUsuarioId(int userId);
        Task<Result<List<PokemonDto>>> FiltrarPokemons(PokemonFilter filter);
        Task<Result<bool>> ProcessarPokemonsCapturadosCsv(IFormFile file, int userId);

    }
}
