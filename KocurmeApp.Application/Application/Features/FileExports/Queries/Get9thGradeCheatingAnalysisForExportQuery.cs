using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using MediatR;

namespace KocurmeApp.Application.Application.Features.FileExports.Queries
{
    /// <summary>
    /// Seçilmiş imtahan üzrə zal köçürmə analizi (dbo.fn_CheatingRoomStatsForExam).
    /// Excel-də yalnız bu imtahanın dataları olur; normallaşdırma və "Orta qiymət"
    /// isə həmin imtahanın sinfinə aid bütün imtahanların qlobal ortasına görədir.
    /// </summary>
    public class Get9thGradeCheatingAnalysisForExportQuery : IRequest<NinthGradeCheatingAnalysisExportResult>
    {
        public int ExamId { get; set; }
    }
}
