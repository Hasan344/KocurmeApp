using KocurmeApp.Application.Application.Features.FileExports.Commands;
using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using KocurmeApp.Application.Application.Features.FileExports.Queries;
using KocurmeApp.Application.Interfaces;
using MediatR;

namespace KocurmeApp.Application.Application.Features.FileExports.Handlers
{
    public class Export9thGradeCheatingAnalysisCommandHandler
        : IRequestHandler<Export9thGradeCheatingAnalysisCommand, ExportResultDTO>
    {
        private readonly IMediator _mediator;
        private readonly IExcelExportService _excelExportService;

        public Export9thGradeCheatingAnalysisCommandHandler(
            IMediator mediator,
            IExcelExportService excelExportService)
        {
            _mediator = mediator;
            _excelExportService = excelExportService;
        }

        public async Task<ExportResultDTO> Handle(
            Export9thGradeCheatingAnalysisCommand request,
            CancellationToken cancellationToken)
        {
            var query = new Get9thGradeCheatingAnalysisForExportQuery
            {
                ExamId = request.ExamId
            };

            var data = await _mediator.Send(query, cancellationToken);

            var fileContent = await _excelExportService.ExportNinthGradeCheatingAnalysisToExcelAsync(
                data,
                request.SheetName);

            return new ExportResultDTO
            {
                FileContent = fileContent,
                FileName = $"9CuSinifKocurmeAnalizi_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
    }
}
