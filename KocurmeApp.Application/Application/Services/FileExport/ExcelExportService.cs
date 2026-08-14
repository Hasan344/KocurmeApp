
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
    CheatingAnalysisExportResult data,
    string sheetName = "Cheating Analysis")
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // ===== ƏSAS CƏDVƏL =====
            var headers = new Dictionary<int, string>
    {
        { 1, "Zal" },
        { 2, "Kontingentin kodu"},
        { 3, "Kontingent yaşı" },
        { 4, "Zalda köçürən abituriyentlərin sayı" },
        { 5, "Köçürülən fənlərin ümumi sayı" },
        { 6, "Zalda olan abituriyentlərin sayı" },
        { 7, "Zalda köçürmə faizi 1" },
        { 8, "Zalda köçürmə faizi 2" },
        { 9, "Kolon 3" },
        { 10,"Kolon 4" },
        { 11,"Kolon 5" },
        { 12,"Kolon5 / Əmsal" },
        { 13,"Zalda köçürmənin dərəcəsi" }
    };

            // HEADER
            foreach (var header in headers)
            {
                var cell = worksheet.Cells[1, header.Key];
                cell.Value = header.Value;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.WrapText = true;
            }

            worksheet.Row(1).Height = 42;

            // DATA
            int row = 2;
            foreach (var item in data.AnalysisData)
            {
                worksheet.Cells[row, 1].Value = item.Zal;
                worksheet.Cells[row, 2].Value = item.KontingentKodu;
                worksheet.Cells[row, 3].Value = item.KontingentYasi;
                worksheet.Cells[row, 4].Value = item.ZaldaKocurenAbituriyentlerinSayi;
                worksheet.Cells[row, 5].Value = item.KocurulenFenlerinUmumiSayi;
                worksheet.Cells[row, 6].Value = item.ZaldaOlanAbituriyentlerinSayi;
                worksheet.Cells[row, 7].Value = item.ZaldaKocurmeFaizi1;
                worksheet.Cells[row, 8].Value = item.ZaldaKocurmeFaizi2;
                worksheet.Cells[row, 9].Value = item.Kolon3;
                worksheet.Cells[row, 10].Value = item.Kolon4;
                worksheet.Cells[row, 11].Value = item.Kolon5;
                worksheet.Cells[row, 12].Value = item.Kolon5BolunmusEmsal;
                worksheet.Cells[row, 13].Value = item.ZaldaKocurmeninDerecesi;

                // Rəng təyini - statusə görə
                Color rowColor = item.ZaldaKocurmeninDerecesi switch
                {
                    "Zəif" => Color.FromArgb(255, 255, 153),    // Açıq sarı
                    "Orta" => Color.FromArgb(255, 204, 153),    // Açıq narıncı
                    "Ağır" => Color.FromArgb(255, 153, 102),    // Tünd narıncı
                    _ => Color.White
                };

                for (int col = 1; col <= 13; col++)
                {
                    var cell = worksheet.Cells[row, col];

                    // Rəng tətbiqi
                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    cell.Style.Fill.BackgroundColor.SetColor(rowColor);

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    if (col >= 7 && col <= 12 && cell.Value != null)
                    {
                        cell.Style.Numberformat.Format = "0.00";
                    }
                }

                row++;
            }

            // COLUMN WIDTHS
            for (int col = 1; col <= 13; col++)
            {
                worksheet.Column(col).Width = col switch
                {
                    1 => 6,
                    2 => 6,
                    3 => 15,
                    4 => 15,
                    5 => 15,
                    6 => 15,
                    7 => 15,
                    8 => 8,
                    9 => 8,
                    10 => 8,
                    11 => 12,
                    12 => 14,
                    13 => 14,
                    _ => 15
                };
            }
            worksheet.Cells[1, 1, row - 1, 13].AutoFilter = true;


            worksheet.View.FreezePanes(2, 1);

            // ===== STATİSTİKA CƏDVƏLİ =====
            int statsStartRow = row + 3; // 3 sətir boşluq buraxaq

            // Statistika başlığı
            var statsHeaderCell = worksheet.Cells[statsStartRow, 1, statsStartRow, 2];
            statsHeaderCell.Merge = true;
            statsHeaderCell.Value = "Köçürmə Faizinə Görə Statistika";
            statsHeaderCell.Style.Font.Bold = true;
            statsHeaderCell.Style.Font.Size = 12;
            statsHeaderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            statsHeaderCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            statsHeaderCell.Style.Font.Color.SetColor(Color.White);
            statsHeaderCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            statsHeaderCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            statsStartRow++;

            // Statistika sütun başlıqları
            var statsCol1 = worksheet.Cells[statsStartRow, 1];
            statsCol1.Value = "Köçürmə faizi";
            statsCol1.Style.Font.Bold = true;
            statsCol1.Style.Fill.PatternType = ExcelFillStyle.Solid;
            statsCol1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            statsCol1.Style.Font.Color.SetColor(Color.White);
            statsCol1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            statsCol1.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            var statsCol2 = worksheet.Cells[statsStartRow, 2];
            statsCol2.Value = "Köçürmə olan zalların sayı";
            statsCol2.Style.Font.Bold = true;
            statsCol2.Style.Fill.PatternType = ExcelFillStyle.Solid;
            statsCol2.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            statsCol2.Style.Font.Color.SetColor(Color.White);
            statsCol2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            statsCol2.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            statsStartRow++;

            // Statistika dataları
            foreach (var stat in data.Statistics)
            {
                worksheet.Cells[statsStartRow, 1].Value = stat.KocurmeFaiziAraligi;
                worksheet.Cells[statsStartRow, 2].Value = stat.KocurmeOlanZallarinSayi;

                for (int col = 1; col <= 2; col++)
                {
                    var cell = worksheet.Cells[statsStartRow, col];

                    if (statsStartRow % 2 == 0)
                    {
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                    }

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                statsStartRow++;
            }

            // Statistika sütun genişlikləri
            worksheet.Column(1).Width = 20;
            worksheet.Column(2).Width = 30;

            return await package.GetAsByteArrayAsync();
        }
        // ExcelExportService-ə əlavə edin:
        public async Task<byte[]> ExportSupervisorCheatingAnalysisToExcelAsync(
            SupervisorCheatingAnalysisExportResult data,
            string sheetName = "Supervisor Analysis")
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            var headers = new Dictionary<int, string>
    {
        { 1, "№" },
        { 2, "B Kod" },
        { 3, "Bina" },
        { 4, "İmtahan ID" },
        { 5, "Tam Ad" },
        { 6, "Zal Siyahısı" },
        { 7, "Rəhbərin zallarındakı faizlərin orta qiyməti" },
        { 8, "Rəhbərin zallarındakı faizlərin orta qiyməti / əmsal" }
    };

            // HEADER
            foreach (var header in headers)
            {
                var cell = worksheet.Cells[1, header.Key];
                cell.Value = header.Value;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.WrapText = true;
            }

            worksheet.Row(1).Height = 42;

            // DATA
            int row = 2;
            foreach (var item in data.AnalysisData)
            {
                worksheet.Cells[row, 1].Value = item.IRehber;
                worksheet.Cells[row, 2].Value = item.BKod;
                worksheet.Cells[row, 3].Value = item.VBina;
                worksheet.Cells[row, 4].Value = item.ExamId;
                worksheet.Cells[row, 5].Value = item.TamAd;
                worksheet.Cells[row, 6].Value = item.ZalSiyahisi;
                worksheet.Cells[row, 7].Value = item.RehberinZallarindakiFaizlerinOrtaQiymeti;
                worksheet.Cells[row, 8].Value = item.RehberinZallarindakiFaizlerinOrtaQiymetiEmsal;

                for (int col = 1; col <= 8; col++)
                {
                    var cell = worksheet.Cells[row, col];

                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    if (row % 2 == 0)
                    {
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                    }
                    else
                    {
                        cell.Style.Fill.BackgroundColor.SetColor(Color.White);
                    }

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    // Decimal formatı - sütun 7 və 8 üçün
                    if ((col == 7 || col == 8) && cell.Value != null)
                    {
                        cell.Style.Numberformat.Format = "0.00";
                    }
                }

                row++;
            }

            // COLUMN WIDTHS
            worksheet.Column(1).Width = 8;
            worksheet.Column(2).Width = 10;
            worksheet.Column(3).Width = 15;
            worksheet.Column(4).Width = 12;
            worksheet.Column(5).Width = 30;
            worksheet.Column(6).Width = 40;
            worksheet.Column(7).Width = 25;
            worksheet.Column(8).Width = 30;

            // AutoFilter
            worksheet.Cells[1, 1, row - 1, 8].AutoFilter = true;

            worksheet.View.FreezePanes(2, 1);

            return await package.GetAsByteArrayAsync();
        }

        public async Task<byte[]> ExportNinthGradeCheatingAnalysisToExcelAsync(
            NinthGradeCheatingAnalysisExportResult data,
            string sheetName = "9-cu sinif Zal Köçürmə Analizi")
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

            const int lastCol = 9;

            // HEADER
            foreach (var header in headers)
            {
                var cell = worksheet.Cells[1, header.Key];
                cell.Value = header.Value;
                cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 11;
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
                cell.Style.Font.Color.SetColor(Color.White);
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                cell.Style.WrapText = true;
            }

            worksheet.Row(1).Height = 42;

            // DATA
            int row = 2;
            foreach (var item in data.AnalysisData)
            {
                // "Orta qiymət" sətri fərqli rənglənir.
                bool isSummaryRow = item.IsSummary;

                worksheet.Cells[row, 1].Value = item.Zal;
                worksheet.Cells[row, 2].Value = item.ZaldaKocurenAbituriyentlerinSayi;
                worksheet.Cells[row, 3].Value = item.KocurulenFenlerinUmumiSayi;
                worksheet.Cells[row, 4].Value = item.ZaldaOlanAbituriyentlerinSayi;
                worksheet.Cells[row, 5].Value = item.ZaldaKocurmeFaizi1;
                worksheet.Cells[row, 6].Value = item.ZaldaKocurmeFaizi2;
                worksheet.Cells[row, 7].Value = item.Kolon3;
                worksheet.Cells[row, 8].Value = item.Kolon4;
                worksheet.Cells[row, 9].Value = item.Kolon5;

                for (int col = 1; col <= lastCol; col++)
                {
                    var cell = worksheet.Cells[row, col];

                    cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    if (isSummaryRow)
                    {
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 217, 217));
                        cell.Style.Font.Bold = true;
                    }
                    else if (row % 2 == 0)
                    {
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                    }
                    else
                    {
                        cell.Style.Fill.BackgroundColor.SetColor(Color.White);
                    }

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                    // Faiz və kolon sütunları üçün ondalıq format
                    if (col >= 5 && col <= 9 && cell.Value != null)
                    {
                        cell.Style.Numberformat.Format = "0.00";
                    }
                }

                row++;
            }

            // COLUMN WIDTHS
            for (int col = 1; col <= lastCol; col++)
            {
                worksheet.Column(col).Width = col switch
                {
                    1 => 12,
                    2 => 18,
                    3 => 18,
                    4 => 18,
                    5 => 14,
                    6 => 14,
                    7 => 10,
                    8 => 10,
                    9 => 10,
                    _ => 15
                };
            }

            worksheet.Cells[1, 1, row - 1, lastCol].AutoFilter = true;
            worksheet.View.FreezePanes(2, 1);

            // ===== KÖÇÜRMƏ FAİZİ PAYLANMASI (STATİSTİKA CƏDVƏLİ) =====
            int statsStartRow = row + 3; // 3 sətir boşluq

            // Başlıq
            var statsHeaderCell = worksheet.Cells[statsStartRow, 1, statsStartRow, 2];
            statsHeaderCell.Merge = true;
            statsHeaderCell.Value = "Köçürmə Faizinə Görə Statistika";
            statsHeaderCell.Style.Font.Bold = true;
            statsHeaderCell.Style.Font.Size = 12;
            statsHeaderCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
            statsHeaderCell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            statsHeaderCell.Style.Font.Color.SetColor(Color.White);
            statsHeaderCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            statsHeaderCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            statsStartRow++;

            // Sütun başlıqları
            var statsCol1 = worksheet.Cells[statsStartRow, 1];
            statsCol1.Value = "Köçürmə faizi";
            statsCol1.Style.Font.Bold = true;
            statsCol1.Style.Fill.PatternType = ExcelFillStyle.Solid;
            statsCol1.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            statsCol1.Style.Font.Color.SetColor(Color.White);
            statsCol1.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            statsCol1.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            var statsCol2 = worksheet.Cells[statsStartRow, 2];
            statsCol2.Value = "Köçürmə olan zalların sayı";
            statsCol2.Style.Font.Bold = true;
            statsCol2.Style.Fill.PatternType = ExcelFillStyle.Solid;
            statsCol2.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            statsCol2.Style.Font.Color.SetColor(Color.White);
            statsCol2.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            statsCol2.Style.Border.BorderAround(ExcelBorderStyle.Thin);

            statsStartRow++;

            // Data
            foreach (var stat in data.Statistics)
            {
                worksheet.Cells[statsStartRow, 1].Value = stat.KocurmeFaiziAraligi;
                worksheet.Cells[statsStartRow, 2].Value = stat.KocurmeOlanZallarinSayi;

                for (int col = 1; col <= 2; col++)
                {
                    var cell = worksheet.Cells[statsStartRow, col];

                    if (statsStartRow % 2 == 0)
                    {
                        cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        cell.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
                    }

                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                statsStartRow++;
            }

            return await package.GetAsByteArrayAsync();
        }

    }
}