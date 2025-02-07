using Microsoft.AspNetCore.Mvc;
using MyCatalogApi.Application.Interfaces;
using MyCatalogApi.Application.Services;
using MyCatalogApi.Domain.DTOs.EntryObjects;

namespace MyCatalogApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("AdicionarNovoUsuario")]
        public async Task<IActionResult> AdicionarUsuario([FromBody] NovoUserDto user)
        {
            var usuarioAdicionado = await _userService.AdicionarUsuario(user);
            if (usuarioAdicionado != null && usuarioAdicionado.IsSuccess)
            {
                return Ok(usuarioAdicionado);
            }
            return BadRequest(usuarioAdicionado);
        }

        [HttpGet("CatalogoDoUsuario")]
        public async Task<IActionResult> CatalogoDoUsuario([FromHeader] int userId)
        {
            var catalogoDoUsuario = await _userService.CatalogoDoUsuario(userId);
            if (catalogoDoUsuario != null && catalogoDoUsuario.IsSuccess)
            {
                return Ok(catalogoDoUsuario);
            }
            return NoContent();
        }

        [HttpGet("TodosOsUsuarios")]
        public async Task<IActionResult> TodosOsUsuarios()
        {
            var todosOsUsuarios = await _userService.TodosOsUsuarios();
            if(todosOsUsuarios != null && todosOsUsuarios.IsSuccess)
            {
                return Ok(todosOsUsuarios);
            }
            return NoContent();
        }
        [HttpGet("BuscarUsuariosPorFiltro")]
        public async Task<IActionResult> BuscarUsuariosPorFiltro([FromQuery] int? userId, [FromQuery] string? nome, [FromQuery] string? apelido)
        {
            var usuarios = await _userService.BuscarUsuariosPorFiltro(userId, nome, apelido);
            if (usuarios != null && usuarios.IsSuccess)
            {
                return Ok(usuarios);
            }
            return NoContent();
        }
    }
}
