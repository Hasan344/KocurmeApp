-- ============================================================
-- 9-cu sinif zal köçürmə analizi (SQL tərəfi)
-- Verilmiş zal köçürmə analizi sorğusu @ExamId parametri ilə
-- inline table-valued function kimi hazırlanıb.
-- Backend (Get9thGradeCheatingAnalysisForExportQueryHandler)
-- bu funksiyanı FromSql ilə çağırır.
--
-- İstifadə: SELECT * FROM dbo.fn_NinthGradeCheatingRoomStats(@ExamId);
-- ============================================================

CREATE OR ALTER FUNCTION dbo.fn_NinthGradeCheatingRoomStats (@ExamId INT)
RETURNS TABLE
AS
RETURN
(
    WITH t1 AS (
        SELECT
            m1.*,
            CAST(CAST(m1.EYNI_Y + m1.EYNI_B AS DECIMAL(10,2)) / CAST(30 - m1.EYNI_D AS DECIMAL(10,2)) * 100 AS DECIMAL(10,2)) AS ehtimal,
            m2.KOL_ABT AS Oda_Student_Sayisi
        FROM [dbo].CheatingStudents m1
        JOIN [dbo].Rooms m2
            ON m2.Z_KOD = m1.ZAL1
        WHERE m1.EYNI_Y >= 5 AND m1.ExamId = @ExamId AND m2.ExamId = @ExamId
    ),
    t2 AS (
        SELECT * FROM t1 WHERE ehtimal >= 70
    ),
    summary AS (
        SELECT
            ZAL1,
            COUNT(DISTINCT IS_N1) AS Kopya_Ceken_Student_Sayisi,
            COUNT(DISTINCT FENN) AS Kopya_Cekilen_Fenn_Sayisi,
            MAX(Oda_Student_Sayisi) AS Oda_Student_Sayisi
        FROM t2
        GROUP BY ZAL1
    ),
    t3 AS (
        SELECT
            ZAL1 AS Zal,
            Kopya_Ceken_Student_Sayisi AS KocurenSayi,
            Kopya_Cekilen_Fenn_Sayisi AS FennSayi,
            Oda_Student_Sayisi AS OdaSayi,
            CAST(
                (CAST(Kopya_Cekilen_Fenn_Sayisi AS DECIMAL(10,2)) / NULLIF(Oda_Student_Sayisi * 3, 0)) * 100
                AS DECIMAL(5,2)
            ) AS Faiz1,
            CAST(
                (CAST(Kopya_Ceken_Student_Sayisi AS DECIMAL(10,2)) / NULLIF(Oda_Student_Sayisi, 0)) * 100
                AS DECIMAL(5,2)
            ) AS Faiz2
        FROM summary
    ),
    t4 AS (
        SELECT
            Zal, KocurenSayi, FennSayi, OdaSayi, Faiz1, Faiz2,
            CAST(ROUND(Faiz1 / AVG(Faiz1) OVER(), 2) AS DECIMAL(5,2)) AS Kolon3,
            CAST(ROUND(Faiz2 / AVG(Faiz2) OVER(), 2) AS DECIMAL(5,2)) AS Kolon4
        FROM t3
    ),
    t5 AS (
        SELECT *, CAST(ROUND((Kolon3 + Kolon4) / 2, 2) AS DECIMAL(5,2)) AS Kolon5
        FROM t4
    )
    SELECT
        CAST(Zal AS SMALLINT) AS Zal,
        CAST(KocurenSayi AS INT) AS KocurenSayi,
        CAST(FennSayi   AS INT) AS FennSayi,
        CAST(OdaSayi    AS INT) AS OdaSayi,
        Faiz1, Faiz2, Kolon3, Kolon4, Kolon5
    FROM t5

    UNION ALL

    -- Orta qiymət sətri (kolon5 = NULL olduğu üçün ORDER BY kolon5 ASC-də ən üstdə çıxır)
    SELECT
        CAST(1 AS SMALLINT) AS Zal,
        CAST(NULL AS INT) AS KocurenSayi,
        CAST(NULL AS INT) AS FennSayi,
        CAST(NULL AS INT) AS OdaSayi,
        CAST(AVG(Faiz1) AS DECIMAL(5,2)) AS Faiz1,
        CAST(AVG(Faiz2) AS DECIMAL(5,2)) AS Faiz2,
        CAST(NULL AS DECIMAL(5,2)) AS Kolon3,
        CAST(NULL AS DECIMAL(5,2)) AS Kolon4,
        CAST(NULL AS DECIMAL(5,2)) AS Kolon5
    FROM t5
);
GO
