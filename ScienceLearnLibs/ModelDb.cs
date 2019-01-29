using LearnLibs.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using LearnLibs.Models;
using LearnLibs.Enums;

namespace LearnLibs
{
    #region
    /// <summary>
    /// 外键参数类型
    /// </summary>
    public class ForeignKeyArg
    {
        /// <summary>
        /// 外键特性
        /// </summary>
        public ForeignKeyAttribute Key { get; private set; }
        /// <summary>
        /// 外键的值对象
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// 以外键类型和外键值对象初始化外键参数
        /// <para>如果外键是枚举，则值对象必须可转换为对应枚举常量</para>
        /// <para>如果外键是BaseModel的派生类，则值对象必须是内存数据行、派生类实例、数值类型、枚举、字符串类型之一</para>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public ForeignKeyArg(ForeignKeyAttribute key, object value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");
            this.Key = key;
            Type kt = key.ModelType;
            Type vt = value.GetType();
            if (kt.IsEnum)
            {
                if (kt != vt)
                {
                    if (vt.IsValueType || vt == typeof(string))
                    {
                        if (!Enum.IsDefined(kt, value))
                        {
                            throw new ArgumentException("外键是枚举类型，而值不在定义的枚举常量中");
                        }
                        else
                        {
                            this.Value = Enum.ToObject(kt, value);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("外键是枚举类型，而值的类型无法转化为枚举");
                    }
                }
                else
                {
                    this.Value = value;
                }
            }
            else if (BaseModel.IsSubclass(kt))
            {
                if (vt == typeof(DataRow))
                {
                    DataRow r = (DataRow)value;
                    string mName = F.GetTableName(kt);
                    if (r.Table.TableName != mName)
                    {
                        throw new ArgumentException("DataRow的表与外键类型所需的表不同");
                    }
                    else
                    {
                        this.Value = r;
                    }
                }
                else if (vt == kt)
                {
                    PropertyInfo p = F.GetProperty(kt, key.DisplayField);
                    if (p != null)
                    {
                        this.Value = p.GetValue(value, null);
                    }
                }
                else if (vt.IsValueType || vt == typeof(string))
                {
                    this.Value = value;
                }
                else
                {
                    throw new ArgumentException("外键所需参数值的类型无效");
                }
            }
        }
    }

    /// <summary>
    /// 类型数据列类型
    /// </summary>
    public class ModelDbItem : IComparable, IComparable<ModelDbItem>
    {
        #region private fields
        PropertyInfo _property = null;
        DisplayColumnAttribute _displayCol = null;
        DbColumnAttribute _col = null;
        PrimaryKeyAttribute _primaryKey = null;
        ForeignKeyAttribute _foreignKey = null;
        UnqiueKeyAttribute _unqiueKey = null;
        AutoIdentityAttribute _autoIdentity = null;
        OrderByAttribute _orderBy = null;
        TreeNodeColumnAttribute _treenodeColumn = null;
        #endregion

        #region private methods

        #endregion

        #region public properties
        /// <summary>
        /// 数据列关联的属性
        /// </summary>
        public PropertyInfo Property { get { return _property; } }

        /// <summary>
        /// 数据列
        /// </summary>
        public DbColumnAttribute Column { get { return _col; } }

        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName
        {
            get { return _col.FieldName; }
        }

        public string DisplayField {
            get {
                if (this.IsDisplayColumn && BaseModel.IsSubclass(DisplayColumn.DisplayType))
                {
                    return FieldName + "_" + DisplayColumn.DisplayField;
                }
                else {
                    return FieldName;
                }
            }
        }

        /// <summary>
        /// 数据列的数据类型
        /// </summary>
        public DbType DataType { get { return _col.DataType; } }
        /// <summary>
        /// 数据列大小
        /// </summary>
        public int Size { get { return _col.Size; } }
        /// <summary>
        /// 用于SqlCommand参数的参数名
        /// </summary>
        public string ParameterName { get { return "@" + FieldName; } }
        /// <summary>
        /// 数据列的显示特性
        /// </summary>
        public DisplayColumnAttribute DisplayColumn { get { return _displayCol; } }
        /// <summary>
        /// 是否包含显示特性
        /// </summary>
        public bool IsDisplayColumn
        {
            get { return _displayCol != null; }
        }
        public PrimaryKeyAttribute PrimaryKey { get { return _primaryKey; } }
        public bool IsPrimaryKey { get { return _primaryKey != null; } }
        public ForeignKeyAttribute ForeignKey { get { return _foreignKey; } }
        public bool IsForeignKey { get { return _foreignKey != null; } }
        public AutoIdentityAttribute AutoIdentity { get { return _autoIdentity; } }
        public bool IsAutoIdentity { get { return _autoIdentity != null; } }
        public OrderByAttribute OrderBy { get { return _orderBy; } }
        public bool IsOrderColumn { get { return _orderBy != null; } }
        public TreeNodeColumnAttribute TreeNodeColumn
        {
            get { return _treenodeColumn; }
        }
        /// <summary>
        /// 是否属于树形节点列
        /// </summary>
        public bool IsTreeNodeColumn
        {
            get { return _treenodeColumn != null; }
        }
        public UnqiueKeyAttribute UnqiueKey { get { return _unqiueKey; } }
        public bool IsUnqiueKey { get { return _unqiueKey != null; } }
        #endregion

        #region public methods
        public SQLiteParameter GetParameterByForeignKey(object obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (!IsForeignKey) throw new NotContainAttributeException(typeof(ForeignKeyAttribute));
            Type t = obj.GetType();
            SQLiteParameter parameter = null;
            if (BaseModel.IsSubclass(t) && ForeignKey.ModelType == t)
            {
                parameter = new SQLiteParameter();
                parameter.ParameterName = this.ParameterName;
                parameter.DbType = DataType;
                parameter.Size = this.Size;
                parameter.Value = Property.GetValue(obj, null);
            }
            if (t == typeof(DataRow))
            {
                string name1 = F.GetTableName(_foreignKey.ModelType);
                string name2 = ((DataRow)obj).Table.TableName;
                if (name1 == name2)
                {
                    parameter = new SQLiteParameter();
                    parameter.ParameterName = this.ParameterName;
                    parameter.DbType = this.DataType;
                    parameter.Size = this.Size;
                    parameter.Value = ((DataRow)obj)[this.FieldName];
                }
            }
            if (t.IsValueType || t == typeof(string))
            {
                parameter = new SQLiteParameter();
                parameter.ParameterName = this.ParameterName;
                parameter.DbType = this.DataType;
                parameter.Size = this.Size;
                parameter.Value = obj;
            }
            return parameter;
        }

        /// <summary>
        /// 根据外键获取此数据列的Where条件表达式
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public SelectExpression GetWhere(ForeignKeyArg arg)
        {
            if (arg == null) throw new ArgumentNullException("obj");
            if (!IsForeignKey) throw new NotContainAttributeException(typeof(ForeignKeyAttribute));
            SelectExpression SE = new SelectExpression();
            if (F.IsDigital(this.DataType))
            {
                SE.DataTableWhere = SE.SQLiteWhere = this.FieldName + "=" + arg.Value.ToString();
            }
            else if (DataType != DbType.Guid)
            {
                SE.DataTableWhere = SE.SQLiteWhere = this.FieldName + "='" + arg.Value.ToString() + "'";
            }
            else
            {
                Guid guid = Guid.Empty;
                if (Guid.TryParse(arg.Value.ToString(), out guid))
                {
                    SE.SQLiteWhere = "HEX(" + FieldName + ")='" + F.byteToHexStr(guid.ToByteArray()) + "'";
                    SE.DataTableWhere = "Convert(" + FieldName + ",'System.String')='" + guid.ToString() + "'";
                }
                else
                {
                    SE.SQLiteWhere = "HEX(" + FieldName + ")='" + F.byteToHexStr(guid.ToByteArray()) + "'";
                    SE.DataTableWhere = "Convert(" + FieldName + ",'System.String')='" + guid.ToString() + "'";
                }
            }
            //Console.WriteLine(SE.DataTableWhere);
            return SE;
        }

        /// <summary>
        /// 获取该数据列的Order By表达式：FieldName ASC|DESC
        /// </summary>
        /// <returns></returns>
        public string GetOrder()
        {
            string order = "";
            if (IsOrderColumn)
            {
                if (this.OrderBy.OrderType != SortOrder.None)
                {
                    if (string.IsNullOrWhiteSpace(order))
                    {
                        order = this.FieldName + " " + (this.OrderBy.OrderType == SortOrder.Ascending ? "ASC" : "DESC");
                    }
                    else
                    {
                        order += "," + this.FieldName + " " + (this.OrderBy.OrderType == SortOrder.Ascending ? "ASC" : "DESC");
                    }
                }
            }
            return order;
        }
        #endregion

        public ModelDbItem(PropertyInfo p)
        {
            if (p == null) throw new ArgumentNullException("p");
            this._property = p;
            object[] attrs = p.GetCustomAttributes(true);
            if (attrs.Length > 0)
            {
                foreach (object attr in attrs)
                {
                    Type t = attr.GetType();
                    if (t == typeof(DisplayColumnAttribute))
                    {
                        _displayCol = attr as DisplayColumnAttribute;
                    }
                    else if (t == typeof(DbColumnAttribute))
                    {
                        _col = attr as DbColumnAttribute;
                    }
                    else if (t == typeof(PrimaryKeyAttribute))
                    {
                        _primaryKey = attr as PrimaryKeyAttribute;
                    }
                    else if (t == typeof(ForeignKeyAttribute))
                    {
                        _foreignKey = attr as ForeignKeyAttribute;
                    }
                    else if (t == typeof(AutoIdentityAttribute))
                    {
                        _autoIdentity = attr as AutoIdentityAttribute;
                    }
                    else if (t == typeof(OrderByAttribute))
                    {
                        _orderBy = attr as OrderByAttribute;
                    }
                    else if (t == typeof(UnqiueKeyAttribute))
                    {
                        _unqiueKey = attr as UnqiueKeyAttribute;
                    }
                    else if (t == typeof(TreeNodeColumnAttribute))
                    {
                        _treenodeColumn = attr as TreeNodeColumnAttribute;
                    }
                }
            }
            if (_col == null)
            {
                throw new NotDbColumnAsPropertyException(p);
            }
        }

        public int CompareTo(object obj)
        {
            int i1 = Column.Index;
            int i2 = ((ModelDbItem)obj).Column.Index;
            return i1 - i2;
        }

        public int CompareTo(ModelDbItem other)
        {
            return this.Column.Index - other.Column.Index;
        }
    }

    /// <summary>
    /// 模型数据操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelDb
    {
        #region private fields
        private Dictionary<string, ModelDb> pri_field_MDs = null;
        internal string pri_field_tableName = null;
        Type pri_field_type = null;
        internal PropertyInfo[] internal_field_Properties = null;
        List<ModelDbItem> pri_field_items = new List<ModelDbItem>();
        List<ModelDbItem> _displayCols = null;
        List<ModelDbItem> _foreignKeyCols = null;
        ModelPrimaryKey _primaryKey = null;
        List<ModelDbItem> _orderByCols = null;
        ModelDbItem _autoIdentity = null;
        ModelUnqiueKeyCollection _unqiueKeys = null;
        string _tnNameField = "";
        string _tnTextField = "";
        DataGridView grid = new DataGridView();
        DisplayScenes _currentScenes = DisplayScenes.未设置;
        List<ModelDbItem> dcols = null;
        List<string> JoinTables = new List<string>();
        #endregion

        #region private motheds
        Dictionary<string, ModelDb> MDs
        {
            get
            {
                if (pri_field_MDs == null)
                {
                    pri_field_MDs = new Dictionary<string, ModelDb>();
                    Assembly ass = Assembly.GetExecutingAssembly();
                    Type[] types = ass.GetTypes();
                    foreach (Type t in types)
                    {
                        if (BaseModel.IsSubclass(t))
                        {
                            pri_field_MDs.Add(t.Name, new ModelDb(t, Connection, GlobalDataSet));
                        }
                    }
                }
                return pri_field_MDs;
            }
        }

        /// <summary>
        /// 检查目前DataSet中是否存在某记录
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool rowExists(Type t, string fieldName, Guid value)
        {
            if (t != null && BaseModel.IsSubclass(t) && !string.IsNullOrWhiteSpace(fieldName) && value != null)
            {
                DataTable dt = getDataRows(t, fieldName, value);
                if (dt == null || dt.Rows.Count == 0) return false;
                return true;
            }
            return false;
        }

        void selectRows(string tableName, string where, string orderby = null)
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
                    sda.Fill(GlobalDataSet, tableName);
                }
            }
        }

        void selectRows(Type type, string where, string orderby = null)
        {
            if (type == null) return;
            if (!BaseModel.IsSubclass(type)) return;
            ModelDb md = MDs[type.Name];
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
                    sda.Fill(GlobalDataSet, md.TableName);
                }
            }
        }

        DataTable getDataRows(Type type, string fieldName, Guid value)
        {
            DataRow[] rows = new DataRow[0];
            ModelDb md = MDs[type.Name];
            string rowFilter = "Convert(" + fieldName + ",'System.String')='" + value.ToString() + "'";
            if (type != null && BaseModel.IsSubclass(type) && !string.IsNullOrWhiteSpace(fieldName) && value != null)
            {
                if (md != null && GlobalDataSet.Tables.Contains(md.TableName) && GlobalDataSet.Tables[md.TableName].Columns.Contains(fieldName))
                {
                    rows = GlobalDataSet.Tables[md.TableName].Select(rowFilter);
                }
            }
            if (rows == null)
            {
                selectRows(type, "UPPER(HEX(" + fieldName + "))='" + F.byteToHexStr(value.ToByteArray()) + "'");
                rows = GlobalDataSet.Tables[md.TableName].Select(rowFilter);
            }
            DataTable dt = GlobalDataSet.Tables[md.TableName].Clone();
            if (rows != null && rows.Length > 0)
            {
                dt.Rows.Add(rows);
            }
            return dt;
        }

        string getForeignKeyText(Type type, string idField, Guid value, string textField)
        {
            string rStr = string.Empty;
            if (type != null && BaseModel.IsSubclass(type) && !string.IsNullOrWhiteSpace(idField) && value != null)
            {
                DataTable dt = getDataRows(type, idField, value);
                if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains(textField))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (rStr == string.Empty)
                        {
                            rStr = row.IsNull(textField) ? "null" : row[textField].ToString();
                        }
                        else
                        {
                            rStr += "," + (row.IsNull(textField) ? "null" : row[textField].ToString());
                        }
                    }
                }
            }
            return rStr;
        }

        internal void buildItems()
        {
            if (internal_field_Properties.Length > 0)
            {
                foreach (PropertyInfo p in internal_field_Properties)
                {
                    ModelDbItem item = new ModelDbItem(p);
                    pri_field_items.Add(item);
                    if (item.IsPrimaryKey)
                    {
                        if (_primaryKey == null)
                        {
                            _primaryKey = new ModelPrimaryKey(item);
                        }
                        else
                        {
                            if (_primaryKey.Name != item.PrimaryKey.Name)
                            {
                                throw new PrimaryKeyTooMuchException();
                            }
                            else
                            {
                                _primaryKey.Add(item);
                            }
                        }
                    }
                    if (item.IsAutoIdentity)
                    {
                        if (_autoIdentity == null) _autoIdentity = new ModelDbItem(p);
                    }
                    if (item.IsDisplayColumn)
                    {
                        if (_displayCols == null) _displayCols = new List<ModelDbItem>();
                        _displayCols.Add(item);
                    }
                    if (item.IsForeignKey)
                    {
                        if (_foreignKeyCols == null) _foreignKeyCols = new List<ModelDbItem>();
                        _foreignKeyCols.Add(item);
                    }
                    if (item.IsOrderColumn)
                    {
                        if (_orderByCols == null) _orderByCols = new List<ModelDbItem>();
                        _orderByCols.Add(item);
                    }
                    if (item.IsUnqiueKey)
                    {
                        if (_unqiueKeys == null) { _unqiueKeys = new ModelUnqiueKeyCollection(item); } else { _unqiueKeys.Add(item); }
                    }
                }
            }
            if (pri_field_items.Count == 0)
            {
                throw new NotDbColumnAsModelException(pri_field_type);
            }
            else
            {
                pri_field_items.Sort();
            }
        }

        List<ModelDbItem> getDisplayColumns()
        {
            if (dcols == null)
            {
                dcols = new List<ModelDbItem>();
                if (DisplayColumns != null && DisplayColumns.Count > 0)
                {
                    foreach (ModelDbItem b in DisplayColumns)
                    {
                        if (b.DisplayColumn.Scenes == _currentScenes)
                        {
                            dcols.Add(b);
                        }
                    }
                }
            }
            return dcols;
        }

        void buildGridColumns()
        {
            grid.AutoGenerateColumns = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowDrop = false;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.Columns.Clear();
            List<ModelDbItem> cols = getDisplayColumns();
            foreach (ModelDbItem b in cols)
            {
                if (b.DisplayColumn.Scenes == _currentScenes)
                {
                    DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
                    c.HeaderText = b.DisplayColumn.HeaderText;
                    c.FillWeight = b.DisplayColumn.FillWeight;
                    c.ReadOnly = true;
                    if (BaseModel.IsSubclass(b.DisplayColumn.DisplayType))
                    {
                        c.DataPropertyName = b.FieldName + "_" + b.DisplayColumn.DisplayField;
                    }
                    else
                    {
                        c.DataPropertyName = b.FieldName;
                    }
                    grid.Columns.Add(c);
                }
            }
            //grid.CellFormatting += new DataGridViewCellFormattingEventHandler(cellFormatting);
            //grid.DataError += new DataGridViewDataErrorEventHandler(dataError);
        }

        void dataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
        void cellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.FormattingApplied) return;
            if (e.Value == null) { e.Value = "NULL"; e.FormattingApplied = true; return; }
            if (e.Value.GetType() == typeof(System.DBNull)) { e.Value = "DBNULL"; e.FormattingApplied = true; return; }
            if (dcols == null) { getDisplayColumns(); }
            Console.WriteLine("cellFormatting " + dcols[e.ColumnIndex].DisplayColumn.HeaderText + " ValueType=" + e.Value.GetType().ToString() + " Value=" + e.Value.ToString());
            if (e.ColumnIndex < dcols.Count && !e.FormattingApplied && e.Value != null && e.Value.GetType() != typeof(DBNull) && !string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                //Console.WriteLine(dcols[e.ColumnIndex].DisplayColumn.HeaderText + "," + dcols[e.ColumnIndex].FieldName);
                ModelDbItem dbCol = dcols[e.ColumnIndex];
                DisplayColumnAttribute dspColAttr = dbCol.DisplayColumn;
                Type displayType = dspColAttr.DisplayType;
                if (displayType == typeof(bool))
                {
                    bool b = false;
                    if (bool.TryParse(e.Value.ToString(), out b))
                    {
                        e.Value = b ? "是" : "否";
                    }
                    else
                    {
                        e.Value = b ? "是" : "否";
                    }
                }
                else if (displayType == typeof(string) || displayType == typeof(int) || displayType == typeof(float))
                {
                    e.Value = e.Value.ToString();
                }
                else if (displayType == typeof(UserRole))
                {
                    e.Value = ((UserRole)e.Value).ToString("G");
                }
                else if (displayType == typeof(SchoolType))
                {
                    e.Value = ((SchemaType)e.Value).ToString("G");
                }
                else if (displayType == typeof(SchoolGrades))
                {
                    e.Value = ((SchoolGrades)e.Value).ToString("G");
                }
                else if (displayType == typeof(Semesters))
                {
                    e.Value = ((Semesters)e.Value).ToString("G");
                }
                else if (displayType == typeof(EditState))
                {
                    e.Value = ((EditState)e.Value).ToString("G");
                }
                else if (displayType == typeof(ReleaseState))
                {
                    e.Value = ((ReleaseState)e.Value).ToString("G");
                }
                else if (displayType == typeof(AnswerMode))
                {
                    e.Value = ((AnswerMode)e.Value).ToString("G");
                }
            }
            e.FormattingApplied = true;
        }

        private string getSelectSql1()
        {
            string joins = "";
            string fields = "";
            List<string> tables = new List<string>() { TableName };
            string ta = "";
            string a = "a.";
            foreach (ModelDbItem item in Columns)
            {
                if (string.IsNullOrWhiteSpace(fields))
                {
                    fields = a + item.FieldName;
                }
                else
                {
                    fields += "," + a + item.FieldName;
                }
                if (item.IsForeignKey && BaseModel.IsSubclass(item.ForeignKey.ModelType))
                {
                    if (item.DisplayColumn.DisplayType == item.ForeignKey.ModelType)
                    {
                        ModelDb md = MDs[item.ForeignKey.ModelType.Name];
                        tables.Add(md.TableName);
                        ta = item.FieldName + "_" + item.DisplayColumn.DisplayField;
                        if (string.IsNullOrWhiteSpace(fields))
                        {
                            fields = ta + "." + item.DisplayColumn.DisplayField;
                        }
                        else
                        {
                            fields += "," + ta + "_" + md + "." + item.DisplayColumn.DisplayField;
                        }
                        string j = " LEFT JOIN " + md.TableName + " AS " + ta + " ON " + ta + "." + item.ForeignKey.ValueField + "=" + a + item.FieldName;
                        if (string.IsNullOrWhiteSpace(joins))
                        {
                            joins = j;
                        }
                        else
                        {
                            joins += " " + j;
                        }
                    }
                }
            }
            return "SELECT " + fields + " FROM " + TableName + " AS a " + joins;
        }

        private string getInsertString()
        {
            string fields = "";
            string parameters = "";
            for (int i = 0; i < Columns.Count; i++)
            {
                ModelDbItem dbItem = Columns[i];
                if (fields == "")
                {
                    fields = dbItem.FieldName;
                    parameters = dbItem.ParameterName;
                }
                else
                {
                    fields += "," + dbItem.FieldName;
                    parameters += "," + dbItem.ParameterName;
                }
            }
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2})", new object[] { TableName, fields, parameters });
        }

        private string getDeleteString()
        {
            string where = "";
            for (int i = 0; i < PrimaryKeyColumn.Columns.Count; i++)
            {
                ModelDbItem dbItem = PrimaryKeyColumn.Columns[i];
                if (where == "")
                {
                    where = dbItem.FieldName + "=" + dbItem.ParameterName;
                }
                else
                {
                    where += " AND " + dbItem.FieldName + "=" + dbItem.ParameterName;
                }
            }
            return string.Format("DELETE FROM {0} WHERE {1}", new object[] { TableName, where });
        }

        private string getCreateTableString()
        {
            string sql = "CREATE TABLE " + TableName + "(";
            for (int i = 0; i < Columns.Count; i++)
            {
                ModelDbItem dbItem = Columns[i];
                string s = dbItem.FieldName + " " + (dbItem.IsAutoIdentity ? "INTEGER" : F.GetDbTypeStringForSqlite(dbItem.DataType));
                if (dbItem.IsPrimaryKey && PrimaryKeyColumn.Columns.Count == 1)
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
                if (i == 0)
                {
                    sql += s;
                }
                else
                {
                    sql += "," + s;
                }
            }
            if (HasPrimaryColumn && PrimaryKeyColumn.Columns.Count > 1)
            {
                sql += ",PRIMARY KEY(";
                for (int i = 0; i < PrimaryKeyColumn.Columns.Count; i++)
                {
                    string s = PrimaryKeyColumn.Columns[i].FieldName;
                    if (i == 0)
                    {
                        sql += s;
                    }
                    else
                    {
                        sql += "," + s;
                    }
                }
                sql += ")";
            }
            sql += ")";
            return sql;
        }

        private SQLiteParameter[] getParamters()
        {
            SQLiteParameter[] sps = new SQLiteParameter[Columns.Count];
            for (int i = 0; i < Columns.Count; i++)
            {
                sps[i] = new SQLiteParameter();
                sps[i].ParameterName = Columns[i].ParameterName;
                sps[i].DbType = Columns[i].DataType;
                sps[i].Size = Columns[i].Size;
                sps[i].SourceColumn = Columns[i].FieldName;
            }
            return sps;
        }

        /// <summary>
        /// 获取主键参数集合
        /// </summary>
        /// <returns></returns>
        private SQLiteParameter[] getPrimaryParameters()
        {
            SQLiteParameter[] parameters = new SQLiteParameter[PrimaryKeyColumn.Columns.Count];
            for (int i = 0; i < parameters.Length; i++)
            {
                ModelDbItem dbItem = PrimaryKeyColumn.Columns[i];
                parameters[i] = new SQLiteParameter();
                parameters[i].ParameterName = dbItem.ParameterName;
                parameters[i].DbType = dbItem.DataType;
                parameters[i].Size = dbItem.Size;
                parameters[i].SourceColumn = dbItem.FieldName;
            }
            return parameters;
        }

        private string getUpdateString()
        {
            string fields = "";
            string where = "";
            for (int i = 0; i < Columns.Count; i++)
            {
                ModelDbItem dbItem = Columns[i];
                if (dbItem.IsPrimaryKey)
                {
                    if (where == "")
                    {
                        where = dbItem.FieldName + "=" + dbItem.ParameterName;
                    }
                    else
                    {
                        where += " AND " + dbItem.FieldName + "=" + dbItem.ParameterName;
                    }
                }
                else
                {
                    if (fields == "")
                    {
                        fields = dbItem.FieldName + "=" + dbItem.ParameterName;
                    }
                    else
                    {
                        fields += "," + dbItem.FieldName + "=" + dbItem.ParameterName;
                    }
                }
            }
            return string.Format("UPDATE {0} SET {1} WHERE {2}", new object[] { TableName, fields, where });
        }

        /// <summary>
        /// 外键是否包含在本类型
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private bool IsConainForeignKey(ForeignKeyArg arg)
        {
            if (arg == null) return false;
            if (this._foreignKeyCols == null || this._foreignKeyCols.Count == 0) return false;
            for (int i = 0; i < _foreignKeyCols.Count; i++)
            {
                if (_foreignKeyCols[i].ForeignKey.ModelType == arg.Key.ModelType)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据外键参数获取包含该外键的数据列
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private ModelDbItem getForeignKey(ForeignKeyArg arg)
        {
            if (arg == null) return null;
            if (this._foreignKeyCols == null || this._foreignKeyCols.Count == 0) return null;
            for (int i = 0; i < _foreignKeyCols.Count; i++)
            {
                if (_foreignKeyCols[i].ForeignKey.ModelType == arg.Key.ModelType)
                {
                    return _foreignKeyCols[i];
                }
            }
            return null;
        }

        private string getOrderStr()
        {
            string order = "";
            if (HasOrderColumns)
            {
                for (int i = 0; i < OrderColumns.Count; i++)
                {
                    ModelDbItem orderCol = OrderColumns[i];
                    if (orderCol.OrderBy.OrderType != SortOrder.None)
                    {
                        if (string.IsNullOrWhiteSpace(order))
                        {
                            order = orderCol.GetOrder();
                        }
                        else
                        {
                            order += "," + orderCol.GetOrder();
                        }
                    }
                }
            }
            return order;
        }

        ModelDbItem getItemForFieldName(string fn)
        {
            if (string.IsNullOrWhiteSpace(fn)) return null;
            foreach (ModelDbItem item in pri_field_items)
            {
                if (item.FieldName == fn)
                {
                    return item;
                }
            }
            return null;
        }
        #endregion

        #region public properties
        public DisplayScenes CurrentScenes
        {
            get { return _currentScenes; }
            set { _currentScenes = value; }
        }
        /// <summary>
        /// 获取TreeNode节点Name属性需要的字段名
        /// </summary>
        public string TreeNodeNameField
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_tnNameField))
                {
                    for (int i = 0; i < pri_field_items.Count; i++)
                    {
                        if (pri_field_items[i].IsTreeNodeColumn && !pri_field_items[i].TreeNodeColumn.IsTextColumn)
                        {
                            _tnNameField = pri_field_items[i].FieldName;
                            break;
                        }
                    }
                }
                return _tnNameField;
            }
        }
        /// <summary>
        /// 获取TreeNode节点Text属性需要的字段名
        /// </summary>
        public string TreeNodeTextField
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_tnTextField))
                {
                    for (int i = 0; i < pri_field_items.Count; i++)
                    {
                        if (pri_field_items[i].IsTreeNodeColumn && pri_field_items[i].TreeNodeColumn.IsTextColumn)
                        {
                            _tnTextField = pri_field_items[i].FieldName;
                            break;
                        }
                    }
                }
                return _tnTextField;
            }
        }
        /// <summary>
        /// 类型对应的表名称
        /// </summary>
        public string TableName { get { return pri_field_tableName; } }
        /// <summary>
        /// SQLite数据连接
        /// </summary>
        public SQLiteConnection Connection { get; private set; }

        /// <summary>
        /// 全局内存数据集
        /// </summary>
        public DataSet GlobalDataSet { get; private set; }

        /// <summary>
        /// 获取模型中包含DbColumn特性的所有列
        /// </summary>
        public List<ModelDbItem> Columns { get { return pri_field_items; } }

        /// <summary>
        /// 获取具有显示特性的数据列集合
        /// </summary>
        public List<ModelDbItem> DisplayColumns
        {
            get
            {
                return _displayCols;
            }
        }

        /// <summary>
        /// 类型中是否包含了DisplayColumn特性
        /// </summary>
        public bool HasDisplayColumns
        {
            get { return _displayCols != null && _displayCols.Count > 0; }
        }

        /// <summary>
        /// 获取具有外键特性的数据列集合
        /// </summary>
        public List<ModelDbItem> ForeignKeyColumns
        {
            get
            {
                return _foreignKeyCols;
            }
        }

        /// <summary>
        /// 类型中是否包含外键特性
        /// </summary>
        public bool HasForeignKeyColumns
        {
            get { return _foreignKeyCols != null && _foreignKeyCols.Count > 0; }
        }

        /// <summary>
        /// 获取具有主键特性的数据列
        /// </summary>
        public ModelPrimaryKey PrimaryKeyColumn
        {
            get
            {
                return _primaryKey;
            }
        }

        /// <summary>
        /// 获取类型是否设置了主键特性
        /// </summary>
        public bool HasPrimaryColumn
        {
            get
            {
                return _primaryKey != null;
            }
        }

        /// <summary>
        /// 排序特性列集合
        /// </summary>
        public List<ModelDbItem> OrderColumns { get { return _orderByCols; } }

        /// <summary>
        /// 获取类型是否包含排序特性
        /// </summary>
        public bool HasOrderColumns
        {
            get
            {
                return _orderByCols != null && _orderByCols.Count > 0;
            }
        }

        /// <summary>
        /// 自增特性列
        /// </summary>
        public ModelDbItem AutoIdentityColumn { get { return _autoIdentity; } }

        /// <summary>
        /// 获取类型是否包含自增特性
        /// </summary>
        public bool HasAutoIdentityColumn { get { return _autoIdentity != null; } }

        /// <summary>
        /// 唯一键特性列集合
        /// </summary>
        public ModelUnqiueKeyCollection UnqiueKeyCollection { get { return _unqiueKeys; } }

        /// <summary>
        /// 获取类型是否包含唯一键特性
        /// </summary>
        public bool HasUnqiueKeyColumn
        {
            get
            {
                return _unqiueKeys != null;
            }
        }

        public DataGridView Grid
        {
            get { return grid; }
        }
        #endregion

        #region public methods
        public ModelDbItem GetForeignKeyColumn(Type type)
        {
            for (int i = 0; i < ForeignKeyColumns.Count; i++)
            {
                if (ForeignKeyColumns[i].ForeignKey.ModelType == type)
                {
                    return ForeignKeyColumns[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 返回是否包含某种类型的外键
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ContainForeignKey(Type type)
        {
            return GetForeignKeyColumn(type) != null;
        }

        /// <summary>
        /// 检查是否引用了某个外键的值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsQuoteForeignKeyValue(Type type, object value)
        {
            if (!GlobalDataSet.Tables.Contains(TableName)) return false;
            ModelDbItem item = GetForeignKeyColumn(type);
            if (item != null)
            {
                switch (item.DataType)
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
                        if (GlobalDataSet.Tables[TableName].Select(item.FieldName + "='" + value.ToString() + "'").Length > 0)
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
                        if (GlobalDataSet.Tables[TableName].Select(item.FieldName + "=" + value.ToString()).Length > 0)
                        {
                            return true;
                        }
                        break;
                    case DbType.Guid:
                        if (GlobalDataSet.Tables[TableName].Select("Convert(" + item.FieldName + ",'System.String')='" + ((Guid)value).ToString() + "'").Length > 0)
                        {
                            return true;
                        }
                        break;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据单一外键数据表
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public DataView SelectByForeignKey(List<ForeignKeyArg> args)
        {
            SelectExpression ses = new SelectExpression();
            string order = getOrderStr();
            if (args != null)
            {
                foreach (ForeignKeyArg arg in args)
                {
                    if (arg == null) throw new ArgumentNullException("arg");
                    if (!IsConainForeignKey(arg))
                    {
                        throw new Exception("此类型中的公共属性没有设置类型名为\"" + arg.Key.ModelType.Name + "\"的外键");
                    }
                    ModelDbItem item = getForeignKey(arg);
                    SelectExpression se = item.GetWhere(arg);
                    if (!string.IsNullOrWhiteSpace(se.SQLiteWhere))
                    {
                        if (string.IsNullOrWhiteSpace(ses.SQLiteWhere))
                        {
                            ses.SQLiteWhere = se.SQLiteWhere;
                            ses.DataTableWhere = se.DataTableWhere;
                        }
                        else
                        {
                            ses.SQLiteWhere += " AND " + se.SQLiteWhere;
                            ses.DataTableWhere += " AND " + se.DataTableWhere;
                        }
                    }
                }
            }
            if (!GlobalDataSet.Tables.Contains(TableName))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Connection))
                {
                    cmd.Connection = Connection;
                    string sql = getSelectSql1();
                    if (!string.IsNullOrWhiteSpace(ses.SQLiteWhere))
                    {
                        sql += " WHERE " + ses.SQLiteWhere;
                    }
                    if (!string.IsNullOrWhiteSpace(order))
                    {
                        sql += " ORDER BY " + order;
                    }
                    cmd.CommandText = sql;
                    using (SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd))
                    {
                        sda.Fill(GlobalDataSet, TableName);
                        if (HasPrimaryColumn)
                        {
                            DataColumn[] dcs = new DataColumn[PrimaryKeyColumn.Columns.Count];
                            for (int i = 0; i < dcs.Length; i++)
                            {
                                string fieldName = PrimaryKeyColumn.Columns[i].FieldName;
                                DbType dt = PrimaryKeyColumn.Columns[i].DataType;
                                dcs[i] = GlobalDataSet.Tables[TableName].Columns[fieldName];
                                switch (dt)
                                {
                                    case DbType.AnsiString:
                                    case DbType.AnsiStringFixedLength:
                                    case DbType.String:
                                    case DbType.StringFixedLength:
                                    case DbType.Xml:
                                        dcs[i].DataType = typeof(string);
                                        break;
                                    case DbType.Date:
                                    case DbType.DateTime:
                                    case DbType.DateTime2:
                                    case DbType.DateTimeOffset:
                                    case DbType.Time:
                                        dcs[i].DataType = typeof(DateTime);
                                        break;
                                    case DbType.Byte:
                                        dcs[i].DataType = typeof(byte);
                                        break;
                                    case DbType.SByte:
                                        dcs[i].DataType = typeof(sbyte);
                                        break;
                                    case DbType.Int16:
                                        dcs[i].DataType = typeof(Int16);
                                        break;
                                    case DbType.Int32:
                                        dcs[i].DataType = typeof(Int32);
                                        break;
                                    case DbType.Int64:
                                        dcs[i].DataType = typeof(Int64);
                                        break;
                                    case DbType.UInt16:
                                        dcs[i].DataType = typeof(UInt16);
                                        break;
                                    case DbType.UInt32:
                                        dcs[i].DataType = typeof(UInt32);
                                        break;
                                    case DbType.UInt64:
                                        dcs[i].DataType = typeof(UInt64);
                                        break;
                                    case DbType.Guid:
                                        dcs[i].DataType = typeof(Guid);
                                        break;
                                    case DbType.Object:
                                    case DbType.Binary:
                                        dcs[i].DataType = typeof(byte[]);
                                        break;
                                    case DbType.Single:
                                    case DbType.Currency:
                                    case DbType.Decimal:
                                    case DbType.Double:
                                    case DbType.VarNumeric:
                                        dcs[i].DataType = typeof(double);
                                        break;
                                    case DbType.Boolean:
                                        dcs[i].DataType = typeof(bool);
                                        break;
                                }
                            }
                            GlobalDataSet.Tables[TableName].PrimaryKey = dcs;
                        }
                    }
                }
            }
            //Console.WriteLine("DataTableWHERE='" + ses.DataTableWhere + "' ; SQLiteWHERE='" + ses.SQLiteWhere + "'");
            DataView dv = GlobalDataSet.Tables[TableName].DefaultView;
            dv.RowFilter = ses.DataTableWhere;
            dv.Sort = order;
            return dv;
        }

        public SQLiteCommand InsertCommandForDataTable()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = Connection;
            cmd.CommandText = getInsertString();
            cmd.Parameters.AddRange(getParamters());
            return cmd;
        }

        public SQLiteCommand UpdateCommandForDataTable()
        {
            SQLiteCommand cmd = new SQLiteCommand(getUpdateString(), Connection);
            cmd.Parameters.AddRange(getParamters());
            return cmd;
        }

        public SQLiteCommand DeleteCommandForDataTable()
        {
            SQLiteCommand cmd = new SQLiteCommand(getDeleteString(), Connection);
            cmd.Parameters.AddRange(getPrimaryParameters());
            return cmd;
        }

        /// <summary>
        /// 获取创建表的命令字符串
        /// </summary>
        /// <returns></returns>
        public string CreateTableString()
        {
            return getCreateTableString();
        }

        public object ToObject(DataRow r)
        {
            object t = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(pri_field_type))
            {
                t = Activator.CreateInstance(pri_field_type);
                for (int i = 0; i < Columns.Count; i++)
                {
                    PropertyInfo p = Columns[i].Property;
                    Type pt = p.PropertyType;
                    string fn = Columns[i].FieldName;
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
        public DataRow SaveDataRow(object obj)
        {
            Type t = obj.GetType();
            DataRow r = null;
            bool exis = false;
            Guid guid;
            Guid objId;
            if (t == pri_field_type)
            {
                foreach (DataRow row in GlobalDataSet.Tables[TableName].Rows)
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
                    r = GlobalDataSet.Tables[TableName].NewRow();
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
            }
            else
            {
                throw new Exception("不是需要的类型");
            }
            if (!exis)
            {
                GlobalDataSet.Tables[TableName].Rows.Add(r);
            }
            grid.Refresh();
            return r;
        }

        public TreeNode CreateTreeNode(DataRow r)
        {
            TreeNode node = new TreeNode();
            node.Name = GetTreeNodeName(r);
            node.Text = r[TreeNodeTextField].ToString();
            node.Tag = ToObject(r);
            return node;
        }

        public string GetTreeNodeName(DataRow r)
        {
            string strGuid = string.Format("{0:x2}", r[TreeNodeNameField].ToString());
            return "tn_" + Guid.Parse(strGuid).ToString();
        }

        public string GetTreeNodeName(object obj)
        {
            if (obj.GetType() == pri_field_type)
            {
                return "tn_" + ((BaseModel)obj).Id.ToString();
            }
            else
            {
                return "tn_" + Guid.NewGuid().ToString("N");
            }
        }

        public void RefreshGrid(List<ForeignKeyArg> args)
        {
            grid.DataSource = SelectByForeignKey(args);
        }

        public bool CheckData(object obj)
        {
            return true;
        }
        #endregion

        public ModelDb(Type type, SQLiteConnection conn, DataSet globalDataSet, DisplayScenes scenes = DisplayScenes.运营端)
        {
            if (BaseModel.IsSubclass(type))
            {
                if (conn == null) throw new ArgumentNullException("conn");
                if (globalDataSet == null) throw new ArgumentNullException("globalDataSet");
                if (conn.State == ConnectionState.Closed) conn.Open();

                this.Connection = conn;
                this.GlobalDataSet = globalDataSet;
                this.pri_field_type = type;
                this.pri_field_tableName = F.GetTableName(this.pri_field_type);
                this.internal_field_Properties = this.pri_field_type.GetProperties();
                this._currentScenes = scenes;
                buildItems();
                buildJoinTables();
                buildGridColumns();
            }
            else
            {
                throw new NotInheritBaseModelException(type);
            }
        }

        private void buildJoinTables()
        {
            JoinTables.Add(TableName);
            foreach (ModelDbItem item in Columns)
            {
                if (item.IsDisplayColumn && BaseModel.IsSubclass(item.DisplayColumn.DisplayType))
                {
                    if (item.IsForeignKey && item.ForeignKey.ModelType == item.DisplayColumn.DisplayType)
                    {
                        ModelDb md = MDs[item.DisplayColumn.DisplayType.Name];
                        if (!JoinTables.Contains(md.TableName))
                        {
                            JoinTables.Add(md.TableName);
                        }
                    }
                }
            }
        }

        internal ModelDb(Type type)
        {
            if (BaseModel.IsSubclass(type))
            {
                this.pri_field_type = type;
                this.pri_field_tableName = F.GetTableName(this.pri_field_type);
                this.internal_field_Properties = this.pri_field_type.GetProperties();
                buildItems();
                buildJoinTables();
                buildGridColumns();
            }
            else
            {
                throw new NotInheritBaseModelException(type);
            }
        }


        #region 静态函数
        public bool HasForeignKeyQuote(string fn, Guid Id)
        {
            foreach (KeyValuePair<string, ModelDb> kmd in MDs)
            {
                string sql = "SELECT COUNT(*) FROM " + kmd.Value.TableName + " WHERE UPPER(HEX(" + fn + "))='" + F.byteToHexStr(Id.ToByteArray()) + "'";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, Connection))
                {
                    int rows = (int)cmd.ExecuteScalar();
                    if (rows > 0) return true;
                }
                if (GlobalDataSet.Tables[kmd.Value.TableName].Select("Convert(" + fn + ",'System.String')='" + Id.ToString() + "')").Length > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasChild(Guid Id)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(Connection))
            {
                cmd.CommandText = "SELECT COUNT(*) FROM " + TableName + " WHERE UPPER(HEX(" + BaseModel.FN.ParentId + "))='" + F.byteToHexStr(Id.ToByteArray()) + "'";
                int rows = (int)cmd.ExecuteScalar();
                if (rows > 0) return true;
            }
            if (GlobalDataSet.Tables[TableName].Select("Convert(" + BaseModel.FN.ParentId + ",'System.String')='" + Id.ToString() + "'").Length > 0)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region classes
        /// <summary>
        /// 模型主键类型
        /// </summary>
        public class ModelPrimaryKey
        {
            #region private fields
            List<ModelDbItem> _cols = new List<ModelDbItem>();
            #endregion
            public string Name { get; set; }
            public List<ModelDbItem> Columns { get { return _cols; } }

            /// <summary>
            /// 添加ModelDbItem到ModelDbItem集合。如果加入成功或已经加入返回true，否则返回flase
            /// </summary>
            /// <param name="item"></param>
            /// <returns></returns>
            public bool Add(ModelDbItem item)
            {
                if (_cols.Count == 0)
                {
                    Name = item.PrimaryKey.Name;
                    _cols.Add(item);
                    return true;
                }
                else
                {
                    if (item.PrimaryKey.Name == Name)
                    {
                        bool isexists = false;
                        for (int i = 0; i < _cols.Count; i++)
                        {
                            if (_cols[i].FieldName == item.FieldName)
                            {
                                isexists = true;
                            }
                        }
                        if (!isexists)
                        {
                            _cols.Add(item);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            public ModelPrimaryKey(ModelDbItem item)
            {
                this.Name = item.PrimaryKey.Name;
                this._cols.Add(item);
            }
        }

        /// <summary>
        /// 唯一键特性
        /// </summary>
        public class ModelUnqiueKey
        {
            #region private fields
            List<ModelDbItem> _cols = new List<ModelDbItem>();
            #endregion

            #region public properties
            /// <summary>
            /// 获取唯一性特性列集合
            /// </summary>
            public List<ModelDbItem> Columns { get { return _cols; } }
            public string Name { get; set; }
            #endregion

            public bool Add(ModelDbItem item)
            {
                if (_cols.Count == 0)
                {
                    Name = item.PrimaryKey.Name;
                    _cols.Add(item);
                    return true;
                }
                else
                {
                    if (item.UnqiueKey.Name == Name)
                    {
                        bool isexists = false;
                        for (int i = 0; i < _cols.Count; i++)
                        {
                            if (_cols[i].FieldName == item.FieldName)
                            {
                                isexists = true;
                            }
                        }
                        if (!isexists)
                        {
                            _cols.Add(item);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            public ModelUnqiueKey(ModelDbItem item)
            {
                this.Name = item.UnqiueKey.Name;
                this._cols.Add(item);
            }
        }

        /// <summary>
        /// 模型唯一键特性集合
        /// </summary>
        public class ModelUnqiueKeyCollection : CollectionBase
        {
            public void Add(ModelDbItem item)
            {
                ModelUnqiueKey key = null;
                if (this.List.Count == 0)
                {
                    key = new ModelUnqiueKey(item);
                    this.List.Add(key);
                }
                else
                {
                    bool exists = false;
                    for (int i = 0; i < this.List.Count; i++)
                    {
                        key = (ModelUnqiueKey)List[i];
                        if (key.Name == item.UnqiueKey.Name)
                        {
                            key.Add(item);
                            break;
                        }
                    }
                    if (!exists)
                    {
                        key = new ModelUnqiueKey(item);
                        List.Add(key);
                    }
                }
            }
            /// <summary>
            /// 按索引获取唯一键特性
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            public ModelUnqiueKey this[int index]
            {
                get
                {
                    if (index >= 0 && index < List.Count)
                    {
                        return (ModelUnqiueKey)List[index];
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("index");
                    }
                }
            }

            /// <summary>
            /// 按名称获取唯一键特性
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public ModelUnqiueKey this[string name]
            {
                get
                {
                    if (List.Count == 0) return null;
                    foreach (ModelUnqiueKey key in List)
                    {
                        if (key.Name == name) { return key; }
                    }
                    return null;
                }
            }

            /// <summary>
            /// 返回唯一键特性集合中是否包含指定名称的唯一键特性
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public bool Contain(string name)
            {
                if (List.Count == 0) return false;
                foreach (ModelUnqiueKey key in List)
                {
                    if (key.Name == name) return true;
                }
                return false;
            }

            public ModelUnqiueKeyCollection(ModelDbItem item)
            {
                if (item.IsUnqiueKey)
                {
                    List.Add(new ModelUnqiueKey(item));
                }
                else
                {
                    throw new NotContainAttributeException(typeof(UnqiueKeyAttribute));
                }
            }
        }
        #endregion
    }
    #endregion

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
