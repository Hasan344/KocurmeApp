using System.Globalization;
using System.Text;
using DotNetDBF;
using KocurmeApp.Application.Features.Rooms.Commands;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KocurmeApp.Application.Features.Rooms.Handlers
{
    public class ImportRoomsCommandHandler : IRequestHandler<ImportRoomsCommand, bool>
    {
        private readonly AppDbContext _context;

        // ZAL.DBF faylı DOS-Kiril (code page 866) kodlamasındadır.
        // DotNetDBF standart olaraq UTF-8 istifadə edir, bu da otaq adlarını (AADI/WAADI) korlayır.
        private static readonly Encoding DbfEncoding = Encoding.GetEncoding(866);

        public ImportRoomsCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ImportRoomsCommand request, CancellationToken cancellationToken)
        {
            // 1) Fayl yoxlaması
            if (request.File == null || request.File.Length == 0)
                throw new InvalidOperationException("İdxal üçün DBF faylı təqdim olunmayıb və ya boşdur.");

            // 2) İmtahanın mövcudluğunu yoxla (əsas 500 səbəbi: mövcud olmayan ExamId → FK pozulması).
            //    Digər idxal handler-lərində olduğu kimi burada da yoxlama əlavə edilir.
            var examExists = await _context.Exams
                .AsNoTracking()
                .AnyAsync(e => e.Id == request.ExamId, cancellationToken);

            if (!examExists)
                throw new InvalidOperationException(
                    $"Göstərilən imtahan tapılmadı (ExamId={request.ExamId}). Əvvəlcə imtahan seçin/yaradın.");

            var rooms = new List<Room>();
            var failed = 0;

            using (var stream = request.File.OpenReadStream())
            using (var reader = new DBFReader(stream) { CharEncoding = DbfEncoding })
            {
                while (true)
                {
                    object[] record;
                    try
                    {
                        record = reader.NextRecord();
                    }
                    catch (Exception ex)
                    {
                        // Bir korlanmış sətir bütün idxalı dayandırmasın.
                        failed++;
                        Console.WriteLine($"DBF sətri oxunmadı: {ex.Message}");
                        continue;
                    }

                    if (record == null)
                        break;

                    try
                    {
                        var room = new Room
                        {
                            ExamId = request.ExamId,

                            Z_KOD = SafeToInt16(record[0]),
                            XAR_DIL = SafeToByte(record[1]),
                            NUMMETN = SafeToNullableByte(record[2]),
                            B_KOD = SafeToInt16(record[3]),
                            V_BINA = record[4]?.ToString()?.Trim() ?? string.Empty,
                            MERTEBE = SafeToByte(record[5]),
                            KOL_SIRA = SafeToByte(record[6]),
                            KOL_YER = SafeToByte(record[7]),
                            KOL_SIRA0 = ToNullableString(record[8]),
                            KOL_YER0 = ToNullableString(record[9]),
                            TUTUMU = SafeToByte(record[10]),
                            TUTUMU0 = ToNullableString(record[11]),
                            GR_FL = record[12]?.ToString() ?? string.Empty,
                            KOL_ABT = SafeToByte(record[13]),
                            KOL_NAZ = SafeToByte(record[14]),
                            IMT_YERI = SafeToByte(record[15]),
                            DIL = SafeToByte(record[16]),
                            YASHKATEG = ToNullableString(record[17]),
                            AADI = record[18]?.ToString()?.Trim() ?? string.Empty,
                            WAADI = ToNullableString(record[19]),
                            MODUL = SafeToByte(record[20]),
                            OK = ToNullableString(record[21]),
                            TEKTEK = ToNullableString(record[22]),
                            MEKT_KOD = ToNullableString(record[23]),
                            INDMEKTEB = ToNullableString(record[24])
                        };

                        rooms.Add(room);
                    }
                    catch (Exception ex)
                    {
                        failed++;
                        Console.WriteLine($"Sətir emal olunmadı: {ex.Message}");
                    }
                }
            }

            if (rooms.Count == 0)
                throw new InvalidOperationException(
                    "Fayldan heç bir otaq oxunmadı. Fayl formatını yoxlayın.");

            await _context.Rooms.AddRangeAsync(rooms, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            Console.WriteLine($"İdxal tamamlandı: {rooms.Count} otaq əlavə olundu, {failed} sətir buraxıldı.");
            return true;
        }

        private static string? ToNullableString(object value)
        {
            if (value == null || value is DBNull) return null;
            var s = value.ToString();
            return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        }

        private static byte SafeToByte(object value)
        {
            if (value == null || value is DBNull) return 0;
            var s = value.ToString();
            if (byte.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var dec))
                return (byte)Math.Clamp(dec, 0, 255);
            return 0;
        }

        private static byte? SafeToNullableByte(object value)
        {
            if (value == null || value is DBNull) return null;
            var s = value.ToString();
            if (byte.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var dec))
                return (byte)Math.Clamp(dec, 0, 255);
            return null;
        }

        private static short SafeToInt16(object value)
        {
            if (value == null || value is DBNull) return 0;
            var s = value.ToString();
            if (short.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;
            if (decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var dec))
                return (short)Math.Clamp(dec, short.MinValue, short.MaxValue);
            return 0;
        }
    }
}