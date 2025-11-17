using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.FileExports.DTOs
{
    public class CheatingAnalysisExportDTO
    {
        public string Zal { get; set; } = default!;
        public int? ZaldaKocurenAbituriyentlerinSayi { get; set; }
        public int? KocurulenFenlerinUmumiSayi { get; set; }
        public int? ZaldaOlanAbituriyentlerinSayi { get; set; }
        public decimal? ZaldaKocurmeFaizi1 { get; set; }
        public decimal? ZaldaKocurmeFaizi2 { get; set; }
        public decimal? Kolon3 { get; set; }
        public decimal? Kolon4 { get; set; }
        public decimal? Kolon5 { get; set; }
    }
}
