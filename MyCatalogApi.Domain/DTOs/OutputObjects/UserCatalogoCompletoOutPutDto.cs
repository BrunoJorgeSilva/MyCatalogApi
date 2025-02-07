using MyCatalogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCatalogApi.Domain.DTOs.OutputObjects
{
    public class UserCatalogoCompletoOutPutDto
    {
        public static UserCatalogoCompletoOutPutDto PreencheDadosUsuario(User? user)
        {
            UserCatalogoCompletoOutPutDto userCatalogoCompletoOutPutDto = new UserCatalogoCompletoOutPutDto()
            {
                Nome = user?.Nome ?? string.Empty,
                Apelido = user?.Apelido ?? string.Empty,
            };

            return userCatalogoCompletoOutPutDto;
        }

        public static UserCatalogoCompletoOutPutDto PreenchePokemonsEFilmes(UserCatalogoCompletoOutPutDto userCatalogoCompletoOutPutDto,
                                                                 List<PokemonDto>? pokemons,
                                                                 List<Filme>? filmes)

        {
            userCatalogoCompletoOutPutDto.Pokemons = pokemons ?? new List<PokemonDto>();
            userCatalogoCompletoOutPutDto.Filmes = filmes ?? new List<Filme>();
            return userCatalogoCompletoOutPutDto;
        }

        public string Nome { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
        public List<PokemonDto> Pokemons { get; set; } = new List<PokemonDto>();
        public List<Filme> Filmes { get; set; } = new List<Filme>();
    }
}
