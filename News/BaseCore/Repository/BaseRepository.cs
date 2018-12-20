using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using News.Common.Library;
using News.Models;
using Oracle.ManagedDataAccess.Client;

namespace News.BaseCore.Repository
{
    public class BaseRepository : IDisposable
    {
        /// <summary>
        /// Chuỗi kết nối CSDL
        /// </summary>
        private string _connectionString;

        private OracleConnection _oraConnection;

        private OracleCommand _oraCommand;
        /// <summary>
        /// Tên file Log
        /// </summary>
        //private string _logFile;

        /// <summary>
        /// Lấy thông tin đối tượng OracleConnection.
        /// </summary>
        /// <value>The connection.</value>
        public OracleConnection Connection
        {
            get
            {
                return _oraConnection;
            }
        }

        /// <summary>
        /// Khởi tạo đối tượng lớp <see cref="T:BaseDAO"/>.
        /// </summary>
        /// <param name="connectionstring">The connectionstring.</param>
        /// <param name="provider">The provider.</param> 
        public BaseRepository()
        {
            _connectionString = DataBaseConnection.SsoConnectionString;

            _oraConnection = new OracleConnection();
            _oraCommand = new OracleCommand();

            _oraConnection.ConnectionString = _connectionString;
            _oraCommand.Connection = _oraConnection;
        }

        public UserInfoModel CurrentUser;
        public BaseRepository(UserInfoModel currentUser)
        {
            if (currentUser == null)
                currentUser = new UserInfoModel();
            CurrentUser = currentUser;

            _connectionString = DataBaseConnection.SsoConnectionString;

            _oraConnection = new OracleConnection();
            _oraCommand = new OracleCommand();

            _oraConnection.ConnectionString = _connectionString;
            _oraCommand.Connection = _oraConnection;
        }

        #region AddParameter
        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public OracleParameter AddParameter(string name, object value)
        {
            var p = new OracleParameter { ParameterName = name, Value = value ?? DBNull.Value };
            return Command.Parameters.Add(p);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public OracleParameter AddParameter(string name, OracleDbType dataType, object value)
        {
            var p = new OracleParameter { ParameterName = name, OracleDbType = dataType, Value = value ?? DBNull.Value };
            return Command.Parameters.Add(p);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>

        /// <param name="dataType"></param>
        /// <returns></returns>
        public OracleParameter AddParameter(string name, OracleDbType dataType)
        {
            var p = new OracleParameter { ParameterName = name, OracleDbType = dataType };
            return Command.Parameters.Add(p);
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name">The name.</param>

        /// <param name="dataType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public OracleParameter AddParameter(string name, OracleDbType dataType, int size)
        {
            var p = new OracleParameter { ParameterName = name, OracleDbType = dataType, Size = size };
            return Command.Parameters.Add(p);
        }
        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        public OracleParameter AddParameter(OracleParameter parameter)
        {
            return Command.Parameters.Add(parameter);
        }
        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataType"></param>
        /// <param name="size"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public OracleParameter AddParameter(string name, OracleDbType dataType, int size, ParameterDirection direction)
        {
            var p = new OracleParameter
            {
                ParameterName = name,
                OracleDbType = dataType,
                Direction = direction,
                Size = size
            };
            return Command.Parameters.Add(p);
        }
        /// <summary>
        ///  Adds the parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataType"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public OracleParameter AddParameter(string name, OracleDbType dataType, ParameterDirection direction)
        {
            var p = new OracleParameter { ParameterName = name, OracleDbType = dataType, Direction = direction };
            return Command.Parameters.Add(p);
        }
        #endregion AddParameter

        /// <summary>
        /// Gets the command.
        /// </summary>
        /// <value>The command.</value>
        public OracleCommand Command
        {
            get
            {
                return _oraCommand;
            }
        }
        #region Working Transaction
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (_oraConnection.State == System.Data.ConnectionState.Closed)
            {
                _oraConnection.Open();
            }
            _oraCommand.Transaction = _oraConnection.BeginTransaction();
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            _oraCommand.Transaction.Commit();
            _oraConnection.Close();
        }

        /// <summary>
        /// Rollbacks the transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            _oraCommand.Transaction.Rollback();
            _oraConnection.Close();
        }
        #endregion Working Transaction

        #region ExcuteNonQuery
        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query)
        {
            return ExecuteNonQuery(query, CommandType.Text, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, CommandType commandType)
        {
            return ExecuteNonQuery(query, commandType, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, CustomConnectionState connectionState)
        {
            return ExecuteNonQuery(query, CommandType.Text, connectionState);
        }

        /// <summary>
        /// Executes the non query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, CommandType commandType, CustomConnectionState connectionState)
        {
            _oraCommand.CommandText = query;
            _oraCommand.CommandType = commandType;
            int i = -1;
            try
            {
                if (_oraConnection.State == System.Data.ConnectionState.Closed)
                {
                    _oraConnection.Open();
                }
                i = _oraCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                _oraCommand.Parameters.Clear();
                if (connectionState == CustomConnectionState.CloseOnExit)
                {
                    _oraConnection.Close();
                }
            }

            return i;
        }
        #endregion ExcuteNonQuery

        #region ExecuteScalar
        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public object ExecuteScalar(string query)
        {
            return ExecuteScalar(query, CommandType.Text, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <returns></returns>
        public object ExecuteScalar(string query, CommandType commandType)
        {
            return ExecuteScalar(query, commandType, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public object ExecuteScalar(string query, CustomConnectionState connectionState)
        {
            return ExecuteScalar(query, CommandType.Text, connectionState);
        }

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public object ExecuteScalar(string query, CommandType commandType, CustomConnectionState connectionState)
        {
            _oraCommand.CommandText = query;
            _oraCommand.CommandType = commandType;
            object o = null;
            try
            {
                if (_oraConnection.State == System.Data.ConnectionState.Closed)
                {
                    _oraConnection.Open();
                }
                o = _oraCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                _oraCommand.Parameters.Clear();
                if (connectionState == CustomConnectionState.CloseOnExit)
                {
                    _oraConnection.Close();
                }
            }

            return o;
        }
        #endregion ExecuteScalar

        #region ExecuteReader
        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query)
        {
            return ExecuteReader(query, CommandType.Text, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query, CommandType commandType)
        {
            return ExecuteReader(query, commandType, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query, CustomConnectionState connectionState)
        {
            return ExecuteReader(query, CommandType.Text, connectionState);
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string query, CommandType commandType, CustomConnectionState connectionState)
        {
            _oraCommand.CommandText = query;
            _oraCommand.CommandType = commandType;
            DbDataReader reader = null;
            try
            {
                if (_oraConnection.State == System.Data.ConnectionState.Closed)
                {
                    _oraConnection.Open();
                }
                if (connectionState == CustomConnectionState.CloseOnExit)
                {
                    reader = _oraCommand.ExecuteReader(CommandBehavior.CloseConnection);
                }
                else
                {
                    reader = _oraCommand.ExecuteReader();
                }

            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                _oraCommand.Parameters.Clear();
            }

            return reader;
        }
        #endregion ExecuteReader

        #region ExecuteDataSet
        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query)
        {
            return ExecuteDataSet(query, CommandType.Text, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query, CommandType commandType)
        {
            return ExecuteDataSet(query, commandType, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query, CustomConnectionState connectionState)
        {
            return ExecuteDataSet(query, CommandType.Text, connectionState);
        }

        /// <summary>
        /// Executes the data set.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string query, CommandType commandType, CustomConnectionState connectionState)
        {
            OracleDataAdapter adapter = new OracleDataAdapter();
            _oraCommand.CommandText = query;
            _oraCommand.CommandType = commandType;
            adapter.SelectCommand = _oraCommand;
            DataSet ds = new DataSet();
            try
            {
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                _oraCommand.Parameters.Clear();
                if (connectionState == CustomConnectionState.CloseOnExit)
                {
                    if (_oraConnection.State == System.Data.ConnectionState.Open)
                    {
                        _oraConnection.Close();
                    }
                }
            }
            return ds;
        }

        #endregion ExecuteDataSet


        /// <summary>
        /// Lấy kết quả dạng List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="commandType"></param>
        /// <param name="connectionState"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string query, CommandType commandType, CustomConnectionState connectionState, bool ignoreCaseMapping = false)
        {
            try
            {
                var dr = ExecuteReader(query, commandType);
                if (dr == null || dr.FieldCount == 0)
                    return null;
                var fCount = dr.FieldCount;
                var mType = typeof(T);
                var mList = new List<T>();
                while (dr.Read())
                {
                    var obj = Activator.CreateInstance(mType);
                    for (var i = 0; i < fCount; i++)
                    {
                        if (dr[i] == DBNull.Value) continue;
                        PropertyInfo p = null;
                        if (ignoreCaseMapping)
                            p = mType.GetProperty(dr.GetName(i),
                                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        else
                            p = mType.GetProperty(dr.GetName(i));
                        if (p == null) continue;
                        try
                        {
                            var type = p.PropertyType;
                            //var value = Convert.ChangeType(dr[i], type);
                            var value = ChangeType(dr[i], type);
                            p.SetValue(obj, value, null);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                    mList.Add((T)obj);
                }
                dr.Close();
                return mList;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                Command.Parameters.Clear();
                if (connectionState == CustomConnectionState.CloseOnExit)
                {
                    Connection.Close();
                }
            }
            return null;
        }
        public List<T> GetList<T>(string query, CommandType commandType)
        {
            try
            {
                var dr = ExecuteReader(query, commandType);
                if (dr == null || dr.FieldCount == 0)
                    return null;
                var fCount = dr.FieldCount;
                var mType = typeof(T);
                var mList = new List<T>();
                while (dr.Read())
                {
                    var obj = Activator.CreateInstance(mType);
                    for (var i = 0; i < fCount; i++)
                    {
                        if (dr[i] == DBNull.Value) continue;
                        var p = mType.GetProperty(dr.GetName(i));
                        if (p == null) continue;
                        try
                        {
                            var type = p.PropertyType;
                            //var value = Convert.ChangeType(dr[i], type);
                            var value = ChangeType(dr[i], type);
                            p.SetValue(obj, value, null);
                        }
                        catch (Exception)
                        {
                            // ignored
                        }
                    }
                    mList.Add((T)obj);
                }
                dr.Close();
                return mList;
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                Command.Parameters.Clear();

            }
            return null;
        }

        public static List<T> DataSetToObject<T>(DataSet dataSet)
        {
            var array = new List<T>();
            var dataTable = dataSet.Tables;
            var dt = dataTable[0];

            foreach (DataRow dataRow in dt.Rows)
            {
                var row = Activator.CreateInstance<T>();

                var propertyInfos = typeof(T).GetProperties();

                foreach (var ppi in propertyInfos)
                {
                    var name = ppi.Name.ToUpper();
                    //try
                    //{
                    //bo sung kiem tra ten cot co ton tai khong o day
                    if (!dataRow.Table.Columns.Contains(name)) continue;
                    var value = dataRow[name];
                    ppi.SetValue(row, Convert.ChangeType(value, ppi.PropertyType), null);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                }
                array.Add((T)row);
            }
            return array;
        }

        public static List<T> DataTableToObject<T>(DataTable dataTable)
        {
            var array = new List<T>();

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var row = Activator.CreateInstance<T>();

                var propertyInfos = typeof(T).GetProperties();

                foreach (var ppi in propertyInfos)
                {
                    var name = ppi.Name.ToUpper();
                    //try
                    //{
                    //bo sung kiem tra ten cot co ton tai khong o day
                    if (!dataRow.Table.Columns.Contains(name)) continue;
                    var value = dataRow[name];

                    ppi.SetValue(row, Convert.ChangeType(value, ppi.PropertyType), null);
                    //}
                    //catch (Exception ex)
                    //{
                    //}
                }
                array.Add((T)row);
            }
            return array;
        }
        /// <summary>
        /// Handles the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private void HandleExceptions(Exception ex)
        {

            ExceptionHandlers.Handle(ex, ExceptionTypes.BASE_DAO_EXCEPTION);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _oraConnection.Close();
            _oraConnection.Dispose();
            _oraCommand.Dispose();
        }

        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t); ;
            }

            return Convert.ChangeType(value, t);
        }

        /// <summary>
        /// Handles the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void HandleDAOExceptions(Exception ex)
        {
            ExceptionHandlers.Handle(ex, ExceptionTypes.BASE_DAO_EXCEPTION);

        }

        #region GetNextValueOfSquence
        /// <summary>
        /// Lấy giá trị tiếp theo của Sequence
        /// </summary>
        /// <param name="sequenceName">The query.</param>
        /// <returns></returns>
        public object GetNextValueOfSquence(string sequenceName)
        {
            return GetNextValueOfSquence(sequenceName, CustomConnectionState.CloseOnExit);
        }

        /// <summary>
        /// Lấy giá trị tiếp theo của Sequence.
        /// </summary>
        /// <param name="sequenceName">The query.</param>
        /// <param name="commandType">The commandType.</param>
        /// <param name="connectionstate">The connectionstate.</param>
        /// <returns></returns>
        public object GetNextValueOfSquence(string sequenceName, CustomConnectionState connectionState)
        {
            _oraCommand.CommandText = "SELECT " + sequenceName + ".nextval FROM DUAL";
            _oraCommand.CommandType = CommandType.Text;
            object o = null;
            try
            {
                if (_oraConnection.State == System.Data.ConnectionState.Closed)
                {
                    _oraConnection.Open();
                }
                o = _oraCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                HandleExceptions(ex);
            }
            finally
            {
                _oraCommand.Parameters.Clear();
                if (connectionState == CustomConnectionState.CloseOnExit)
                {
                    _oraConnection.Close();
                }
            }

            return o;
        }
        #endregion GetNextValueOfSquence
    }

    public enum CustomConnectionState
    {
        KeepOpen,
        CloseOnExit
    }
}


