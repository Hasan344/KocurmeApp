// Application/Features/FileExport/Queries/GetCheatingAnalysisForExport/GetCheatingAnalysisForExportQueryHandler.cs
using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using KocurmeApp.Application.Interfaces;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Application.Features.FileExport.Queries.GetCheatingAnalysisForExport
{
    public class GetCheatingAnalysisForExportQueryHandler
        : IRequestHandler<FileExports.Queries.GetCheatingAnalysisForExportQuery, List<CheatingAnalysisExportDTO>>
    {
        private readonly AppDbContext _context;

        public GetCheatingAnalysisForExportQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CheatingAnalysisExportDTO>> Handle(
            FileExports.Queries.GetCheatingAnalysisForExportQuery request,
            CancellationToken cancellationToken)
        {
            // T1: CheatingStudents və Rooms join - ehtimal hesablama
            var t1 = await (from cs in _context.CheatingStudents
                            join r in _context.Rooms on cs.ZAL1 equals r.Z_KOD
                            where cs.EYNI_Y >= request.MinEyniY
                            select new
                            {
                                cs.ZAL1,
                                cs.IS_N1,
                                cs.FENN,
                                cs.EYNI_Y,
                                cs.EYNI_B,
                                cs.EYNI_D,
                                Ehtimal = Math.Round(
                                    ((decimal)(cs.EYNI_Y + cs.EYNI_B) / (30 - cs.EYNI_D)) * 100,
                                    2
                                ),
                                Oda_Student_Sayisi = r.KOL_ABT
                            })
                           .ToListAsync(cancellationToken);

            if (!t1.Any())
                return new List<CheatingAnalysisExportDTO>();

            // T2: Ehtimal >= MinEhtimal filtri
            var t2 = t1.Where(x => x.Ehtimal >= request.MinEhtimal).ToList();

            if (!t2.Any())
                return new List<CheatingAnalysisExportDTO>();

            // Summary: ZAL1-ə görə qruplaşdırma
            var summary = t2.GroupBy(x => x.ZAL1)
                           .Select(g => new
                           {
                               ZAL1 = g.Key,
                               Kopya_Ceken_Student_Sayisi = g.Select(x => x.IS_N1).Distinct().Count(),
                               Kopya_Cekilen_Fenn_Sayisi = g.Select(x => x.FENN).Distinct().Count(),
                               Oda_Student_Sayisi = g.Max(x => x.Oda_Student_Sayisi)
                           })
                           .ToList();

            // T3: Faiz hesablamaları
            var t3 = summary.Select(s => new
            {
                Zal = s.ZAL1,
                Kopya_Ceken_Student_Sayisi = s.Kopya_Ceken_Student_Sayisi,
                Kopya_Cekilen_Fenn_Sayisi = s.Kopya_Cekilen_Fenn_Sayisi,
                Oda_Student_Sayisi = s.Oda_Student_Sayisi,
                ZaldaKocurmeFaizi1 = s.Oda_Student_Sayisi > 0
                    ? Math.Round(((decimal)s.Kopya_Cekilen_Fenn_Sayisi / (s.Oda_Student_Sayisi * 3)) * 100, 2)
                    : 0m,
                ZaldaKocurmeFaizi2 = s.Oda_Student_Sayisi > 0
                    ? Math.Round(((decimal)s.Kopya_Ceken_Student_Sayisi / s.Oda_Student_Sayisi) * 100, 2)
                    : 0m
            }).ToList();

            // Ortalama dəyərlər
            var avgFaiz1 = t3.Any() ? t3.Average(x => x.ZaldaKocurmeFaizi1) : 0m;
            var avgFaiz2 = t3.Any() ? t3.Average(x => x.ZaldaKocurmeFaizi2) : 0m;

            // T4: Kolon3 və Kolon4 hesablamaları
            var t4 = t3.Select(x => new
            {
                x.Zal,
                x.Kopya_Ceken_Student_Sayisi,
                x.Kopya_Cekilen_Fenn_Sayisi,
                x.Oda_Student_Sayisi,
                x.ZaldaKocurmeFaizi1,
                x.ZaldaKocurmeFaizi2,
                Kolon3 = avgFaiz1 > 0 ? Math.Round(x.ZaldaKocurmeFaizi1 / avgFaiz1, 2) : 0m,
                Kolon4 = avgFaiz2 > 0 ? Math.Round(x.ZaldaKocurmeFaizi2 / avgFaiz2, 2) : 0m
            }).ToList();

            // T5: Kolon5 hesablaması və DTO'ya map etmə
            var t5 = t4.Select(x => new CheatingAnalysisExportDTO
            {
                Zal = x.Zal.ToString(),
                ZaldaKocurenAbituriyentlerinSayi = x.Kopya_Ceken_Student_Sayisi,
                KocurulenFenlerinUmumiSayi = x.Kopya_Cekilen_Fenn_Sayisi,
                ZaldaOlanAbituriyentlerinSayi = x.Oda_Student_Sayisi,
                ZaldaKocurmeFaizi1 = x.ZaldaKocurmeFaizi1,
                ZaldaKocurmeFaizi2 = x.ZaldaKocurmeFaizi2,
                Kolon3 = x.Kolon3,
                Kolon4 = x.Kolon4,
                Kolon5 = Math.Round((x.Kolon3 + x.Kolon4) / 2, 2)
            }).ToList();

            // Ortalama sətri əlavə et (UNION ALL)
            var avgRow = new CheatingAnalysisExportDTO
            {
                Zal = "1",
                ZaldaKocurenAbituriyentlerinSayi = null,
                KocurulenFenlerinUmumiSayi = null,
                ZaldaOlanAbituriyentlerinSayi = null,
                ZaldaKocurmeFaizi1 = Math.Round(avgFaiz1, 2),
                ZaldaKocurmeFaizi2 = Math.Round(avgFaiz2, 2),
                Kolon3 = null,
                Kolon4 = null,
                Kolon5 = null
            };

            t5.Add(avgRow);

            // ORDER BY Zal DESC
            return t5.OrderByDescending(x => x.Zal).ToList();
        }
    }
}