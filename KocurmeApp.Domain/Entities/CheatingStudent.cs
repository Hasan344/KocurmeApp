using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Domain.Entities
{
    public class CheatingStudent
    {
        public int Id { get; set; } 
        public int ExamId { get; set; } 
        public Exam Exam { get; set; } = default!;

        public byte IMT_GUN { get; set; }
        public string V_BINA { get; set; } = default!;
        public int IS_N1 { get; set; }
        public short BINA { get; set; }
        public short ZAL1 { get; set; }
        public byte FENN { get; set; }
        public string FNADI { get; set; } = default!;
        public int IS_N2 { get; set; }
        public short ZAL2 { get; set; }
        public byte EYNI_D { get; set; }
        public byte EYNI_Y { get; set; }
        public byte EYNI_B { get; set; }
        public decimal Y_OXSHAR { get; set; }
        public decimal T_OXSHAR { get; set; }
        public decimal BAL1 { get; set; }
        public decimal BAL2 { get; set; }
    }
}
