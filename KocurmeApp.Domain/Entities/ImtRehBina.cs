using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Domain.Entities
{
    [Table("imtrehbina")]

    public class ImtRehBina
    {
        [Column("exam_id")]
        public short? ExamId { get; set; }

        [Column("i_r")]
        public int? IR { get; set; }

        [Column("VN")]
        public byte? VN { get; set; }

        [Column("v_bina")]
        public string? VBina { get; set; }

        [Column("B_KOD")]
        public short? BKod { get; set; }
    }
}
