using MediatR;
using Microsoft.AspNetCore.Http;

namespace KocurmeApp.Application.Features.Rooms.Commands
{
    public class ImportRoomsCommand : IRequest<bool>
    {
        public IFormFile File { get; set; } = default!;
        public int ExamId { get; set; }
    }
}
