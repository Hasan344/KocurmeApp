using KocurmeApp.Application.Application.Features.CheatingStudents.Commands;
using KocurmeApp.Application.Application.Features.CheatingStudents.Queries;
using KocurmeApp.Application.Application.Features.Exams.Queries;
using KocurmeApp.Application.Features.CheatingStudents.Commands;
using KocurmeApp.Application.Features.Exams.Commands;
using KocurmeApp.Application.Features.Rooms.Queries;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KocurmeApp.Api.Controllers
{
    [ApiController]
    [Route("api/cheatingstudents")]

    public class CheatingStudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CheatingStudentController(IMediator mediator, AppDbContext context)
        {
            _mediator = mediator;
        }


        [HttpPost("cheating-students")]
        public async Task<IActionResult> ImportCheatingStudents([FromForm] ImportCheatingStudentsCommand command)
        {
            var result = await _mediator.Send(command);
            return result ? Ok("Import successful!") : BadRequest("Import failed!");
        }
        [HttpGet("{examId}/cheating-students")]
        public async Task<IActionResult> GetCheatingStudentsByExam(int examId)
        {
            var students = await _mediator.Send(new GetCheatingStudentsByExamQuery(examId));
            return Ok(students);
        }
        [HttpDelete("delete-by-exam/{examId}")]
        public async Task<IActionResult> DeleteCheatingStudentsByExam(int examId)
        {
            var result = await _mediator.Send(new DeleteCheatingStudentsByExamCommand(examId));
            return result ? Ok("All cheating students for the exam deleted.") : NotFound("No cheating students found for this exam.");
        }

    }

}