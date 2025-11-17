using KocurmeApp.Application.Application.Features.CheatingStudents.DTOs;
using KocurmeApp.Application.Application.Features.CheatingStudents.Queries;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Features.CheatingStudents.Handlers
{
    public class GetContingentsByExamQueryHandler : IRequestHandler<GetCheatingStudentsByExamQuery, List<CheatingStudentDTO>>
    {
        private readonly AppDbContext _context;

        public GetContingentsByExamQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CheatingStudentDTO>> Handle(GetCheatingStudentsByExamQuery request, CancellationToken cancellationToken)
        {
            var students = await _context.CheatingStudents
                .Where(s => s.ExamId == request.ExamId)
                .Select(s => new CheatingStudentDTO
                {
                    Id = s.Id,
                    V_BINA = s.V_BINA,
                    IS_N1 = s.IS_N1,
                    BINA = s.BINA,
                    ZAL1 = s.ZAL1,
                    FENN = s.FENN,
                    FNADI = s.FNADI,
                    IS_N2 = s.IS_N2,
                    ZAL2 = s.ZAL2,
                    EYNI_D = s.EYNI_D,
                    EYNI_Y = s.EYNI_Y,
                    EYNI_B = s.EYNI_B,
                    Y_OXSHAR = s.Y_OXSHAR,
                    T_OXSHAR = s.T_OXSHAR,
                    BAL1 = s.BAL1,
                    BAL2 = s.BAL2
                })
                .ToListAsync(cancellationToken);

            return students;
        }
    }
}
