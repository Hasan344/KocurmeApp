using KocurmeApp.Application.Application.Features.Exams.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.Exams.Queries
{

    public record GetExamLookupQuery : IRequest<List<ExamLookupDto>>;
}
