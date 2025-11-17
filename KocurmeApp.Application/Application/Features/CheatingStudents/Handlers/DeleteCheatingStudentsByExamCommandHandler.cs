using KocurmeApp.Application.Features.CheatingStudents.Commands;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Features.CheatingStudents.Handlers
{
    public class DeleteContingentsByExamCommandHandler : IRequestHandler<DeleteCheatingStudentsByExamCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteContingentsByExamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCheatingStudentsByExamCommand request, CancellationToken cancellationToken)
        {
            var students = await _context.CheatingStudents
                .Where(s => s.ExamId == request.ExamId)
                .ToListAsync(cancellationToken);

            if (!students.Any())
                return false;

            _context.CheatingStudents.RemoveRange(students);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
