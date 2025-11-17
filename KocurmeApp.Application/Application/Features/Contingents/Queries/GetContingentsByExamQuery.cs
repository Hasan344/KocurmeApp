using KocurmeApp.Application.Application.Features.CheatingStudents.DTOs;
using KocurmeApp.Application.Application.Features.Contingents.DTOs;
using MediatR;

namespace KocurmeApp.Application.Application.Features.Contingents.Queries
{
    public record GetContingentsByExamQuery(int ExamId) : IRequest<List<ContingentDTO>>;
}
