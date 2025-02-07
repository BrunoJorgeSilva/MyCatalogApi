using MyCatalogApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCatalogApi.Domain.DTOs.OutputObjects
{
    public class PokemonDto
    {
        public PokemonDto()
        {
            
        }
        public static List<PokemonDto> RetornaListaTratada(List<Pokemon> pokemons)
        {
            List<PokemonDto> pokemonsTratados = new List<PokemonDto>();
            foreach (var pokemon in pokemons)
            {
                var pokemonTratado = new PokemonDto(pokemon);
                pokemonsTratados.Add(pokemonTratado);
            }
            return pokemonsTratados;
        }
        public PokemonDto (Pokemon pokemonEntity)
        {
            PokemonId = pokemonEntity.PokemonId;
            Name = pokemonEntity.Name;
            HP = pokemonEntity.HP;
            Ataque = pokemonEntity.Ataque;
            Defesa = pokemonEntity.Defesa;
            AtaqueEspecial = pokemonEntity.AtaqueEspecial;
            Velocidade = pokemonEntity.Velocidade;
        }
        public int PokemonId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int HP { get; set; }
        public int Ataque { get; set; }
        public int Defesa { get; set; }
        public int AtaqueEspecial { get; set; }
        public int DefesaEspecial { get; set; }
        public int Velocidade { get; set; }
    }
}
