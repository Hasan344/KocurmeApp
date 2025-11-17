using KocurmeApp.Application.Application.Features.CheatingStudents.DTOs;
using KocurmeApp.Application.Application.Features.CheatingStudents.Queries;
using KocurmeApp.Application.Application.Features.Contingents.DTOs;
using KocurmeApp.Application.Application.Features.Contingents.Queries;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Features.Contingents.Handlers
{
    public class GetContingentsByExamQueryHandler : IRequestHandler<GetContingentsByExamQuery, List<ContingentDTO>>
    {
        private readonly AppDbContext _context;

        public GetContingentsByExamQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ContingentDTO>> Handle(GetContingentsByExamQuery request, CancellationToken cancellationToken)
        {
            var contingents = await _context.Contingents
                .Where(s => s.ExamId == request.ExamId)
                .Select(s => new ContingentDTO
                {
                    Id = s.Id,
                    IMT_GUN = s.IMT_GUN,
                    IMT_YERI = s.IMT_YERI,
                    IZAHI = s.IZAHI,
                    NUM_K = s.NUM_K,
                    YASH_KATEQ = s.YASH_KATEQ,
                    SAYI = s.SAYI,
                    SAYI0 = s.SAYI0,
                    SEC = s.SEC,
                    TIP_OTUR = s.TIP_OTUR

                })
                .ToListAsync(cancellationToken);

            return contingents;
        }

    }
}
