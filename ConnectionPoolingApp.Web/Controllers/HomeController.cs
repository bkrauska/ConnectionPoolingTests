using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.Mvc;
using kDev.Database;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ConnectionPoolingApp.Web.Controllers {
    public class HomeController : Controller {
        Random random = new Random();
        readonly string sqlText;
        readonly int randomId;

        public HomeController() : base() {
            this.randomId = random.Next(10000);
            this.sqlText = "SELECT * FROM Purchase WHERE Id = " + this.randomId;
        }

        public void SqlConn() {
            var random = new Random();
            var randomId = random.Next(10000);

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString)) {
                using (var cmd = new SqlCommand(this.sqlText, conn) { CommandType = CommandType.Text }) {
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
                            Response.Write("No Results for " + this.randomId);
                        }
                    }
                }
            }
        }

        public void EntLib() {
            var db = DatabaseFactory.CreateDatabase("DefaultDb");
            var cmd = db.GetSqlStringCommand(this.sqlText);

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
                    Response.Write("No Results for " + this.randomId);
                }
            }
        }

        public void Index() {
            var connectionString = ConfigurationManager.ConnectionStrings["DefaultDb"].ConnectionString;

            using (var reader = SqlDatabase.ExecuteDataReader(connectionString, CommandType.Text, this.sqlText)) {
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
                    Response.Write("No Results for " + this.randomId);
                }
            }
        }
    }

    
}
