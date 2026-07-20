using KocurmeApp.Application.Application.Features.Contingents.Queries;
using KocurmeApp.Application.Features.Contingents.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KocurmeApp.Api.Controllers
{
    [ApiController]
    [Route("api/contingents")]
    public class ContingentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContingentController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("contingent")]
        public async Task<IActionResult> ImportContingents([FromForm] ImportContingentCommand command)
        {
            var result = await _mediator.Send(command);
            return result ? Ok("Import successful!") : BadRequest("Import failed!");
        }
        [HttpGet("{examId}/contingents")]
        public async Task<IActionResult> GetContingentsByExam(int examId)
        {
            var students = await _mediator.Send(new GetContingentsByExamQuery(examId));
            return Ok(students);
        }
        [HttpDelete("delete-by-exam/{examId}")]
        public async Task<IActionResult> DeleteContingentsByExam(int examId)
        {
            var result = await _mediator.Send(new DeleteContingentByExamCommand (examId));
            return result ? Ok("All Contingents for the exam deleted.") : NotFound("No contingents found for this exam.");
        }
    }
}