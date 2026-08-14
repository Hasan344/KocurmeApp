using MediatR;
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
        if (request.File == null || request.File.Length == 0)
            throw new InvalidOperationException("İdxal üçün fayl təqdim olunmayıb və ya boşdur.");

        var exam = await _context.Exams
            .Include(e => e.CheatingStudents)
            .FirstOrDefaultAsync(e => e.Id == request.ExamId, cancellationToken);

        if (exam == null)
            throw new InvalidOperationException(
                $"Göstərilən imtahan tapılmadı (ExamId={request.ExamId}). Əvvəlcə imtahan seçin/yaradın.");

        using var stream = new MemoryStream();
        await request.File.CopyToAsync(stream, cancellationToken);
        stream.Position = 0;

        var students = await _dbfService.ImportCheatingStudentsAsync(stream);

        foreach (var student in students)
        {
            student.ExamId = exam.Id;
            exam.CheatingStudents.Add(student);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}