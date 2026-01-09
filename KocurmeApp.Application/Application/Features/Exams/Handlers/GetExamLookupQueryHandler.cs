using KocurmeApp.Application.Application.Features.Exams.DTOs;
using KocurmeApp.Application.Application.Features.Exams.Queries;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.Exams.Handlers
{
    public class GetExamLookupQueryHandler
    : IRequestHandler<GetExamLookupQuery, List<ExamLookupDto>>
    {
        private readonly AppDbContext _context;

        public GetExamLookupQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ExamLookupDto>> Handle(
            GetExamLookupQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Exams
                .OrderByDescending(x => x.ExamDate)
                .Select(x => new ExamLookupDto
                {
                    Id = x.Id,
                    // 👇 user-friendly ad
                    Name = x.Name + " – "
                         + x.ExamDate.ToString("dd.MM.yyyy")
                         
                })
                .ToListAsync(cancellationToken);
        }
    }
}
