using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KocurmeApp.Api.Controllers
{
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FileController> _logger;

        public FileController(IMediator mediator, ILogger<FileController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost("export/cheating-analysis")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportCheatingAnalysis(
          [FromBody] Application.Application.Features.FileExports.Commands.ExportCheatingAnalysisCommand command)
        {
            try
            {
                _logger.LogInformation(
                    "Köçürmə analizi export başladı. MinEyniY: {MinEyniY}, MinEhtimal: {MinEhtimal}",
                    command.MinEyniY,
                    command.MinEhtimal);

                var result = await _mediator.Send(command);

                _logger.LogInformation("Excel fayl uğurla yaradıldı: {FileName}", result.FileName);

                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel export zamanı xəta baş verdi");
                return StatusCode(500, new { message = "Excel export zamanı xəta baş verdi", error = ex.Message });
            }
        }

        [HttpGet("export/cheating-analysis")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportCheatingAnalysisGet(
            [FromQuery] int examId,
            [FromQuery] int minEyniY = 5,
            [FromQuery] decimal minEhtimal = 70,
            [FromQuery] string? sheetName = null)
        {
            try
            {
                if (examId <= 0)
                {
                    return BadRequest(new { message = "ExamId məcburidir və müsbət olmalıdır" });
                }
                var command = new Application.Application.Features.FileExports.Commands.ExportCheatingAnalysisCommand
                {
                    ExamId = examId,
                    MinEyniY = minEyniY,
                    MinEhtimal = minEhtimal,
                    SheetName = sheetName ?? "Köçürmə Analizi"
                };

                _logger.LogInformation(
                    "Köçürmə analizi export başladı (GET). Imtahan id: {ExamId} MinEyniY: {MinEyniY}, MinEhtimal: {MinEhtimal}",
                    command.ExamId,
                    command.MinEyniY,
                    command.MinEhtimal);

                var result = await _mediator.Send(command);

                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel export zamanı xəta baş verdi");
                return StatusCode(500, new { message = "Excel export zamanı xəta baş verdi", error = ex.Message });
            }
        }
        // FileController-ə əlavə edin:
        [HttpGet("export/supervisor-cheating-analysis")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportSupervisorCheatingAnalysisGet(
            [FromQuery] int cheatingExamId = 5,
            [FromQuery] int supervisorExamId = 456,
            [FromQuery] string? sheetName = null)
        {
            try
            {
                if (cheatingExamId <= 0 || supervisorExamId <= 0)
                {
                    return BadRequest(new { message = "ExamId-lər məcburidir və müsbət olmalıdır" });
                }

                var command = new Application.Application.Features.FileExports.Commands.ExportSupervisorCheatingAnalysisCommand
                {
                    CheatingExamId = cheatingExamId,
                    SupervisorExamId = supervisorExamId,
                    SheetName = sheetName ?? "Nəzarətçi Köçürmə Analizi"
                };

                _logger.LogInformation(
                    "Nəzarətçi köçürmə analizi export başladı. CheatingExamId: {CheatingExamId}, SupervisorExamId: {SupervisorExamId}",
                    command.CheatingExamId,
                    command.SupervisorExamId);

                var result = await _mediator.Send(command);

                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Nəzarətçi analizi Excel export zamanı xəta baş verdi");
                return StatusCode(500, new { message = "Excel export zamanı xəta baş verdi", error = ex.Message });
            }
        }

        [HttpPost("export/supervisor-cheating-analysis")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportSupervisorCheatingAnalysis(
            [FromBody] Application.Application.Features.FileExports.Commands.ExportSupervisorCheatingAnalysisCommand command)
        {
            try
            {
                _logger.LogInformation(
                    "Nəzarətçi köçürmə analizi export başladı. CheatingExamId: {CheatingExamId}, SupervisorExamId: {SupervisorExamId}",
                    command.CheatingExamId,
                    command.SupervisorExamId);

                var result = await _mediator.Send(command);

                _logger.LogInformation("Excel fayl uğurla yaradıldı: {FileName}", result.FileName);

                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excel export zamanı xəta baş verdi");
                return StatusCode(500, new { message = "Excel export zamanı xəta baş verdi", error = ex.Message });
            }
        }

        // ===== 9-cu sinif zal köçürmə analizi =====

        [HttpGet("export/ninth-grade-cheating-analysis")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportNinthGradeCheatingAnalysisGet(
            [FromQuery] int examId,
            [FromQuery] string? sheetName = null)
        {
            try
            {
                if (examId <= 0)
                {
                    return BadRequest(new { message = "ExamId məcburidir və müsbət olmalıdır" });
                }

                var command = new Application.Application.Features.FileExports.Commands.Export9thGradeCheatingAnalysisCommand
                {
                    ExamId = examId,
                    SheetName = sheetName ?? "9-cu sinif Zal Köçürmə Analizi"
                };

                _logger.LogInformation(
                    "Seçilmiş imtahan üzrə zal köçürmə analizi export başladı (GET). ExamId: {ExamId}",
                    command.ExamId);

                var result = await _mediator.Send(command);

                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "9-cu sinif zal köçürmə analizi Excel export zamanı xəta baş verdi");
                return StatusCode(500, new { message = "Excel export zamanı xəta baş verdi", error = ex.Message });
            }
        }

        [HttpPost("export/ninth-grade-cheating-analysis")]
        [ProducesResponseType(typeof(FileResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ExportNinthGradeCheatingAnalysis(
            [FromBody] Application.Application.Features.FileExports.Commands.Export9thGradeCheatingAnalysisCommand command)
        {
            try
            {
                _logger.LogInformation(
                    "Seçilmiş imtahan üzrə zal köçürmə analizi export başladı. ExamId: {ExamId}",
                    command.ExamId);

                var result = await _mediator.Send(command);

                _logger.LogInformation("Excel fayl uğurla yaradıldı: {FileName}", result.FileName);

                return File(
                    result.FileContent,
                    result.ContentType,
                    result.FileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "9-cu sinif zal köçürmə analizi Excel export zamanı xəta baş verdi");
                return StatusCode(500, new { message = "Excel export zamanı xəta baş verdi", error = ex.Message });
            }
        }


    }
}
