using DotNetDBF;
using DbfDataReader;
using KocurmeApp.Domain.Entities;
using System.Text;

namespace KocurmeApp.Infrastructure.Services.FileImport
{
    public class DbfImportService
    {
        public async Task<List<CheatingStudent>> ImportCheatingStudentsAsync(Stream fileStream)
        {
            var students = new List<CheatingStudent>();

            using (var reader = new DBFReader(fileStream))
            {
                object[] record;
                while ((record = reader.NextRecord()) != null)
                {
                    var student = new CheatingStudent
                    {
                        IMT_GUN = SafeToByte(record[0]),
                        V_BINA = record[1]?.ToString(),
                        IS_N1 = Convert.ToInt32(record[2]),
                        BINA = Convert.ToInt16(record[3]),
                        ZAL1 = Convert.ToInt16(record[4]),
                        FENN = SafeToByte(record[5]),
                        FNADI = record[6]?.ToString(),
                        IS_N2 = Convert.ToInt32(record[7]),
                        ZAL2 = Convert.ToInt16(record[8]),
                        EYNI_D = SafeToByte(record[9]),
                        EYNI_Y = SafeToByte(record[10]),
                        EYNI_B = SafeToByte(record[11]),
                        Y_OXSHAR = Convert.ToDecimal(record[12]),
                        T_OXSHAR = Convert.ToDecimal(record[13]),
                        BAL1 = Convert.ToDecimal(record[14]),
                        BAL2 = Convert.ToDecimal(record[15])

                    };

                    students.Add(student);
                }
            }

            return students;
        }
        byte SafeToByte(object value)
        {
            if (value == null) return 0;
            if (byte.TryParse(value.ToString(), out var result))
                return result;
            return 0;
        }

        public async Task<List<Contingent>> ImportContingentsAsync(Stream fileStream)
        {
            var contingents = new List<Contingent>();
            using (var reader = new DBFReader(fileStream))
            {
                object[] record;
                while ((record = reader.NextRecord()) != null)
                {
                    var contingent = new Contingent
                    {
                        IMT_GUN = SafeToNullableByte(record[0]),
                        IMT_YERI = SafeToNullableByte(record[1]),
                        NUM_K = SafeToNullableByte(record[2]),
                        YASH_KATEQ = SafeToNullableByte(record[3]),
                        IZAHI = CleanString(record[4]?.ToString()),
                        SEC = SafeToNullableByte(record[5]),
                        TIP_OTUR = SafeToNullableByte(record[6]),
                        SAYI = SafeToNullableShort(record[7]),
                        SAYI0 = CleanString(record[8]?.ToString())
                    };
                    contingents.Add(contingent);
                }
            }
            return contingents;
        }

        private string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            try
            {
                // 1. Null byte'ları temizle
                input = input.Replace("\0", "");

                // 2. Encoding düzelt
                byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
                string result = Encoding.GetEncoding(1254).GetString(bytes);

                // 3. Trim ve kontrol
                result = result.Trim();

                return string.IsNullOrEmpty(result) ? null : result;
            }
            catch
            {
                // Fallback: sadece trim
                return input.Trim();
            }
        }

        private byte? SafeToNullableByte(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            try
            {
                // Eğer zaten byte ise direkt dön
                if (value is byte b)
                    return b;

                string strValue = value.ToString().Trim();

                if (string.IsNullOrEmpty(strValue))
                    return null;

                // Sadece rakamları al
                strValue = new string(strValue.Where(char.IsDigit).ToArray());

                if (string.IsNullOrEmpty(strValue))
                    return null;

                if (byte.TryParse(strValue, out byte result))
                    return result;

                return null;
            }
            catch
            {
                return null;
            }
        }

        private short? SafeToNullableShort(object value)
        {
            if (value == null || value == DBNull.Value)
                return null;

            try
            {
                // Eğer zaten short/int ise direkt dön
                if (value is short s)
                    return s;
                if (value is int i && i >= short.MinValue && i <= short.MaxValue)
                    return (short)i;

                string strValue = value.ToString().Trim();

                if (string.IsNullOrEmpty(strValue))
                    return null;

                // Rakamlar ve eksi işareti
                strValue = new string(strValue.Where(c => char.IsDigit(c) || c == '-').ToArray());

                if (string.IsNullOrEmpty(strValue) || strValue == "-")
                    return null;

                if (short.TryParse(strValue, out short result))
                    return result;

                return null;
            }
            catch
            {
                return null;
            }
        }
        //byte? SafeToNullableByte(object value)
        //{
        //    if (value == null) return null;
        //    if (byte.TryParse(value.ToString(), out var result))
        //        return result;
        //    return null;
        //}

        //short? SafeToNullableShort(object value)
        //{
        //    if (value == null) return null;
        //    if (short.TryParse(value.ToString(), out var result))
        //        return result;
        //    return null;
        //}
    }
}
