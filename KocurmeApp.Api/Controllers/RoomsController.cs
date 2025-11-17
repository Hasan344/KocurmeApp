using KocurmeApp.Application.Application.Features.Rooms.Commands;
using KocurmeApp.Application.Features.Rooms.Commands;
using KocurmeApp.Application.Features.Rooms.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KocurmeApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportRooms([FromForm] ImportRoomsCommand command)
        {
            var result = await _mediator.Send(command);
            return result ? Ok("Rooms imported successfully!") : BadRequest("Import failed!");
        }

        [HttpGet("{examId}/rooms")]
        public async Task<IActionResult> GetRoomsByExam(int examId)
        {
            var rooms = await _mediator.Send(new GetRoomsByExamQuery(examId));
            return Ok(rooms);
        }
        [HttpDelete("delete-by-exam/{examId}")]
        public async Task<IActionResult> DeleteRoomsByExam(int examId)
        {
            var result = await _mediator.Send(new DeleteRoomsByExamCommand(examId));
            return result ? Ok("All rooms for the exam deleted.") : NotFound("No rooms found for this exam.");
        }

    }
}
