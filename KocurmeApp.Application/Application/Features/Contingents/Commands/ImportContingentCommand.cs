using MediatR;
using Microsoft.AspNetCore.Http;

namespace KocurmeApp.Application.Features.Contingents.Commands
{
    public record ImportContingentCommand(
        IFormFile File,
        int ExamId
    ) : IRequest<bool>;
}
