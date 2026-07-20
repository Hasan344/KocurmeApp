using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.FileExports.DTOs
{
    public class CheatingAnalysisExportResult
    {
        public List<CheatingAnalysisExportDTO> AnalysisData { get; set; }
        public List<CheatingStatisticsDTO> Statistics { get; set; }
    }
}
