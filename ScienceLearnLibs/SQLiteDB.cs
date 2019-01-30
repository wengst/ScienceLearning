using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace LearnLibs
{
    /// <summary>
    /// 负责处理数据本地存储的静态类型
    /// </summary>
    public static class SQLiteDB
    {
        public static void CreatedDatabase(string dbfile)
        {
            if (File.Exists(dbfile))
            {
                File.Delete(dbfile);
            }
            SQLiteConnection.CreateFile(dbfile);
            string connStr = @"DataSource=" + dbfile + ";Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(connStr))
            {
                if (conn.State != ConnectionState.Open) conn.Open();
                Assembly ass = Assembly.GetExecutingAssembly();
                Type[] types = ass.GetTypes();
                string createSql = "";
                foreach (Type t in types)
                {
                    if (BaseModel.IsSubclass(t))
                    {
                        string fields = "";
                        string tableName = F.GetTableName(t);
                        PropertyInfo[] pfs = t.GetProperties();
                        List<ModelDbItem> items = new List<ModelDbItem>();
                        foreach (PropertyInfo p in pfs)
                        {
                            items.Add(new ModelDbItem(p));
                        }
                        items.Sort();
                        foreach (ModelDbItem item in items)
                        {
                            Type pt = item.Property.PropertyType;
                            if (string.IsNullOrWhiteSpace(fields))
                            {
                                fields = item.FieldName;
                            }
                            else
                            {
                                fields += "," + item.FieldName;
                            }
                            switch (item.DataType)
                            {
                                case DbType.Guid:
                                    fields += " GUID";
                                    break;
                                case DbType.AnsiString:
                                    fields += " VARCHAR(" + item.Size.ToString() + ")";
                                    break;
                                case DbType.AnsiStringFixedLength:
                                    fields += " CHAR(" + item.Size.ToString() + ")";
                                    break;
                                case DbType.Boolean:
                                    fields += " BOOLEAN";
                                    break;
                                case DbType.Byte:
                                    fields += " BYTE";
                                    break;
                                case DbType.Date:
                                case DbType.DateTime:
                                case DbType.DateTime2:
                                case DbType.DateTimeOffset:
                                case DbType.Time:
                                    fields += " DATETIME";
                                    break;
                                case DbType.Int16:
                                case DbType.UInt16:
                                    fields += " INT16";
                                    break;
                                case DbType.Int32:
                                case DbType.UInt32:
                                    fields += " INT32";
                                    break;
                                case DbType.Int64:
                                case DbType.UInt64:
                                    fields += " INT64";
                                    break;
                                case DbType.String:
                                    fields += " NVARCHAR(" + item.Size.ToString() + ")";
                                    break;
                                case DbType.StringFixedLength:
                                    fields += " NCHAR(" + item.Size.ToString() + ")";
                                    break;
                                case DbType.VarNumeric:
                                case DbType.Single:
                                case DbType.Double:
                                case DbType.Decimal:
                                case DbType.Currency:
                                    fields += " DOUBLE";
                                    break;
                                case DbType.Binary:
                                case DbType.Object:
                                    fields += " BLOB";
                                    break;
                            }
                            if (item.IsPrimaryKey)
                            {
                                fields += " PRIMARY KEY NOT NULL UNIQUE";
                            }
                            if (item.DataType != DbType.Guid && item.Column.IsAllowNull)
                            {
                                fields += " NULL";
                            }
                            else if (!item.Column.IsAllowNull)
                            {
                                fields += " NOT NULL";
                                switch (item.DataType)
                                {
                                    case DbType.AnsiString:
                                    case DbType.String:
                                        if (item.Column.DefaultValue == null)
                                        {
                                            fields += " DEFAULT('')";
                                        }
                                        else
                                        {
                                            fields += " DEFAULT('" + item.Column.DefaultValue.ToString() + "')";
                                        }
                                        break;
                                    case DbType.AnsiStringFixedLength:
                                    case DbType.StringFixedLength:
                                        if (item.Column.DefaultValue == null)
                                        {
                                            fields += " DEFAULT('" + "".PadRight(item.Size, '0') + "')";
                                        }
                                        else
                                        {
                                            fields += " DEFAULT('" + item.Column.DefaultValue.ToString().PadRight(item.Size, '0') + "')";
                                        }
                                        break;
                                    case DbType.Guid:
                                        fields += " DEFAULT('{" + Guid.Empty.ToString() + "}')";
                                        break;
                                    case DbType.Int16:
                                    case DbType.Int32:
                                    case DbType.Int64:
                                        if (item.Column.DefaultValue == null)
                                        {
                                            fields += " DEFAULT(0)";
                                        }
                                        else
                                        {
                                            fields += " DEFAULT(" + item.Column.DefaultValue.ToString() + ")";
                                        }
                                        break;
                                    case DbType.DateTime:
                                        if (item.Column.DefaultValue == null)
                                        {
                                            fields += " DEFAULT(DATETIME('NOW'))";
                                        }
                                        else
                                        {
                                            fields += " DEFAULT(DATETIME('" + item.Column.DefaultValue.ToString() + "','localtime'))";
                                        }
                                        break;
                                    case DbType.Boolean:
                                        if (item.Column.DefaultValue == null)
                                        {
                                            fields += " DEFAULT(0)";
                                        }
                                        else
                                        {
                                            fields += " DEFAULT(" + ((bool)item.Column.DefaultValue ? "1" : "0") + ")";
                                        }
                                        break;
                                }
                            }
                        }
                        if (string.IsNullOrWhiteSpace(createSql))
                        {
                            createSql = string.Format("CREATE TABLE {0}({1})", tableName, fields);
                        }
                        else
                        {
                            createSql += ";" + string.Format("CREATE TABLE {0}({1})", tableName, fields);
                        }
                    }
                }
                using (SQLiteCommand cmd = new SQLiteCommand(createSql, conn))
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "VACUUM";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
