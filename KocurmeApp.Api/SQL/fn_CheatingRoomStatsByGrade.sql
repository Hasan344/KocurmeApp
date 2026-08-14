-- ============================================================
-- Sinif üzrə zal köçürmə analizi (SQL tərəfi)
--
-- @Sinif: imtahan tipi (məs. 9 = 9-cu sinif, 11 = 11-ci sinif).
-- Funksiya Exams.Sinif = @Sinif olan BÜTÜN imtahanları avtomatik götürür,
-- normallaşdırmanı həmin sinfin QLOBAL ortasına görə aparır.
--
--   Detal sətir (RowType = 0):
--       Kolon3 = Faiz1 / QlobalAvgFaiz1
--       Kolon4 = Faiz2 / QlobalAvgFaiz2
--   Hər imtahanın "Orta qiymət" sətri (RowType = 1):
--       Faiz1  = həmin imtahanın orta Faiz1-i
--       Kolon3 = imtahanOrtaFaiz1 / QlobalAvgFaiz1   (məs. 3.54/4.72 = 0.75)
--
-- İstifadə: SELECT * FROM dbo.fn_CheatingRoomStatsByGrade(9);
--
-- QEYD: Əvvəlcə Exams cədvəlinə Sinif sütunu əlavə olunur (aşağıda),
--       sonra hər imtahan üçün Sinif dəyəri təyin edilməlidir, məs.:
--       UPDATE dbo.Exams SET Sinif = 9  WHERE Id IN (...);
--       UPDATE dbo.Exams SET Sinif = 11 WHERE Id IN (...);
-- ============================================================

-- 1) Exams cədvəlinə Sinif (imtahan tipi) sütunu
IF COL_LENGTH('dbo.Exams', 'Sinif') IS NULL
    ALTER TABLE dbo.Exams ADD Sinif INT NULL;
GO

-- 2) Sinif üzrə analiz funksiyası
CREATE OR ALTER FUNCTION dbo.fn_CheatingRoomStatsByGrade (@Sinif INT)
RETURNS @res TABLE
(
    ExamId       INT,
    Zal          SMALLINT,
    KocurenSayi  INT,
    FennSayi     INT,
    OdaSayi      INT,
    Faiz1        DECIMAL(5,2),
    Faiz2        DECIMAL(5,2),
    Kolon3       DECIMAL(5,2),
    Kolon4       DECIMAL(5,2),
    Kolon5       DECIMAL(5,2),
    RowType      TINYINT          -- 0 = detal, 1 = imtahan orta qiyməti
)
AS
BEGIN
    -- Hər (imtahan, zal) üzrə Faiz1 / Faiz2
    DECLARE @faiz TABLE
    (
        ExamId       INT,
        ZAL1         SMALLINT,
        KocurenSayi  INT,
        FennSayi     INT,
        OdaSayi      INT,
        Faiz1        DECIMAL(5,2),
        Faiz2        DECIMAL(5,2)
    );

    INSERT INTO @faiz (ExamId, ZAL1, KocurenSayi, FennSayi, OdaSayi, Faiz1, Faiz2)
    SELECT
        s.ExamId,
        s.ZAL1,
        s.KocurenSayi,
        s.FennSayi,
        s.OdaSayi,
        CAST((CAST(s.FennSayi    AS DECIMAL(10,2)) / NULLIF(s.OdaSayi * 3, 0)) * 100 AS DECIMAL(5,2)),
        CAST((CAST(s.KocurenSayi AS DECIMAL(10,2)) / NULLIF(s.OdaSayi, 0))     * 100 AS DECIMAL(5,2))
    FROM (
        SELECT
            m1.ExamId,
            m1.ZAL1,
            COUNT(DISTINCT m1.IS_N1) AS KocurenSayi,
            COUNT(DISTINCT m1.FENN)  AS FennSayi,
            MAX(m2.KOL_ABT)          AS OdaSayi
        FROM dbo.CheatingStudents m1
        JOIN dbo.Rooms m2
            ON m2.Z_KOD = m1.ZAL1 AND m2.ExamId = m1.ExamId
        WHERE m1.EYNI_Y >= 5
          AND m1.ExamId IN (SELECT Id FROM dbo.Exams WHERE Sinif = @Sinif)
          AND (CAST(CAST(m1.EYNI_Y + m1.EYNI_B AS DECIMAL(10,2))
                    / CAST(30 - m1.EYNI_D AS DECIMAL(10,2)) * 100 AS DECIMAL(10,2))) >= 70
        GROUP BY m1.ExamId, m1.ZAL1
    ) s;

    -- Qlobal ortalar (bu sinfin bütün imtahanları üzrə)
    DECLARE @G1 DECIMAL(18,6) = (SELECT AVG(Faiz1) FROM @faiz);
    DECLARE @G2 DECIMAL(18,6) = (SELECT AVG(Faiz2) FROM @faiz);

    -- Detal sətirlər (qlobal normallaşdırma)
    INSERT INTO @res (ExamId, Zal, KocurenSayi, FennSayi, OdaSayi, Faiz1, Faiz2, Kolon3, Kolon4, Kolon5, RowType)
    SELECT
        f.ExamId,
        f.ZAL1,
        f.KocurenSayi,
        f.FennSayi,
        f.OdaSayi,
        f.Faiz1,
        f.Faiz2,
        CAST(ROUND(f.Faiz1 / NULLIF(@G1, 0), 2) AS DECIMAL(5,2)),
        CAST(ROUND(f.Faiz2 / NULLIF(@G2, 0), 2) AS DECIMAL(5,2)),
        CAST(ROUND(((f.Faiz1 / NULLIF(@G1, 0)) + (f.Faiz2 / NULLIF(@G2, 0))) / 2, 2) AS DECIMAL(5,2)),
        0
    FROM @faiz f;

    -- Hər imtahan üçün "Orta qiymət" sətri
    INSERT INTO @res (ExamId, Zal, KocurenSayi, FennSayi, OdaSayi, Faiz1, Faiz2, Kolon3, Kolon4, Kolon5, RowType)
    SELECT
        e.ExamId,
        CAST(0 AS SMALLINT),
        NULL, NULL, NULL,
        CAST(e.AvgF1 AS DECIMAL(5,2)),
        CAST(e.AvgF2 AS DECIMAL(5,2)),
        CAST(ROUND(e.AvgF1 / NULLIF(@G1, 0), 2) AS DECIMAL(5,2)),
        CAST(ROUND(e.AvgF2 / NULLIF(@G2, 0), 2) AS DECIMAL(5,2)),
        CAST(ROUND(((e.AvgF1 / NULLIF(@G1, 0)) + (e.AvgF2 / NULLIF(@G2, 0))) / 2, 2) AS DECIMAL(5,2)),
        1
    FROM (
        SELECT ExamId, AVG(Faiz1) AS AvgF1, AVG(Faiz2) AS AvgF2
        FROM @faiz
        GROUP BY ExamId
    ) e;

    RETURN;
END
GO
