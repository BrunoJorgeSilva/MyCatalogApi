namespace MyCatalogApi.Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Apelido { get; set; } = string.Empty;
    }
}
