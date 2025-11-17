using KocurmeApp.Application.Application.Features.CheatingStudents.DTOs;
using KocurmeApp.Application.Application.Features.Rooms.DTOs;

namespace KocurmeApp.Application.Features.Exams.Dtos
{
    public class ExamDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime ExamDate { get; set; }
        public DateTime ImportedAt { get; set; }

        public List<RoomDTO>? Rooms { get; set; }
        public List<CheatingStudentDTO>? CheatingStudents { get; set; }
    }
}