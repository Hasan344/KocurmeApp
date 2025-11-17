using MediatR;

namespace KocurmeApp.Application.Features.Rooms.Commands
{
    public class DeleteRoomsByExamCommand : IRequest<bool>
    {
        public int ExamId { get; set; }

        public DeleteRoomsByExamCommand(int examId)
        {
            ExamId = examId;
        }
    }
}
