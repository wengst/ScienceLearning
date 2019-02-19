using LearnLibs.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using LearnLibs.Controls;
using System.Xml;

namespace LearnLibs
{
    public static class ModelDbSet
    {
        #region private fields
        static Dictionary<Type, ModelDb> _tts = null;
        static DisplayScenes _scenes = DisplayScenes.未设置;
        #endregion

        #region private motheds

        static Dictionary<string, Type> XmlTables
        {
            get
            {
                Dictionary<string, Type> tables = new Dictionary<string, Type>();
                foreach (KeyValuePair<Type, ModelDb> kv in ModelDbs)
                {
                    if (!string.IsNullOrWhiteSpace(kv.Value.XmlItemName))
                    {
                        tables.Add(kv.Value.XmlItemName, kv.Key);
                    }
                }
                return tables;
            }
        }

        static T getAttribute<T>(Type type) where T : Attribute
        {
            if (type == null) return null;
            object[] attrs = type.GetCustomAttributes(typeof(T), true);
            if (attrs != null && attrs.Length > 0)
            {
                return attrs[0] as T;
            }
            return null;
        }

        static T getAttribute<T>(PropertyInfo p) where T : Attribute
        {
            if (p == null) return null;
            object[] attrs = p.GetCustomAttributes(typeof(T), true);
            if (attrs != null && attrs.Length > 0)
            {
                return attrs[0] as T;
            }
            return null;
        }

        static string getGridColumnName<T>(int colIndex) where T : BaseModel, new()
        {
            ModelDb md = ModelDbs[typeof(T)];
            DataGridView grid = md.Grid;
            if (grid.Columns.Count > colIndex)
            {
                return grid.Columns[colIndex].DataPropertyName;
            }
            return string.Empty;
        }
        static string getGridColumnName(Type type, int colIndex)
        {
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                DataGridView grid = md.Grid;
                if (grid.Columns.Count > colIndex)
                {
                    return grid.Columns[colIndex].DataPropertyName;
                }
            }
            return string.Empty;
        }

        static ModelField getModelField<T>(int colIndex) where T : BaseModel, new()
        {
            string colName = getGridColumnName<T>(colIndex);
            if (!string.IsNullOrWhiteSpace(colName))
            {
                return ModelDbs[typeof(T)].ModelTableInfo[colName];
            }
            return null;
        }

        static ModelField getModelField(Type type, int colIndex)
        {
            if (isBaseModel(type))
            {
                string colName = getGridColumnName(type, colIndex);
                if (!string.IsNullOrWhiteSpace(colName))
                {
                    return ModelDbs[type].ModelTableInfo[colName];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取类型是否是BaseModel的派生类
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        static bool isBaseModel(Type t)
        {
            if (t == null) return false;
            return BaseModel.IsSubclass(t);
        }

        /// <summary>
        /// 获取本应用程序集中所有BaseModel的派生类
        /// </summary>
        static void getDerivedTypes()
        {
            _tts = new Dictionary<Type, ModelDb>();
            Assembly ass = Assembly.GetExecutingAssembly();
            Type[] types = ass.GetTypes();
            foreach (Type t in types)
            {
                if (isBaseModel(t))
                {
                    _tts.Add(t, new ModelDb(t));
                }
            }
            foreach (KeyValuePair<Type, ModelDb> kv in _tts)
            {
                ModelDb md = kv.Value;
                md.ModelTableInfo.MainTable = md.TableName;
                foreach (ModelDbItem m in md.Columns)
                {
                    bool dc = m.IsDisplayColumn;
                    bool fk = m.IsForeignKey;
                    md.ModelTableInfo.Add(m.FieldName);
                    if (dc && fk && isBaseModel(m.DisplayColumn.FromType) && isBaseModel(m.ForeignKey.Type) && m.ForeignKey.Type == m.DisplayColumn.FromType)
                    {
                        ModelDb om = ModelDbs[m.DisplayColumn.FromType];
                        md.ModelTableInfo.Add(m.FieldName, om.TableName, m.ForeignKey.Field, m.DisplayColumn.Field);
                    }
                }
                //Console.WriteLine(kv.Key.Name + "有" + md.PTC.List.Count.ToString());
            }
        }

        /// <summary>
        /// 获取模型数据操作集合
        /// </summary>
        static Dictionary<Type, ModelDb> ModelDbs
        {
            get
            {
                if (_tts == null)
                {
                    getDerivedTypes();
                }
                return _tts;
            }
        }

        /// <summary>
        /// 根据表名、Where条件填充DataSet
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where"></param>
        /// <param name="orderby"></param>
        static void fillRows(string tableName, string where, string orderby = null)
        {
            if (string.IsNullOrWhiteSpace(tableName)) return;
            string selectSql = "SELECT * FROM " + tableName;

            if (!string.IsNullOrWhiteSpace(where))
            {
                selectSql += " WHERE " + where;
            }
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                selectSql += " ORDER BY " + orderby;
            }
            if (string.IsNullOrWhiteSpace(selectSql)) return;
            using (SQLiteCommand cmd = new SQLiteCommand(selectSql, Connection))
            {
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd))
                {
                    sda.Fill(AppDataSet, tableName);
                }
            }
        }

        static void fillRows(Type type, string where, string orderby = null)
        {
            if (type == null) return;
            if (!BaseModel.IsSubclass(type)) return;
            string selectSql = getSelectSqlNoWhere(type);
            Console.WriteLine(selectSql);
            ModelDb md = ModelDbs[type];
            if (!string.IsNullOrWhiteSpace(where))
            {
                selectSql += " WHERE " + where;
            }
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                selectSql += " ORDER BY " + orderby;
            }
            using (SQLiteCommand cmd = new SQLiteCommand(selectSql, Connection))
            {
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd))
                {
                    sda.Fill(AppDataSet, md.TableName);
                    DataTable dt = AppDataSet.Tables[md.TableName];
                    if (dt.PrimaryKey == null || dt.PrimaryKey.Length == 0)
                    {
                        if (md.PrimaryKey != null)
                        {
                            DataColumn[] dcs = new DataColumn[md.PrimaryKey.Columns.Count];
                            for (int i = 0; i < md.PrimaryKey.Columns.Count; i++)
                            {
                                dcs[i] = dt.Columns[md.PrimaryKey.Columns[i].FieldName];
                            }
                            dt.PrimaryKey = dcs;
                        }
                    }
                }
            }
        }

        static void fillRows<T>(WhereArgs args, string orderby) where T : BaseModel, new()
        {
            Type type = typeof(T);
            string selectSql = getSelectSqlNoWhere(type);
            ModelDb md = ModelDbs[type];
            if (args != null)
            {
                selectSql += " WHERE " + args.Where;
            }
            if (!string.IsNullOrWhiteSpace(orderby))
            {
                selectSql += " ORDER BY " + orderby;
            }
            using (SQLiteCommand cmd = new SQLiteCommand(selectSql, Connection))
            {
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd))
                {
                    sda.Fill(AppDataSet, md.TableName);
                }
            }
        }

        /// <summary>
        /// 获取无Where条件表达式和Order表达式的Select语句
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string getSelectSqlNoWhere(Type type, JoinType joinType = JoinType.Left)
        {
            if (isBaseModel(type))
            {
                switch (joinType)
                {
                    case JoinType.Inner:
                        return ModelDbs[type].ModelTableInfo.InnerJoinSelectSql;
                    case JoinType.Right:
                        return ModelDbs[type].ModelTableInfo.RightJoinSelectSql;
                    case JoinType.Left:
                    default:
                        return ModelDbs[type].ModelTableInfo.LeftJoinSelectSql;
                }
            }
            return string.Empty;
        }

        static string getSelectSql<T>(JoinType joinType) where T : BaseModel, new()
        {
            switch (joinType)
            {
                case JoinType.Inner:
                    return ModelDbs[typeof(T)].ModelTableInfo.InnerJoinSelectSql;
                case JoinType.Right:
                    return ModelDbs[typeof(T)].ModelTableInfo.RightJoinSelectSql;
                case JoinType.Left:
                default:
                    return ModelDbs[typeof(T)].ModelTableInfo.LeftJoinSelectSql;
            }
        }

        /// <summary>
        /// 获取类型相关的OrderBy字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string getOrderStr(Type type)
        {
            string order = string.Empty;
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                if (md.HasOrderColumns)
                {
                    foreach (ModelDbItem dbItem in md.OrderColumns)
                    {
                        if (dbItem.OrderBy.OrderType != SortOrder.None)
                        {
                            order = F.JoinString(order, dbItem.GetOrder(), ",");
                        }
                    }
                }
            }
            return order;
        }

        static string getUpdateString(Type type)
        {
            string updateStr = string.Empty;
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                List<ModelDbItem> Columns = md.Columns;
                string tableName = md.TableName;
                string fields = string.Empty;
                string where = string.Empty;
                for (int i = 0; i < Columns.Count; i++)
                {
                    ModelDbItem dbItem = Columns[i];
                    if (dbItem.IsPrimaryKey)
                    {
                        where = F.JoinString(where, dbItem.FieldName + "=" + dbItem.ParameterName, ",");
                    }
                    else
                    {
                        fields = F.JoinString(fields, dbItem.FieldName + "=" + dbItem.ParameterName, ",");
                    }
                }
                updateStr = string.Format("UPDATE {0} SET {1} WHERE {2}", new object[] { tableName, fields, where });
            }
            return updateStr;
        }

        /// <summary>
        /// 获取主键参数集合
        /// </summary>
        /// <param name="type">BaseModel派生类</param>
        /// <returns></returns>
        static SQLiteParameter[] getPrimaryParameters(Type type)
        {
            SQLiteParameter[] parameters = null;
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                parameters = new SQLiteParameter[md.PrimaryKey.Columns.Count];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ModelDbItem dbItem = md.PrimaryKey.Columns[i];
                    parameters[i] = new SQLiteParameter();
                    parameters[i].ParameterName = dbItem.ParameterName;
                    parameters[i].DbType = dbItem.DataType;
                    parameters[i].Size = dbItem.Size;
                    parameters[i].SourceColumn = dbItem.FieldName;
                }
            }
            return parameters;
        }

        /// <summary>
        /// 根据类型构建BaseModel派生类的SQLite参数数组
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static SQLiteParameter[] getParamters(Type type)
        {
            SQLiteParameter[] sps = null;
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                List<ModelDbItem> Columns = md.Columns;
                sps = new SQLiteParameter[Columns.Count];
                for (int i = 0; i < Columns.Count; i++)
                {
                    sps[i] = new SQLiteParameter();
                    sps[i].ParameterName = Columns[i].ParameterName;
                    sps[i].DbType = Columns[i].DataType;
                    sps[i].Size = Columns[i].Size;
                    sps[i].SourceColumn = Columns[i].FieldName;
                }
            }
            return sps;
        }

        /// <summary>
        /// 获取类型的Insert字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string getInsertString(Type type)
        {
            string insertStr = string.Empty;
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                List<ModelDbItem> Columns = md.Columns;
                string tableName = md.TableName;
                string fields = "";
                string parameters = "";
                for (int i = 0; i < Columns.Count; i++)
                {
                    ModelDbItem dbItem = Columns[i];
                    fields = F.JoinString(fields, dbItem.FieldName, ",");
                    parameters = F.JoinString(parameters, dbItem.ParameterName, ",");
                }
                return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", new object[] { tableName, fields, parameters });
            }
            return insertStr;
        }

        /// <summary>
        /// 获取类型的Delete字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string getDeleteString(Type type)
        {
            string deleteStr = string.Empty;
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                List<ModelDbItem> Columns = md.PrimaryKey.Columns;
                string tableName = md.TableName;
                string where = "";
                for (int i = 0; i < Columns.Count; i++)
                {
                    ModelDbItem dbItem = Columns[i];
                    where = F.JoinString(where, dbItem.FieldName + "=" + dbItem.ParameterName, " AND ");
                }
                return string.Format("DELETE FROM {0} WHERE {1}", new object[] { tableName, where });
            }
            return deleteStr;
        }

        /// <summary>
        /// 构建创建类型表的sql语句字符串
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string getCreateTableString(Type type)
        {
            string createTableStr = string.Empty;
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                List<ModelDbItem> Columns = md.Columns;
                string tableName = md.TableName;
                int primaryKeyCount = md.PrimaryKey.Columns.Count;
                createTableStr = "CREATE TABLE " + tableName + "(";
                //构建表字段
                foreach (ModelDbItem dbItem in Columns)
                {
                    string s = dbItem.FieldName + " " + (dbItem.IsAutoIdentity ? "INTEGER" : F.GetDbTypeStringForSqlite(dbItem.DataType));
                    if (dbItem.IsPrimaryKey && primaryKeyCount == 1)
                    {
                        s += " PRIMARY KEY";
                    }
                    if (dbItem.IsAutoIdentity)
                    {
                        s += " AUTOINCREMENT";
                    }
                    if (dbItem.IsUnqiueKey)
                    {
                        s += " UNIQUE";
                    }
                    createTableStr = F.JoinString(createTableStr, s, ",");
                }
                //如果表主键包含多个字段
                if (md.HasPrimaryColumn && primaryKeyCount > 1)
                {
                    createTableStr += ",PRIMARY KEY(";
                    string primaryKeys = string.Empty;
                    foreach (ModelDbItem dbItem in md.PrimaryKey.Columns)
                    {
                        primaryKeys = F.JoinString(primaryKeys, dbItem.FieldName, ",");
                    }
                    createTableStr += primaryKeys + ")";
                }
                createTableStr += ")";
            }
            return createTableStr;
        }

        internal static SQLiteCommand InsertCommand(Type type)
        {
            SQLiteCommand cmd = null;
            if (isBaseModel(type))
            {
                cmd = new SQLiteCommand();
                cmd.Connection = Connection;
                cmd.CommandText = getInsertString(type);
                cmd.Parameters.AddRange(getParamters(type));
            }
            return cmd;
        }

        internal static SQLiteCommand UpdateCommand(Type type)
        {
            SQLiteCommand cmd = null;
            if (isBaseModel(type))
            {
                cmd = new SQLiteCommand(getUpdateString(type), Connection);
                cmd.Parameters.AddRange(getParamters(type));
            }
            return cmd;
        }

        internal static SQLiteCommand DeleteCommand(Type type)
        {
            SQLiteCommand cmd = null;
            if (isBaseModel(type))
            {
                cmd = new SQLiteCommand(getDeleteString(type), Connection);
                cmd.Parameters.AddRange(getPrimaryParameters(type));
            }
            return cmd;
        }

        static void setDataRowField(DataRow row, string field, string value, object defaultValue)
        {
            if (row == null || row.Table == null || string.IsNullOrWhiteSpace(field) || string.IsNullOrWhiteSpace(value)) return;
            DataTable dt = row.Table;
            if (dt.Columns.Contains(field))
            {
                Type t = dt.Columns[field].DataType;
                Console.WriteLine("setDataRowField " + t.Name);
                if (t == typeof(Guid))
                {
                    Guid guid;
                    if (Guid.TryParse(value, out guid))
                    {
                        row[field] = guid;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(string))
                {
                    row[field] = value;
                }
                else if (t == typeof(int))
                {
                    int n0 = 0;
                    if (int.TryParse(value, out n0))
                    {
                        row[field] = n0;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(double))
                {
                    double f0 = 0.0f;
                    if (double.TryParse(value, out f0))
                    {
                        row[field] = f0;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(Decimal))
                {
                    Decimal d1;
                    if (Decimal.TryParse(value, out d1))
                    {
                        row[field] = d1;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(DateTime))
                {
                    DateTime d0 = DateTime.Now;
                    if (DateTime.TryParse(value, out d0))
                    {
                        row[field] = d0;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(bool))
                {
                    bool b0 = false;
                    if (bool.TryParse(value, out b0))
                    {
                        row[field] = b0;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(Char))
                {
                    Char c0;
                    if (Char.TryParse(value, out c0))
                    {
                        row[field] = c0;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(Int16))
                {
                    Int16 n1;
                    if (Int16.TryParse(value, out n1))
                    {
                        row[field] = n1;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(Int64))
                {
                    Int64 n2;
                    if (Int64.TryParse(value, out n2))
                    {
                        row[field] = n2;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(UInt16))
                {
                    UInt16 un1;
                    if (UInt16.TryParse(value, out un1))
                    {
                        row[field] = un1;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(UInt32))
                {
                    UInt32 un2;
                    if (UInt32.TryParse(value, out un2))
                    {
                        row[field] = un2;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(UInt64))
                {
                    UInt64 un3;
                    if (UInt64.TryParse(value, out un3))
                    {
                        row[field] = un3;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(Single))
                {
                    Single s0;
                    if (Single.TryParse(value, out s0))
                    {
                        row[field] = s0;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(TimeSpan))
                {
                    TimeSpan ts0;
                    if (TimeSpan.TryParse(value, out ts0))
                    {
                        row[field] = ts0;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(SByte))
                {
                    SByte sb0;
                    if (SByte.TryParse(value, out sb0))
                    {
                        row[field] = sb0;
                    }
                    else
                    {
                        row[field] = defaultValue;
                    }
                }
                else if (t == typeof(byte[]))
                {
                    row[field] = System.Text.Encoding.Default.GetBytes(value);
                }
            }
        }

        static void setDataRowField(DataRow row, string field, object objValue, object defaultValue)
        {
            if (row == null || row.Table == null || string.IsNullOrWhiteSpace(field) || objValue == null) return;
            string value = objValue.ToString();
            setDataRowField(row, field, value, defaultValue);
        }
        #endregion

        #region public properties
        public static DisplayScenes Scenes { get { return _scenes; } set { _scenes = value; } }
        public static DataSet AppDataSet
        {
            get;
            set;
        }

        public static SQLiteConnection Connection { get; set; }
        #endregion

        #region public methods
        static bool dataRowFieldNotNull(DataRow row, string field)
        {
            if (row != null && !string.IsNullOrWhiteSpace(field) && row.Table != null && row.Table.Columns.Contains(field) && !row.IsNull(field) && row[field].GetType() != typeof(DBNull))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void updateForeignColumn(Type type, DataRow row)
        {
            if (isBaseModel(type) && row != null)
            {
                ModelDb md = ModelDbs[type];
                ModelTable mt = md.ModelTableInfo;
                DataTable dt = AppDataSet.Tables[md.TableName];
                if (dt != null && mt != null && mt.Fields.Count > 0)
                {
                    for (int colIndex = 0; colIndex < mt.Fields.Count; colIndex++)
                    {
                        ModelField mf = mt.Fields[colIndex];
                        if (mf.IsJoin && dataRowFieldNotNull(row, mf.SourceField))
                        {
                            object sourceValue = row[mf.SourceField];
                            if (!string.IsNullOrWhiteSpace(mf.JoinTableName) && AppDataSet.Tables.Contains(mf.JoinTableName))
                            {
                                WhereArgs args = new WhereArgs();
                                WhereArg arg = new WhereArg(mf.ForeignKeyField, sourceValue);
                                args.Add(arg);
                                DataRow[] rows = AppDataSet.Tables[mf.JoinTableName].Select(args.RowFilter);
                                if (rows != null && rows.Length > 0)
                                {
                                    string dfv = string.Empty;
                                    foreach (DataRow r in rows)
                                    {
                                        if (dataRowFieldNotNull(r, mf.DisplayField))
                                        {
                                            dfv = F.JoinString(dfv, r[mf.DisplayField].ToString(), ",");
                                        }
                                    }
                                    row[mf.AsField] = dfv;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static string GetCellValue<T>(int colIndex, int rowIndex) where T : BaseModel, new()
        {
            return GetCellValue(typeof(T), colIndex, rowIndex);
        }

        public static string GetCellValue(Type type, int colIndex, int rowIndex)
        {
            if (isBaseModel(type))
            {
                ModelField mf = getModelField(type, colIndex);
                if (mf != null && !string.IsNullOrWhiteSpace(mf.JoinTableName))
                {
                    DataGridView grid = ModelDbs[type].Grid;
                    object dataSource = grid.DataSource;
                    DataRow row = null;
                    if (dataSource != null)
                    {
                        Type dsType = dataSource.GetType();

                        if (dsType == typeof(DataTable))
                        {
                            row = ((DataTable)dataSource).Rows[rowIndex];
                        }
                        else if (dsType == typeof(DataView))
                        {
                            row = ((DataView)dataSource)[rowIndex].Row;
                        }
                    }
                    if (row != null && row.Table != null && row.Table.Columns.Contains(mf.SourceField))
                    {
                        object value = row[mf.SourceField];
                        if (value != null)
                        {
                            return GetFieldString(mf.JoinTableName, mf.DisplayField, mf.ForeignKeyField, value);
                        }
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 检查是否引用了某个外键的值
        /// </summary>
        /// <param name="checkingType">将要在此类型中检查是否包含某外键的数据</param>
        /// <param name="foreignKeyType">被检查的外键</param>
        /// <param name="foreignKeyValue">被检查的外键数据</param>
        /// <returns></returns>
        public static bool IsQuoteForeignKeyValue(Type checkingType, Type foreignKeyType, object foreignKeyValue)
        {
            if (isBaseModel(checkingType) && isBaseModel(foreignKeyType))
            {
                ModelDb me = ModelDbs[checkingType];
                List<ModelDbItem> foreignColumns = me.ForeignKeyColumns;
                string tableName = me.TableName;
                if (!AppDataSet.Tables.Contains(tableName)) return false;
                foreach (ModelDbItem dbitem in foreignColumns)
                {
                    if (dbitem != null && dbitem.ForeignKey.Type == foreignKeyType)
                    {
                        switch (dbitem.DataType)
                        {
                            case DbType.AnsiString:
                            case DbType.AnsiStringFixedLength:
                            case DbType.String:
                            case DbType.StringFixedLength:
                            case DbType.Xml:
                            case DbType.Date:
                            case DbType.DateTime:
                            case DbType.DateTime2:
                            case DbType.DateTimeOffset:
                                if (AppDataSet.Tables[tableName].Select(dbitem.FieldName + "='" + foreignKeyValue.ToString() + "'").Length > 0)
                                {
                                    return true;
                                }
                                break;
                            case DbType.Byte:
                            case DbType.SByte:
                            case DbType.Int16:
                            case DbType.Int32:
                            case DbType.Int64:
                            case DbType.UInt16:
                            case DbType.UInt32:
                            case DbType.UInt64:
                            case DbType.Single:
                            case DbType.Currency:
                            case DbType.Decimal:
                            case DbType.Double:
                                if (AppDataSet.Tables[tableName].Select(dbitem.FieldName + "=" + foreignKeyValue.ToString()).Length > 0)
                                {
                                    return true;
                                }
                                break;
                            case DbType.Guid:
                                if (AppDataSet.Tables[tableName].Select("Convert(" + dbitem.FieldName + ",'System.String')='" + ((Guid)foreignKeyValue).ToString() + "'").Length > 0)
                                {
                                    return true;
                                }
                                break;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取内存数据集中是否存在指定条件的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool RowIsExists<T>(WhereArgs args) where T : BaseModel, new()
        {
            Type t = typeof(T);
            ModelDb md = ModelDbs[t];
            if (args != null)
            {
                if (AppDataSet.Tables.Contains(md.TableName))
                {
                    DataTable dt = AppDataSet.Tables[md.TableName];
                    foreach (WhereArg arg in args.Items)
                    {
                        if (!dt.Columns.Contains(arg.Field))
                        {
                            throw new Exception("字段不存在");
                        }
                    }
                    Console.WriteLine(args.RowFilter);
                    return dt.Select(args.RowFilter).Length > 0;
                }
            }
            else
            {
                throw new ArgumentNullException("args");
            }
            return false;
        }

        public static bool RowIsExists(string tableName, WhereArgs args)
        {
            if (!string.IsNullOrWhiteSpace(tableName) && AppDataSet.Tables.Contains(tableName) && args != null)
            {
                DataTable dt = AppDataSet.Tables[tableName];
                foreach (WhereArg arg in args.Items)
                {
                    if (!dt.Columns.Contains(arg.Field))
                    {
                        throw new Exception("字段不存在");
                    }
                }
                return dt.Select(args.RowFilter).Length > 0;
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException("tableName");
            }
            if (args == null) throw new ArgumentNullException("args");
            return false;
        }

        public static DataView GetDataView(Type type, WhereArg where)
        {
            DataView dv = null;
            if (isBaseModel(type))
            {
                ModelDb md = ModelDbs[type];
                if (!AppDataSet.Tables.Contains(md.TableName))
                {
                    fillRows(type, where == null ? "" : where.Where);
                }
                if (where == null)
                {
                    dv = AppDataSet.Tables[md.TableName].DefaultView;
                }
                else
                {
                    dv = new DataView();
                    dv.Table = AppDataSet.Tables[md.TableName];
                    dv.RowFilter = where.RowFilter;
                }
            }
            return dv;
        }

        public static DataView GetDataView<T>(WhereArgs where) where T : BaseModel, new()
        {
            /*
             * 选用DataView做DataGridView和TreeView的数据源，不用DataTable。 
             */
            Type type = typeof(T);
            DataView dv = null;

            ModelDb md = ModelDbs[type];
            bool isexists = where == null ? false : RowIsExists<T>(where);
            if (!AppDataSet.Tables.Contains(md.TableName) || !isexists)
            {
                fillRows(type, where == null ? "" : where.Where);
            }
            if (where == null)
            {
                dv = AppDataSet.Tables[md.TableName].DefaultView;
            }
            else
            {
                dv = new DataView();
                dv.Table = AppDataSet.Tables[md.TableName];
                dv.RowFilter = where.RowFilter;
            }
            return dv;
        }

        public static void RefreshGrid<T>(WhereArgs args) where T : BaseModel, new()
        {
            ModelDb md = ModelDbs[typeof(T)];
            md.Args = args;
            md.Grid.DataSource = GetDataView<T>(args);
            md.Grid.Refresh();
        }

        public static void RefreshGrid<T>(string whereField, object whereValue) where T : BaseModel, new()
        {
            WhereArg arg = new WhereArg(whereField, whereValue);
            WhereArgs args = new WhereArgs() { arg };
            RefreshGrid<T>(args);
        }

        /// <summary>
        /// 获取表格中选定行的DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataRow GetSelectedDataRow<T>() where T : BaseModel, new()
        {
            Type t = typeof(T);
            DataRow row = null;
            if (isBaseModel(t))
            {
                DataGridView grid = ModelDbs[t].Grid;
                if (grid != null && grid.SelectedRows.Count > 0)
                {
                    if (grid.DataSource != null)
                    {
                        Type dataSourceType = grid.DataSource.GetType();
                        if (dataSourceType == typeof(DataTable))
                        {
                            row = (DataRow)grid.SelectedRows[0].DataBoundItem;
                        }
                        else if (dataSourceType == typeof(DataView))
                        {
                            row = ((DataRowView)grid.SelectedRows[0].DataBoundItem).Row;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请先选择欲编辑项所在的行");
                }
            }
            else
            {
                MessageBox.Show("泛型类必须是BaseModel的派生类", C.OperationPrompt, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return row;
        }

        /// <summary>
        /// 获取选定行的对象。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObjectAtGrid<T>() where T : BaseModel, new()
        {
            DataRow row = GetSelectedDataRow<T>();
            if (row != null)
            {
                return ToObj<T>(row);
            }
            else
            {
                return null;
            }
        }

        public static DataGridView GetDataGridView<T>() where T : BaseModel
        {
            ModelDb md = ModelDbs[typeof(T)];
            md.CurrentScenes = _scenes;
            return md.Grid;
        }

        /// <summary>
        /// 泛型实例化方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="r"></param>
        /// <returns></returns>
        public static T ToObj<T>(DataRow r) where T : BaseModel, new()
        {
            T t = null;
            ModelDb me = ModelDbs[typeof(T)];
            if (r != null && r.Table != null && r.Table.TableName == me.TableName)
            {
                t = new T();
                for (int i = 0; i < me.Columns.Count; i++)
                {
                    PropertyInfo p = me.Columns[i].Property;
                    Type pt = p.PropertyType;
                    string fn = me.Columns[i].FieldName;
                    if (pt == typeof(Guid))
                    {
                        p.SetValue(t, F.GetValue<Guid>(r, fn, Guid.Empty), null);
                    }
                    else if (pt == typeof(int))
                    {
                        p.SetValue(t, F.GetValue<int>(r, fn, 0), null);
                    }
                    else if (pt == typeof(bool))
                    {
                        p.SetValue(t, F.GetValue<bool>(r, fn, false), null);
                    }
                    else if (pt == typeof(string))
                    {
                        p.SetValue(t, F.GetValue<string>(r, fn, String.Empty), null);
                    }
                    else if (pt == typeof(float))
                    {
                        p.SetValue(t, F.GetValue<float>(r, fn, 0f), null);
                    }
                    else if (pt == typeof(DateTime))
                    {
                        p.SetValue(t, F.GetValue<DateTime>(r, fn, DateTime.Now), null);
                    }
                    else if (pt == typeof(UserRole))
                    {
                        p.SetValue(t, F.GetValue<UserRole>(r, fn, UserRole.未知), null);
                    }
                    else if (pt == typeof(SchoolType))
                    {
                        p.SetValue(t, F.GetValue<SchoolType>(r, fn, SchoolType.全日制学校), null);
                    }
                    else if (pt == typeof(SchoolGrades))
                    {
                        p.SetValue(t, F.GetValue<SchoolGrades>(r, fn, SchoolGrades.七年级), null);
                    }
                    else if (pt == typeof(Semesters))
                    {
                        p.SetValue(t, F.GetValue<Semesters>(r, fn, Semesters.第一学期), null);
                    }
                    else if (pt == typeof(EditState))
                    {
                        p.SetValue(t, F.GetValue<EditState>(r, fn, EditState.草稿), null);
                    }
                    else if (pt == typeof(ReleaseState))
                    {
                        p.SetValue(t, F.GetValue<ReleaseState>(r, fn, ReleaseState.未发布), null);
                    }
                    else if (pt == typeof(AnswerMode))
                    {
                        p.SetValue(t, F.GetValue<AnswerMode>(r, fn, AnswerMode.选择), null);
                    }
                }
            }
            return t;
        }

        public static T GetObject<T>(Guid id) where T : BaseModel, new()
        {
            WhereArg arg = new WhereArg(BaseModel.FN.Id, id);
            WhereArgs args = new WhereArgs() { arg };
            DataView dv = GetDataView<T>(args);
            if (dv != null && dv.Count > 0)
            {
                return ToObj<T>(dv[0].Row);
            }
            return null;
        }

        /// <summary>
        /// 类型实参实例化方法
        /// </summary>
        /// <param name="type"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static object GetInstance(Type type, DataRow r)
        {
            object obj = null;
            if (type != null && r != null)
            {
                if (isBaseModel(type))
                {
                    ModelDb md = ModelDbs[type];
                    if (r.Table != null && r.Table.TableName == md.TableName)
                    {
                        obj = Activator.GetObject(type, null);
                        foreach (ModelDbItem dbItem in md.Columns)
                        {
                            PropertyInfo p = dbItem.Property;
                            Type pt = p.PropertyType;
                            string fn = dbItem.FieldName;
                            if (pt == typeof(Guid))
                            {
                                p.SetValue(obj, F.GetValue<Guid>(r, fn, Guid.Empty), null);
                            }
                            else if (pt == typeof(int))
                            {
                                p.SetValue(obj, F.GetValue<int>(r, fn, 0), null);
                            }
                            else if (pt == typeof(bool))
                            {
                                p.SetValue(obj, F.GetValue<bool>(r, fn, false), null);
                            }
                            else if (pt == typeof(string))
                            {
                                p.SetValue(obj, F.GetValue<string>(r, fn, String.Empty), null);
                            }
                            else if (pt == typeof(float))
                            {
                                p.SetValue(obj, F.GetValue<float>(r, fn, 0f), null);
                            }
                            else if (pt == typeof(DateTime))
                            {
                                p.SetValue(obj, F.GetValue<DateTime>(r, fn, DateTime.Now), null);
                            }
                            else if (pt == typeof(UserRole))
                            {
                                p.SetValue(obj, F.GetValue<UserRole>(r, fn, UserRole.未知), null);
                            }
                            else if (pt == typeof(SchoolType))
                            {
                                p.SetValue(obj, F.GetValue<SchoolType>(r, fn, SchoolType.全日制学校), null);
                            }
                            else if (pt == typeof(SchoolGrades))
                            {
                                p.SetValue(obj, F.GetValue<SchoolGrades>(r, fn, SchoolGrades.七年级), null);
                            }
                            else if (pt == typeof(Semesters))
                            {
                                p.SetValue(obj, F.GetValue<Semesters>(r, fn, Semesters.第一学期), null);
                            }
                            else if (pt == typeof(EditState))
                            {
                                p.SetValue(obj, F.GetValue<EditState>(r, fn, EditState.草稿), null);
                            }
                            else if (pt == typeof(ReleaseState))
                            {
                                p.SetValue(obj, F.GetValue<ReleaseState>(r, fn, ReleaseState.未发布), null);
                            }
                            else if (pt == typeof(AnswerMode))
                            {
                                p.SetValue(obj, F.GetValue<AnswerMode>(r, fn, AnswerMode.选择), null);
                            }
                        }
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// 根据类型实例更新DataRow。如果属性值与对应字段值没有发生变化则不更新相应的字段
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static DataRow SaveDataRow<T>(T obj, bool isNew) where T : BaseModel, new()
        {
            DataRow r = null;
            if (obj != null)
            {
                Type t = obj.GetType();
                if (isBaseModel(t))
                {
                    ModelDb md = ModelDbs[t];
                    string TableName = md.TableName;
                    if (!AppDataSet.Tables.Contains(TableName))
                    {
                        fillRows<T>(null, null);
                    }
                    if (isNew)
                    {
                        r = AppDataSet.Tables[TableName].NewRow();
                    }
                    else
                    {
                        WhereArg arg = new WhereArg(BaseModel.FN.Id, obj.Id);
                        WhereArgs args = new WhereArgs() { arg };
                        if (RowIsExists<T>(args))
                        {
                            r = AppDataSet.Tables[md.TableName].Select(args.RowFilter)[0];
                        }
                    }
                    DataTable dt = AppDataSet.Tables[TableName];
                    if (dt != null)
                    {
                        List<ModelDbItem> TypeCols = md.Columns;
                        foreach (ModelDbItem typeCol in TypeCols)
                        {
                            if (dt.Columns.Contains(typeCol.FieldName))
                            {
                                object value = typeCol.Property.GetValue(obj, null);
                                setDataRowField(r, typeCol.FieldName, value, typeCol.Column.DefaultValue);
                            }
                        }
                        updateForeignColumn(t, r);
                    }
                    if (isNew)
                    {
                        AppDataSet.Tables[TableName].Rows.Add(r);
                    }
                    md.Grid.DataSource = GetDataView<T>(md.Args);
                }
            }
            return r;
        }

        public static string GetNodeName(DataRow row)
        {
            if (row == null) return string.Empty;
            return F.byteToHexStr(((Guid)row[BaseModel.FN.Id]).ToByteArray());
        }

        public static string GetNodeName<T>(T obj) where T : BaseModel, new()
        {
            if (obj == null) return string.Empty;
            return F.byteToHexStr(obj.Id.ToByteArray());
        }

        public static string GetNodeText<T>(T obj) where T : BaseModel, new()
        {
            if (obj == null) return string.Empty;
            string retStr = string.Empty;
            Type t = typeof(T);
            ModelDb md = ModelDbs[t];
            if (md.ListMember != null)
            {
                object textObj = md.ListMember.DisplayProperty.GetValue(obj, null);
                if (textObj != null)
                {
                    retStr = textObj.ToString();
                }
                else
                {
                    PropertyInfo[] pis = t.GetProperties();
                    foreach (PropertyInfo p in pis)
                    {
                        if (p.PropertyType == typeof(string))
                        {
                            object txtObj = p.GetValue(obj, null);
                            if (txtObj != null)
                            {
                                retStr = txtObj.ToString();
                                break;
                            }
                        }
                    }
                }
            }
            return retStr;
        }

        public static string GetNodeText<T>(DataRow row)
        {
            string retStr = string.Empty;
            Type t = typeof(T);
            if (isBaseModel(t) && row != null && row.Table != null)
            {
                ModelDb md = ModelDbs[t];
                if (md.ListMember != null)
                {
                    if (row.Table.Columns.Contains(md.ListMember.DisplayMember))
                    {
                        if (!row.IsNull(md.ListMember.DisplayMember))
                        {
                            retStr = row[md.ListMember.DisplayMember].ToString();
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(retStr))
                {
                    for (int i = 0; i < row.Table.Columns.Count; i++)
                    {
                        if (!row.IsNull(i))
                        {
                            retStr = row[i].ToString();
                            break;
                        }
                    }
                }
            }
            return retStr;
        }

        public static void AddNode<T>(TreeNode selectedNode, DataRow row) where T : BaseModel, new()
        {
            if (row != null && !row.IsNull(BaseModel.FN.Id) && selectedNode != null)
            {
                TreeNode node = CreateTreeNode<T>(row);
                node.ForeColor = System.Drawing.Color.Red;
                selectedNode.Nodes.Add(node);
            }
        }

        public static void RemoveNode<T>(TreeNodeCollection nodes, T obj) where T : BaseModel, new()
        {
            if (nodes != null && obj != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    TreeNode node = nodes[i];
                    T tagObj = (T)node.Tag;
                    if (tagObj == obj)
                    {
                        nodes.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public static TreeNode CreateTreeNode<T>(DataRow r) where T : BaseModel, new()
        {
            TreeNode node = null;
            Type t = typeof(T);
            if (isBaseModel(t))
            {
                ModelDb md = ModelDbs[t];
                node = new TreeNode();
                node.Name = GetNodeName(r);
                node.Text = GetNodeText<T>(r);
                node.Tag = ToObj<T>(r);
            }
            return node;
        }

        public static void UpdateNode<T>(TreeNode selectedNode, T newObj) where T : BaseModel, new()
        {
            if (selectedNode != null && newObj != null)
            {
                foreach (TreeNode node in selectedNode.Nodes)
                {
                    T tagObj = (T)node.Tag;
                    if (GetNodeName<T>(newObj) == node.Name)
                    {
                        node.Text = GetNodeText<T>(newObj);
                        node.Tag = newObj;
                        if (node.ForeColor == node.TreeView.ForeColor)
                        {
                            node.ForeColor = System.Drawing.Color.Blue;
                        }
                        break;
                    }
                }
            }
        }

        public static bool HasForeignKeyQuote(Type self, WhereArg arg)
        {
            if (isBaseModel(self))
            {
                foreach (KeyValuePair<Type, ModelDb> kv in ModelDbs)
                {
                    if (kv.Key != self && kv.Value.ContainField(arg.Field))
                    {
                        if (!AppDataSet.Tables.Contains(kv.Value.TableName))
                        {
                            fillRows(kv.Value.TableName, null);
                        }
                        DataView dv = GetDataView(kv.Key, arg);
                        if (dv != null && dv.Count > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool HasChild(Type self, Guid Id)
        {
            if (isBaseModel(self))
            {
                ModelDb me = ModelDbs[self];
                if (AppDataSet.Tables[me.TableName].Select("Convert(" + BaseModel.FN.ParentId + ",'System.String')='" + Id.ToString() + "'").Length > 0)
                {
                    return true;
                }
                using (SQLiteCommand cmd = new SQLiteCommand(Connection))
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM " + me.TableName + " WHERE UPPER(HEX(" + BaseModel.FN.ParentId + "))='" + F.byteToHexStr(Id.ToByteArray()) + "'";
                    int rows = (int)cmd.ExecuteScalar();
                    if (rows > 0) return true;
                }
            }
            return false;
        }

        public static void Update()
        {
            if (AppDataSet != null && Connection != null)
            {
                if (Connection.State == ConnectionState.Closed) Connection.Open();
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                {
                    foreach (KeyValuePair<Type, ModelDb> kv in ModelDbs)
                    {
                        if (AppDataSet.Tables[kv.Value.TableName] != null)
                        {
                            sda.InsertCommand = InsertCommand(kv.Key);
                            sda.UpdateCommand = UpdateCommand(kv.Key);
                            sda.DeleteCommand = DeleteCommand(kv.Key);
                            sda.Update(AppDataSet.Tables[kv.Value.TableName]);
                            AppDataSet.Tables[kv.Value.TableName].AcceptChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 显示模型编辑器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="selectedNode">选中的节点参数。该参数用于当数据更新时，可以同时更新该节点的子节点中的对应节点的数据。</param>
        /// <param name="isNew">新增=true，修改=false</param>
        public static void ShowEditor<T>(Form owner, TreeNode selectedNode, T obj, bool isUpdateChildNodes) where T : BaseModel, new()
        {
            if (obj == null) return;
            Type t = typeof(T);
            ModelDb md = ModelDbs[t];
            DataRow row = null;
            bool isNew = obj.Id == Guid.Empty;
            if (md.ModelEditor != null && md.ModelEditor.Editor.BaseType == typeof(FormDialog))
            {
                FormDialog f = (FormDialog)Activator.CreateInstance(md.ModelEditor.Editor, null);
                f.Object = obj;
                f.Owner = owner;
                f.StartPosition = FormStartPosition.CenterParent;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    obj = (T)f.Object;
                    if (!CheckData<T>(obj)) return;
                    if (isNew)
                    {
                        obj.Id = Guid.NewGuid();
                    }
                    row = SaveDataRow<T>(obj, isNew);
                    md.Grid.DataSource = GetDataView<T>(md.Args);
                    md.Grid.Refresh();
                    if (selectedNode != null && isUpdateChildNodes)
                    {
                        if (isNew)
                        {
                            AddNode<T>(selectedNode, row);
                        }
                        else
                        {
                            UpdateNode<T>(selectedNode, obj);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除指定项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes"></param>
        public static void Delete<T>(TreeNodeCollection nodes) where T : BaseModel, new()
        {
            Type t = typeof(T);
            ModelDb me = ModelDbs[t];
            if (me.Grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("请在表格中选择要删除的项", C.OperationPrompt, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataRow row = GetSelectedDataRow<T>();
            T obj = ToObj<T>(row);
            foreach (KeyValuePair<Type, ModelDb> kv in ModelDbs)
            {
                ModelDbItem frKey = kv.Value.GetForeignKey(t);
                if (frKey != null)
                {
                    WhereArg arg = new WhereArg(frKey.FieldName, row[frKey.ForeignKey.Field]);
                    DataView dv = GetDataView(kv.Key, arg);
                    if (dv != null && dv.Count > 0)
                    {
                        MessageBox.Show("该项至少被引用了一次，所以不能删除!");
                        return;
                    }
                }
            }
            row.Delete();
            me.Grid.Refresh();
            if (nodes != null)
            {
                RemoveNode<T>(nodes, obj);
            }
        }

        public static object GetFieldValue(string tableName, string fieldName, WhereArgs args)
        {
            if (!string.IsNullOrWhiteSpace(tableName) && !string.IsNullOrWhiteSpace(fieldName) && args != null)
            {
                if (!AppDataSet.Tables.Contains(tableName))
                {
                    fillRows(tableName, args.Where);
                }
                DataTable dt = AppDataSet.Tables[tableName];
                if (dt.Columns.Contains(fieldName))
                {
                    DataRow[] rows = dt.Select(args.RowFilter);
                    if (rows != null && rows.Length > 0)
                    {
                        DataRow row = rows[0];
                        if (row.IsNull(fieldName))
                        {
                            return null;
                        }
                        else
                        {
                            return row[fieldName];
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 根据WhereArgs查询参数，获取泛型所对应表的相应字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object GetFieldValue<T>(string field, WhereArgs args) where T : BaseModel, new()
        {
            if (!string.IsNullOrWhiteSpace(field) && args != null)
            {
                ModelDb md = ModelDbs[typeof(T)];
                return GetFieldValue(md.TableName, field, args);
            }
            return null;
        }

        public static string GetFieldString<T>(string field, WhereArgs args) where T : BaseModel, new()
        {
            object value = GetFieldValue<T>(field, args);
            if (value != null)
            {
                return value.ToString();
            }
            else
            {
                return "";
            }
        }

        public static string GetFieldString<T>(string returnField, string findField, object findValue) where T : BaseModel, new()
        {
            return GetFieldString<T>(returnField, new WhereArgs() { new WhereArg(findField, findValue) });
        }

        public static string GetFieldString(string tableName, string returnField, string findField, object findValue)
        {
            if (!string.IsNullOrWhiteSpace(tableName) && !string.IsNullOrWhiteSpace(returnField) && findValue != null)
            {
                WhereArgs args = new WhereArgs();
                args.Add(new WhereArg(findField, findValue));
                object value = GetFieldValue(tableName, returnField, args);
                if (value != null)
                {
                    return value.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// 创建SQLite数据库
        /// </summary>
        /// <param name="dbfile"></param>
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
                string createSql = string.Empty;
                foreach (KeyValuePair<Type, ModelDb> kv in ModelDbs)
                {
                    string fields = "";
                    string tableName = kv.Value.TableName;
                    List<ModelDbItem> Columns = kv.Value.Columns;
                    foreach (ModelDbItem item in Columns)
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
                using (SQLiteCommand cmd = new SQLiteCommand(createSql, conn))
                {
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "VACUUM";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool CheckData<T>(T obj) where T : BaseModel, new()
        {
            return true;
        }

        public static bool CheckData<T>(DataRow row) where T : BaseModel, new()
        {
            return true;
        }

        public static ListItemAttribute GetListItem<T>() where T : BaseModel, new()
        {
            ModelDb md = ModelDbs[typeof(T)];
            return md.ListMember;
        }
        #endregion

        #region public controls mothods
        public static T GetSelectedModel<T>(ComboBox cmb) where T : BaseModel, new()
        {
            T obj = null;
            ModelDb md = ModelDbs[typeof(T)];
            if (cmb.DataSource != null && cmb.SelectedIndex >= 0)
            {
                object dataObj = cmb.DataSource;
                Type sourceType = dataObj.GetType();
                if (sourceType == typeof(DataView))
                {
                    DataView dv = (DataView)dataObj;
                    if (dv.Table.TableName == md.TableName)
                    {
                        obj = ToObj<T>(dv[cmb.SelectedIndex].Row);
                    }
                }
                else if (sourceType == typeof(DataTable))
                {
                    DataTable dt = (DataTable)dataObj;
                    if (dt.TableName == md.TableName)
                    {
                        obj = ToObj<T>(dt.Rows[cmb.SelectedIndex]);
                    }
                }
                else if (sourceType == typeof(List<T>))
                {
                    obj = ((List<T>)dataObj)[cmb.SelectedIndex];
                }
            }
            return obj;
        }
        public static T GetSelectedModel<T>(ListBox lb) where T : BaseModel, new() {
            T obj = null;
            ModelDb md = ModelDbs[typeof(T)];
            if (lb.DataSource != null && lb.SelectedIndex >= 0)
            {
                object dataObj = lb.DataSource;
                Type sourceType = dataObj.GetType();
                if (sourceType == typeof(DataView))
                {
                    DataView dv = (DataView)dataObj;
                    if (dv.Table.TableName == md.TableName)
                    {
                        obj = ToObj<T>(dv[lb.SelectedIndex].Row);
                    }
                }
                else if (sourceType == typeof(DataTable))
                {
                    DataTable dt = (DataTable)dataObj;
                    if (dt.TableName == md.TableName)
                    {
                        obj = ToObj<T>(dt.Rows[lb.SelectedIndex]);
                    }
                }
                else if (sourceType == typeof(List<T>))
                {
                    obj = ((List<T>)dataObj)[lb.SelectedIndex];
                }
            }
            return obj;
        }
        public static void SetSelectedModel<T>(ComboBox cmb, Guid objId) where T : BaseModel, new()
        {
            if (cmb == null || objId == Guid.Empty) return;
            if (cmb.DataSource == null) return;
            object dataObj = cmb.DataSource;
            Type sType = dataObj.GetType();
            ListItemAttribute listItem = GetListItem<T>();
            int i = -1;
            if (sType == typeof(DataView))
            {
                DataView dv = (DataView)dataObj;
                foreach (DataRowView dvr in dv)
                {
                    i++;
                    if ((Guid)dvr[listItem.ValueMember] == objId)
                    {
                        cmb.SelectedIndex = i;
                        break;
                    }
                }
            }
            else if (sType == typeof(DataTable))
            {
                DataTable dt = (DataTable)dataObj;
                foreach (DataRow row in dt.Rows)
                {
                    i++;
                    if ((Guid)row[listItem.ValueMember] == objId)
                    {
                        cmb.SelectedIndex = i;
                        break;
                    }
                }
            }
            else if (sType == typeof(List<T>))
            {
                List<T> listObj = (List<T>)dataObj;
                foreach (T item in listObj)
                {
                    i++;
                    if ((Guid)listItem.ValueProperty.GetValue(item, null) == objId)
                    {
                        cmb.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        public static void SetSelectedModel<T>(ListBox lb, Guid objId) where T : BaseModel, new() {
            if (lb == null || objId == Guid.Empty) return;
            if (lb.DataSource == null) return;
            object dataObj = lb.DataSource;
            Type sType = dataObj.GetType();
            ListItemAttribute listItem = GetListItem<T>();
            int i = -1;
            if (sType == typeof(DataView))
            {
                DataView dv = (DataView)dataObj;
                foreach (DataRowView dvr in dv)
                {
                    i++;
                    if ((Guid)dvr[listItem.ValueMember] == objId)
                    {
                        lb.SelectedIndex = i;
                        break;
                    }
                }
            }
            else if (sType == typeof(DataTable))
            {
                DataTable dt = (DataTable)dataObj;
                foreach (DataRow row in dt.Rows)
                {
                    i++;
                    if ((Guid)row[listItem.ValueMember] == objId)
                    {
                        lb.SelectedIndex = i;
                        break;
                    }
                }
            }
            else if (sType == typeof(List<T>))
            {
                List<T> listObj = (List<T>)dataObj;
                foreach (T item in listObj)
                {
                    i++;
                    if ((Guid)listItem.ValueProperty.GetValue(item, null) == objId)
                    {
                        lb.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        public static void SetSelectedModel<T>(ComboBox cmb, T obj) where T : BaseModel, new()
        {
            Guid objId = Guid.Empty;
            ListItemAttribute listItem = GetListItem<T>();
            objId = (Guid)listItem.ValueProperty.GetValue(obj, null);
            SetSelectedModel<T>(cmb, objId);
        }
        public static void SetSelectedModel<T>(ListBox lb, T obj) where T : BaseModel, new() {
            Guid objId = Guid.Empty;
            ListItemAttribute listItem = GetListItem<T>();
            objId = (Guid)listItem.ValueProperty.GetValue(obj, null);
            SetSelectedModel<T>(lb, objId);
        }
        public static int GetSelectedInt(ComboBox cmb, int defaultValue = 0)
        {
            int r = defaultValue;
            if (cmb != null && cmb.Items.Count > 0 && cmb.SelectedItem != null)
            {
                int.TryParse(cmb.SelectedItem.ToString(), out r);
            }
            return r;
        }
        public static T GetSelectedEnum<T>(ComboBox cmb, T defaultValue = default(T)) where T : struct
        {
            if (typeof(T).IsEnum)
            {
                T r = defaultValue;
                if (cmb != null && cmb.Items.Count > 0 && cmb.SelectedItem != null)
                {
                    Enum.TryParse<T>(cmb.SelectedItem.ToString(), out r);
                }
                return r;
            }
            else
            {
                throw new ArgumentException("泛型参数不是一个枚举类型");
            }
        }
        public static void SetSelectedInt(ComboBox cmb, int value)
        {
            if (cmb != null && cmb.Items.Count > 0)
            {
                for (int i = 0; i < cmb.Items.Count; i++)
                {
                    if (cmb.Items[i].ToString() == value.ToString())
                    {
                        cmb.SelectedIndex = i;
                        break;
                    }
                }
            }
        }
        public static void BindComboBox<T>(ComboBox cmb, WhereArgs args) where T : BaseModel, new()
        {
            if (cmb != null)
            {
                cmb.Items.Clear();
                ListItemAttribute listItem = GetListItem<T>();
                if (listItem != null)
                {
                    if (string.IsNullOrWhiteSpace(cmb.ValueMember))
                    {
                        cmb.ValueMember = listItem.ValueMember;
                    }
                    if (string.IsNullOrWhiteSpace(cmb.DisplayMember))
                    {
                        cmb.DisplayMember = listItem.DisplayMember;
                    }
                    cmb.DataSource = GetDataView<T>(args);
                    if (cmb.Items.Count > 0)
                    {
                        cmb.SelectedIndex = 0;
                    }
                }
                else
                {
                    throw new Exception("类型" + typeof(T).Name + "没有设置ListItem特性");
                }
            }
        }
        public static void BindComboBoxSimple<T>(ComboBox cmb, WhereArg arg) where T : BaseModel, new()
        {
            if (arg != null)
            {
                BindComboBox<T>(cmb, new WhereArgs() { arg });
            }
            else
            {
                BindComboBox<T>(cmb, null);
            }
        }
        public static void BindListBox<T>(ListBox lb, WhereArgs args) where T : BaseModel, new()
        {
            if (lb != null)
            {
                lb.Items.Clear();
                ListItemAttribute listItem = GetListItem<T>();
                if (listItem != null)
                {
                    if (string.IsNullOrWhiteSpace(lb.ValueMember))
                    {
                        lb.ValueMember = listItem.ValueMember;
                    }
                    if (string.IsNullOrWhiteSpace(lb.DisplayMember))
                    {
                        lb.DisplayMember = listItem.DisplayMember;
                    }
                    lb.DataSource = GetDataView<T>(args);
                    if (lb.Items.Count > 0)
                    {
                        lb.SelectedIndex = 0;
                    }
                }
                else
                {
                    throw new Exception("类型" + typeof(T).Name + "没有设置ListItem特性");
                }
            }
        }
        public static void BindListBoxSimple<T>(ListBox lb, WhereArg arg) where T : BaseModel, new()
        {
            if (arg != null)
            {
                BindListBox<T>(lb, new WhereArgs() { arg });
            }
            else
            {
                BindListBox<T>(lb, null);
            }
        }
        public static void BindComboBoxByInts(ComboBox cmb, int min, int max)
        {
            if (cmb != null)
            {
                cmb.Items.Clear();
                for (int i = min; i <= max; i++)
                {
                    cmb.Items.Add(i);
                }
                cmb.SelectedIndex = 0;
            }
        }
        public static void BindComboBoxByEmuns<T>(ComboBox cmb) where T : struct
        {
            if (cmb != null && typeof(T).IsEnum)
            {
                string[] names = Enum.GetNames(typeof(T));
                cmb.Items.Clear();
                cmb.Items.AddRange(names);
            }
        }

        public static Tmodel ShowDialog<Tfrom, Tmodel>(Form owner, Tmodel obj)
            where Tfrom : FormDialog, new()
            where Tmodel : BaseModel, new()
        {
            Tfrom f = new Tfrom();
            f.Owner = owner;
            f.Object = obj;
            f.StartPosition = FormStartPosition.CenterParent;
            if (f.ShowDialog() == DialogResult.OK)
            {
                obj = (Tmodel)f.Object;
            }
            return obj;
        }
        #endregion

        #region XML


        static object saveXmlNode(Type type, XmlNode node)
        {
            if (type != null && node != null)
            {
                object obj = Activator.CreateInstance(type);
                ModelDb md = ModelDbs[type];
                if (md != null)
                {
                    if (!AppDataSet.Tables.Contains(md.TableName))
                    {
                        fillRows(type, "1=1");
                    }
                    DataTable dt = AppDataSet.Tables[md.TableName];
                    DataRow row = dt.NewRow();

                    foreach (KeyValuePair<string, ModelDbItem> sps in md.XmlAttrs)
                    {
                        XmlAttributeCollection xms = node.Attributes;
                        for (int i = xms.Count - 1; i >= 0; i--)
                        {
                            XmlAttribute xa = node.Attributes[i];
                            if (xa.Name == sps.Key && !string.IsNullOrWhiteSpace(xa.Value))
                            {
                                string value = xa.Value;
                                string field = sps.Value.FieldName;
                                try
                                {
                                    setDataRowField(row, field, value, sps.Value.Column.DefaultValue);
                                    break;
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }
                    }
                    Guid guid = Guid.Empty;
                    if (!row.IsNull(BaseModel.FN.Id))
                    {
                        if (!Guid.TryParse(row[BaseModel.FN.Id].ToString(), out guid)) { guid = Guid.Empty; }
                    }
                    if (dt.Select("Convert(" + BaseModel.FN.Id + ",'System.String'='" + guid.ToString() + "')").Length == 0)
                    {
                        dt.Rows.Add(row);
                    }
                    if (node.ChildNodes.Count > 0)
                    {
                        for (int j = 0; j < node.ChildNodes.Count; j++)
                        {
                            Type ct = XmlTables[node.ChildNodes[j].Name];
                            if (isBaseModel(ct))
                            {
                                saveXmlNode(ct, node.ChildNodes[j]);
                            }
                        }
                    }
                }
            }
            return null;
        }

        public static void ImportFromXML(XmlDocument xmlDoc)
        {
            foreach (XmlNode node in xmlDoc.ChildNodes)
            {
                Type nt = XmlTables[node.Name];
                if (isBaseModel(nt))
                {
                    saveXmlNode(nt, node);
                    ModelDbs[nt].Grid.Refresh();
                }
            }
        }

        public static void ExportToXml(Type type)
        {

        }
        #endregion
    }
}
