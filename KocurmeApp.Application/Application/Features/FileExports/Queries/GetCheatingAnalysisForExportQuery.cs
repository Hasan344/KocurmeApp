using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application.Features.FileExports.Queries
{
    public class GetCheatingAnalysisForExportQuery : IRequest<List<CheatingAnalysisExportDTO>>
    {
        public int MinEyniY { get; set; } = 5;
        public decimal MinEhtimal { get; set; } = 60;
    }
}
