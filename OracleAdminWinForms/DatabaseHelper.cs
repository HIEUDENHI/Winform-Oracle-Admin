using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace OracleAdminWinForms
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Thực thi lệnh DDL, DCL (Create, Drop, Grant, Revoke, Alter...)
        public int ExecuteNonQuery(string sql)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new OracleCommand(sql, conn))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        // Thực thi SELECT trả về DataTable
        public DataTable ExecuteQuery(string sql)
        {
            using (var conn = new OracleConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new OracleCommand(sql, conn))
                {
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }
    }
}
