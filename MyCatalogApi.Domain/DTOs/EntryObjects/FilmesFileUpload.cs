using Microsoft.AspNetCore.Http;

namespace MyCatalogApi.Domain.DTOs.EntryObjects
{
    public class FilmesFileUpload
    {
        public required IFormFile File { get; set; }
        public int UserId { get; set; }
    }
}
