using DotNetDBF;
using KocurmeApp.Application.Features.Rooms.Commands;
using KocurmeApp.Domain.Entities;
using KocurmeApp.Infrastructure;
using MediatR;

namespace KocurmeApp.Application.Features.Rooms.Handlers
{
    public class ImportRoomsCommandHandler : IRequestHandler<ImportRoomsCommand, bool>
    {
        private readonly AppDbContext _context;

        public ImportRoomsCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(ImportRoomsCommand request, CancellationToken cancellationToken)
        {
            var rooms = new List<Room>();

            using (var stream = request.File.OpenReadStream())
            using (var reader = new DBFReader(stream))
            {
                object[] record;
                while ((record = reader.NextRecord()) != null)
                {
                    try
                    {
                        var room = new Room
                        {
                            ExamId = request.ExamId,

                            Z_KOD = SafeToInt16(record[0]),
                            XAR_DIL = SafeToByte(record[1]),
                            NUMMETN = SafeToNullableByte(record[2]),
                            B_KOD = SafeToInt16(record[3]),
                            V_BINA = record[4]?.ToString() ?? string.Empty,
                            MERTEBE = SafeToByte(record[5]),
                            KOL_SIRA = SafeToByte(record[6]),
                            KOL_YER = SafeToByte(record[7]),
                            KOL_SIRA0 = record[8]?.ToString(),
                            KOL_YER0 = record[9]?.ToString(),
                            TUTUMU = SafeToByte(record[10]),
                            TUTUMU0 = record[11]?.ToString(),
                            GR_FL = record[12]?.ToString() ?? string.Empty,
                            KOL_ABT = SafeToByte(record[13]),
                            KOL_NAZ = SafeToByte(record[14]),
                            IMT_YERI = SafeToByte(record[15]),
                            DIL = SafeToByte(record[16]),
                            YASHKATEG = record[17]?.ToString(),
                            AADI = record[18]?.ToString() ?? string.Empty,
                            WAADI = record[19]?.ToString(),
                            MODUL = SafeToByte(record[20]),
                            OK = record[21]?.ToString(),
                            TEKTEK = record[22]?.ToString(),
                            MEKT_KOD = record[23]?.ToString(),
                            INDMEKTEB = record[24]?.ToString()
                        };

                        rooms.Add(room);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing record: {ex.Message}");
                    }
                }
            }

            await _context.Rooms.AddRangeAsync(rooms, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        private byte SafeToByte(object value)
        {
            if (value == null) return 0;
            if (byte.TryParse(value.ToString(), out var result))
                return result;
            if (decimal.TryParse(value.ToString(), out var dec))
                return (byte)Math.Clamp(dec, 0, 255);
            return 0;
        }

        private byte? SafeToNullableByte(object value)
        {
            if (value == null) return null;
            if (byte.TryParse(value.ToString(), out var result))
                return result;
            return null;
        }

        private short SafeToInt16(object value)
        {
            if (value == null) return 0;
            if (short.TryParse(value.ToString(), out var result))
                return result;
            if (decimal.TryParse(value.ToString(), out var dec))
                return (short)Math.Clamp(dec, short.MinValue, short.MaxValue);
            return 0;
        }
    }
}
