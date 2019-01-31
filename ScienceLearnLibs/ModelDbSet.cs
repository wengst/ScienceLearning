using LearnLibs.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Forms;

namespace LearnLibs
{
    public static class ModelDbSet
    {
        #region private fields
        static SQLiteConnection Connection = null;
        static DataSet AppDataSet = null;
        static Dictionary<string, ModelDb> _mds = null;
        #endregion

        #region private motheds
        /// <summary>
        /// 连接2个字符串，中间用seq字符串分隔
        /// </summary>
        /// <param name="frontStr">分隔符前面的字符串</param>
        /// <param name="backStr">分隔符后面的字符串</param>
        /// <param name="seq">分隔字符串</param>
        /// <returns></returns>
        static string joinString(string frontStr, string backStr, string seq)
        {
            if (!string.IsNullOrWhiteSpace(backStr) && !string.IsNullOrWhiteSpace(seq))
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
        /// 获取模型数据操作集合
        /// </summary>
        static Dictionary<string, ModelDb> ModelDbs
        {
            get
            {
                if (_mds == null)
                {
                    _mds = new Dictionary<string, ModelDb>();
                    Assembly ass = Assembly.GetExecutingAssembly();
                    Type[] types = ass.GetTypes();
                    foreach (Type t in types)
                    {
                        if (isBaseModel(t))
                        {
                            _mds.Add(t.Name, new ModelDb(t));
                        }
                    }
                }
                return _mds;
            }
        }

        /// <summary>
        /// 将Where条件参数集转换为Where条件字符串表达式
        /// </summary>
        /// <param name="args"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        static string getWhere(List<WhereArg> args, WhereTarget target = WhereTarget.SQLite)
        {
            string where = string.Empty;
            if (args != null)
            {
                foreach (WhereArg arg in args)
                {
                    if (string.IsNullOrWhiteSpace(where))
                    {
                        where = target == WhereTarget.SQLite ? arg.ToWhereString() : arg.ToRowFilterString();
                    }
                    else
                    {
                        where += " AND " + (target == WhereTarget.SQLite ? arg.ToWhereString() : arg.ToRowFilterString());
                    }
                }
            }
            return where;
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
            ModelDb md = ModelDbs[type.Name];
            string selectSql = "SELECT * FROM " + md.TableName;
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
                }
            }
        }

        /// <summary>
        /// 获取无Where条件表达式和Order表达式的Select语句
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static string getSelectSqlNoWhere(Type type)
        {
            if (isBaseModel(type))
            {
                string joinTableStr = "LEFT JOIN {0} AS {1} ON {1}.{2}=a.{3}";
                string joinFieldStr = "{0}.{1} AS {2}";
                string[] joinTableAry = new string[4];
                string[] joinFieldAry = new string[3];

                ModelDb me = ModelDbs[type.Name];
                List<ModelDbItem> Columns = me.Columns;
                string TableName = me.TableName;
                string joins = "";
                string fields = "";
                foreach (ModelDbItem item in Columns)
                {

                    if (!item.IsDisplayColumn)
                    {
                        fields = joinString(fields, "a." + item.FieldName, ",");
                    }
                    else
                    {
                        if (isBaseModel(item.DisplayColumn.FromType))
                        {
                            PropertyInfo p = item.Property;

                            string tAlias = me.PTC.TableAlias(p);
                            string tName = me.PTC.TableName(p);
                            if (!string.IsNullOrWhiteSpace(tAlias))
                            {
                                joinTableAry[0] = tName;
                                joinTableAry[1] = tAlias;
                                joinTableAry[2] = item.ForeignKey.Field;
                                joinTableAry[3] = item.FieldName;

                                joinFieldAry[0] = tAlias;
                                joinFieldAry[1] = item.DisplayColumn.Field;
                                joinFieldAry[2] = item.AsField;
                                fields = joinString(fields, string.Format(joinFieldStr, joinFieldAry), ",");
                                joins = joinString(joins, string.Format(joinTableStr, joinTableAry), " ");
                            }
                        }
                    }

                }
                return "SELECT " + fields + " FROM " + TableName + " AS a " + joins;
            }
            return string.Empty;
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
                ModelDb md = ModelDbs[type.Name];
                if (md.HasOrderColumns)
                {
                    foreach (ModelDbItem dbItem in md.OrderColumns)
                    {
                        if (dbItem.OrderBy.OrderType != SortOrder.None)
                        {
                            order = joinString(order, dbItem.GetOrder(), ",");
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
                ModelDb md = ModelDbs[type.Name];
                List<ModelDbItem> Columns = md.Columns;
                string tableName = md.TableName;
                string fields = string.Empty;
                string where = string.Empty;
                for (int i = 0; i < Columns.Count; i++)
                {
                    ModelDbItem dbItem = Columns[i];
                    if (dbItem.IsPrimaryKey)
                    {
                        where = joinString(where, dbItem.FieldName + "=" + dbItem.ParameterName, ",");
                    }
                    else
                    {
                        fields = joinString(fields, dbItem.FieldName + "=" + dbItem.ParameterName, ",");
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
                ModelDb md = ModelDbs[type.Name];
                parameters = new SQLiteParameter[md.PrimaryKeyColumn.Columns.Count];
                for (int i = 0; i < parameters.Length; i++)
                {
                    ModelDbItem dbItem = md.PrimaryKeyColumn.Columns[i];
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
                ModelDb md = ModelDbs[type.Name];
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
                ModelDb md = ModelDbs[type.Name];
                List<ModelDbItem> Columns = md.Columns;
                string tableName = md.TableName;
                string fields = "";
                string parameters = "";
                for (int i = 0; i < Columns.Count; i++)
                {
                    ModelDbItem dbItem = Columns[i];
                    fields = joinString(fields, dbItem.FieldName, ",");
                    parameters = joinString(parameters, dbItem.ParameterName, ",");
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
                ModelDb md = ModelDbs[type.Name];
                List<ModelDbItem> Columns = md.PrimaryKeyColumn.Columns;
                string tableName = md.TableName;
                string where = "";
                for (int i = 0; i < Columns.Count; i++)
                {
                    ModelDbItem dbItem = Columns[i];
                    where = joinString(where, dbItem.FieldName + "=" + dbItem.ParameterName, " AND ");
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
                ModelDb md = ModelDbs[type.Name];
                List<ModelDbItem> Columns = md.Columns;
                string tableName = md.TableName;
                int primaryKeyCount = md.PrimaryKeyColumn.Columns.Count;
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
                    createTableStr = joinString(createTableStr, s, ",");
                }
                //如果表主键包含多个字段
                if (md.HasPrimaryColumn && primaryKeyCount > 1)
                {
                    createTableStr += ",PRIMARY KEY(";
                    string primaryKeys = string.Empty;
                    foreach (ModelDbItem dbItem in md.PrimaryKeyColumn.Columns)
                    {
                        primaryKeys = joinString(primaryKeys, dbItem.FieldName, ",");
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
        #endregion

        #region public properties
        public static ModelDb this[string typeName]
        {
            get
            {
                if (ModelDbs.ContainsKey(typeName))
                {
                    return ModelDbs[typeName];
                }
                return null;
            }
        }

        public static ModelDb this[Type type]
        {
            get
            {
                if (ModelDbs.ContainsKey(type.Name))
                {
                    return ModelDbs[type.Name];
                }
                return null;
            }
        }
        #endregion

        #region public methods
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
                ModelDb me = ModelDbs[checkingType.Name];
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

        public static bool RowIsExists(Type t, string fieldName, Guid value)
        {
            if (t != null && BaseModel.IsSubclass(t) && !string.IsNullOrWhiteSpace(fieldName) && value != null)
            {
                DataTable dt = GetDataTable(t, fieldName, value);
                if (dt == null || dt.Rows.Count == 0) return false;
                return true;
            }
            return false;
        }

        public static DataTable GetDataTable(Type type, string fieldName, Guid value)
        {
            DataRow[] rows = new DataRow[0];
            ModelDb md = ModelDbs[type.Name];
            string rowFilter = "Convert(" + fieldName + ",'System.String')='" + value.ToString() + "'";
            if (type != null && BaseModel.IsSubclass(type) && !string.IsNullOrWhiteSpace(fieldName) && value != null)
            {
                if (md != null && AppDataSet.Tables.Contains(md.TableName) && AppDataSet.Tables[md.TableName].Columns.Contains(fieldName))
                {
                    rows = AppDataSet.Tables[md.TableName].Select(rowFilter);
                }
            }
            if (rows == null)
            {
                fillRows(type, "UPPER(HEX(" + fieldName + "))='" + F.byteToHexStr(value.ToByteArray()) + "'");
                rows = AppDataSet.Tables[md.TableName].Select(rowFilter);
            }
            DataTable dt = AppDataSet.Tables[md.TableName].Clone();
            if (rows != null && rows.Length > 0)
            {
                dt.Rows.Add(rows);
            }
            return dt;
        }

        public static T ToObj<T>(DataRow r) where T : BaseModel, new()
        {
            T t = null;
            ModelDb me = ModelDbs[typeof(T).Name];
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

        /// <summary>
        /// 根据类型实例更新DataRow。如果属性值与对应字段值没有发生变化则不更新相应的字段
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static DataRow SaveDataRow(object obj)
        {
            DataRow r = null;
            if (obj != null && isBaseModel(obj.GetType()))
            {
                Type t = obj.GetType();
                ModelDb md = ModelDbs[t.Name];
                string TableName = md.TableName;
                bool exis = false;
                Guid guid;
                Guid objId;
                foreach (DataRow row in AppDataSet.Tables[TableName].Rows)
                {
                    guid = F.GetValue<Guid>(row, BaseModel.FN.Id, Guid.Empty);
                    objId = ((BaseModel)obj).Id;
                    Console.WriteLine("DataRow Guid Value[" + guid.ToString() + "]=?Object Guid Value[" + objId.ToString() + "]");
                    if (guid == objId)
                    {
                        r = row;
                        exis = true;
                        break;
                    }
                }
                if (r == null)
                {
                    r = AppDataSet.Tables[TableName].NewRow();
                    exis = false;
                }
                DataColumnCollection dcc = r.Table.Columns;
                foreach (DataColumn dc in dcc)
                {
                    ModelDbItem dbitem = getItemForFieldName(dc.ColumnName);
                    if (dbitem != null)
                    {
                        object value = dbitem.Property.GetValue(obj, null);
                        if (value != null)
                        {
                            if (dc.DataType == typeof(int))
                            {
                                r[dc.ColumnName] = (int)value;
                            }
                            else if (dc.DataType == typeof(Guid))
                            {
                                r[dc.ColumnName] = (Guid)value;
                            }
                            else
                            {
                                r[dc.ColumnName] = value;
                            }
                        }
                    }
                }
                if (!exis)
                {
                    GlobalDataSet.Tables[TableName].Rows.Add(r);
                }
                grid.Refresh();
            }
            return r;
        }
        #endregion
    }
}
