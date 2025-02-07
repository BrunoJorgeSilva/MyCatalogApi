namespace MyCatalogApi.Domain.DTOs.EntryObjects
{
    public class FilmeFilter
    {
        public string? Titulo { get; set; }
        public string? Diretor { get; set; }
        public int? TempoDeFilme { get; set; }
        public int? Ano { get; set; }
        public string? AtoresPrincipais { get; set; }
        public string? Genero { get; set; }
        public string? ClassificacaoIndicativa { get; set; }
        public int? OrcamentoProducaoMilhao { get; set; }
        public decimal? BilheteriaMundialMilhao { get; set; }
        public string? Sinopse { get; set; }
        public string? PremiosEIndicacoes { get; set; }
        public string? Roteiristas { get; set; }
        public string? CronologiaNoUniverso { get; set; }
        public string? CameosOuParticipacoes { get; set; }
        public string? RecepcaoDaCritica { get; set; }
        public string? CuriosidadeDeProducao { get; set; }
        public string? TrilhaSonora { get; set; }
        public string? EstudioProducao { get; set; }
    }
}
