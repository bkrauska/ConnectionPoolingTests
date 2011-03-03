using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ConnectionPoolingApp.Web.Controllers {
    public class HomeController : Controller {
        public void SqlConn() {
            var random = new Random();
            var randomId = random.Next(10000);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString)) {
                var cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM Purchase WHERE Id = " + randomId;
                cmd.CommandType = System.Data.CommandType.Text;

                conn.Open();
                using (var reader = cmd.ExecuteReader()) {

                    if (reader.Read()) {
                        var sb = new StringBuilder();
                        sb.Append(reader.GetValue(0).ToString() + ",");
                        sb.Append(reader.GetValue(1).ToString() + ",");
                        sb.Append(reader.GetValue(2).ToString() + ",");
                        sb.Append(reader.GetValue(3).ToString() + ",");
                        sb.Append(reader.GetValue(4).ToString() + ",");
                        sb.Append(reader.GetValue(5).ToString());
                        Response.Write(sb.ToString());
                    }
                    else {
                        Response.Write("No Results for " + randomId);
                    }
                }
            }
        }

        public void EntLib() {
            var random = new Random();
            var randomId = random.Next(10000);

            var db = DatabaseFactory.CreateDatabase("DefaultDb");
            var cmd = db.GetSqlStringCommand("SELECT * FROM Purchase WHERE Id = " + randomId);

            using (var reader = db.ExecuteReader(cmd)) {
                if (reader.Read()) {
                    var sb = new StringBuilder();
                    sb.Append(reader.GetValue(0).ToString() + ",");
                    sb.Append(reader.GetValue(1).ToString() + ",");
                    sb.Append(reader.GetValue(2).ToString() + ",");
                    sb.Append(reader.GetValue(3).ToString() + ",");
                    sb.Append(reader.GetValue(4).ToString() + ",");
                    sb.Append(reader.GetValue(5).ToString());
                    Response.Write(sb.ToString());
                }
                else {
                    Response.Write("No Results for " + randomId);
                }
            }
        }

        public void Custom() {
            var random = new Random();
            var randomId = random.Next(10000);

            using (var reader = SqlHelper.ExecuteDataReader(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString, "SELECT * FROM Purchase WHERE Id = " + randomId)) {
                if (reader.Read()) {
                    var sb = new StringBuilder();
                    sb.Append(reader.GetValue(0).ToString() + ",");
                    sb.Append(reader.GetValue(1).ToString() + ",");
                    sb.Append(reader.GetValue(2).ToString() + ",");
                    sb.Append(reader.GetValue(3).ToString() + ",");
                    sb.Append(reader.GetValue(4).ToString() + ",");
                    sb.Append(reader.GetValue(5).ToString());
                    Response.Write(sb.ToString());
                }
                else {
                    Response.Write("No Results for " + randomId);
                }
            }
        }
    }

    public class SqlHelper {
        public static IDataReader ExecuteDataReader(string connectionString, string sqlText) {
            var conn = new SqlConnection(connectionString);
            var cmd = new SqlCommand() { Connection = conn, CommandText = sqlText, CommandType = CommandType.Text };
            conn.Open();
            var dataReader = cmd.ExecuteReader();

            return new CustomDataReader() { Connection = conn, DataReader = dataReader };
        }

    }

    public class CustomDataReader : IDataReader, IDisposable {
        public SqlDataReader DataReader { get; set; }
        public SqlConnection Connection { get; set; }

        public void Dispose() {
            if (Connection.State == ConnectionState.Open) {
                this.Connection.Close();
            }

            this.Connection.Dispose();
            this.DataReader.Dispose();
        }

        public void Close() {
            this.DataReader.Close();
        }

        public int Depth { get { return this.DataReader.Depth; } }

        public DataTable GetSchemaTable() { return this.DataReader.GetSchemaTable();  }

        public bool IsClosed { get { return this.DataReader.IsClosed; } }

        public bool NextResult() { return this.DataReader.NextResult(); }

        public bool Read() { return this.DataReader.Read(); }

        public int RecordsAffected { get { return this.DataReader.RecordsAffected; } }

        public int FieldCount { get { return this.DataReader.FieldCount; } }

        public bool GetBoolean(int i) { return this.DataReader.GetBoolean(i); }

        public byte GetByte(int i)  { return this.DataReader.GetByte(i); }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) {
            return this.DataReader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i) { return this.DataReader.GetChar(i); }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) {
            return this.DataReader.GetChars(i, fieldoffset, buffer, bufferoffset, length); 
        }

        public IDataReader GetData(int i) { throw new NotImplementedException(); }

        public string GetDataTypeName(int i) { return this.DataReader.GetDataTypeName(i); }

        public DateTime GetDateTime(int i) { return this.DataReader.GetDateTime(i); }

        public decimal GetDecimal(int i) { return this.DataReader.GetDecimal(i); }

        public double GetDouble(int i) { return this.DataReader.GetDouble(i); }

        public Type GetFieldType(int i) { return this.DataReader.GetFieldType(i); }

        public float GetFloat(int i) { return this.DataReader.GetFloat(i); }

        public Guid GetGuid(int i) { return this.DataReader.GetGuid(i); }

        public short GetInt16(int i) { return this.DataReader.GetInt16(i); }

        public int GetInt32(int i) { return this.DataReader.GetInt32(i); }

        public long GetInt64(int i) { return this.DataReader.GetInt64(i); }

        public string GetName(int i) { return this.DataReader.GetName(i); }

        public int GetOrdinal(string name) { return this.DataReader.GetOrdinal(name); }

        public string GetString(int i) { return this.DataReader.GetString(i); }

        public object GetValue(int i) { return this.DataReader.GetValue(i); }

        public int GetValues(object[] values) { return this.DataReader.GetValues(values); }

        public bool IsDBNull(int i) { return this.DataReader.GetBoolean(i); }

        public object this[string name] {
            get { return this.DataReader[name]; }
        }

        public object this[int i] {
            get { return this.DataReader[i]; }
        }
    }
}
