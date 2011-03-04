using System.Data;
using System.Data.SqlClient;

namespace kDev.Database {
    public class SqlDatabase {
        #region ExecuteDataReader

        public static IDataReader ExecuteDataReader(string connectionString, CommandType commandType, string commandText, SqlParameter[] sqlParameters) {
            var connection = SqlDatabase.CreateConnection(connectionString);
            var command = SqlDatabase.CreateCommand(connection, commandType, commandText, sqlParameters);
            SqlDatabase.AddSqlParameters(command, sqlParameters);
            connection.Open();
            var dataReader = command.ExecuteReader();
            return new ConnectionAwareDataReader() { Connection = connection, DataReader = dataReader };
        }

        public static IDataReader ExecuteDataReader(string connectionString, CommandType commandType, string commandText) {
            return SqlDatabase.ExecuteDataReader(connectionString, commandType, commandText, null);
        }

        #endregion

        #region ExecuteNonQuery

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, SqlParameter[] sqlParameters) {
            using (var connection = SqlDatabase.CreateConnection(connectionString)) {
                using (var command = SqlDatabase.CreateCommand(connection, commandType, commandText, sqlParameters)) {
                    SqlDatabase.AddSqlParameters(command, sqlParameters);
                    connection.Open();
                    var returnInt = command.ExecuteNonQuery();
                    return returnInt;
                }
            }
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText) {
            return SqlDatabase.ExecuteNonQuery(connectionString, commandType, commandText);
        }

        #endregion

        #region ExecuteScalar

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, SqlParameter[] sqlParameters) {
            using (var connection = SqlDatabase.CreateConnection(connectionString)) {
                using (var command = SqlDatabase.CreateCommand(connection, commandType, commandText, sqlParameters)) {
                    SqlDatabase.AddSqlParameters(command, sqlParameters);
                    connection.Open();
                    var returnObject = command.ExecuteScalar();
                    return returnObject;
                }
            }
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText) {
            return SqlDatabase.ExecuteScalar(connectionString, commandType, commandText);
        }

        #endregion

        #region Helper Methods

        private static SqlConnection CreateConnection(string connectionString) {
            return new SqlConnection(connectionString);
        }

        private static SqlCommand CreateCommand(SqlConnection connection, CommandType commandType, string commandText, SqlParameter[] sqlParameters) {
            return new SqlCommand(commandText, connection) { CommandType = commandType };
        }

        private static void AddSqlParameters(SqlCommand command, SqlParameter[] sqlParameters) {
            if (sqlParameters != null) {
                foreach (var parameter in sqlParameters) {
                    command.Parameters.Add(parameter);
                }
            }
        }

        #endregion
    }
}
