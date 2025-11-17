using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.Rooms.Commands
{
    public class UpdateRoomCommand : IRequest<bool>
    {
        public int Id { get; set; }   
        public int ExamId { get; set; }
        public short Z_KOD { get; set; }
        public byte XAR_DIL { get; set; }
        public byte? NUMMETN { get; set; }
        public short B_KOD { get; set; }
        public string V_BINA { get; set; } = default!;
        public byte MERTEBE { get; set; }
        public byte KOL_SIRA { get; set; }
        public byte KOL_YER { get; set; }
        public byte TUTUMU { get; set; }
        public string GR_FL { get; set; } = default!;
        public byte KOL_ABT { get; set; }
        public byte KOL_NAZ { get; set; }
        public byte IMT_YERI { get; set; }
        public byte DIL { get; set; }
        public string? YASHKATEG { get; set; }
        public string AADI { get; set; } = default!;
        public string? WAADI { get; set; }
        public byte MODUL { get; set; }
    }
}
