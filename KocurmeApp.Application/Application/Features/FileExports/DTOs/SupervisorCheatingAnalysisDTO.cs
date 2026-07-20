using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.FileExports.DTOs
{
    public class SupervisorCheatingAnalysisDTO
    {
        public int IRehber { get; set; }
        public int BKod { get; set; }
        public string VBina { get; set; }
        public int ExamId { get; set; }
        public string TamAd { get; set; }
        public string ZalSiyahisi { get; set; }
        public decimal RehberinZallarindakiFaizlerinOrtaQiymeti { get; set; }
        public decimal RehberinZallarindakiFaizlerinOrtaQiymetiEmsal { get; set; }
    }

    public class SupervisorCheatingAnalysisExportResult
    {
        public List<SupervisorCheatingAnalysisDTO> AnalysisData { get; set; }
    }
}
