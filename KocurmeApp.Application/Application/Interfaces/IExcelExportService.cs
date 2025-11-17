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
            List<CheatingAnalysisExportDTO> data,
            string sheetName = "Sheet1");

        /// <summary>
        /// Generic Excel export metodu (gələcək üçün)
        /// </summary>
        Task<byte[]> ExportToExcelAsync<T>(
            IEnumerable<T> data,
            string sheetName = "Sheet1",
            Dictionary<string, string>? columnHeaders = null);
    }
}