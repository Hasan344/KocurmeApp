using KocurmeApp.Application.Features.Rooms.Commands;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Features.Rooms.Handlers
{
    public class DeleteRoomsByExamCommandHandler : IRequestHandler<DeleteRoomsByExamCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteRoomsByExamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteRoomsByExamCommand request, CancellationToken cancellationToken)
        {
            var rooms = await _context.Rooms
                .Where(r => r.ExamId == request.ExamId)
                .ToListAsync(cancellationToken);

            if (!rooms.Any())
                return false;

            _context.Rooms.RemoveRange(rooms);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
