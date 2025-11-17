using MediatR;

namespace KocurmeApp.Application.Features.CheatingStudents.Commands
{
    public class DeleteCheatingStudentsByExamCommand : IRequest<bool>
    {
        public int ExamId { get; set; }

        public DeleteCheatingStudentsByExamCommand(int examId)
        {
            ExamId = examId;
        }
    }
}
