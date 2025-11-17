namespace KocurmeApp.Domain.Entities
{
    public class Contingent
    {
        public int Id { get; set; }
        public int ExamId { get; set; } 
        public Exam Exam { get; set; } = default!;
        public byte? IMT_GUN { get; set; }
        public byte? IMT_YERI { get; set; }
        public byte? NUM_K { get; set; }
        public byte? YASH_KATEQ { get; set; }
        public string? IZAHI { get; set; }
        public byte? SEC { get; set; }
        public byte? TIP_OTUR { get; set; }
        public short? SAYI { get; set; }
        public string? SAYI0 { get; set; }
    }
}