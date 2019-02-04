using LearnLibs.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using LearnLibs.Controls;

namespace LearnLibs
{
    public static class ModelDbSet
    {
        #region private fields
        static Dictionary<Type, ModelDb> _tts = null;
        static DisplayScenes _scenes = DisplayScenes.未设置;
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
                foreach (ModelDbItem m in md.Columns)
                {
                    if (m.IsDisplayColumn && m.IsForeignKey && isBaseModel(m.DisplayColumn.FromType) && isBaseModel(m.ForeignKey.Type) && m.ForeignKey.Type == m.DisplayColumn.FromType)
                    {
                        PropertyTable pt = new PropertyTable(m.Property, _tts[m.DisplayColumn.FromType].TableName);
                        md.PTC.Add(pt);
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
            Console.WriteLine(getSelectSqlNoWhere(type));
            ModelDb md = ModelDbs[type];
            string selectSql = getSelectSqlNoWhere(type);
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
                string joinFieldStr = "{0}.{1} as {2}";
                string[] joinTableAry = null;
                string[] joinFieldAry = null;

                ModelDb me = ModelDbs[type];
                List<ModelDbItem> Columns = me.Columns;
                string TableName = me.TableName;
                string joins = "";
                string fields = "";
                foreach (ModelDbItem item in Columns)
                {
                    fields = joinString(fields, string.Format(joinFieldStr, new object[] { "a", item.FieldName, item.FieldName }), ",");

                    if (item.IsDisplayColumn && isBaseModel(item.DisplayColumn.FromType))
                    {
                        joinTableAry = new string[4];
                        joinFieldAry = new string[3];
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
                            var i = 0;
                            Console.WriteLine("joins" + joins);
                        }
                    }
                }
                Console.WriteLine("JOIN=" + joins);
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
                ModelDb md = ModelDbs[type];
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
                ModelDb md = ModelDbs[type];
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
                ModelDb md = ModelDbs[type];
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
                ModelDb md = ModelDbs[type];
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
        public static DisplayScenes Scenes { get { return _scenes; } set { _scenes = value; } }
        public static DataSet AppDataSet
        {
            get;
            set;
        }

        public static SQLiteConnection Connection { get; set; }
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
        /// 行是否存在
        /// </summary>
        /// <param name="t"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static bool RowIsExists(Type t, WhereArg where)
        {
            if (t != null && BaseModel.IsSubclass(t))
            {
                DataView dv = GetDataView(t, where);
                if (dv == null || dv.Count == 0) return false;
                return true;
            }
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
            Type type = typeof(T);
            DataView dv = null;

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
            return dv;
        }

        public static DataView GetDataView(WhereArgs where, Type type)
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

        public static void RefreshGrid<T>(WhereArgs args) where T : BaseModel, new()
        {
            ModelDbs[typeof(T)].Grid.DataSource = GetDataView<T>(args);
            ModelDbs[typeof(T)].Grid.Refresh();
        }

        /// <summary>
        /// 获取表格中选定行的DataRow
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataRow GetSelectedGridDataRow<T>() where T : BaseModel, new()
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

        public static T GetSelectedObjectAtGrid<T>() where T : BaseModel, new()
        {
            DataRow row = GetSelectedGridDataRow<T>();
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
                    bool exis = false;
                    Guid guid;
                    Guid objId;
                    foreach (DataRow row in AppDataSet.Tables[TableName].Rows)
                    {
                        guid = F.GetValue<Guid>(row, BaseModel.FN.Id, Guid.Empty);
                        objId = ((BaseModel)obj).Id;
                        //Console.WriteLine("DataRow Guid Value[" + guid.ToString() + "]=?Object Guid Value[" + objId.ToString() + "]");
                        if (guid == objId)
                        {
                            r = row;
                            exis = true;
                            break;
                        }
                    }
                    if (isNew)
                    {
                        r = AppDataSet.Tables[TableName].NewRow();
                        exis = false;
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
                                if (value != null)
                                {
                                    DataColumn dc = dt.Columns[typeCol.FieldName];
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
                    }
                    if (!exis)
                    {
                        AppDataSet.Tables[TableName].Rows.Add(r);
                    }
                    md.Grid.Refresh();
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
            Type t = typeof(T);
            ModelDb md = ModelDbs[t];
            DataRow row = null;
            bool isNew = obj.Id != Guid.Empty;
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
                    row = SaveDataRow<T>(obj, isNew);
                    ModelDbs[t].Grid.Refresh();
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
            DataRow row = GetSelectedGridDataRow<T>();
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

        /// <summary>
        /// 根据WhereArgs查询参数，获取泛型所对应表的相应字段值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object GetFieldValue<T>(string field, WhereArgs args) where T : BaseModel, new()
        {
            if (string.IsNullOrWhiteSpace(field) && args == null) return null;
            Type t = typeof(T);
            ModelDb md = ModelDbs[t];
            if (!AppDataSet.Tables.Contains(md.TableName))
            {
                fillRows(md.TableName, null);
            }
            DataRow[] rows = AppDataSet.Tables[md.TableName].Select(args.RowFilter);
            if (rows != null && rows.Length > 0)
            {
                DataRow row = rows[0];
                if (row.IsNull(field))
                {
                    return null;
                }
                else
                {
                    return row[field];
                }
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
        #endregion
    }
}
