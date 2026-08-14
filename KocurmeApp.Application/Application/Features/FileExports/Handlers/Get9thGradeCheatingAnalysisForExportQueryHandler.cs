using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using KocurmeApp.Application.Application.Features.FileExports.Queries;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Application.Features.FileExports.Handlers
{
    /// <summary>
    /// Seçilmiş imtahan üzrə zal köçürmə analizi.
    /// dbo.fn_CheatingRoomStatsForExam funksiyası yalnız bu imtahanın sətirlərini
    /// qaytarır, lakin normallaşdırmanı həmin sinfin qlobal ortasına görə aparır.
    /// </summary>
    public class Get9thGradeCheatingAnalysisForExportQueryHandler
        : IRequestHandler<Get9thGradeCheatingAnalysisForExportQuery, NinthGradeCheatingAnalysisExportResult>
    {
        private readonly AppDbContext _context;

        public Get9thGradeCheatingAnalysisForExportQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NinthGradeCheatingAnalysisExportResult> Handle(
            Get9thGradeCheatingAnalysisForExportQuery request,
            CancellationToken cancellationToken)
        {
            var rows = await _context.Set<NinthGradeCheatingRoomStatsResult>()
                .FromSqlInterpolated(
                    $"SELECT * FROM dbo.fn_CheatingRoomStatsForExam({request.ExamId})")
                .ToListAsync(cancellationToken);

            if (rows.Count == 0)
            {
                return new NinthGradeCheatingAnalysisExportResult();
            }

            var examName = await _context.Exams
                .Where(e => e.Id == request.ExamId)
                .Select(e => e.Name)
                .FirstOrDefaultAsync(cancellationToken) ?? request.ExamId.ToString();

            // Sıralama: əvvəlcə detal sətirlər (RowType 0) Kolon5-ə görə artan,
            // sonra "Orta qiymət" sətri (RowType 1).
            var ordered = rows
                .OrderBy(r => r.RowType)
                .ThenBy(r => r.Kolon5 ?? decimal.MaxValue)
                .Select(r => new NinthGradeCheatingAnalysisDTO
                {
                    ExamId = r.ExamId,
                    ExamName = examName,
                    IsSummary = r.RowType == 1,
                    Zal = r.RowType == 1 ? "Orta qiymət (9 imtahan)" : r.Zal.ToString(),
                    ZaldaKocurenAbituriyentlerinSayi = r.KocurenSayi,
                    KocurulenFenlerinUmumiSayi = r.FennSayi,
                    ZaldaOlanAbituriyentlerinSayi = r.OdaSayi,
                    ZaldaKocurmeFaizi1 = r.Faiz1,
                    ZaldaKocurmeFaizi2 = r.Faiz2,
                    Kolon3 = r.Kolon3,
                    Kolon4 = r.Kolon4,
                    Kolon5 = r.Kolon5
                })
                .ToList();

            // Köçürmə faizi (Kolon5) paylanması — yalnız detal (zal) sətirləri üzərində.
            var statistics = CalculateStatistics(
                ordered.Where(x => !x.IsSummary).ToList());

            return new NinthGradeCheatingAnalysisExportResult
            {
                AnalysisData = ordered,
                Statistics = statistics
            };
        }

        private static List<CheatingStatisticsDTO> CalculateStatistics(
            List<NinthGradeCheatingAnalysisDTO> data)
        {
            var ranges = new List<(decimal Min, decimal Max, string Label)>
            {
                (0m, 1m,  "(0-1]"),
                (1m, 2m,  "(1-2]"),
                (2m, 3m,  "(2-3]"),
                (3m, 4m,  "(3-4]"),
                (4m, 5m,  "(4-5]"),
                (5m, 6m,  "(5-6]"),
                (6m, 7m,  "(6-7]"),
                (7m, 8m,  "(7-8]"),
                (8m, 9m,  "(8-9]"),
                (9m, 10m, "(9-10]")
            };

            return ranges
                .Select(range => new CheatingStatisticsDTO
                {
                    KocurmeFaiziAraligi = range.Label,
                    KocurmeOlanZallarinSayi = data.Count(x =>
                        x.Kolon5.HasValue &&
                        x.Kolon5.Value > range.Min &&
                        x.Kolon5.Value <= range.Max)
                })
                .ToList();
        }
    }
}
