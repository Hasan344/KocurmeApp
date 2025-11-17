using KocurmeApp.Application.Application.Features.Rooms.DTOs;
using KocurmeApp.Application.Features.Rooms.Queries;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Features.Rooms.Handlers
{
    public class GetRoomsByExamQueryHandler : IRequestHandler<GetRoomsByExamQuery, List<RoomDTO>>
    {
        private readonly AppDbContext _context;

        public GetRoomsByExamQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoomDTO>> Handle(GetRoomsByExamQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _context.Rooms
                .Where(r => r.ExamId == request.ExamId)
                .Select(r => new RoomDTO
                {
                    Id = r.Id,
                    Z_KOD = r.Z_KOD,
                    V_BINA = r.V_BINA,
                    B_KOD = r.B_KOD,
                    MERTEBE = r.MERTEBE,
                    KOL_SIRA = r.KOL_SIRA,
                    KOL_YER = r.KOL_YER,
                    TUTUMU = r.TUTUMU,
                    AADI = r.AADI
                })
                .ToListAsync(cancellationToken);

            return rooms;
        }
    }
}
