// Application/Features/FileExports/Commands/ExportSupervisorCheatingAnalysisCommandHandler.cs
using KocurmeApp.Application.Application.Features.FileExports.Commands;
using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using KocurmeApp.Application.Interfaces;
using MediatR;

namespace KocurmeApp.Application.Application.Features.FileExport.Commands.ExportSupervisorCheatingAnalysis
{
    public class ExportSupervisorCheatingAnalysisCommandHandler
        : IRequestHandler<ExportSupervisorCheatingAnalysisCommand, ExportResultDTO>
    {
        private readonly IMediator _mediator;
        private readonly IExcelExportService _excelExportService;

        public ExportSupervisorCheatingAnalysisCommandHandler(
            IMediator mediator,
            IExcelExportService excelExportService)
        {
            _mediator = mediator;
            _excelExportService = excelExportService;
        }

        public async Task<ExportResultDTO> Handle(
            ExportSupervisorCheatingAnalysisCommand request,
            CancellationToken cancellationToken)
        {
            var query = new FileExports.Queries.GetSupervisorCheatingAnalysisForExportQuery
            {
                CheatingExamId = request.CheatingExamId,
                SupervisorExamId = request.SupervisorExamId
            };

            var data = await _mediator.Send(query, cancellationToken);

            var fileContent = await _excelExportService.ExportSupervisorCheatingAnalysisToExcelAsync(
                data,
                request.SheetName
            );

            return new ExportResultDTO
            {
                FileContent = fileContent,
                FileName = $"NezaretciKocurmeAnalizi_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
    }
}