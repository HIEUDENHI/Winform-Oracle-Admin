-- ==========================================
-- 1. STANDARD AUDIT
-- ==========================================
AUDIT INSERT, UPDATE, DELETE ON ADMIN.DANGKY BY ACCESS;
AUDIT SELECT, UPDATE ON ADMIN.NHANVIEN BY ACCESS WHENEVER SUCCESSFUL;
AUDIT SESSION WHENEVER NOT SUCCESSFUL;
AUDIT ROLE BY ACCESS;
AUDIT GRANT ANY ROLE, GRANT ANY PRIVILEGE BY ACCESS;
AUDIT TABLE BY ACCESS;
AUDIT SELECT, INSERT, UPDATE, DELETE ON ADMIN.VW_MOMON_PDT BY ACCESS WHENEVER SUCCESSFUL;

-- ==========================================
-- 2. FINE-GRAINED AUDITING 
-- ==========================================
-- Tạo hàm kiểm tra vai trò NV PKT
CREATE OR REPLACE FUNCTION ADMIN.CHECK_NOT_NV_PKT
RETURN PLS_INTEGER
AS
  ROLE_COUNT NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO ROLE_COUNT
    FROM DBA_ROLE_PRIVS
    WHERE GRANTEE = SYS_CONTEXT('USERENV', 'SESSION_USER')
      AND GRANTED_ROLE = 'ROLE_NV_PKT';
    
    IF ROLE_COUNT > 0 THEN
        RETURN 1;  -- Có vai trò "NV PKT", không audit
    ELSE
        RETURN 0;  -- Không có vai trò "NV PKT", audit
    END IF;
END;
/

BEGIN
  BEGIN
    DBMS_FGA.DROP_POLICY(
      object_schema => 'ADMIN',
      object_name   => 'DANGKY',
      policy_name   => 'AUDIT_UPDATE_DIEM_NOT_NV_PKT'
    );
  EXCEPTION
    WHEN OTHERS THEN
      IF SQLCODE = -28102 OR SQLCODE = -20000 THEN
        DBMS_OUTPUT.PUT_LINE('Chính sách AUDIT_UPDATE_DIEM_NOT_NV_PKT không tồn tại, không cần xóa.');
      ELSE
        RAISE;
      END IF;
  END;
END;
/

-- Tạo chính sách FGA cho hành vi cập nhật điểm số
BEGIN
  DBMS_FGA.ADD_POLICY(
    object_schema    => 'ADMIN',
    object_name      => 'DANGKY',
    policy_name      => 'AUDIT_UPDATE_DIEM_NOT_NV_PKT',
    audit_column     => 'ĐIEMTH, ĐIEMQT, ĐIEMCK, ĐIEMTK',
    audit_condition  => 'ADMIN.CHECK_NOT_NV_PKT = 0',
    statement_types  => 'UPDATE',
    audit_trail      => DBMS_FGA.DB + DBMS_FGA.EXTENDED
  );
END;
/

BEGIN
  BEGIN
    DBMS_FGA.DROP_POLICY(
      object_schema => 'ADMIN',
      object_name   => 'DANGKY',
      policy_name   => 'AUDIT_UPDATE_DIEM_NOT_NV_PKT'
    );
  EXCEPTION
    WHEN OTHERS THEN
      IF SQLCODE = -28102 OR SQLCODE = -20000 THEN
        DBMS_OUTPUT.PUT_LINE('Chính sách AUDIT_UPDATE_DIEM_NOT_NV_PKT không tồn tại, không cần xóa.');
      ELSE
        RAISE;
      END IF;
  END;
END;
/

-- Tạo hàm kiểm tra vai trò NOT TCHC
CREATE OR REPLACE FUNCTION ADMIN.CHECK_NOT_TCHC
RETURN PLS_INTEGER
AS
  ROLE_COUNT NUMBER;
BEGIN
    SELECT COUNT(*)
    INTO ROLE_COUNT
    FROM DBA_ROLE_PRIVS
    WHERE GRANTEE = SYS_CONTEXT('USERENV', 'SESSION_USER')
      AND GRANTED_ROLE = 'ROLE_TCHC';
    
    IF ROLE_COUNT > 0 THEN
        RETURN 1;  -- Có vai trò "ROLE_TCHC", không audit
    ELSE
        RETURN 0;  -- Không có vai trò "ROLE_TCHC", audit
    END IF;
END;
/

-- Xóa chính sách SELECT
BEGIN
  BEGIN
    DBMS_FGA.DROP_POLICY(
      object_schema => 'ADMIN',
      object_name   => 'NHANVIEN',
      policy_name   => 'AUDIT_SELECT_LUONG_PHUCAP_NOT_TCHC'
    );
  EXCEPTION
    WHEN OTHERS THEN
      IF SQLCODE = -28102 OR SQLCODE = -20000 THEN
        DBMS_OUTPUT.PUT_LINE('Chính sách AUDIT_SELECT_LUONG_PHUCAP_NOT_TCHC không tồn tại, không cần xóa.');
      ELSE
        RAISE;
      END IF;
  END;
END;
/

-- Xóa chính sách UPDATE
BEGIN
  BEGIN
    DBMS_FGA.DROP_POLICY(
      object_schema => 'ADMIN',
      object_name   => 'NHANVIEN',
      policy_name   => 'AUDIT_UPDATE_LUONG_PHUCAP_NOT_TCHC'
    );
  EXCEPTION
    WHEN OTHERS THEN
      IF SQLCODE = -28102 OR SQLCODE = -20000 THEN
        DBMS_OUTPUT.PUT_LINE('Chính sách AUDIT_UPDATE_LUONG_PHUCAP_NOT_TCHC không tồn tại, không cần xóa.');
      ELSE
        RAISE;
      END IF;
  END;
END;
/

-- Tạo chính sách FGA cho hành vi SELECT trên LUONG, PHUCAP
BEGIN
  DBMS_FGA.ADD_POLICY(
    object_schema    => 'ADMIN',
    object_name      => 'NHANVIEN',
    policy_name      => 'AUDIT_SELECT_LUONG_PHUCAP_NOT_TCHC',
    audit_column     => 'LUONG, PHUCAP',
    audit_condition  => 'MANV != SYS_CONTEXT(''USERENV'', ''SESSION_USER'') AND ADMIN.CHECK_NOT_TCHC = 0',
    statement_types  => 'SELECT',
    audit_trail      => DBMS_FGA.DB + DBMS_FGA.EXTENDED
  );
END;
/

-- Tạo chính sách FGA cho hành vi UPDATE trên LUONG, PHUCAP
BEGIN
  DBMS_FGA.ADD_POLICY(
    object_schema    => 'ADMIN',
    object_name      => 'NHANVIEN',
    policy_name      => 'AUDIT_UPDATE_LUONG_PHUCAP_NOT_TCHC',
    audit_column     => 'LUONG, PHUCAP',
    audit_condition  => 'ADMIN.CHECK_NOT_TCHC = 0',
    statement_types  => 'UPDATE',
    audit_trail      => DBMS_FGA.DB + DBMS_FGA.EXTENDED
  );
END;
/

-- Tạo hàm kiểm tra tích hợp MASV và thời gian hiệu chỉnh
CREATE OR REPLACE FUNCTION ADMIN.CHECK_INVALID_DANGKY_TIME(p_masv IN VARCHAR2, p_mamm IN VARCHAR2)
RETURN PLS_INTEGER
AS
  v_hk NUMBER;
  v_nam NUMBER;
  v_start_date DATE;
  v_current_date DATE := SYSDATE;
  v_user VARCHAR2(100) := SYS_CONTEXT('USERENV', 'SESSION_USER');
BEGIN
    -- Kiểm tra nếu MASV khác với người dùng hiện tại
    IF p_masv != v_user THEN
        RETURN 1; -- Audit nếu thao tác trên dữ liệu của sinh viên khác
    END IF;
    
    -- Kiểm tra nếu p_mamm rỗng hoặc null
    IF p_mamm IS NULL THEN
        RETURN 1; -- Audit nếu MAMM không hợp lệ
    END IF;
    
    -- Lấy HK và NAM từ MOMON dựa trên MAMM
    SELECT HK, NAM
    INTO v_hk, v_nam
    FROM ADMIN.MOMON
    WHERE MAMM = p_mamm;
    
    -- Xác định ngày bắt đầu học kỳ dựa trên HK
    IF v_hk = 1 THEN
        v_start_date := TO_DATE(v_nam || '-09-01', 'YYYY-MM-DD');
    ELSIF v_hk = 2 THEN
        v_start_date := TO_DATE(v_nam || '-01-01', 'YYYY-MM-DD');
    ELSIF v_hk = 3 THEN
        v_start_date := TO_DATE(v_nam || '-05-01', 'YYYY-MM-DD');
    ELSE
        RETURN 1; -- Học kỳ không hợp lệ
    END IF;
    
    -- Kiểm tra xem SYSDATE có trong 14 ngày đầu của học kỳ hay không
    IF v_current_date BETWEEN v_start_date AND (v_start_date + 13) THEN
        RETURN 0; -- Trong 14 ngày đầu, không audit
    ELSE
        RETURN 1; -- Ngoài 14 ngày đầu, audit
    END IF;
EXCEPTION
    WHEN NO_DATA_FOUND THEN
        RETURN 1; -- Nếu MAMM không tồn tại, audit
    WHEN OTHERS THEN
        RETURN 1; -- Audit trong trường hợp lỗi khác
END;
/

-- Xóa chính sách cũ nếu tồn tại
BEGIN
  BEGIN
    DBMS_FGA.DROP_POLICY(
      object_schema => 'ADMIN',
      object_name   => 'DANGKY',
      policy_name   => 'AUDIT_DANGKY_INVALID_SV'
    );
  EXCEPTION
    WHEN OTHERS THEN
      IF SQLCODE = -28102 OR SQLCODE = -20000 THEN
        DBMS_OUTPUT.PUT_LINE('Chính sách AUDIT_DANGKY_INVALID_SV không tồn tại, không cần xóa.');
      ELSE
        RAISE;
      END IF;
  END;
END;
/

-- Tạo chính sách FGA cho hành vi INSERT, UPDATE, DELETE trên DANGKY
BEGIN
  DBMS_FGA.ADD_POLICY(
    object_schema    => 'ADMIN',
    object_name      => 'DANGKY',
    policy_name      => 'AUDIT_DANGKY_INVALID_SV',
    audit_column     => NULL, -- Audit tất cả cột
    audit_condition  => 'ADMIN.CHECK_INVALID_DANGKY_TIME(DANGKY.MASV, DANGKY.MAMM) = 1',
    statement_types  => 'INSERT, UPDATE, DELETE',
    audit_trail      => DBMS_FGA.DB + DBMS_FGA.EXTENDED
  );
END;
/