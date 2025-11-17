using KocurmeApp.Application.Features.CheatingStudents.Commands;
using KocurmeApp.Application.Features.Contingents.Commands;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Features.Contingents.Handlers
{
    public class DeleteContingentsByExamCommandHandler : IRequestHandler<DeleteContingentByExamCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteContingentsByExamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteContingentByExamCommand request, CancellationToken cancellationToken)
        {
            var contingents = await _context.Contingents
                .Where(s => s.ExamId == request.ExamId)
                .ToListAsync(cancellationToken);

            if (!contingents.Any())
                return false;

            _context.Contingents.RemoveRange(contingents);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
