using KocurmeApp.Application.Features.Contingents.Commands;
using KocurmeApp.Application.Features.Exams.Commands;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Features.Exams.Handlers
{
    public class DeleteExamCommandHandler : IRequestHandler<DeleteExamCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteExamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteExamCommand request, CancellationToken cancellationToken)
        {
            var exam = await _context.Exams
                .Include(e => e.Rooms)
                .Include(e => e.CheatingStudents)
                .Include(e => e.Contingents)
                .FirstOrDefaultAsync(e => e.Id == request.ExamId, cancellationToken);

            if (exam == null)
                return false;

            _context.Rooms.RemoveRange(exam.Rooms);
            _context.CheatingStudents.RemoveRange(exam.CheatingStudents);
            _context.Contingents.RemoveRange(exam.Contingents);

            _context.Exams.Remove(exam);

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
