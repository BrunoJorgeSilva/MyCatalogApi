using MyCatalogApi.Domain.DTOs.EntryObjects;

namespace MyCatalogApi.Infrastructure.Helper
{

    public static class QueryHelper
    {
        public static string BuildWhereClausePokemon(PokemonFilter filter)
        {
            var conditions = new List<string> { "1 = 1" };  

            if (filter.PokemonId.HasValue)
                conditions.Add($"IdExternoPokemon = {filter.PokemonId.Value}");

            if (!string.IsNullOrEmpty(filter.Name))
                conditions.Add($"UPPER(Name) LIKE UPPER('%{filter.Name}%')");

            if (filter.HP.HasValue)
                conditions.Add($"HP = {filter.HP.Value}");

            if (filter.Ataque.HasValue)
                conditions.Add($"Ataque = {filter.Ataque.Value}");

            if (filter.Defesa.HasValue)
                conditions.Add($"Defesa = {filter.Defesa.Value}");

            if (filter.AtaqueEspecial.HasValue)
                conditions.Add($"AtaqueEspecial = {filter.AtaqueEspecial.Value}");

            if (filter.DefesaEspecial.HasValue)
                conditions.Add($"DefesaEspecial = {filter.DefesaEspecial.Value}");

            if (filter.Velocidade.HasValue)
                conditions.Add($"Velocidade = {filter.Velocidade.Value}");

            return "WHERE " + string.Join(" AND ", conditions);
        }

        public static string BuildWhereClauseUser(int? userId, string? nome, string? apelido)
        {
            var query = "WHERE 1=1";

            if (userId.HasValue)
            {
                query += $" AND UserId = {userId}";
            }

            if (!string.IsNullOrEmpty(nome))
            {
                query += $" AND UPPER(Nome) LIKE UPPER('%{nome}%')";
            }

            if (!string.IsNullOrEmpty(apelido))
            {
                query += $" AND UPPER(Apelido) LIKE UPPER('%{apelido}%')";
            }
            return query;
        }

        public static string BuildWhereClauseFilme(FilmeFilter filter)
        {
            var conditions = new List<string> { "1 = 1" }; 

            if (!string.IsNullOrEmpty(filter.Titulo))
                conditions.Add($"UPPER(Titulo) LIKE UPPER('%{filter.Titulo}%')");

            if (!string.IsNullOrEmpty(filter.Diretor))
                conditions.Add($"UPPER(Diretor) LIKE UPPER('%{filter.Diretor}%')");

            if (filter.TempoDeFilme.HasValue)
                conditions.Add($"TempoDeFilme = {filter.TempoDeFilme.Value}");

            if (filter.Ano.HasValue)
                conditions.Add($"Ano = {filter.Ano.Value}");

            if (!string.IsNullOrEmpty(filter.AtoresPrincipais))
                conditions.Add($"UPPER(AtoresPrincipais) LIKE UPPER('%{filter.AtoresPrincipais}%')");

            if (!string.IsNullOrEmpty(filter.Genero))
                conditions.Add($"UPPER(Genero) LIKE UPPER('%{filter.Genero}%')");

            if (!string.IsNullOrEmpty(filter.ClassificacaoIndicativa))
                conditions.Add($"UPPER(ClassificacaoIndicativa) LIKE UPPER('%{filter.ClassificacaoIndicativa}%')");

            if (filter.OrcamentoProducaoMilhao.HasValue)
                conditions.Add($"OrcamentoProducaoMilhao = {filter.OrcamentoProducaoMilhao.Value}");

            if (filter.BilheteriaMundialMilhao.HasValue)
                conditions.Add($"BilheteriaMundialMilhao = {filter.BilheteriaMundialMilhao.Value}");

            if (!string.IsNullOrEmpty(filter.Sinopse))
                conditions.Add($"UPPER(Sinopse) LIKE UPPER('%{filter.Sinopse}%')");

            if (!string.IsNullOrEmpty(filter.PremiosEIndicacoes))
                conditions.Add($"UPPER(PremiosEIndicacoes) LIKE UPPER('%{filter.PremiosEIndicacoes}%')");

            if (!string.IsNullOrEmpty(filter.Roteiristas))
                conditions.Add($"UPPER(Roteiristas) LIKE UPPER('%{filter.Roteiristas}%')");

            if (!string.IsNullOrEmpty(filter.CronologiaNoUniverso))
                conditions.Add($"UPPER(CronologiaNoUniverso) LIKE UPPER('%{filter.CronologiaNoUniverso}%')");

            if (!string.IsNullOrEmpty(filter.CameosOuParticipacoes))
                conditions.Add($"UPPER(CameosOuParticipacoes) LIKE UPPER('%{filter.CameosOuParticipacoes}%')");

            if (!string.IsNullOrEmpty(filter.RecepcaoDaCritica))
                conditions.Add($"UPPER(RecepcaoDaCritica) LIKE UPPER('%{filter.RecepcaoDaCritica}%')");

            if (!string.IsNullOrEmpty(filter.CuriosidadeDeProducao))
                conditions.Add($"UPPER(CuriosidadeDeProducao) LIKE UPPER('%{filter.CuriosidadeDeProducao}%')");

            if (!string.IsNullOrEmpty(filter.TrilhaSonora))
                conditions.Add($"UPPER(TrilhaSonora) LIKE UPPER('%{filter.TrilhaSonora}%')");

            if (!string.IsNullOrEmpty(filter.EstudioProducao))
                conditions.Add($"UPPER(EstudioProducao) LIKE UPPER('%{filter.EstudioProducao}%')");

            return "WHERE " + string.Join(" AND ", conditions);
        }

    }

}
