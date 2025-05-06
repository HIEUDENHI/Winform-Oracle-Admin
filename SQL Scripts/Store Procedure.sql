CREATE OR REPLACE PROCEDURE ADMIN.UPDATE_NHANVIEN_DT(
    p_manv IN VARCHAR2, -- Mã nhân viên
    p_dt   IN VARCHAR2  -- Số điện thoại mới
) IS
BEGIN
    -- Kiểm tra nếu MANV của user hiện tại khớp với p_manv
    IF p_manv = SYS_CONTEXT('USERENV', 'SESSION_USER') THEN
        -- Cập nhật số điện thoại nếu đúng nhân viên
        UPDATE ADMIN.NHANVIEN
        SET ĐT = p_dt
        WHERE MANV = p_manv;
    ELSE
        RAISE_APPLICATION_ERROR(-20001, 'Bạn chỉ có thể cập nhật thông tin của chính mình');
    END IF;
END;
/


GRANT EXECUTE ON ADMIN.UPDATE_NHANVIEN_DT TO ROLE_NVCB;




CREATE OR REPLACE PROCEDURE ADMIN.UPDATE_SINHVIEN_INFO (
    p_masv IN VARCHAR2,
    p_dc   IN VARCHAR2,
    p_dt   IN VARCHAR2
) IS
BEGIN
    IF p_masv = SYS_CONTEXT('USERENV', 'SESSION_USER') THEN
        UPDATE ADMIN.SINHVIEN
        SET ĐCHI = p_dc, ĐT = p_dt
        WHERE MASV = p_masv;
    ELSE
        RAISE_APPLICATION_ERROR(-20001, 'Bạn chỉ có thể cập nhật thông tin của chính mình');
    END IF;
END;
/

GRANT EXECUTE ON ADMIN.UPDATE_SINHVIEN_INFO TO ROLE_SV;
