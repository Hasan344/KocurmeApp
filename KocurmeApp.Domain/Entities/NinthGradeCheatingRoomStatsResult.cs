using System;

namespace KocurmeApp.Domain.Entities
{
    /// <summary>
    /// dbo.fn_NinthGradeCheatingRoomStats funksiyasının nəticə sətri (keyless).
    /// 9-cu sinif zal köçürmə analizi (çox-imtahanlı) üçün istifadə olunur.
    /// RowType: 0 = detal (zal) sətri, 1 = imtahan üzrə "Orta qiymət" sətri.
    /// </summary>
    public class NinthGradeCheatingRoomStatsResult
    {
        public int ExamId { get; set; }
        public short Zal { get; set; }
        public int? KocurenSayi { get; set; }
        public int? FennSayi { get; set; }
        public int? OdaSayi { get; set; }
        public decimal? Faiz1 { get; set; }
        public decimal? Faiz2 { get; set; }
        public decimal? Kolon3 { get; set; }
        public decimal? Kolon4 { get; set; }
        public decimal? Kolon5 { get; set; }
        public byte RowType { get; set; }
    }
}
