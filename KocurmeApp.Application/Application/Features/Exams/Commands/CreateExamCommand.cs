using MediatR;

namespace KocurmeApp.Application.Features.Exams.Commands
{
    public record CreateContingentCommand(string Name, DateTime? ExamDate = null) : IRequest<int>;
}
