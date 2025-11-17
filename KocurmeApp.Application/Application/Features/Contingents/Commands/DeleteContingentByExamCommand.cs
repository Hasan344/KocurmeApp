using MediatR;

namespace KocurmeApp.Application.Features.Contingents.Commands
{
    public class DeleteContingentByExamCommand : IRequest<bool>
    {
        public int ExamId { get; set; } 


        public DeleteContingentByExamCommand(int examId)
        {
            ExamId = examId;
        }
    }
}
