using MyCatalogApi.Domain.DTOs.EntryObjects;
using MyCatalogApi.Domain.Entities;

namespace MyCatalogApi.Application.Interfaces
{
    public interface IPokemonRepository
    {
        Task<bool> InserirCapturados(List<int>? resultPokemons, int userId);
        Task<List<Pokemon>> PokemonsPorUsuarioId(int userId);
        Task<List<Pokemon>> FiltrarPokemons(PokemonFilter filter);
    }
}
