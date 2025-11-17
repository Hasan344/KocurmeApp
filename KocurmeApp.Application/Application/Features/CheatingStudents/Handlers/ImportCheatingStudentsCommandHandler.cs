using MediatR;
using ClosedXML.Excel;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Application.Application.Features.CheatingStudents.Commands;
using KocurmeApp.Infrastructure;
using KocurmeApp.Infrastructure.Services.FileImport;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Features.CheatingStudents.Handlers;

    public class ImportCheatingStudentsHandler : IRequestHandler<ImportCheatingStudentsCommand, bool>
    {
        private readonly DbfImportService _dbfService;
        private readonly AppDbContext _context;

        public ImportCheatingStudentsHandler(DbfImportService dbfService, AppDbContext context)
        {
            _dbfService = dbfService;
            _context = context;
        }

    public async Task<bool> Handle(ImportCheatingStudentsCommand request, CancellationToken cancellationToken)
    {
        // 1️⃣ İlgili sınavı al
        var exam = await _context.Exams
            .Include(e => e.CheatingStudents)
            .FirstOrDefaultAsync(e => e.Id == request.ExamId, cancellationToken);

        if (exam == null)
            return false;

        // 2️⃣ Dosyayı MemoryStream'e al
        using var stream = new MemoryStream();
        await request.File.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        // 3️⃣ DBF dosyasını oku
        var students = await _dbfService.ImportCheatingStudentsAsync(stream);

        // 4️⃣ Exam ile ilişkilendir
        foreach (var student in students)
        {
            student.ExamId = exam.Id; // FK ilişkisi
            exam.CheatingStudents.Add(student);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}

