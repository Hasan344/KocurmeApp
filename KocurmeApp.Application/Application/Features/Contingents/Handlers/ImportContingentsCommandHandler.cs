using MediatR;
using ClosedXML.Excel;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Application.Application.Features.CheatingStudents.Commands;
using KocurmeApp.Infrastructure;
using KocurmeApp.Infrastructure.Services.FileImport;
using Microsoft.EntityFrameworkCore;
using KocurmeApp.Application.Features.Contingents.Commands;

namespace KocurmeApp.Application.Features.CheatingStudents.Handlers;

    public class ImportContingentsCommandHandler : IRequestHandler<ImportContingentCommand, bool>
    {
        private readonly DbfImportService _dbfService;
        private readonly AppDbContext _context;

        public ImportContingentsCommandHandler(DbfImportService dbfService, AppDbContext context)
        {
            _dbfService = dbfService;
            _context = context;
        }

    public async Task<bool> Handle(ImportContingentCommand request, CancellationToken cancellationToken)
    {
        var exam = await _context.Exams
            .Include(e => e.Contingents)
            .FirstOrDefaultAsync(e => e.Id == request.ExamId, cancellationToken);

        if (exam == null)
            return false;

        using var stream = new MemoryStream();
        await request.File.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        var students = await _dbfService.ImportContingentsAsync(stream);

        foreach (var student in students)
        {
            student.ExamId = exam.Id; 
            exam.Contingents.Add(student);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

