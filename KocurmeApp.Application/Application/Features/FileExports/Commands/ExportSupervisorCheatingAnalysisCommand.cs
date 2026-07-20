using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.FileExports.Commands
{
    public class ExportSupervisorCheatingAnalysisCommand : IRequest<ExportResultDTO>
    {
        public int CheatingExamId { get; set; } // 5
        public int SupervisorExamId { get; set; } // 456
        public string SheetName { get; set; } = "Nəzarətçi Analizi";
    }
}
