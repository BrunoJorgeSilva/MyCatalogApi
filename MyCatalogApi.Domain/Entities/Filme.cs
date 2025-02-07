using System;

namespace MyCatalogApi.Domain.Entities
{
    public class Filme
    {
        public int FilmeId { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Diretor { get; set; } = string.Empty;
        public int? TempoDeFilme { get; set; }
        public int? Ano { get; set; }
        public string AtoresPrincipais { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public string ClassificacaoIndicativa { get; set; } = string.Empty;
        public int? OrcamentoProducaoMilhao { get; set; }
        public decimal? BilheteriaMundialMilhao { get; set; }
        public string Sinopse { get; set; } = string.Empty;
        public string PremiosEIndicacoes { get; set; } = string.Empty;
        public string Roteiristas { get; set; } = string.Empty;
        public string CronologiaNoUniverso { get; set; } = string.Empty;
        public string CameosOuParticipacoes { get; set; } = string.Empty;
        public string RecepcaoDaCritica { get; set; } = string.Empty;
        public string CuriosidadeDeProducao { get; set; } = string.Empty;
        public string TrilhaSonora { get; set; } = string.Empty;
        public string EstudioProducao { get; set; } = string.Empty;
    }
}
