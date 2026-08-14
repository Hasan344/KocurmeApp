namespace KocurmeApp.Application.Application.Features.FileExports.DTOs
{
    /// <summary>
    /// 9-cu sinif zal köçürmə analizi export sətri.
    /// IsSummary = true olduqda bu, imtahan üzrə "Orta qiymət" sətridir.
    /// </summary>
    public class NinthGradeCheatingAnalysisDTO
    {
        public int ExamId { get; set; }
        public string ExamName { get; set; } = default!;
        public string Zal { get; set; } = default!;
        public int? ZaldaKocurenAbituriyentlerinSayi { get; set; }
        public int? KocurulenFenlerinUmumiSayi { get; set; }
        public int? ZaldaOlanAbituriyentlerinSayi { get; set; }
        public decimal? ZaldaKocurmeFaizi1 { get; set; }
        public decimal? ZaldaKocurmeFaizi2 { get; set; }
        public decimal? Kolon3 { get; set; }
        public decimal? Kolon4 { get; set; }
        public decimal? Kolon5 { get; set; }
        public bool IsSummary { get; set; }
    }
}
