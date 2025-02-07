namespace MyCatalogApi.Domain.Entities
{
    public class Pokemon
    {
        public int Id { get; set; }
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
