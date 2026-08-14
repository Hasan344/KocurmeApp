using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Domain.Entities
{
    public class Exam
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime ExamDate { get; set; }
        public DateTime ImportedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// İmtahanın sinif tipi (məs. 9 = 9-cu sinif, 11 = 11-ci sinif).
        /// Sinif üzrə köçürmə analizində qlobal ortanı hesablamaq üçün istifadə olunur.
        /// </summary>
        public int? Sinif { get; set; }

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<CheatingStudent> CheatingStudents { get; set; } = new List<CheatingStudent>();
        public ICollection<Contingent> Contingents { get; set; } = new List<Contingent>();
    }
}