using Microsoft.AspNetCore.Http;
using MyCatalogApi.Application.ResultPattern;
using MyCatalogApi.Domain.ValueObjects;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MyCatalogApi.Application.Helpers
{
    public class CsvHelper
    {
        public static Result<List<FilmeCsvVO>> ProcessarFilmeCsv(IFormFile file)
        {
            var filmes = new List<FilmeCsvVO>();

            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                stream.ReadLine();  
                var lineNumber = 1;

                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue; 

                    var values = SplitCsvLine(line);

                    var filme = new FilmeCsvVO
                    {
                        Titulo = ObterValor(values, 0),
                        Diretor = ObterValor(values, 1),
                        TempoDeFilme = ObterValorInt(values, 2),
                        Ano = ObterValorInt(values, 3),
                        AtoresPrincipais = $"{ObterValor(values, 4)} {ObterValor(values, 5)} {ObterValor(values, 6)}".Trim(),
                        Genero = ObterValor(values, 7),
                        ClassificacaoIndicativa = ObterValor(values, 8),
                        OrcamentoProducaoMilhao = ObterValorInt(values, 9),
                        BilheteriaMundialMilhao = ObterValorDecimal(values, 10),
                        Sinopse = ObterValor(values, 11),
                        PremiosEIndicacoes = ObterValor(values, 12),
                        TrilhaSonora = ObterValor(values, 13),
                        EstudioProducao = ObterValor(values, 14),
                        Roteiristas = ObterValor(values, 15),
                        CronologiaNoUniverso = ObterValor(values, 16),
                        CameosOuParticipacoes = ObterValor(values, 17),
                        RecepcaoDaCritica = ObterValor(values, 18),
                        CuriosidadeDeProducao = ObterValor(values, 19)
                    };

                    filmes.Add(filme);
                    lineNumber++;
                }
            }

            return filmes.Count > 0
                ? Result<List<FilmeCsvVO>>.Success(filmes)
                : Result<List<FilmeCsvVO>>.Failure("Nenhuma linha válida foi encontrada no arquivo CSV.", filmes);
        }


        private static string[] SplitCsvLine(string line)
        {
            var pattern = @"(?:^|,)(?:""([^""]*)""|([^,]*))";
            var matches = Regex.Matches(line, pattern);
            return matches.Select(m => m.Groups[1].Success ? m.Groups[1].Value : m.Groups[2].Value).ToArray();
        }


        public static Result<List<int>> ProcessarCapturadosCsv(IFormFile file)
        {
            var pokemonsIds = new List<int>();
            try
            {
                using (var stream = new StreamReader(file.OpenReadStream()))
                {
                    var lineNumber = 0;

                    while (!stream.EndOfStream)
                    {
                        var line = stream.ReadLine();

                        if (lineNumber == 0)
                        {
                            lineNumber++;
                            continue;
                        }
                        var values = line?.Split(',');
                        if (values is not null && values.Length > 0)
                        {
                            pokemonsIds.Add(int.Parse(values[0]?.Trim(), CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
            catch (Exception)
            {
                return Result<List<int>>.Failure("Não foi possível converter o csv, revise o arquivo por favor.", new List<int>());
            }
            return pokemonsIds.Count > 0 ? Result<List<int>>.Success(pokemonsIds) :
                                                        Result<List<int>>.Failure("Não foi possível converter o csv, revise o arquivo por favor.", pokemonsIds);
        }

        private static string ObterValor(string[] values, int index)
        {
            return index < values.Length ? values[index]?.Trim() ?? string.Empty : null;
        }

        private static int? ObterValorInt(string[] values, int index)
        {
            return index < values.Length && int.TryParse(values[index]?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var result)
                ? result
                : (int?)null;
        }

        private static decimal? ObterValorDecimal(string[] values, int index)
        {
            return index < values.Length && decimal.TryParse(values[index]?.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var result)
                ? result
                : (decimal?)null;
        }
    }
}

