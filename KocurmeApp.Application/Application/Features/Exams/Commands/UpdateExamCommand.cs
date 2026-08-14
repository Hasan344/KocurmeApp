using MediatR;
using System;

namespace KocurmeApp.Application.Features.Exams.Commands
{
    public class UpdateContingentCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime ExamDate { get; set; }
        public int? Sinif { get; set; }
    }
}
