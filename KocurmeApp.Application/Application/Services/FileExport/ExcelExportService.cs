
using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using KocurmeApp.Application.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace KocurmeApp.Application.Application.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public ExcelExportService()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task<byte[]> ExportCheatingAnalysisToExcelAsync(
            List<CheatingAnalysisExportDTO> data,
            string sheetName = "Sheet1")
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            var headers = new Dictionary<int, string>
            {
                { 1, "Zal" },
                { 2, "Zalda köçürən abituriyentlərin sayı" },
                { 3, "Köçürülən fənlərin ümumi sayı" },
                { 4, "Zalda olan abituriyentlərin sayı" },
                { 5, "Zalda köçürmə faizi 1" },
                { 6, "Zalda köçürmə faizi 2" },
                { 7, "Kolon 3" },
                { 8, "Kolon 4" },
                { 9, "Kolon 5" }
            };

            foreach (var header in headers)
            {
                var cell = worksheet.Cells[1, header.Key];
                cell.Value = header.Value;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189)); // Mavi
                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.WrapText = true;
            }

            worksheet.Row(1).Height = 40;

            int row = 2;
            foreach (var item in data)
            {
                worksheet.Cells[row, 1].Value = item.Zal;
                worksheet.Cells[row, 2].Value = item.ZaldaKocurenAbituriyentlerinSayi;
                worksheet.Cells[row, 3].Value = item.KocurulenFenlerinUmumiSayi;
                worksheet.Cells[row, 4].Value = item.ZaldaOlanAbituriyentlerinSayi;
                worksheet.Cells[row, 5].Value = item.ZaldaKocurmeFaizi1;
                worksheet.Cells[row, 6].Value = item.ZaldaKocurmeFaizi2;
                worksheet.Cells[row, 7].Value = item.Kolon3;
                worksheet.Cells[row, 8].Value = item.Kolon4;
                worksheet.Cells[row, 9].Value = item.Kolon5;

                bool isAverageRow = item.Zal == "1";

                for (int col = 1; col <= 9; col++)
                {
                    var cell = worksheet.Cells[row, col];

                    if (isAverageRow)
                    {
                        cell.Style.Font.Bold = true;
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 255, 153)); // Açıq sarı
                        cell.Style.Font.Color.SetColor(Color.Black);
                    }
                    else
                    {
                        if (row % 2 == 0)
                        {
                            cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242)); // Açıq boz
                        }
                    }

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    if (col >= 5 && col <= 9 && cell.Value != null)
                    {
                        cell.Style.Numberformat.Format = "0.00";
                    }
                }

                row++;
            }

            for (int col = 1; col <= 9; col++)
            {
                worksheet.Column(col).Width = col switch
                {
                    1 => 10,  // Zal
                    2 => 25,  // Zalda köçürən...
                    3 => 25,  // Köçürülən...
                    4 => 25,  // Zalda olan...
                    5 => 20,  // Faiz 1
                    6 => 20,  // Faiz 2
                    7 => 12,  // Kolon 3
                    8 => 12,  // Kolon 4
                    9 => 12,  // Kolon 5
                    _ => 15
                };
            }

            worksheet.View.FreezePanes(2, 1);

            return await package.GetAsByteArrayAsync();
        }


        public async Task<byte[]> ExportToExcelAsync<T>(
            IEnumerable<T> data,
            string sheetName = "Sheet1",
            Dictionary<string, string>? columnHeaders = null)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            var properties = typeof(T).GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                var cell = worksheet.Cells[1, i + 1];
                cell.Value = columnHeaders?.GetValueOrDefault(properties[i].Name) ?? properties[i].Name;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }

            int row = 2;
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    var cell = worksheet.Cells[row, i + 1];
                    cell.Value = properties[i].GetValue(item);
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }
                row++;
            }

            // Auto-fit kolonlar
            worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

            return await package.GetAsByteArrayAsync();
        }
    }
}