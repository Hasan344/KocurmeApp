using KocurmeApp.Application.Application.Features.CheatingStudents.DTOs;
using MediatR;

namespace KocurmeApp.Application.Application.Features.CheatingStudents.Queries
{
    public record GetCheatingStudentsByExamQuery(int ExamId) : IRequest<List<CheatingStudentDTO>>;
}
