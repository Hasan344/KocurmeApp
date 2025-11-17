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

        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<CheatingStudent> CheatingStudents { get; set; } = new List<CheatingStudent>();
        public ICollection<Contingent> Contingents { get; set; } = new List<Contingent>();
    }
}