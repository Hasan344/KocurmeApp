// Application/Features/FileExport/Commands/ExportCheatingAnalysis/ExportCheatingAnalysisCommandHandler.cs
using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using KocurmeApp.Application.Interfaces;
using MediatR;

namespace KocurmeApp.Application.Application.Features.FileExport.Commands.ExportCheatingAnalysis
{
    public class ExportCheatingAnalysisCommandHandler
        : IRequestHandler<FileExports.Commands.ExportCheatingAnalysisCommand, ExportResultDTO>
    {
        private readonly IMediator _mediator;
        private readonly IExcelExportService _excelExportService;

        public ExportCheatingAnalysisCommandHandler(
            IMediator mediator,
            IExcelExportService excelExportService)
        {
            _mediator = mediator;
            _excelExportService = excelExportService;
        }

        public async Task<ExportResultDTO> Handle(
            FileExports.Commands.ExportCheatingAnalysisCommand request,
            CancellationToken cancellationToken)
        {
            var query = new FileExports.Queries.GetCheatingAnalysisForExportQuery
            {
                MinEyniY = request.MinEyniY,
                MinEhtimal = request.MinEhtimal
            };

            var data = await _mediator.Send(query, cancellationToken);

            var fileContent = await _excelExportService.ExportCheatingAnalysisToExcelAsync(
                data,
                request.SheetName
            );

            return new ExportResultDTO
            {
                FileContent = fileContent,
                FileName = $"KocurmeAnalizi_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
    }
}