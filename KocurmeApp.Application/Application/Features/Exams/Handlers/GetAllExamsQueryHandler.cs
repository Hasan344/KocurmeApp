using MediatR;
using Microsoft.EntityFrameworkCore;
using KocurmeApp.Application.Features.Exams.Dtos;
using KocurmeApp.Application.Application.Features.CheatingStudents.DTOs;
using KocurmeApp.Application.Application.Features.Rooms.DTOs;
using KocurmeApp.Infrastructure;
using KocurmeApp.Application.Application.Features.Exams.Queries;

namespace KocurmeApp.Application.Features.Exams.Queries
{
    public class GetAllExamsQueryHandler : IRequestHandler<GetAllExamsQuery, List<ExamDTO>>
    {
        private readonly AppDbContext _context;

        public GetAllExamsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamDTO>> Handle(GetAllExamsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Exams
                .Select(e => new ExamDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    ExamDate = e.ExamDate,
                    ImportedAt = e.ImportedAt,
                    Sinif = e.Sinif
                }).ToListAsync(cancellationToken);
        }
    }
}
