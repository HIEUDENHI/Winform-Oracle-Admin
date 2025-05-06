-- 1. Cấp quyền tạm thời 
GRANT SELECT, UPDATE ON ADMIN.NHANVIEN TO GV002;
GRANT SELECT, UPDATE ON ADMIN.DANGKY TO GV002;


BEGIN
  DBMS_RLS.DROP_POLICY(
    object_schema => 'ADMIN',
    object_name   => 'DANGKY',
    policy_name   => 'VPD_DANGKY_DIEM'
  );
EXCEPTION
  WHEN OTHERS THEN
    IF SQLCODE = -28102 THEN
      DBMS_OUTPUT.PUT_LINE('Chính sách VPD_DANGKY_DIEM không tồn tại, không cần xóa.');
    ELSE
      RAISE;
    END IF;
END;
/

BEGIN
  DBMS_RLS.DROP_POLICY(
    object_schema => 'ADMIN',
    object_name   => 'DANGKY',
    policy_name   => 'VPD_DANGKY'
  );
EXCEPTION
  WHEN OTHERS THEN
    IF SQLCODE = -28102 THEN
      DBMS_OUTPUT.PUT_LINE('Chính sách VPD_DANGKY không tồn tại, không cần xóa.');
    ELSE
      RAISE;
    END IF;
END;
/

BEGIN
  DBMS_RLS.DROP_POLICY(
    object_schema => 'ADMIN',
    object_name   => 'DANGKY',
    policy_name   => 'VPD_DANGKY_SELECT'
  );
EXCEPTION
  WHEN OTHERS THEN
    IF SQLCODE = -28102 THEN
      DBMS_OUTPUT.PUT_LINE('Chính sách VPD_DANGKY_SELECT không tồn tại, không cần xóa.');
    ELSE
      RAISE;
    END IF;
END;
/

BEGIN
  DBMS_RLS.DROP_POLICY(
    object_schema => 'ADMIN',
    object_name   => 'DANGKY',
    policy_name   => 'VPD_DANGKY_MODIFY'
  );
EXCEPTION
  WHEN OTHERS THEN
    IF SQLCODE = -28102 THEN
      DBMS_OUTPUT.PUT_LINE('Chính sách VPD_DANGKY_MODIFY không tồn tại, không cần xóa.');
    ELSE
      RAISE;
    END IF;
END;
/
COMMIT;
-- 2. Kiểm tra Standard Audit và FGA trên DANGKY
-- 2.1. User SV001 (ROLE_SV): Thử UPDATE trên dữ liệu của mình và người khác
-- Kết quả mong đợi: 
-- - Standard Audit: Ghi lại mọi UPDATE
-- - FGA (AUDIT_DANGKY_INVALID_SV): Audit nếu ngoài 14 ngày hoặc trên dữ liệu người khác
-- SELECT trên dữ liệu SINHVIEN
SELECT * FROM ADMIN.DANGKY;
-- UPDATE trên dữ liệu của chính mình (ngoài 14 ngày, nên bị audit)
UPDATE ADMIN.DANGKY 
SET ĐIEMTH = 8.0 
WHERE MASV = 'SV001' AND MAMM = 'MM001';
-- Khôi phục dữ liệu
UPDATE ADMIN.DANGKY 
SET ĐIEMTH = NULL 
WHERE MASV = 'SV001' AND MAMM = 'MM001';
-- UPDATE trên dữ liệu của người khác (bị audit do p_masv != v_user)
UPDATE ADMIN.DANGKY 
SET ĐIEMTH = 7.5 
WHERE MASV = 'SV002' AND MAMM = 'MM002';

SELECT * FROM ADMIN.DANGKY;
-- Khôi phục dữ liệu
UPDATE ADMIN.DANGKY 
SET ĐIEMTH = NULL 
WHERE MASV = 'SV002' AND MAMM = 'MM002';
COMMIT;
/
-- 2.2. User NV002 (ROLE_NV_PKT): Thử UPDATE trên DANGKY
-- Kết quả mong đợi:
-- - Standard Audit: Ghi lại mọi UPDATE
-- - FGA (AUDIT_UPDATE_DIEM_NOT_NV_PKT): Không audit vì có ROLE_NV_PKT
-- - FGA (AUDIT_DANGKY_INVALID_SV): Audit vì ngoài 14 ngày
-- UPDATE trên cột điểm
UPDATE ADMIN.DANGKY 
SET ĐIEMTH = 9.0, ĐIEMQT = 8.5 
WHERE MASV = 'SV001' AND MAMM = 'MM001';
-- Khôi phục dữ liệu
UPDATE ADMIN.DANGKY 
SET ĐIEMTH = NULL, ĐIEMQT = NULL 
WHERE MASV = 'SV001' AND MAMM = 'MM001';
COMMIT;


-- 3. Kiểm tra Standard Audit và FGA trên NHANVIEN
-- 3.1. User GV002 (ROLE_GV): Thử SELECT và UPDATE trên NHANVIEN
-- Kết quả mong đợi:
-- - Standard Audit: Ghi lại SELECT, UPDATE thành công
-- - FGA (AUDIT_UPDATE_DIEM_NOT_NV_PKT): Không audit vì có ROLE_NV_PKT
-- - FGA (AUDIT_SELECT_LUONG_PHUCAP_NOT_TCHC): Audit SELECT trên LUONG, PHUCAP
-- - FGA (AUDIT_UPDATE_LUONG_PHUCAP_NOT_TCHC): Audit UPDATE trên LUONG, PHUCAP
-- UPDATE trên Điểm - Bị audit 
UPDATE ADMIN.DANGKY 
SET ĐIEMTH = 9.0, ĐIEMQT = 8.5 
WHERE MASV = 'SV001' AND MAMM = 'MM001';
COMMIT;
-- SELECT trên LUONG, PHUCAP (bị audit)
SELECT LUONG, PHUCAP 
FROM ADMIN.NHANVIEN 
WHERE MANV = 'GV001';
-- UPDATE trên LUONG (bị audit)
UPDATE ADMIN.NHANVIEN 
SET LUONG = LUONG + 1000000 
WHERE MANV = 'GV001';
-- Khôi phục dữ liệu
UPDATE ADMIN.NHANVIEN 
SET LUONG = LUONG - 1000000 
WHERE MANV = 'GV001';
COMMIT;


-- 3.2. User NV003 (ROLE_TCHC): Thử SELECT và UPDATE trên NHANVIEN
-- Kết quả mong đợi:
-- - Standard Audit: Ghi lại SELECT, UPDATE thành công
-- - FGA: Không audit vì có ROLE_TCHC
-- SELECT trên LUONG, PHUCAP
SELECT LUONG, PHUCAP 
FROM ADMIN.NHANVIEN 
WHERE MANV = 'GV001';
-- UPDATE trên LUONG
UPDATE ADMIN.NHANVIEN 
SET LUONG = LUONG + 500000 
WHERE MANV = 'GV001';
-- Khôi phục dữ liệu
UPDATE ADMIN.NHANVIEN 
SET LUONG = LUONG - 500000 
WHERE MANV = 'GV001';
COMMIT;


-- 4. Thu hồi quyền đã cấp
REVOKE SELECT, UPDATE ON ADMIN.NHANVIEN FROM GV002;

-- 5. Xem kết quả audit
SELECT 
    TO_CHAR(TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS') AS AUDIT_TIME,
    USERNAME,                    -- User trong database
    OS_USERNAME AS OS_USER,      -- User trên hệ điều hành
    OWNER AS SCHEMA_NAME,        -- Schema chứa đối tượng
    OBJ_NAME AS OBJECT_NAME,     -- Tên đối tượng
    ACTION_NAME AS ACTION_TYPE,  -- Hành động
    SQL_TEXT AS SQL_STATEMENT,   -- Câu lệnh SQL
    'Standard Audit' AS AUDIT_TYPE,
    RETURNCODE AS RESULT_CODE    -- Kết quả thực thi
FROM 
    DBA_AUDIT_TRAIL
WHERE 
    OWNER = 'ADMIN'
UNION ALL

SELECT 
    TO_CHAR(TIMESTAMP, 'YYYY-MM-DD HH24:MI:SS') AS AUDIT_TIME,
    DB_USER AS USERNAME,         -- User trong database
    OS_USER,                     -- User trên hệ điều hành
    OBJECT_SCHEMA AS SCHEMA_NAME, -- Schema chứa đối tượng (giữ nguyên cho FGA)
    OBJECT_NAME,                 -- Tên đối tượng (giữ nguyên cho FGA)
    STATEMENT_TYPE AS ACTION_TYPE, -- Loại câu lệnh
    SQL_TEXT AS SQL_STATEMENT,   -- Câu lệnh SQL
    'FGA - ' || POLICY_NAME AS AUDIT_TYPE,
    NULL AS RESULT_CODE          
FROM 
    DBA_FGA_AUDIT_TRAIL
WHERE 
    OBJECT_SCHEMA = 'ADMIN'
ORDER BY 
    AUDIT_TIME DESC;

-- 6. Xóa lịch sử audit
DELETE FROM DBA_FGA_AUDIT_TRAIL
COMMIT;


SELECT * FROM DBA_AUDIT_TRAIL