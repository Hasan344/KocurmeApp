using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Domain.Entities
{
    [Table("imtreh")]
    public class ImtReh
    {
        [Column("exam_id")]
        public short? ExamId { get; set; }

        [Column("V_NUM")]
        public int? VNum { get; set; }

        [Column("QIYMET")]
        [StringLength(50)]
        public string? QiyMet { get; set; }

        [Column("SOY")]
        [StringLength(50)]
        public string? Soy { get; set; }

        [Column("ADI")]
        [StringLength(50)]
        public string? Adi { get; set; }

        [Column("BABA")]
        [StringLength(50)]
        public string? Baba { get; set; }

        [Column("AGE")]
        public short? Age { get; set; }

        [Column("SERIYA_P")]
        [StringLength(50)]
        public string? SeriyaP { get; set; }

        [Column("NUM_PASP")]
        [StringLength(50)]
        public string? NumPasp { get; set; }

        [Column("BITIR_UN")]
        [StringLength(100)]
        public string? BitirUn { get; set; }

        [Column("IXTISASI")]
        [StringLength(100)]
        public string? Ixtisasi { get; set; }

        [Column("NAMIZ_M")]
        [StringLength(50)]
        public string? NamizM { get; set; }

        [Column("DOKTOR_M")]
        [StringLength(50)]
        public string? DoktorM { get; set; }

        [Column("ELMI_DER")]
        [StringLength(50)]
        public string? ElmiDer { get; set; }

        [Column("IS_YERI")]
        [StringLength(500)]
        public string? IsYeri { get; set; }

        [Column("UNVAN")]
        [StringLength(300)]
        public string? Unvan { get; set; }

        [Column("BOLME")]
        public short? Bolme { get; set; }

        [Column("TEL_EV")]
        [StringLength(50)]
        public string? TelEv { get; set; }

        [Column("TEL_IS")]
        [StringLength(50)]
        public string? TelIs { get; set; }

        [Column("TEL_EHT")]
        [StringLength(50)]
        public string? TelEht { get; set; }

        [Column("TARIX")]
        [StringLength(50)]
        public string? Tarix { get; set; }

        [Column("VEZIFESI")]
        [StringLength(100)]
        public string? Vezifesi { get; set; }

        [Column("SECHILDI")]
        public bool? Sechildi { get; set; }

        [Column("num_exam")]
        public byte? NumExam { get; set; }

        [Column("cinsi")]
        [StringLength(50)]
        public string? Cinsi { get; set; }

        [Column("reg_id")]
        public byte? RegId { get; set; }

        [Column("Rayon_id")]
        public byte? RayonId { get; set; }

        [Column("SelectedDistrictForExam")]
        [StringLength(50)]
        public string? SelectedDistrictForExam { get; set; }

        [Column("contract_numb")]
        [StringLength(50)]
        public string? ContractNumb { get; set; }

        [Column("contract_date")]
        public DateTime? ContractDate { get; set; }
    }
}
