using MediatR;

namespace KocurmeApp.Application.Features.Exams.Commands
{
    public class DeleteExamCommand : IRequest<bool>
    {
        public int ExamId { get; set; }

        public DeleteExamCommand(int examId)
        {
            ExamId = examId;
        }
    }
}
