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
        /// 返回两字符串用指定字符串连接后的新字符串。如果前字符串空，则返回后字符串。
        /// </summary>
        /// <param name="frontStr"></param>
        /// <param name="backStr"></param>
        /// <param name="seq"></param>
        /// <returns></returns>
        public static string JoinString(string frontStr, string backStr, string seq)
        {
            if (!string.IsNullOrWhiteSpace(backStr) && seq != null && seq != string.Empty && seq != "")
            {
                if (string.IsNullOrWhiteSpace(frontStr))
                {
                    return backStr;
                }
                else
                {
                    return frontStr + seq + backStr;
                }
            }
            return frontStr;
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
