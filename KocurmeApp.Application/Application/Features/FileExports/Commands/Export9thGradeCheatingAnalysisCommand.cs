using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using MediatR;

namespace KocurmeApp.Application.Application.Features.FileExports.Commands
{
    public class Export9thGradeCheatingAnalysisCommand : IRequest<ExportResultDTO>
    {
        /// <summary>Çıxarış ediləcək konkret imtahan.</summary>
        public int ExamId { get; set; }

        public string SheetName { get; set; } = "9-cu sinif Zal Köçürmə Analizi";
    }
}
