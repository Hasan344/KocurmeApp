
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DbfDataReader;
using KocurmeApp.Domain.Entities;

namespace KocurmeApp.Infrastructure.Services.FileImport
{
    public class DbfImportService
    {

        public async Task<List<CheatingStudent>> ImportCheatingStudentsAsync(Stream fileStream)
        {
            var students = new List<CheatingStudent>();

            string tempPath = Path.GetTempFileName();

            using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Position = 0;
                await fileStream.CopyToAsync(fs);
            }

            var options = new DbfDataReaderOptions
            {
                Encoding = Encoding.GetEncoding(1254),
                SkipDeletedRecords = true
            };

            using (var dbfReader = new DbfDataReader.DbfDataReader(tempPath, options))
            {
                while (dbfReader.Read())
                {
                    var student = new CheatingStudent
                    {
                        IMT_GUN = GetByte(dbfReader, "IMT_GUN"),
                        V_BINA = GetString(dbfReader, "V_BINA"),
                        IS_N1 = GetInt(dbfReader, "IS_N1"),
                        BINA = GetShort(dbfReader, "BINA"),
                        ZAL1 = GetShort(dbfReader, "ZAL1"),
                        FENN = GetByte(dbfReader, "FENN"),
                        FNADI = GetString(dbfReader, "FNADI"),
                        IS_N2 = GetInt(dbfReader, "IS_N2"),
                        ZAL2 = GetShort(dbfReader, "ZAL2"),
                        EYNI_D = GetByte(dbfReader, "EYNI_D"),
                        EYNI_Y = GetByte(dbfReader, "EYNI_Y"),
                        EYNI_B = GetByte(dbfReader, "EYNI_B"),
                        Y_OXSHAR = GetDecimal(dbfReader, "Y_OXSHAR"),
                        T_OXSHAR = GetDecimal(dbfReader, "T_OXSHAR"),
                        BAL1 = GetDecimal(dbfReader, "BAL1"),
                        BAL2 = GetDecimal(dbfReader, "BAL2")
                    };

                    students.Add(student);
                }
            }

            File.Delete(tempPath);

            return students;
        }
        public async Task<List<Contingent>> ImportContingentsAsync(Stream fileStream)
        {
            var contingents = new List<Contingent>();
            string tempPath = Path.GetTempFileName();

            using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Position = 0;
                await fileStream.CopyToAsync(fs);
            }

            var options = new DbfDataReaderOptions
            {
                Encoding = Encoding.GetEncoding(1254),
                SkipDeletedRecords = true
            };

            using (var dbfReader = new DbfDataReader.DbfDataReader(tempPath, options))
            {
                while (dbfReader.Read())
                {
                    var contingent = new Contingent
                    {
                        IMT_GUN = GetNullableByte(dbfReader, "IMT_GUN"),
                        IMT_YERI = GetNullableByte(dbfReader, "IMT_YERI"),
                        NUM_K = GetNullableByte(dbfReader, "NUM_K"),
                        YASH_KATEQ = GetNullableByte(dbfReader, "YASH_KATEQ"),
                        IZAHI = GetString(dbfReader, "IZAHI"),
                        SEC = GetNullableByte(dbfReader, "SEC"),
                        TIP_OTUR = GetNullableByte(dbfReader, "TIP_OTUR"),
                        SAYI = GetNullableShort(dbfReader, "SAYI"),
                        SAYI0 = GetString(dbfReader, "SAYI0")
                    };
                    contingents.Add(contingent);
                }
            }

            File.Delete(tempPath);
            return contingents;
        }

        // Yeni helper metodlar (nullable tipler için)
        private byte? GetNullableByte(DbfDataReader.DbfDataReader reader, string col)
        {
            try
            {
                var value = reader[col];
                if (value == null || value == DBNull.Value) return null;
                return Convert.ToByte(value);
            }
            catch { return null; }
        }

        private short? GetNullableShort(DbfDataReader.DbfDataReader reader, string col)
        {
            try
            {
                var value = reader[col];
                if (value == null || value == DBNull.Value) return null;
                return Convert.ToInt16(value);
            }
            catch { return null; }
        }
        private byte GetByte(DbfDataReader.DbfDataReader reader, string col)
        {
            try { return Convert.ToByte(reader[col]); }
            catch { return 0; }
        }

        private short GetShort(DbfDataReader.DbfDataReader reader, string col)
        {
            try { return Convert.ToInt16(reader[col]); }
            catch { return 0; }
        }

        private int GetInt(DbfDataReader.DbfDataReader reader, string col)
        {
            try { return Convert.ToInt32(reader[col]); }
            catch { return 0; }
        }

        private decimal GetDecimal(DbfDataReader.DbfDataReader reader, string col)
        {
            try { return Convert.ToDecimal(reader[col]); }
            catch { return 0; }
        }

        private string GetString(DbfDataReader.DbfDataReader reader, string col)
        {
            try { return reader[col]?.ToString().Trim() ?? ""; }
            catch { return ""; }
        }
    }


}