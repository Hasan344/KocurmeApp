using KocurmeApp.Application.Features.Exams.Dtos;
using KocurmeApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.Exams.Queries
{
    public record GetAllExamsQuery : IRequest<List<ExamDTO>>;
}
