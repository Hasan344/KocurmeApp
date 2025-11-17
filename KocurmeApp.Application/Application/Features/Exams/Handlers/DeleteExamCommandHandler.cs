using KocurmeApp.Application.Features.Contingents.Commands;
using KocurmeApp.Application.Features.Exams.Commands;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Features.Exams.Handlers
{
    public class DeleteExamCommandHandler : IRequestHandler<DeleteContingentByExamCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteExamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteContingentByExamCommand request, CancellationToken cancellationToken)
        {
            var exam = await _context.Exams
                .Include(e => e.Rooms)
                .Include(e => e.CheatingStudents)
                .FirstOrDefaultAsync(e => e.Id == request.ExamId, cancellationToken);

            if (exam == null)
                return false;

            // Alt ilişkileri sil
            _context.Rooms.RemoveRange(exam.Rooms);
            _context.CheatingStudents.RemoveRange(exam.CheatingStudents);

            // Sınavı sil
            _context.Exams.Remove(exam);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
