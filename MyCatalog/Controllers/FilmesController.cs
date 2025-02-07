using Microsoft.AspNetCore.Mvc;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Application.Services;
using MyCatalogApi.Domain.DTOs.EntryObjects;

namespace MyCatalogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
        private readonly IFilmesService _filmesService;

        public FilmesController(IFilmesService filmesService)
        {
            _filmesService = filmesService;
        }
        [HttpGet("FilmesDaMarvelPorUsuarioId")]
        public async Task<IActionResult> FilmesDaMarvelPorUsuarioId([FromHeader(Name = "userId")] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("ID do usuário inválido.");
            }
            var result = await _filmesService.FilmesDaMarvelPorUsuarioId(userId);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(result.ErrorMessage ?? "Nenhum filme encontrado.");
        }

        [HttpPost("UploadFilmesCsv")]
        public async Task<IActionResult> UploadFilmesCsv([FromForm] FilmesFileUpload model)
        {
            if (model?.File == null || model.File.Length == 0)
            {
                return BadRequest("Erro no arquivo csv, revise o arquivo por favor.");
            }
            var processado = await _filmesService.UploadFilmesCsv(model.File, model.UserId);
            return processado != null && processado.IsSuccess ? Ok(processado) : BadRequest(processado);
        }

        [HttpGet("FiltrarFilmes")]
        public async Task<IActionResult> FiltrarFilmes([FromQuery] FilmeFilter filter)
        {
            var result = await _filmesService.FiltrarFilmes(filter);
            return result?.Value?.Count > 0 ? Ok(result) : NotFound("Nenhum filme encontrado.");
        }
    }
}
