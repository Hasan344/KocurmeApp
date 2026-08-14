using System.Collections.Generic;

namespace KocurmeApp.Application.Application.Features.FileExports.DTOs
{
    public class NinthGradeCheatingAnalysisExportResult
    {
        public List<NinthGradeCheatingAnalysisDTO> AnalysisData { get; set; } = new();

        /// <summary>
        /// Köçürmə faizinə (Kolon5) görə paylanma: (0-1], (1-2], ... (9-10]
        /// və hər interval üzrə zalların sayı.
        /// </summary>
        public List<CheatingStatisticsDTO> Statistics { get; set; } = new();
    }
}
