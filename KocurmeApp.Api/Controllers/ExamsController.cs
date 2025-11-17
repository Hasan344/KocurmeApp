using KocurmeApp.Application.Application.Features.Exams.Queries;
using KocurmeApp.Application.Features.Exams.Commands;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KocurmeApp.Api.Controllers;

[ApiController]
[Route("api/exams")]
public class ExamsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExamsController(IMediator mediator, AppDbContext context)
    {
        _mediator = mediator;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateExam([FromForm] CreateContingentCommand command)
    {
        var examId = await _mediator.Send(command);
        return Ok(new { ExamId = examId });
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllExams()
    {
        var exams = await _mediator.Send(new GetAllExamsQuery());
        return Ok(exams);
    }
    [HttpPut("update")]
    public async Task<IActionResult> UpdateExam([FromBody] UpdateContingentCommand command)
    {
        var result = await _mediator.Send(command);
        return result ? Ok("Exam updated successfully!") : NotFound("Exam not found!");
    }

    [HttpDelete("delete/{examId}")]
    public async Task<IActionResult> DeleteExam(int examId)
    {
        var result = await _mediator.Send(new DeleteExamCommand(examId));
        return result ? Ok("Exam and related data deleted successfully!") : NotFound("Exam not found!");
    }
}
