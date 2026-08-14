// Application/Interfaces/IExcelExportService.cs
using KocurmeApp.Application.Application.Features.FileExports.DTOs;

namespace KocurmeApp.Application.Interfaces
{
    public interface IExcelExportService
    {
        /// <summary>
        /// Köçürmə analiz datalarını Excel faylına export edir
        /// </summary>
        Task<byte[]> ExportCheatingAnalysisToExcelAsync(
            CheatingAnalysisExportResult data,
            string sheetName = "Sheet1");
        Task<byte[]> ExportSupervisorCheatingAnalysisToExcelAsync(
        SupervisorCheatingAnalysisExportResult data,
        string sheetName = "Supervisor Analysis");

        /// <summary>
        /// 9-cu sinif zal köçürmə analizini Excel faylına export edir.
        /// </summary>
        Task<byte[]> ExportNinthGradeCheatingAnalysisToExcelAsync(
            NinthGradeCheatingAnalysisExportResult data,
            string sheetName = "9-cu sinif Zal Köçürmə Analizi");
    }
}