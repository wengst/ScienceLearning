using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace LearnLibs
{
    /// <summary>
    /// 常用函数
    /// </summary>
    public static class F
    {
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }

        /// <summary>
        /// 获取类型的数据表名称。
        /// <para>如果类型设置了DbTable特性，则返回DbTable特性的Name属性</para>
        /// <para>如果类型没有DbTable特性，则返回类型名+"s"</para>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTableName(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (type.BaseType != typeof(BaseModel)) throw new NotInheritBaseModelException(type);
            object[] attrs = type.GetCustomAttributes(true);
            if (attrs != null && attrs.Length > 0)
            {
                for (int i = 0; i < attrs.Length; i++)
                {
                    object attr = attrs[i];
                    if (attr.GetType() == typeof(DbTableAttribute))
                    {
                        return ((DbTableAttribute)attr).TableName;
                    }
                }
            }
            return type.Name + "s";
        }
        public static string GetTypeName(string tableName)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            Type[] types = ass.GetTypes();
            foreach (Type t in types)
            {
                if (BaseModel.IsSubclass(t) && GetTableName(t) == tableName)
                {
                    return t.Name;
                }
            }
            return string.Empty;
        }
        public static DbColumnAttribute GetDbColumn(PropertyInfo p)
        {
            if (p == null) return null;
            object[] attrs = p.GetCustomAttributes(true);
            for (int i = 0; i < attrs.Length; i++)
            {
                if (attrs[i].GetType() == typeof(DbColumnAttribute))
                {
                    return attrs[i] as DbColumnAttribute;
                }
            }
            return null;
        }
        public static PropertyInfo GetProperty(Type type, string fieldName)
        {
            PropertyInfo rp = null;
            if (type != null && BaseModel.IsSubclass(type) && !string.IsNullOrWhiteSpace(fieldName))
            {
                PropertyInfo[] ps = type.GetProperties();
                foreach (PropertyInfo p in ps)
                {
                    if (rp == null)
                    {
                        object[] attrs = p.GetCustomAttributes(true);
                        foreach (object a in attrs)
                        {
                            if (a.GetType() == typeof(DbColumnAttribute))
                            {
                                DbColumnAttribute d = a as DbColumnAttribute;
                                if (d.FieldName == fieldName)
                                {
                                    rp = p;
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        break;
                    }
                }
            }
            return rp;
        }

        public static string GetDbTypeStringForSqlite(DbType type)
        {
            switch (type)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                case DbType.Xml:
                    return "TEXT";
                case DbType.Binary:
                case DbType.Object:
                case DbType.Guid:
                    return "BLOB";
                case DbType.Boolean:
                case DbType.Byte:
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.SByte:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                    return "INTEGER";
                default:
                    return "NUMERIC";
            }
        }

        /// <summary>
        /// DbType枚举值是否数字类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public static bool IsDigital(DbType dbType)
        {
            switch (dbType)
            {
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                case DbType.UInt16:
                case DbType.UInt32:
                case DbType.UInt64:
                case DbType.Boolean:
                case DbType.Byte:
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.Double:
                case DbType.SByte:
                case DbType.Single:
                case DbType.VarNumeric:
                    return true;
                default:
                    return false;
            }
        }

        public static string FormatDateTime(DateTime? dt)
        {
            if (dt != null)
            {
                double d = Math.Abs((DateTime.Now - dt.Value).TotalDays);
                if (d > 365)
                {
                    return dt.Value.ToString("d").Replace(" ", "");
                }
                else if (d <= 365 && d >= 1)
                {
                    return dt.Value.Month.ToString() + "月" + dt.Value.Day.ToString() + " " + string.Format("{0:HH:mm}", dt);
                }
                else
                {
                    return string.Format("{0:HH:mm}", dt);
                }
            }
            else
            {
                return "UnKnow";
            }
        }
        public static void RemoveEvent<T>(T c, string name)
        {
            Delegate[] invokeList = GetObjectEventList(c, name);
            if (invokeList != null)
            {
                foreach (Delegate del in invokeList)
                {
                    Console.WriteLine(del.ToString());
                    typeof(T).GetEvent(name).RemoveEventHandler(c, del);
                }
            }
        }

        ///  <summary>     
        /// 获取对象事件 zgke@sina.com qq:116149     
        ///  </summary>     
        ///  <param name="p_Object">对象 </param>     
        ///  <param name="p_EventName">事件名 </param>     
        ///  <returns>委托列 </returns>     
        public static Delegate[] GetObjectEventList(object p_Object, string p_EventName)
        {
            EventInfo[] events = p_Object.GetType().GetEvents(BindingFlags.Public | BindingFlags.NonPublic);
            if (events != null && events.Length > 0)
            {
                foreach (EventInfo e in events)
                {
                    Console.WriteLine(e.Name);
                    Console.WriteLine(e.GetRaiseMethod().Name);
                    Console.WriteLine(e.GetAddMethod().Name);
                }
            }
            return null;
        }

        /// <summary>
        /// 是否是指定长度的整型数字字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static bool IsIntegerNumber(string s, int l)
        {
            if (string.IsNullOrWhiteSpace(s)) return false;
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[0-9]{" + l.ToString() + "}");
            return reg.IsMatch(s);
        }

        public static T GetValue<T>(DataRow r, string fn, T defaultValue)
        {
            if (r == null ||
                string.IsNullOrWhiteSpace(fn) ||
                r.Table == null ||
                !r.Table.Columns.Contains(fn) ||
                r.IsNull(fn))
            { return defaultValue; }
            try
            {
                return (T)r[fn];
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
        public static void SetValue(ref DataRow r, string fn, object value)
        {
            if (r == null || string.IsNullOrWhiteSpace(fn) || value == null || r.Table == null || !r.Table.Columns.Contains(fn)) return;
            try
            {
                r[fn] = value;
            }
            catch { }
        }
        public static void SetValues(ref DataRow r, Dictionary<string, object> values)
        {
            if (r != null && values != null && values.Count > 0)
            {
                foreach (KeyValuePair<string, object> item in values)
                {
                    SetValue(ref r, item.Key, item.Value);
                }
            }
        }
    }
}
