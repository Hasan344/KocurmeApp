using MediatR;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Application.Features.Exams.Commands;
using KocurmeApp.Infrastructure;

namespace KocurmeApp.Application.Features.Exams.Handlers
{
    public class CreateExamCommandHandler : IRequestHandler<CreateContingentCommand, int>
    {
        private readonly AppDbContext _context;

        public CreateExamCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateContingentCommand request, CancellationToken cancellationToken)
        {
            var exam = new Exam
            {
                Name = request.Name,
                ExamDate = request.ExamDate ?? DateTime.UtcNow,
                ImportedAt = DateTime.UtcNow,
                Sinif = request.Sinif
            };

            _context.Exams.Add(exam);
            await _context.SaveChangesAsync(cancellationToken);

            return exam.Id;
        }
    }
}
