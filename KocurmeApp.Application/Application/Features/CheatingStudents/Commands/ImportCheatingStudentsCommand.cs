using MediatR;
using Microsoft.AspNetCore.Http;

namespace KocurmeApp.Application.Application.Features.CheatingStudents.Commands;
public record ImportCheatingStudentsCommand(
        IFormFile File,  
        int ExamId       
    ) : IRequest<bool>;