-- đăng nhập SYSDBA
-- chạy lệnh này để kích hoạt OLS
EXEC LBACSYS.CONFIGURE_OLS;
EXEC LBACSYS.OLS_ENFORCEMENT.ENABLE_OLS;

select name, status, description from dba_ols_status;


SHOW CON_ID;
SELECT CON_ID, NAME, OPEN_MODE FROM V$PDBS;

-- tắt dữ liệu oracle và mở lại rồi chạy lệnh dưới đây
select * from v$services;
ALTER USER lbacsys IDENTIFIED BY lbacsys ACCOUNT UNLOCK;
SHOW CON_NAME;

ALTER SESSION SET CONTAINER = XEPDB1;
CREATE USER ADMIN_OLS IDENTIFIED BY 123 CONTAINER = CURRENT;
GRANT CONNECT,RESOURCE TO ADMIN_OLS; --CẤP QUYỀN CONNECT VÀ RESOURCE
GRANT UNLIMITED TABLESPACE TO ADMIN_OLS; --CẤP QUOTA CHO ADMIN_OLS
GRANT SELECT ANY DICTIONARY TO ADMIN_OLS; --CẤP QUYỀN ĐỌC DICTIONARY
GRANT EXECUTE ON LBACSYS.SA_COMPONENTS TO ADMIN_OLS WITH GRANT OPTION;
GRANT EXECUTE ON LBACSYS.sa_user_admin TO ADMIN_OLS WITH GRANT OPTION;
GRANT EXECUTE ON LBACSYS.sa_label_admin TO ADMIN_OLS WITH GRANT OPTION;
GRANT EXECUTE ON sa_policy_admin TO ADMIN_OLS WITH GRANT OPTION;
GRANT EXECUTE ON char_to_label TO ADMIN_OLS WITH GRANT OPTION;

GRANT LBAC_DBA TO ADMIN_OLS;
GRANT EXECUTE ON sa_sysdba TO ADMIN_OLS;
GRANT EXECUTE ON TO_LBAC_DATA_LABEL TO ADMIN_OLS; -- CẤP QUYỀN THỰC THI
GRANT CREATE USER TO ADMIN_OLS;
GRANT ALTER USER TO ADMIN_OLS;
GRANT DROP USER TO ADMIN_OLS;
GRANT GRANT ANY PRIVILEGE TO ADMIN_OLS;
GRANT GRANT ANY ROLE TO ADMIN_OLS;


GRANT DBA TO ADMIN_OLS;
COMMIT;

--Đăng nhập user ADMIN_OLS để chạy các lệnh tiếp theo
-- OLS--
                 -- Tạo chính sách OLS
/*                
BEGIN
  SA_SYSDBA.DROP_POLICY(
    policy_name => 'THONGBAO_POLICY'
  );
END;
/
*/
BEGIN
  SA_SYSDBA.CREATE_POLICY(
    policy_name     => 'THONGBAO_POLICY',
    column_name     => 'OLS_LABEL'
  );
END;
/
--DROP TABLE THONGBAO;
--chạy lệnh kích hoạt chính sách OLS (hãy khởi động lại database sau khi chạy lệnh)
EXEC SA_SYSDBA.ENABLE_POLICY ('THONGBAO_POLICY');


                    -- Tạo component của label
BEGIN
                    -- Tạo Level
    SA_COMPONENTS.CREATE_LEVEL('THONGBAO_POLICY', 300, 'TRDV', 'Trưởng đơn vị');
    SA_COMPONENTS.CREATE_LEVEL('THONGBAO_POLICY', 200, 'NV', 'Nhân viên');
    SA_COMPONENTS.CREATE_LEVEL('THONGBAO_POLICY', 100, 'SV', 'Sinh viên');
                    -- Tạo Compartment                    
    SA_COMPONENTS.CREATE_COMPARTMENT('THONGBAO_POLICY', 10, 'TOAN', 'Toán');
    SA_COMPONENTS.CREATE_COMPARTMENT('THONGBAO_POLICY', 20, 'LY', 'Lý');
    SA_COMPONENTS.CREATE_COMPARTMENT('THONGBAO_POLICY', 30, 'HOA', 'Hóa');
    SA_COMPONENTS.CREATE_COMPARTMENT('THONGBAO_POLICY', 40, 'HC', 'Hành chính');
                    -- Tạo Group
    SA_COMPONENTS.CREATE_GROUP('THONGBAO_POLICY', 1, 'CS1', 'Cơ sở 1');
    SA_COMPONENTS.CREATE_GROUP('THONGBAO_POLICY', 2, 'CS2', 'Cơ sở 2');
END;
/
-- Kiểm tra component tạo đúng chưa
SELECT * FROM DBA_SA_LEVELS;
SELECT * FROM DBA_SA_COMPARTMENTS;
SELECT * FROM DBA_SA_GROUPS;
SELECT * FROM DBA_SA_GROUP_HIERARCHY;


                    -- Tạo bảng THÔNGBÁO
CREATE TABLE THONGBAO (
    MATB NUMBER PRIMARY KEY,
    NOIDUNG VARCHAR2(1000),
    NGAYGUI DATE DEFAULT SYSDATE
);
                    -- Nhập dữ liệu vào bảng THÔNGBÁO
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (1, 'Cần phát tán đến tất cả trưởng đơn vị.');
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (2, 'Cần phát tán đến tất cả nhân viên.');
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (3, 'Cần phát tán đến tất cả sinh viên.');
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (4, 'Cần phát tán đến sinh viên thuộc khoa Hóa ở cơ sở 1.');
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (5, 'Cần phát tán đến sinh viên thuộc khoa Hóa ở cơ sở 2.');
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (6, 'Cần phát tán đến sinh viên thuộc khoa Hóa ở cả hai cơ sở.');
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (7, 'Cần phát tán đến tất cả sinh viên thuộc cả hai cơ sở.');
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (8, 'Cần phát tán đến trưởng khoa Hóa ở cơ sở 1.');
INSERT INTO THONGBAO (MATB, NOIDUNG) VALUES (9, 'Cần phát tán đến trưởng khoa Hóa ở cơ sở 1 và cơ sở 2.');
COMMIT;


BEGIN
  SA_POLICY_ADMIN.APPLY_TABLE_POLICY(
    policy_name    => 'THONGBAO_POLICY',
    schema_name    => 'ADMIN_OLS',
    table_name     => 'THONGBAO',
    table_options  => 'NO_CONTROL'
  );
END;
/

-- Định nghĩa nhãn bằng SA_LABEL_ADMIN.CREATE_LABEL trước khi gán
BEGIN
  -- Nhãn cho tất cả trưởng đơn vị
  SA_LABEL_ADMIN.CREATE_LABEL('THONGBAO_POLICY', 10000, 'TRDV');
  -- Nhãn cho tất cả nhân viên
  SA_LABEL_ADMIN.CREATE_LABEL('THONGBAO_POLICY', 900, 'NV');
  -- Nhãn cho tất cả sinh viên
  SA_LABEL_ADMIN.CREATE_LABEL('THONGBAO_POLICY', 800, 'SV');
  -- Nhãn cho sinh viên khoa Hóa ở cơ sở 1
  SA_LABEL_ADMIN.CREATE_LABEL('THONGBAO_POLICY', 700, 'SV:HOA:CS1');
  -- Nhãn cho sinh viên khoa Hóa ở cơ sở 2
  SA_LABEL_ADMIN.CREATE_LABEL('THONGBAO_POLICY', 600, 'SV:HOA:CS2');
  -- Nhãn cho sinh viên khoa Hóa ở cả hai cơ sở
  SA_LABEL_ADMIN.CREATE_LABEL('THONGBAO_POLICY', 500, 'SV:HOA');
  -- Nhãn cho trưởng khoa Hóa ở cơ sở 1
  SA_LABEL_ADMIN.CREATE_LABEL('THONGBAO_POLICY', 400, 'TRDV:HOA:CS1');
  -- Nhãn cho trưởng khoa Hóa ở cả hai cơ sở
  SA_LABEL_ADMIN.CREATE_LABEL('THONGBAO_POLICY', 300, 'TRDV:HOA');
END;
/
                    -- Gán nhãn cho thông báo
-- t1: Gửi thông báo đến tất cả trưởng đơn vị
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'TRDV') WHERE MATB = 1;
-- t2: Gửi thông báo đến tất cả nhân viên
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'NV') WHERE MATB = 2;
-- t3: Gửi thông báo đến tất cả sinh viên
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'SV') WHERE MATB = 3;
-- t4: Gửi thông báo đến sinh viên thuộc khoa Hóa ở cơ sở 1
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'SV:HOA:CS1') WHERE MATB = 4;
-- t5: Gửi thông báo đến sinh viên thuộc khoa Hóa ở cơ sở 2
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'SV:HOA:CS2') WHERE MATB = 5;
-- t6: Gửi thông báo đến sinh viên thuộc khoa Hóa ở cả hai cơ sở
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'SV:HOA') WHERE MATB = 6;
-- t7: Gửi thông báo đến tất cả sinh viên thuộc cả hai cơ sở
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'SV') WHERE MATB = 7;
-- t8: Gửi thông báo đến trưởng khoa Hóa ở cơ sở 1
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'TRDV:HOA:CS1') WHERE MATB = 8;
-- t9: Gửi thông báo đến trưởng khoa Hóa ở cơ sở 1 và cơ sở 2
UPDATE THONGBAO SET OLS_LABEL = CHAR_TO_LABEL('THONGBAO_POLICY', 'TRDV:HOA') WHERE MATB = 9;
COMMIT;

BEGIN
  SA_POLICY_ADMIN.REMOVE_TABLE_POLICY(
    policy_name => 'THONGBAO_POLICY',
    schema_name => 'ADMIN_OLS',
    table_name  => 'THONGBAO'
  );
  SA_POLICY_ADMIN.APPLY_TABLE_POLICY(
    policy_name    => 'THONGBAO_POLICY',
    schema_name    => 'ADMIN_OLS',
    table_name     => 'THONGBAO',
    table_options  => 'READ_CONTROL,WRITE_CONTROL,CHECK_CONTROL',
    predicate      => NULL
  );
END;
/

SELECT * FROM THONGBAO;

DESC ADMIN_OLS.THONGBAO;

UPDATE THONGBAO SET NOIDUNG = NOIDUNG;
COMMIT;

/*
DROP USER U1 CASCADE;
DROP USER U2 CASCADE;
DROP USER U3 CASCADE;
DROP USER U4 CASCADE;
DROP USER U5 CASCADE;
DROP USER U6 CASCADE;
DROP USER U7 CASCADE;
DROP USER U8 CASCADE;
*/
                    --Tạo người dùng cần gán nhãn
CREATE USER U1 IDENTIFIED BY 123; -- Trưởng đơn vị có thể đọc được toàn bộ thông báo
CREATE USER U2 IDENTIFIED BY 123; -- Trưởng đơn vị phụ trách khoa Hóa tại cơ sở 2
CREATE USER U3 IDENTIFIED BY 123; -- Trưởng đơn vị phụ trách khoa Lý tại cơ sở 2
CREATE USER U4 IDENTIFIED BY 123; -- Nhân viên thuộc khoa Hóa tại cơ sở 2
CREATE USER U5 IDENTIFIED BY 123; -- Sinh viên khoa Hóa tại cơ sở 2
CREATE USER U6 IDENTIFIED BY 123; -- Trưởng đơn vị có thể đọc được các thông báo về Hành chính
CREATE USER U7 IDENTIFIED BY 123; -- Nhân viên có thể đọc toàn bộ thông báo dành cho tất cả nhân viên
CREATE USER U8 IDENTIFIED BY 123; -- Nhân viên có thể đọc thông báo về Hành chính tại cơ sở 1

                    -- Cấp quyền CREATE SESSION để kết nối
GRANT CREATE SESSION TO U1, U2, U3, U4, U5, U6, U7, U8;
GRANT CONNECT TO U1, U2, U3, U4, U5, U6, U7, U8;

                    -- Cấp quyền SELECT trên bảng THÔNGBÁO
GRANT SELECT ON ADMIN_OLS.THONGBAO TO U1, U2, U3, U4, U5, U6, U7, U8;
COMMIT;


                        -- Gán nhãn người dùng

-- User 1 (u1) Trưởng đơn vị có thể đọc được toàn bộ thông báo
BEGIN
SA_USER_ADMIN.SET_USER_LABELS(
    POLICY_NAME  =>'THONGBAO_POLICY',
    USER_NAME  => 'U1',
    MAX_READ_LABEL  => 'TRDV:TOAN,LY,HOA,HC:CS1,CS2'
);
END;
/

-- User 2 (u2) Trưởng đơn vị phụ trách khoa Hóa tại cơ sở 2
BEGIN
SA_USER_ADMIN.SET_USER_LABELS(
    POLICY_NAME  =>'THONGBAO_POLICY',
    USER_NAME  => 'U2',
    MAX_READ_LABEL  => 'TRDV:HOA:CS2'
);
END;
/

--User 3 (u3) Trưởng đơn vị phụ trách khoa Lý tại cơ sở 2
BEGIN
SA_USER_ADMIN.SET_USER_LABELS(
    POLICY_NAME  =>'THONGBAO_POLICY',
    USER_NAME  => 'U3',
    MAX_READ_LABEL  => 'TRDV:LY:CS2'
);
END;
/

--User 4 (u4) Nhân viên thuộc khoa Hóa tại cơ sở 2
BEGIN
SA_USER_ADMIN.SET_USER_LABELS(
    POLICY_NAME  =>'THONGBAO_POLICY',
    USER_NAME  => 'U4',
    MAX_READ_LABEL  => 'NV:HOA:CS2'
);
END;
/

--User 5 (u5) Sinh viên khoa Hóa tại cơ sở 2
BEGIN
SA_USER_ADMIN.SET_USER_LABELS(
    POLICY_NAME  =>'THONGBAO_POLICY',
    USER_NAME  => 'U5',
    MAX_READ_LABEL  => 'SV:HOA:CS2'
);
END;
/

--User 6 (u6) Trưởng đơn vị có thể đọc được các thông báo về Hành chính.
BEGIN
SA_USER_ADMIN.SET_USER_LABELS(
    POLICY_NAME  =>'THONGBAO_POLICY',
    USER_NAME  => 'U6',
    MAX_READ_LABEL  => 'TRDV:HC:CS1,CS2'
);
END;
/

--User 7 (u7) Nhân viên có thể đọc toàn bộ thông báo dành cho tất cả nhân viên
BEGIN
SA_USER_ADMIN.SET_USER_LABELS(
    POLICY_NAME  =>'THONGBAO_POLICY',
    USER_NAME  => 'U7',
    MAX_READ_LABEL  => 'NV:TOAN,LY,HOA,HC:CS1,CS2'
);
END;
/

--User 8 (u8) Nhân viên có thể đọc thông báo về Hành chính tại cơ sở 1
BEGIN
SA_USER_ADMIN.SET_USER_LABELS(
    POLICY_NAME  =>'THONGBAO_POLICY',
    USER_NAME  => 'U8',
    MAX_READ_LABEL  => 'NV:HC:CS1'
);
END;
/

COMMIT;

