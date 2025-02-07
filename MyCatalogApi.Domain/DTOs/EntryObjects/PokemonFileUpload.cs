using Microsoft.AspNetCore.Http;

namespace MyCatalogApi.Domain.DTOs.EntryObjects
{
    public class PokemonFileUpload
    {
        public required IFormFile File { get; set; }
        public int UserId { get; set; }
    }
}
