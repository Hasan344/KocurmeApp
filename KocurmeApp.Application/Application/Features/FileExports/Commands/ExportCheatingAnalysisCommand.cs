using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using MediatR;

namespace KocurmeApp.Application.Application.Features.FileExports.Commands
{
    public class ExportCheatingAnalysisCommand : IRequest<ExportResultDTO>
    {
        public int ExamId { get; set; }
        public int MinEyniY { get; set; } = 5;
        public decimal MinEhtimal { get; set; } = 60;
        public string SheetName { get; set; } = "Köçürmə Analizi";
    }
}
