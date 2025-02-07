using Microsoft.AspNetCore.Mvc;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Domain.DTOs.EntryObjects;

namespace MyCatalogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet("PokemonsCapturadosPorUserId")]
        public async Task<IActionResult> PokemonsCapturadorPorUsuarioId([FromHeader(Name = "userId")] int userId)
        {
            var result = await _pokemonService.PokemonsCapturadorPorUsuarioId(userId);
            return result != null && result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost("UploadPokemonsCapturadosCsv")]
        public async Task<IActionResult> UploadPokemonsCapturadosCsv([FromForm] PokemonFileUpload model)
        { 
            if (model?.File == null || model.File.Length == 0)
            {
                return BadRequest("Erro no arquivo csv, revise o arquivo por favor.");
            }
            var processado = await _pokemonService.ProcessarPokemonsCapturadosCsv(model.File, model.UserId);
            return processado != null && processado.IsSuccess ? Ok(processado) : BadRequest(processado);
        }

        [HttpGet("FiltrarPokemons")]
        public async Task<IActionResult> FiltrarPokemons([FromQuery] PokemonFilter filter)
        {
            var result = await _pokemonService.FiltrarPokemons(filter);
            return result?.Value?.Count > 0 ? Ok(result) : NotFound("Nenhum pokemon encontrado.");
        }
    }
}
