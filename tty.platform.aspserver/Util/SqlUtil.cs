using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using tty.Configs;

namespace tty.Util
{
    //public static class SqlUtil
    //{
    //    private static SqlConnection conn = null;
    //    private static void Open()
    //    {
    //        conn = new SqlConnection
    //        {
    //            ConnectionString = Config.Conn
    //        };
    //        if (conn.State == ConnectionState.Closed)
    //        {
    //            conn.Open();
    //        }
    //    }
    //    public static DataSet Query(string command)
    //    {
    //        try
    //        {
    //            Open();

    //            DataSet dataSet = new DataSet();
    //            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, conn);
    //            sqlDataAdapter.Fill(dataSet);

    //            return dataSet;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //        finally
    //        {
    //            Close();
    //        }
    //    }
    //    public static bool TryQuery(string command,out DataSet set)
    //    {
    //        try
    //        {
    //            Open();

    //            DataSet dataSet = new DataSet();
    //            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, conn);
    //            sqlDataAdapter.Fill(dataSet);

    //            set = dataSet;
    //            return dataSet.Tables[0].Rows.Count !=0;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //        finally
    //        {
    //            Close();
    //        }
    //    }
    //    public static bool Exists(string command)
    //    {
    //        try
    //        {
    //            Open();

    //            DataSet dataSet = new DataSet();
    //            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(command, conn);
    //            sqlDataAdapter.Fill(dataSet);

    //            return dataSet.Tables[0].Rows.Count != 0;
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //        finally
    //        {
    //            Close();
    //        }
    //    }
    //    public static void Execute(string command)
    //    {
    //        try
    //        {
    //            Open();
    //            var cmd = new SqlCommand(command, conn);
    //            cmd.ExecuteNonQuery();
    //        }
    //        catch (Exception)
    //        {

    //            throw;
    //        }
    //        finally
    //        {
    //            Close();
    //        }
    //    }
    //    private static void Close()
    //    {
    //        conn.Close();
    //    }
    //} 
    [Obsolete("请改用SqlExtension")]
    public static class MySqlUtil
    {
        private static MySqlConnection conn = null;
        private static void Open()
        {
            conn = new MySqlConnection
            {
                ConnectionString = Config.Conn
            };
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        private static DataTable Query(string command)
        {
            try
            {
                Open();

                DataSet dataSet = new DataSet();
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command, conn);
                sqlDataAdapter.Fill(dataSet);

                return dataSet.Tables[0];
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }
        private static void Close()
        {
            conn.Close();
        }

        public static bool TryQuery(string command, out DataTable table)
        {
            try
            {
                Open();

                DataSet dataSet = new DataSet();
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command, conn);
                sqlDataAdapter.Fill(dataSet);

                table = dataSet.Tables[0];
                return table.Rows.Count != 0;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }
        private static bool Exists(string command)
        {
            try
            {
                Open();

                DataSet dataSet = new DataSet();
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command, conn);
                sqlDataAdapter.Fill(dataSet);

                return dataSet.Tables[0].Rows.Count != 0;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }
        public static void Execute(string command)
        {
            try
            {
                Open();
                var cmd = new MySqlCommand(command, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }

        [Obsolete]
        public static bool TryQuery(IMySqlQueryable obj, out DataTable table)
        {
            bool result = TryQuery(obj.GetQuerycommand(), out DataTable table2);
            table = table2;
            return result;
        }
        [Obsolete]
        public static void Add(IMySqlQueryable obj) => Execute(obj.GetAddcommand());
        [Obsolete]
        public static bool Exists(IMySqlQueryable obj) => Exists(obj.GetQuerycommand());
    }

    [Obsolete("请使用ISqlObject")]
    public interface IMySqlQueryable
    {
        void Set(DataRow row);
        string GetAddcommand();
        string GetQuerycommand();
    }

    public abstract class SqlBaseProvider
    {
        protected SqlBaseProvider(IDbConnection conn)
        {
            Conn = conn;
        }

        protected IDbConnection Conn { get; set; }

        protected abstract void Open();
        public abstract DataTable Query(string command);
        protected abstract void Close();
        public abstract void Execute(string command);

        public bool Exists(string command)
        {
            var table = Query(command);
            if (table.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool TryQuery(string command, out DataTable table)
        {
            table = Query(command);
            if (table.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// 提供<see cref="ISqlObject"/>依赖的MySql交互实现。
    /// </summary>
    public class MySqlProvider : SqlBaseProvider
    {
        public MySqlProvider(IDbConnection conn) : base(conn)
        {
        }

        protected override void Open()
        {
            if (Conn.State == ConnectionState.Closed)
            {
                Conn.Open();
            }
        }
        protected override void Close() => Conn.Close();
        public override void Execute(string command)
        {
            try
            {
                Open();
                var cmd = new MySqlCommand(command, (MySqlConnection)Conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                Close();
            }
        }
        public override DataTable Query(string command)
        {
            try
            {
                Open();

                DataSet dataSet = new DataSet();
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(command, (MySqlConnection)Conn);
                sqlDataAdapter.Fill(dataSet);

                return dataSet.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Close();
            }
        }
    }

    /// <summary>
    /// 表示一个Sql对象，用反射的方式和数据库交互。
    /// </summary>
    public interface ISqlObject
    {
        /// <summary>
        /// 提供Sql的相关信息及操作。
        /// </summary>
        SqlBaseProvider SqlProvider { get; }
        /// <summary>
        /// 数据表。
        /// </summary>
        string Table { get; }
    }

    /// <summary>
    /// 表示其为一个<see cref="ISqlObject"/>的一个元素，实现和MySql的交互。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SqlElementAttribute : Attribute
    {
        public SqlElementAttribute(string name = null,bool isreadonly = false)
        {
            Name = name;
            IsReadOnly = isreadonly;
        }
        public string Name { get; }
        public bool IsReadOnly { get; }
    }

    /// <summary>
    /// 表示将一个属性绑定到一个命名Update方法。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public sealed class SqlBindingAttribute : Attribute
    {
        public SqlBindingAttribute(string funcName)
        {
            FuncName = funcName;
        }

        public string FuncName { get; }
    }

    /// <summary>
    /// 表示该属性是查询的主键。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class SqlSearchKeyAttribute : Attribute
    {
        public SqlSearchKeyAttribute()
        {
        }
    }

    /// <summary>
    /// 表示该属性需要加密。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class SqlEncryptAttribute : Attribute
    {
        public SqlEncryptAttribute()
        {
        }
    }

    public class NameValue
    {
        public NameValue(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
    public class NameValueList : IList<NameValue>
    {
        List<NameValue> vs = new List<NameValue>();

        public NameValue this[int index] { get => ((IList<NameValue>)vs)[index]; set => ((IList<NameValue>)vs)[index] = value; }
        public int Count => ((IList<NameValue>)vs).Count;
        public bool IsReadOnly => ((IList<NameValue>)vs).IsReadOnly;
        public void Add(NameValue item)
        {
            ((IList<NameValue>)vs).Add(item);
        }

        public void Clear()
        {
            ((IList<NameValue>)vs).Clear();
        }
        public bool Contains(NameValue item)
        {
            return ((IList<NameValue>)vs).Contains(item);
        }
        public void CopyTo(NameValue[] array, int arrayIndex)
        {
            ((IList<NameValue>)vs).CopyTo(array, arrayIndex);
        }
        public IEnumerator<NameValue> GetEnumerator()
        {
            return ((IList<NameValue>)vs).GetEnumerator();
        }
        public int IndexOf(NameValue item)
        {
            return ((IList<NameValue>)vs).IndexOf(item);
        }
        public void Insert(int index, NameValue item)
        {
            ((IList<NameValue>)vs).Insert(index, item);
        }
        public bool Remove(NameValue item)
        {
            return ((IList<NameValue>)vs).Remove(item);
        }
        public void RemoveAt(int index)
        {
            ((IList<NameValue>)vs).RemoveAt(index);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<NameValue>)vs).GetEnumerator();
        }

        public void Add(string name, string value) => Add(new NameValue(name, value));
        public List<string> Names
        {
            get
            {
                List<string> ar = new List<string>();
                foreach (var item in vs)
                {
                    ar.Add(item.Name);
                }
                return ar;
            }
        }
        public List<string> Values
        {
            get
            {
                List<string> ar = new List<string>();
                foreach (var item in vs)
                {
                    ar.Add(item.Value);
                }
                return ar;
            }
        }
    }

    /// <summary>
    /// 提供<see cref="ISqlObject"/>的扩展方法，包括添加记录，查询并更新。
    /// </summary>
    public static class SqlExtension
    {
        /// <summary>
        /// 添加到数据库。
        /// </summary>
        /// <param name="obj"></param>
        public static void Add(this ISqlObject obj)
        {
            //获取成员列表。
            NameValueList vs = new NameValueList();
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.CanWrite && property.CanRead && property.TryGetSqlElementName(out string name,out bool isreadonly))
                {
                    if (property.GetValue(obj) !=null && !isreadonly)
                    {
                        //加密。
                        vs.Add(name, GetSqlValue(property.GetValue(obj), property.IsSqlEncrypt()));
                    }
                }
            }
            //生成命令语句。
            //string cmd = $"insert into {obj.Table} set  {(string.Join(',', vs.Map((m) => $"{m.Name}={m.Value}")))}";

            string cmd = $"insert into {obj.Table} ({ToolUtil.JoinString(',',vs.Names)}) values ({ToolUtil.JoinString(',',vs.Values)})";

            obj.SqlProvider.Execute(cmd);
        }
        /// <summary>
        /// 根据<see cref="SqlSearchKeyAttribute"/>将数据库相应行中<see cref="SqlBindingAttribute"/>绑定的属性更新。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="binding"></param>
        public static void Update(this ISqlObject obj, string binding)
        {
            //获取成员列表。
            NameValueList vs = new NameValueList();
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.CanWrite && property.CanRead && property.TryGetSqlElementName(out string name) && property.SqlBindingExists(binding))
                {
                    //加密。
                    vs.Add(name, GetSqlValue(property.GetValue(obj), property.IsSqlEncrypt()));
                }
            }
            //生成命令语句。
            string cmd = $"update {obj.Table} set " +
                string.Join(',', vs.Map((m) => $"{m.Name}={m.Value}")) + " " +
                obj.GetWhereExpression();
            obj.SqlProvider.Execute(cmd);
        }
        /// <summary>
        /// 尝试查询，如果成功，就将<see cref="SqlElementAttribute"/>标记的属性更新为数据库中的值。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool TryQuery(this ISqlObject obj)
        {
            string cmd = $"select * from {obj.Table} {obj.GetWhereExpression()}";

            var table = obj.SqlProvider.Query(cmd);

            if (table.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                obj.SetValue(table.Rows[0]);
                return true;
            }
        }
        /// <summary>
        /// 跟据查询语句查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query">指where后的那一部分</param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryQuery<T>(string query, out List<T> result) where T : class, ISqlObject, new()
        {
            T obj = new T();
            string cmd = $"select * from {obj.Table} where {query}";
            var table = obj.SqlProvider.Query(cmd);
            if (table.Rows.Count == 0)
            {
                result = null;
                return false;
            }
            else
            {
                List<T> result1 = new List<T>();
                foreach (DataRow item in table.Rows)
                {
                    T example = new T();
                    example.SetValue(item);
                    result1.Add(example);
                }

                result = result1;
                return true;
            }
        }
        /// <summary>
        /// 根据键值对查询。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool TryQuery<T>(string name, object value, out List<T> result) where T : class, ISqlObject, new()
        {
            T obj = new T();
            bool isencrypt = obj.GetType().GetProperty(name).IsSqlEncrypt();
            string cmd = $"select * from {obj.Table} {GetWhereExpression(name, value, isencrypt)}";
            var table = obj.SqlProvider.Query(cmd);
            if (table.Rows.Count == 0)
            {
                result = null;
                return false;
            }
            else
            {
                List<T> result1 = new List<T>();
                foreach (DataRow item in table.Rows)
                {
                    T example = new T();
                    example.SetValue(item);
                    result1.Add(example);
                }

                result = result1;
                return true;
            }
        }
        public static T GetLastRecord<T>() where T : class, ISqlObject, new()
        {
            T obj = new T();
            string cmd = $"select * from {obj.Table} order by ID DESC limit 1";
            var table = obj.SqlProvider.Query(cmd);
            if (table.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                T example = new T();
                example.SetValue(table.Rows[0]);

                return example;
            }
        }
        public static List<T> GetAllItem<T>() where T : class, ISqlObject, new()
        {
            T obj = new T();
            string cmd = $"select * from {obj.Table} order by ID DESC limit 1";
            var table = obj.SqlProvider.Query(cmd);
            if (table.Rows.Count == 0)
            {
                return new List<T>();
            }
            else
            {
                List<T> result1 = new List<T>();
                foreach (DataRow item in table.Rows)
                {
                    T example = new T();
                    example.SetValue(item);
                    result1.Add(example);
                }

                return result1;
            }
        }

        public static bool Exists(this ISqlObject obj)
        {
            string cmd = $"select * from {obj.Table} {obj.GetWhereExpression()}";

            var table = obj.SqlProvider.Query(cmd);

            return table.Rows != null && table.Rows.Count != 0;
        }

        private static string GetWhereExpression(this ISqlObject obj)
        {
            string name = null;
            object value = null;
            bool isencrypt = false;
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.CanWrite && property.CanRead && property.GetCustomAttribute<SqlSearchKeyAttribute>() != null && property.TryGetSqlElementName(out string name1))
                {
                    name = name1;
                    value = property.GetValue(obj);
                    isencrypt = property.IsSqlEncrypt();
                    break;
                }
            }

            if (name != null)
            {
                return GetWhereExpression(name, value, isencrypt);
            }
            else
            {
                throw new KeyNotFoundException();

            }
        }
        private static string GetWhereExpression(string name, object value, bool isencrypt)
        {
            if (value is string str)
            {
                return $"where {name} like {GetSqlValue(str, isencrypt)}";
            }
            else
            {
                return $"where {name}={value}";
            }
        }
        private static bool TryGetSqlElementName(this PropertyInfo obj, out string name,out bool isreadonly)
        {
            var attribute = obj.GetCustomAttribute<SqlElementAttribute>();
            //SOLVED BUG 曾导致程序因为没有SqlElement特性而崩溃。
            if (attribute != null)
            {
                if (attribute.Name == null)
                {
                    name = obj.Name;
                }
                else
                {
                    name = attribute.Name;
                }

                isreadonly = attribute.IsReadOnly;
                return true;
            }
            else
            {
                name = null;
                isreadonly = false;
                return false;
            }
        }
        private static bool TryGetSqlElementName(this PropertyInfo obj, out string name)
        {
            var result = TryGetSqlElementName(obj, out string _name, out bool _isreadonly);

            name = _name;
            return result;
        }
        private static bool SqlBindingExists(this PropertyInfo obj, string binding)
        {
            var attributes = obj.GetCustomAttributes<SqlBindingAttribute>();
            foreach (var attribute in attributes)
            {
                if (attribute.FuncName == binding)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsSqlEncrypt(this PropertyInfo obj)
        {
            var attribute = obj.GetCustomAttribute<SqlEncryptAttribute>();
            if (attribute != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static void SetValue(this ISqlObject obj, DataRow row)
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.CanRead && property.CanWrite && property.TryGetSqlElementName(out string name))
                {
                    if (property.PropertyType == typeof(string))
                    {
                        bool isencrypt = property.IsSqlEncrypt();
                        property.SetValue(obj, GetRowValue((string)row[name], isencrypt));
                    }
                    else
                    {
                        if (row[name]!= DBNull.Value)
                        {
                            property.SetValue(obj, row[name]);
                        }
                    }
                }
            }

        }
        public static void SetValue<T>(this T obj, T other) where T : ISqlObject, new()
        {
            foreach (var property in obj.GetType().GetProperties())
            {
                if (property.CanRead && property.CanWrite && property.TryGetSqlElementName(out string name))
                {
                    property.SetValue(obj, property.GetValue(other));
                }
            }
        }

        /// <summary>
        /// 获取处理后的字符串值。
        /// </summary>
        /// <param name="obj">待处理的对象</param>
        /// <param name="isEncrypt">是否加密(仅限字符串类型)</param>
        /// <returns></returns>
        private static string GetSqlValue(object obj, bool isEncrypt)
        {
            if (obj is string str)
            {
                if (isEncrypt)
                {
                    return $"'{Config.Aes.Encrypt(str)}'";
                }
                else
                {
                    return $"'{str}'";
                }
            }
            else
            {
                if (obj == null)
                {
                    return null;
                }
                else
                {
                    return obj.ToString();
                }
            }

        }
        /// <summary>
        /// 获取处理前的字符串值。
        /// </summary>
        /// <param name="obj">待处理的对象</param>
        /// <param name="isDecrypt">是否解密(仅限字符串类型)</param>
        /// <returns></returns>
        private static string GetRowValue(string value, bool isDecrypt)
        {
            if (isDecrypt)
            {
                return Config.Aes.Decrypt(value);
            }
            else
            {
                return value;
            }
        }
    }
}
