using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KocurmeApp.Application.Application
{
    public interface IFileExportService
    {
        Task<byte[]> ExportToExcelAsync<T>(IEnumerable<T> data, string sheetName = "Sheet1");
    }
}
