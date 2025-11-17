
namespace KocurmeApp.Application.Application.Features.Rooms.DTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public short Z_KOD { get; set; }
        public byte XAR_DIL { get; set; }
        public byte? NUMMETN { get; set; }
        public short B_KOD { get; set; }
        public string V_BINA { get; set; } = default!;
        public byte MERTEBE { get; set; }
        public byte KOL_SIRA { get; set; }
        public byte KOL_YER { get; set; }
        public byte TUTUMU { get; set; }
        public string? GR_FL { get; set; }
        public string AADI { get; set; } = default!;
    }
}
