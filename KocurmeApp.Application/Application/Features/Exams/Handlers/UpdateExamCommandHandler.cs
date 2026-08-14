using KocurmeApp.Application.Features.Exams.Commands;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Features.Exams.Handlers
{
    public class UpdateExamCommandHandler : IRequestHandler<UpdateContingentCommand, bool>
    {
        private readonly AppDbContext _context;

        public UpdateExamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateContingentCommand request, CancellationToken cancellationToken)
        {
            var exam = await _context.Exams.FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
            if (exam == null)
                return false;

            exam.Name = request.Name;
            exam.ExamDate = request.ExamDate;
            exam.Sinif = request.Sinif;

            _context.Exams.Update(exam);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
