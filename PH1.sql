-- Bước 1: Chuyển container về XEPDB1
ALTER SESSION SET CONTAINER = XEPDB1;

-- Bước 3: Kết nối với tài khoản ADMIN

CREATE USER ADMIN IDENTIFIED BY admin
  DEFAULT TABLESPACE USERS
  TEMPORARY TABLESPACE TEMP
  QUOTA UNLIMITED ON USERS;
  
-- 3) Cấp các quyền cơ bản để ADMIN có thể quản trị
GRANT CREATE SESSION, ALTER USER, CREATE USER, DROP USER,
      GRANT ANY ROLE, GRANT ANY PRIVILEGE,
      CREATE PROCEDURE, SELECT ANY DICTIONARY
TO ADMIN;
------------------------------------------------------------
-- PHẦN 1: TẠO CÁC PACKAGE QUẢN TRỊ TRÊN ORACLE DO ADMIN SỬ DỤNG
------------------------------------------------------------

CREATE OR REPLACE PACKAGE ADMIN.PKG_USER_MANAGEMENT AS
    TYPE t_cursor IS REF CURSOR;

    PROCEDURE sp_GetAllUsers(p_cursor OUT t_cursor);

    PROCEDURE sp_CreateUser(p_username IN VARCHAR2, p_password IN VARCHAR2);

    PROCEDURE sp_AlterUser(p_username IN VARCHAR2, p_new_password IN VARCHAR2);

    PROCEDURE sp_DropUser(p_username IN VARCHAR2);
END PKG_USER_MANAGEMENT;
/
-- ================== PACKAGE BODY ADMIN.PKG_USER_MANAGEMENT ===================
CREATE OR REPLACE PACKAGE BODY ADMIN.PKG_USER_MANAGEMENT AS

    PROCEDURE sp_GetAllUsers(p_cursor OUT t_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT USERNAME,
                   ACCOUNT_STATUS,
                   DEFAULT_TABLESPACE,
                   TEMPORARY_TABLESPACE,
                   CREATED
            FROM DBA_USERS
            WHERE USERNAME NOT IN ('SYS','SYSTEM')  -- Loại trừ user hệ thống nếu muốn
            ORDER BY USERNAME;
    END sp_GetAllUsers;

    PROCEDURE sp_CreateUser(p_username IN VARCHAR2, p_password IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE 'CREATE USER ' || p_username ||
                          ' IDENTIFIED BY ' || p_password ||
                          ' DEFAULT TABLESPACE USERS ' ||
                          ' TEMPORARY TABLESPACE TEMP';
        -- Nếu muốn cấp CREATE SESSION mặc định cho user mới, có thể thêm:
        -- EXECUTE IMMEDIATE 'GRANT CREATE SESSION TO ' || p_username;
    END sp_CreateUser;

    PROCEDURE sp_AlterUser(p_username IN VARCHAR2, p_new_password IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE 'ALTER USER ' || p_username ||
                          ' IDENTIFIED BY ' || p_new_password;
    END sp_AlterUser;

    PROCEDURE sp_DropUser(p_username IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE 'DROP USER ' || p_username || ' CASCADE';
    END sp_DropUser;

END PKG_USER_MANAGEMENT;
/

-- ================== PACKAGE SPEC ADMIN.PKG_ROLE_MANAGEMENT ===================
CREATE OR REPLACE PACKAGE ADMIN.PKG_ROLE_MANAGEMENT AS
    TYPE t_cursor IS REF CURSOR;

    -- Lấy danh sách role
    PROCEDURE sp_GetAllRoles(p_cursor OUT t_cursor);

    -- Tạo role; nếu p_password rỗng thì tạo role không password
    PROCEDURE sp_CreateRole(p_role_name IN VARCHAR2, p_password IN VARCHAR2 DEFAULT NULL);

    -- Đổi tên role: tạo role mới, xóa role cũ
    PROCEDURE sp_RenameRole(p_old_role IN VARCHAR2, p_new_role IN VARCHAR2);

    -- Xóa role
    PROCEDURE sp_DropRole(p_role_name IN VARCHAR2);
END PKG_ROLE_MANAGEMENT;
/
-- ================== PACKAGE BODY ADMIN.PKG_ROLE_MANAGEMENT ===================
CREATE OR REPLACE PACKAGE BODY ADMIN.PKG_ROLE_MANAGEMENT AS

    PROCEDURE sp_GetAllRoles(p_cursor OUT t_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT ROLE,
                   PASSWORD_REQUIRED,
                   AUTHENTICATION_TYPE,
                   COMMON,
                   ORACLE_MAINTAINED
            FROM DBA_ROLES
            WHERE ROLE NOT IN ('CONNECT','RESOURCE')  -- Loại trừ role mặc định nếu muốn
            ORDER BY ROLE;
    END sp_GetAllRoles;

    PROCEDURE sp_CreateRole(p_role_name IN VARCHAR2, p_password IN VARCHAR2 DEFAULT NULL) IS
    BEGIN
        IF p_password IS NOT NULL AND p_password <> '' THEN
            EXECUTE IMMEDIATE 'CREATE ROLE ' || p_role_name || ' IDENTIFIED BY ' || p_password;
        ELSE
            EXECUTE IMMEDIATE 'CREATE ROLE ' || p_role_name;
        END IF;
    END sp_CreateRole;

    PROCEDURE sp_RenameRole(p_old_role IN VARCHAR2, p_new_role IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE 'CREATE ROLE ' || p_new_role;
        EXECUTE IMMEDIATE 'DROP ROLE ' || p_old_role;
    END sp_RenameRole;

    PROCEDURE sp_DropRole(p_role_name IN VARCHAR2) IS
    BEGIN
        EXECUTE IMMEDIATE 'DROP ROLE ' || p_role_name;
    END sp_DropRole;

END PKG_ROLE_MANAGEMENT;
/

-- ================== PACKAGE SPEC ADMIN.PKG_USER_ROLES ===================
CREATE OR REPLACE PACKAGE ADMIN.PKG_USER_ROLES AS

    -- Cấp role cho user (không WITH ADMIN OPTION)
    PROCEDURE sp_GrantRoleToUser(
        p_username IN VARCHAR2,
        p_role     IN VARCHAR2
    );

END PKG_USER_ROLES;
/
-- ================== PACKAGE BODY ADMIN.PKG_USER_ROLES ===================
CREATE OR REPLACE PACKAGE BODY ADMIN.PKG_USER_ROLES AS

    PROCEDURE sp_GrantRoleToUser(
        p_username IN VARCHAR2,
        p_role     IN VARCHAR2
    ) IS
    BEGIN
        EXECUTE IMMEDIATE 'GRANT ' || p_role || ' TO ' || p_username;
    END sp_GrantRoleToUser;

END PKG_USER_ROLES;
/

-- ================== PACKAGE SPEC ADMIN.PKG_OBJECT_MANAGEMENT ===================
CREATE OR REPLACE PACKAGE ADMIN.PKG_OBJECT_MANAGEMENT AS
    TYPE t_cursor IS REF CURSOR;

    -- Cấp quyền đối tượng cho Role
    PROCEDURE sp_GrantObjectPrivilegeToRole(
        p_role IN VARCHAR2,
        p_privilege IN VARCHAR2,           -- SELECT, UPDATE, INSERT, DELETE...
        p_schema IN VARCHAR2,
        p_object IN VARCHAR2,              -- Tên table/view/procedure/function
        p_column_list IN VARCHAR2 DEFAULT NULL, -- Danh sách cột
        p_with_grant_option IN VARCHAR2 DEFAULT 'NO'
    );

    -- Lấy danh sách các schema
    PROCEDURE sp_GetSchemas(p_cursor OUT t_cursor);

    -- Lấy danh sách các đối tượng (table, view, procedure, function) trong schema
    PROCEDURE sp_GetAllObjects(p_schema IN VARCHAR2, p_cursor OUT t_cursor);

    -- Cấp quyền cho user
    PROCEDURE sp_GrantObjectPrivilegeToUser(
        p_username IN VARCHAR2,
        p_privilege IN VARCHAR2,
        p_schema IN VARCHAR2,
        p_object IN VARCHAR2,
        p_column_list IN VARCHAR2 DEFAULT NULL,
        p_with_grant_option IN VARCHAR2 DEFAULT 'NO'
    );

    -- Thu hồi quyền đối tượng từ user
    PROCEDURE sp_RevokeObjectPrivilegeFromUser(
        p_username IN VARCHAR2,
        p_privilege IN VARCHAR2,
        p_schema   IN VARCHAR2,
        p_object   IN VARCHAR2
    );

    -- Xem thông tin quyền đối tượng của user
    PROCEDURE sp_GetUserObjectPrivileges(
        p_username IN VARCHAR2,
        p_cursor   OUT t_cursor
    );

    -- Xem thông tin quyền đối tượng của role
    PROCEDURE sp_GetRoleObjectPrivileges(
        p_rolename IN VARCHAR2,
        p_cursor   OUT t_cursor
    );

    -- Thu hồi quyền đối tượng từ role
    PROCEDURE sp_RevokeObjectPrivilegeFromRole(
        p_rolename IN VARCHAR2,
        p_privilege IN VARCHAR2,
        p_schema   IN VARCHAR2,
        p_object   IN VARCHAR2
    );
END PKG_OBJECT_MANAGEMENT;
/
-- ================== PACKAGE BODY ADMIN.PKG_OBJECT_MANAGEMENT ===================
CREATE OR REPLACE PACKAGE BODY ADMIN.PKG_OBJECT_MANAGEMENT AS

    PROCEDURE sp_GrantObjectPrivilegeToRole(
        p_role IN VARCHAR2,
        p_privilege IN VARCHAR2,
        p_schema IN VARCHAR2,
        p_object IN VARCHAR2,
        p_column_list IN VARCHAR2 DEFAULT NULL,
        p_with_grant_option IN VARCHAR2 DEFAULT 'NO'
    ) IS
        v_sql VARCHAR2(1000);
    BEGIN
        IF p_column_list IS NOT NULL AND p_column_list <> '' THEN
            v_sql := 'GRANT ' || p_privilege || ' (' || p_column_list || ') ' ||
                     ' ON ' || p_schema || '.' || p_object ||
                     ' TO ' || p_role;
        ELSE
            v_sql := 'GRANT ' || p_privilege ||
                     ' ON ' || p_schema || '.' || p_object ||
                     ' TO ' || p_role;
        END IF;
    
        IF UPPER(p_with_grant_option) = 'YES' THEN
            v_sql := v_sql || ' WITH GRANT OPTION';
        END IF;
    
        EXECUTE IMMEDIATE v_sql;
    END sp_GrantObjectPrivilegeToRole;

    PROCEDURE sp_GetRoleObjectPrivileges(
        p_rolename IN VARCHAR2,
        p_cursor   OUT t_cursor
    ) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT owner,
                   table_name AS object_name,
                   privilege,
                   grantable
            FROM dba_tab_privs
            WHERE grantee = UPPER(p_rolename)
            ORDER BY owner, table_name, privilege;
    END sp_GetRoleObjectPrivileges;

    PROCEDURE sp_RevokeObjectPrivilegeFromRole(
        p_rolename IN VARCHAR2,
        p_privilege IN VARCHAR2,
        p_schema   IN VARCHAR2,
        p_object   IN VARCHAR2
    ) IS
        v_sql VARCHAR2(1000);
    BEGIN
        v_sql := 'REVOKE ' || p_privilege || ' ON ' ||
                 p_schema || '.' || p_object || ' FROM ' || p_rolename;
        EXECUTE IMMEDIATE v_sql;
    END sp_RevokeObjectPrivilegeFromRole;

    PROCEDURE sp_RevokeObjectPrivilegeFromUser(
        p_username IN VARCHAR2,
        p_privilege IN VARCHAR2,
        p_schema   IN VARCHAR2,
        p_object   IN VARCHAR2
    ) IS
        v_sql VARCHAR2(1000);
    BEGIN
        v_sql := 'REVOKE ' || p_privilege || ' ON ' ||
                 p_schema || '.' || p_object || ' FROM ' || p_username;
        EXECUTE IMMEDIATE v_sql;
    END sp_RevokeObjectPrivilegeFromUser;

    PROCEDURE sp_GetSchemas(p_cursor OUT t_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT USERNAME FROM DBA_USERS;
    END sp_GetSchemas;

    PROCEDURE sp_GetAllObjects(p_schema IN VARCHAR2, p_cursor OUT t_cursor) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT OBJECT_NAME
            FROM ALL_OBJECTS
            WHERE OBJECT_TYPE IN ('TABLE', 'VIEW', 'PROCEDURE', 'FUNCTION')
              AND OWNER = UPPER(p_schema)
            ORDER BY OBJECT_NAME;
    END sp_GetAllObjects;

    PROCEDURE sp_GrantObjectPrivilegeToUser(
        p_username IN VARCHAR2,
        p_privilege IN VARCHAR2,
        p_schema IN VARCHAR2,
        p_object IN VARCHAR2,
        p_column_list IN VARCHAR2 DEFAULT NULL,
        p_with_grant_option IN VARCHAR2 DEFAULT 'NO'
    ) IS
        v_sql VARCHAR2(1000);
    BEGIN
        IF p_column_list IS NOT NULL AND p_column_list <> '' THEN
            v_sql := 'GRANT ' || p_privilege || ' (' || p_column_list || ') ON ' ||
                     p_schema || '.' || p_object || ' TO ' || p_username;
        ELSE
            v_sql := 'GRANT ' || p_privilege || ' ON ' ||
                     p_schema || '.' || p_object || ' TO ' || p_username;
        END IF;
    
        IF UPPER(p_with_grant_option) = 'YES' THEN
            v_sql := v_sql || ' WITH GRANT OPTION';
        END IF;
    
        EXECUTE IMMEDIATE v_sql;
    END sp_GrantObjectPrivilegeToUser;
    
    PROCEDURE sp_GetUserObjectPrivileges(
        p_username IN VARCHAR2,
        p_cursor   OUT t_cursor
    ) IS
    BEGIN
        OPEN p_cursor FOR
            SELECT owner,
                   table_name AS object_name,
                   privilege,
                   grantable
            FROM dba_tab_privs
            WHERE grantee = UPPER(p_username)
            ORDER BY owner, table_name, privilege;
    END sp_GetUserObjectPrivileges;

END PKG_OBJECT_MANAGEMENT;
/
------------------------------------------------------------------------------

-- 5) (Tuỳ chọn) Nếu cần, cấp EXECUTE cho chính ADMIN (hoặc cho user khác)
GRANT EXECUTE ON ADMIN.PKG_USER_MANAGEMENT       TO ADMIN;
GRANT EXECUTE ON ADMIN.PKG_ROLE_MANAGEMENT       TO ADMIN;
GRANT EXECUTE ON ADMIN.PKG_USER_ROLES            TO ADMIN;
GRANT EXECUTE ON ADMIN.PKG_OBJECT_MANAGEMENT     TO ADMIN;


