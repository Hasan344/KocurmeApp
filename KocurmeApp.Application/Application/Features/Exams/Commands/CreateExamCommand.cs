using MediatR;

namespace KocurmeApp.Application.Features.Exams.Commands
{
    public record CreateContingentCommand(string Name, DateTime? ExamDate = null, int? Sinif = null) : IRequest<int>;
}
