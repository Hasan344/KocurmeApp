// Application/Features/FileExports/Handlers/GetSupervisorCheatingAnalysisForExportQueryHandler.cs
using KocurmeApp.Application.Application.Features.FileExports.DTOs;
using KocurmeApp.Application.Application.Features.FileExports.Queries;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KocurmeApp.Application.Application.Features.FileExport.Queries.GetSupervisorCheatingAnalysisForExport
{
    public class GetSupervisorCheatingAnalysisForExportQueryHandler
        : IRequestHandler<GetSupervisorCheatingAnalysisForExportQuery, SupervisorCheatingAnalysisExportResult>
    {
        private readonly AppDbContext _context;
        private readonly ILogger<GetSupervisorCheatingAnalysisForExportQueryHandler> _logger;

        private static readonly Dictionary<int, decimal> AgeCategories = new()
        {
            { 1, 1.00m },
            { 2, 1.58m },
            { 3, 2.68m },
            { 4, 3.68m },
            { 0, 1.00m }
        };

        public GetSupervisorCheatingAnalysisForExportQueryHandler(
            AppDbContext context,
            ILogger<GetSupervisorCheatingAnalysisForExportQueryHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SupervisorCheatingAnalysisExportResult> Handle(
            GetSupervisorCheatingAnalysisForExportQuery request,
            CancellationToken cancellationToken)
        {
            // Step 1: DistinctRooms - Keep as IQueryable
            var distinctRoomsQuery = _context.Rooms
                .Where(r => r.ExamId == request.CheatingExamId)
                .Select(r => new
                {
                    r.B_KOD,
                    r.V_BINA,
                    r.ExamId,
                    r.Z_KOD,
                    r.GR_FL
                })
                .Distinct();

            // Step 2: SupervisorRooms - Join queryables, THEN materialize
            var supervisorRooms = await (
                from t3 in _context.ImtRehBinas
                join dr in distinctRoomsQuery  // Join with queryable, not list
                    on new { B_KOD = (short)t3.BKod!, V_BINA = t3.VBina! }
                    equals new { B_KOD = dr.B_KOD, V_BINA = dr.V_BINA }
                where t3.ExamId == request.SupervisorExamId
                select new
                {
                    i_r = t3.IR,
                    B_KOD = t3.BKod,
                    v_bina = t3.VBina,
                    exam_id = t3.ExamId,
                    dr.Z_KOD,
                    dr.GR_FL
                }
            ).Distinct().ToListAsync(cancellationToken);  // Now materialize


            // Step 3: RoomContingent - Zal və kontingent məlumatı
            var roomContingent =
 (
     from sr in supervisorRooms
     join c0 in _context.Contingents
         on sr.GR_FL equals c0.NUM_K.ToString()
         into contingentGroup
     from c in contingentGroup
         .Where(x => x.ExamId == (short)request.CheatingExamId)
         .DefaultIfEmpty()
     select new
     {
         sr.i_r,
         sr.B_KOD,
         sr.v_bina,
         sr.exam_id,
         sr.Z_KOD,
         YASH_KATEQ = c != null ? c.YASH_KATEQ : (int?)null
     }
 )
 .Distinct()
 .ToList();


            // Step 4: SupervisorStats - Nəzarətçi statistikası
            var supervisors = await (
                from t4 in _context.ImtRehs
                where t4.ExamId == request.SupervisorExamId
                select new
                {
                    t4.VNum,
                    FullName = t4.Adi + " " + t4.Soy + " " + t4.Baba
                }
            ).ToListAsync(cancellationToken);

            var supervisorStats = (
                from rc in roomContingent
                join sup in supervisors on rc.i_r equals sup.VNum
                group rc by new
                {
                    rc.i_r,
                    rc.B_KOD,
                    rc.v_bina,
                    rc.exam_id,
                    sup.FullName
                } into g
                select new
                {
                    g.Key.i_r,
                    g.Key.B_KOD,
                    V_BINA = g.Key.v_bina,
                    g.Key.exam_id,
                    g.Key.FullName,
                    ZAL_LIST = string.Join(", ", g.Select(x => x.Z_KOD).Distinct().OrderBy(z => z)),
                    ZalCount = g.Select(x => x.Z_KOD).Distinct().Count(),
                    emsal = Math.Round((decimal)g.Select(x => x.Z_KOD).Distinct().Count() / 6, 2)
                }
            ).ToList();

            // Step 5: ZalAgeMapping - Zal və yaş kateqoriyası
            var zalAgeMapping = roomContingent
                .Select(rc => new
                {
                    rc.i_r,
                    rc.Z_KOD,
                    rc.YASH_KATEQ
                })
                .Distinct()
                .ToList();

            // Step 6: fn_CheatingRoomStats funksiyasından məlumat
            var cheatingRoomStats = await _context.Set<CheatingRoomStatsResult>()
                .FromSqlInterpolated($"SELECT Zal, kolon5 FROM dbo.fn_CheatingRoomStats({request.CheatingExamId})")
                .ToListAsync(cancellationToken);

            // Step 7: SupervisorZals - Nəzarətçilərin hər bir zalı
            var supervisorZals = supervisorStats
                .SelectMany(ss => ss.ZAL_LIST.Split(',').Select(z => z.Trim()),
                    (ss, zal) => new
                    {
                        ss.i_r,
                        ss.B_KOD,
                        ss.V_BINA,
                        ss.exam_id,
                        ss.FullName,
                        ss.ZalCount,
                        ss.emsal,
                        Zal = zal
                    })
                .ToList();

            // Step 8: SupervisorKolon5Detail - Hər zal üçün kolon5 və YASH_KATEQ
            var supervisorKolon5Detail = (
    from sz in supervisorZals
    join zam0 in zalAgeMapping
        on new
        {
            i_r = (int?)sz.i_r,        // cast int → int? to match nullable
            Zal = sz.Zal.Trim()
        }
        equals new
        {
            i_r = zam0.i_r.HasValue ? (int?)zam0.i_r.Value : null, // short? → int?
            Zal = zam0.Z_KOD.ToString().Trim()
        }
        into zamGroup
    from zam in zamGroup.DefaultIfEmpty()

    join crs in cheatingRoomStats
        on int.Parse(sz.Zal) equals crs.Zal
        into crsGroup
    from crs in crsGroup.DefaultIfEmpty()

    select new
    {
        sz.i_r,
        sz.B_KOD,
        sz.V_BINA,
        sz.exam_id,
        sz.FullName,
        sz.Zal,
        YASH_KATEQ = zam?.YASH_KATEQ ?? 0,
        kolon5 = crs?.kolon5 ?? 0,
        sz.ZalCount,
        sz.emsal
    }
).ToList();




            // Step 9: SupervisorNormalized - Normalize edilmiş dəyərlər
            var supervisorNormalized = (
                from skd in supervisorKolon5Detail
                let ageCoeff = AgeCategories.GetValueOrDefault(skd.YASH_KATEQ, 1.00m)
                select new
                {
                    skd.i_r,
                    skd.B_KOD,
                    skd.V_BINA,
                    skd.exam_id,
                    skd.FullName,
                    skd.Zal,
                    normalized_value = skd.kolon5 / ageCoeff,
                    skd.ZalCount,
                    skd.emsal
                }
            ).ToList();

            // Step 10: SupervisorFinal - Final nəticə
            var result = (
    from sn in supervisorNormalized
    join ss in supervisorStats
        on new
        {
            i_r = (int)sn.i_r,
            B_KOD = (int)sn.B_KOD,
            V_BINA = sn.V_BINA,
            exam_id = (int)sn.exam_id
        }
        equals new
        {
            i_r = (int)ss.i_r,
            B_KOD = (int)ss.B_KOD,
            V_BINA = ss.V_BINA,
            exam_id = (int)ss.exam_id
        }
    group new { sn, ss } by new
    {
        i_r = (int)sn.i_r,
        B_KOD = (int)sn.B_KOD,
        V_BINA = sn.V_BINA,
        exam_id = (int)sn.exam_id,
        sn.FullName,
        ss.ZAL_LIST
    } into g
    let avgNormalized = g.Average(x => x.sn.normalized_value)
    let maxEmsal = g.Max(x => x.sn.emsal)
    let finalEmsal = maxEmsal < 1 ? 1.00m : maxEmsal
    select new SupervisorCheatingAnalysisDTO
    {
        IRehber = g.Key.i_r,
        BKod = g.Key.B_KOD,
        VBina = g.Key.V_BINA,
        ExamId = g.Key.exam_id,
        TamAd = g.Key.FullName,
        ZalSiyahisi = g.Key.ZAL_LIST,
        RehberinZallarindakiFaizlerinOrtaQiymeti = Math.Round(avgNormalized, 2),
        RehberinZallarindakiFaizlerinOrtaQiymetiEmsal = Math.Round(avgNormalized / finalEmsal, 2)
    }
)
.OrderBy(x => x.RehberinZallarindakiFaizlerinOrtaQiymeti)
.ToList();


            return new SupervisorCheatingAnalysisExportResult
            {
                AnalysisData = result
            };
        }
    }
}