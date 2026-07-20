using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using KocurmeApp.Application.Application.Features.FileExports.Queries;
using KocurmeApp.Application.Interfaces;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KocurmeApp.Application.Application.Features.FileExport.Queries.GetCheatingAnalysisForExport
{
    public class GetCheatingAnalysisForExportQueryHandler
        : IRequestHandler<GetCheatingAnalysisForExportQuery,CheatingAnalysisExportResult>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetCheatingAnalysisForExportQueryHandler> _logger;

        // Age category coefficients
        private static readonly Dictionary<int, decimal> AgeCategories = new()
        {
            { 1, 1.00m },
            { 2, 1.58m },
            { 3, 1.58m },
            { 4, 3.68m },
            { 0, 1.00m }
        };

        public GetCheatingAnalysisForExportQueryHandler(
            AppDbContext context,
            ILogger<GetCheatingAnalysisForExportQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CheatingAnalysisExportResult> Handle(
            GetCheatingAnalysisForExportQuery request,
            CancellationToken cancellationToken)
        {
            var t1 = await (
                from cs in _context.CheatingStudents
                join r in _context.Rooms on cs.ZAL1 equals r.Z_KOD
                join c in _context.Contingents
                on new { r.ExamId, NumK = r.GR_FL }
                equals new { c.ExamId, NumK = c.NUM_K.ToString() }

                where cs.ExamId == request.ExamId
                      && r.ExamId == request.ExamId
                      && cs.EYNI_Y >= request.MinEyniY
                select new
                {
                    cs.ZAL1,
                    cs.IS_N1,
                    cs.FENN,
                    Ehtimal = Math.Round(
                        ((decimal)(cs.EYNI_Y + cs.EYNI_B) / (30 - cs.EYNI_D)) * 100, 2),
                    Oda_Student_Sayisi = r.KOL_ABT,
                    YASH_KATEQ = c.YASH_KATEQ
                }
            ).ToListAsync(cancellationToken);


            var summary = t1
                .GroupBy(x => new { x.ZAL1, x.YASH_KATEQ })
                .Select(g => new
                {
                    g.Key.ZAL1,
                    g.Key.YASH_KATEQ,
                    KopyaCeken = g.Select(x => x.IS_N1).Distinct().Count(),
                    KopyaFen = g.Select(x => x.FENN).Distinct().Count(),
                    OdaSayisi = g.Max(x => x.Oda_Student_Sayisi)
                }).ToList();

            var t3 = summary.Select(s => new
            {
                s.ZAL1,
                s.YASH_KATEQ,
                s.KopyaCeken,
                s.KopyaFen,
                s.OdaSayisi,
                Faiz1 = s.OdaSayisi > 0
                    ? Math.Round((decimal)s.KopyaFen / (s.OdaSayisi * 3) * 100, 2)
                    : 0,
                Faiz2 = s.OdaSayisi > 0
                    ? Math.Round((decimal)s.KopyaCeken / s.OdaSayisi * 100, 2)
                    : 0
            }).ToList();

            var avgFaiz1 = t3.Average(x => x.Faiz1);
            var avgFaiz2 = t3.Average(x => x.Faiz2);

            var ageCoeff = new Dictionary<int, decimal>
    {
        { 1, 1.00m },
        { 2, 1.58m },
        { 3, 1.58m },
        { 4, 3.68m },
        { 0, 1.00m }
    };

            var result = t3.Select(x =>
            {
                var kolon3 = avgFaiz1 > 0 ? Math.Round(x.Faiz1 / avgFaiz1, 2) : 0;
                var kolon4 = avgFaiz2 > 0 ? Math.Round(x.Faiz2 / avgFaiz2, 2) : 0;
                var kolon5 = Math.Round((kolon3 + kolon4) / 2, 2);
                var emsal = ageCoeff.GetValueOrDefault(x.YASH_KATEQ ?? 0, 1);
                var kolon5Bolunmus = Math.Round(kolon5 / emsal, 2);

                return new CheatingAnalysisExportDTO
                {
                    Zal = x.ZAL1.ToString(),
                    KontingentKodu = x.YASH_KATEQ,
                    KontingentYasi = x.YASH_KATEQ switch
                    {
                        1 => "<18",
                        2 => "18~20",
                        4 => ">20",
                        _ => "18~20"
                    },
                    ZaldaKocurenAbituriyentlerinSayi = x.KopyaCeken,
                    KocurulenFenlerinUmumiSayi = x.KopyaFen,
                    ZaldaOlanAbituriyentlerinSayi = x.OdaSayisi,
                    ZaldaKocurmeFaizi1 = x.Faiz1,
                    ZaldaKocurmeFaizi2 = x.Faiz2,
                    Kolon3 = kolon3,
                    Kolon4 = kolon4,
                    Kolon5 = kolon5,
                    Kolon5BolunmusEmsal = kolon5Bolunmus,
                    ZaldaKocurmeninDerecesi =
                        kolon5Bolunmus < 0.7m ? "Zəif" :
                        kolon5Bolunmus >= 1.4m ? "Ağır" :
                        "Orta"
                };
            }).OrderBy(x => x.Kolon5BolunmusEmsal).ToList();

            // Statistika hesablama
            var statistics = CalculateStatistics(result);

            return new CheatingAnalysisExportResult
            {
                AnalysisData = result,
                Statistics = statistics
            };
        }

        private List<CheatingStatisticsDTO> CalculateStatistics(List<CheatingAnalysisExportDTO> data)
        {
            var ranges = new List<(decimal Min, decimal Max, string Label)>
        {
            (0.0m, 0.2m, "(0-0.2]"),
            (0.2m, 0.4m, "(0.2-0.4]"),
            (0.4m, 0.6m, "(0.4-0.6]"),
            (0.6m, 0.8m, "(0.6-0.8]"),
            (0.8m, 1.0m, "(0.8-1]"),
            (1.0m, 1.2m, "(1-1.2]"),
            (1.2m, 1.4m, "(1.2-1.4]"),
            (1.4m, 1.6m, "(1.4-1.6]"),
            (1.6m, 1.8m, "(1.6-1.8]"),
            (1.8m, 2.0m, "(1.8-2]"),
            (2.0m, 2.2m, "(2-2.2]"),
            (2.2m, 2.4m, "(2.2-2.4]"),
            (2.4m, 2.6m, "(2.4-2.6]"),
            (2.6m, 2.8m, "(2.6-2.8]"),
            (2.8m, 3.0m, "(2.8-3]"),
            (3.0m, 3.2m, "(3-3.2]"),
            (3.2m, 3.4m, "(3.2-3.4]"),
            (3.4m, 3.6m, "(3.4-3.6]"),
            (3.6m, 3.8m, "(3.6-3.8]"),
            (3.8m, 4.0m, "(3.8-4]"),
            (4.0m, 4.2m, "(4-4.2]"),
            (4.2m, 4.4m, "(4.2-4.4]")
        };

            var statistics = ranges.Select(range => new CheatingStatisticsDTO
            {
                KocurmeFaiziAraligi = range.Label,
                KocurmeOlanZallarinSayi = data.Count(x =>
                    x.Kolon5BolunmusEmsal > range.Min &&
                    x.Kolon5BolunmusEmsal <= range.Max)
            }).ToList();

            return statistics;
        }
    }

    }