using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.Contingents.DTOs
{
    public class ContingentDTO
    {
        public int Id { get; set; }
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
