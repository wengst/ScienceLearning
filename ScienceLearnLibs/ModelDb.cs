using LearnLibs.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Forms;

namespace LearnLibs
{
    /// <summary>
    /// 模型数据操作类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ModelDb
    {
        #region private fields
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
        PropertyTableCollection _ptc = new PropertyTableCollection();
        #endregion

        #region private motheds
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
                    object[] attrs = p.GetCustomAttributes(true);
                    ModelDbItem dbItem = new ModelDbItem();
                    foreach (object a in attrs)
                    {
                        Type at = a.GetType();
                        if (at == typeof(DbColumnAttribute))
                        {
                            dbItem._col = a as DbColumnAttribute;
                        }
                        else if (at == typeof(PrimaryKeyAttribute))
                        {
                            dbItem._primaryKey = a as PrimaryKeyAttribute;
                        }
                        else if (at == typeof(DisplayColumnAttribute))
                        {
                            dbItem._displayCol = a as DisplayColumnAttribute;
                        }
                        else if (at == typeof(ForeignKeyAttribute))
                        {
                            dbItem._foreignKey = a as ForeignKeyAttribute;
                        }
                        else if (at == typeof(OrderByAttribute))
                        {
                            dbItem._orderBy = a as OrderByAttribute;
                        }
                        else if (at == typeof(UnqiueKeyAttribute))
                        {
                            dbItem._unqiueKey = a as UnqiueKeyAttribute;
                        }
                        else if (at == typeof(TreeNodeColumnAttribute))
                        {
                            dbItem._treenodeColumn = a as TreeNodeColumnAttribute;
                        }
                        else if (at == typeof(AutoIdentityAttribute))
                        {
                            dbItem._autoIdentity = a as AutoIdentityAttribute;
                        }
                    }
                    if (dbItem.Column != null) {
                        pri_field_items.Add(dbItem);
                        if (dbItem.IsPrimaryKey) {
                            if (_primaryKey == null) { _primaryKey = new ModelPrimaryKey(dbItem); }
                            else { _primaryKey.Add(dbItem); }
                        }
                        if (dbItem.IsForeignKey) {
                            if (_foreignKeyCols == null) { _foreignKeyCols = new List<ModelDbItem>(); }
                            _foreignKeyCols.Add(dbItem);
                        }
                        if (dbItem.IsDisplayColumn) {
                            if (_displayCols == null) _displayCols = new List<ModelDbItem>();
                            _displayCols.Add(dbItem);
                            if (BaseModel.IsSubclass(dbItem.DisplayColumn.FromType))
                            {
                                PropertyTable pt = new PropertyTable(dbItem.Property, this.TableName);
                                _ptc.Add(pt); 
                            }
                        }
                        if (dbItem.IsAutoIdentity) {
                            _autoIdentity = dbItem;
                        }
                        if (dbItem.IsOrderColumn) {
                            if (_orderByCols == null) _orderByCols = new List<ModelDbItem>();
                            _orderByCols.Add(dbItem);
                        }
                        if (dbItem.IsUnqiueKey) {
                            if (_unqiueKeys == null) { _unqiueKeys = new ModelUnqiueKeyCollection(dbItem); }
                            else { _unqiueKeys.Add(dbItem); }
                        }
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
                    if (BaseModel.IsSubclass(b.DisplayColumn.FromType))
                    {
                        c.DataPropertyName = b.FieldName + "_" + b.DisplayColumn.Field;
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
                Type displayType = dspColAttr.FromType;
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
                if (item.IsForeignKey && BaseModel.IsSubclass(item.ForeignKey.Type))
                {
                    if (item.DisplayColumn.FromType == item.ForeignKey.Type)
                    {
                        ModelDb md = MDs[item.ForeignKey.Type.Name];
                        tables.Add(md.TableName);
                        ta = item.FieldName + "_" + item.DisplayColumn.Field;
                        if (string.IsNullOrWhiteSpace(fields))
                        {
                            fields = ta + "." + item.DisplayColumn.Field;
                        }
                        else
                        {
                            fields += "," + ta + "_" + md + "." + item.DisplayColumn.Field;
                        }
                        string j = " LEFT JOIN " + md.TableName + " AS " + ta + " ON " + ta + "." + item.ForeignKey.Field + "=" + a + item.FieldName;
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
                if (_foreignKeyCols[i].ForeignKey.Type == arg.Key.Type)
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
                if (_foreignKeyCols[i].ForeignKey.Type == arg.Key.Type)
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
        public PropertyTableCollection PTC {
            get { return _ptc; }
        }
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
                if (ForeignKeyColumns[i].ForeignKey.Type == type)
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
                        throw new Exception("此类型中的公共属性没有设置类型名为\"" + arg.Key.Type.Name + "\"的外键");
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

        public bool CheckData(object obj)
        {
            return true;
        }
        #endregion

        public ModelDb(Type type, SQLiteConnection conn, DataSet globalDataSet, DisplayScenes scenes = DisplayScenes.运营端)
        {
            if (BaseModel.IsSubclass(type))
            {
                this.pri_field_type = type;
                this.pri_field_tableName = F.GetTableName(this.pri_field_type);
                this.internal_field_Properties = this.pri_field_type.GetProperties();
                this._currentScenes = scenes;
                buildItems();
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
                if (item.IsDisplayColumn && BaseModel.IsSubclass(item.DisplayColumn.FromType))
                {
                    if (item.IsForeignKey && item.ForeignKey.Type == item.DisplayColumn.FromType)
                    {
                        ModelDb md = MDs[item.DisplayColumn.FromType.Name];
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
}
