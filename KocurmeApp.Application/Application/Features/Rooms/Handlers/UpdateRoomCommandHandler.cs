using KocurmeApp.Application.Application.Features.Rooms.Commands;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Features.Rooms.Handlers
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateRoomCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

            if (room == null)
                return false;

            room.ExamId = request.ExamId;
            room.Z_KOD = request.Z_KOD;
            room.XAR_DIL = request.XAR_DIL;
            room.NUMMETN = request.NUMMETN;
            room.B_KOD = request.B_KOD;
            room.V_BINA = request.V_BINA;
            room.MERTEBE = request.MERTEBE;
            room.KOL_SIRA = request.KOL_SIRA;
            room.KOL_YER = request.KOL_YER;
            room.TUTUMU = request.TUTUMU;
            room.GR_FL = request.GR_FL;
            room.KOL_ABT = request.KOL_ABT;
            room.KOL_NAZ = request.KOL_NAZ;
            room.IMT_YERI = request.IMT_YERI;
            room.DIL = request.DIL;
            room.YASHKATEG = request.YASHKATEG;
            room.AADI = request.AADI;
            room.WAADI = request.WAADI;
            room.MODUL = request.MODUL;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
