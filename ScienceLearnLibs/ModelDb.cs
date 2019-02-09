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
        List<ModelDbItem> items = new List<ModelDbItem>();
        List<ModelDbItem> _displayCols = null;
        List<ModelDbItem> _foreignKeyCols = null;
        ModelPrimaryKey _primaryKey = null;
        List<ModelDbItem> _orderByCols = null;
        ModelDbItem _autoIdentity = null;
        ModelUnqiueKeyCollection _unqiueKeys = null;
        DataGridView grid = new DataGridView();
        DisplayScenes _currentScenes = DisplayScenes.未设置;
        List<ModelDbItem> dcols = null;
        List<string> JoinTables = new List<string>();
        ModelTable _mt = new ModelTable();
        #endregion

        #region private motheds

        internal void buildItems()
        {
            if (internal_field_Properties.Length > 0)
            {
                foreach (PropertyInfo p in internal_field_Properties)
                {
                    object[] attrs = p.GetCustomAttributes(true);
                    ModelDbItem dbItem = new ModelDbItem();
                    dbItem.Property = p;
                    dbItem.ProDataType = p.PropertyType;
                    foreach (object a in attrs)
                    {
                        Type at = a.GetType();
                        if (at == typeof(DbColumnAttribute))
                        {
                            dbItem._col = a as DbColumnAttribute;
                            if (ListMember != null)
                            {
                                if (dbItem.FieldName == ListMember.DisplayMember)
                                {
                                    ListMember.DisplayProperty = p;
                                }
                                if (dbItem.FieldName == ListMember.ValueMember)
                                {
                                    ListMember.ValueProperty = p;
                                }
                            }
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
                        else if (at == typeof(ModelFieldXml)) {
                            dbItem.AttrName = ((ModelFieldXml)a).AttrName;
                        }
                    }
                    if (dbItem.Column != null)
                    {
                        items.Add(dbItem);
                        if (dbItem.IsPrimaryKey)
                        {
                            if (_primaryKey == null) { _primaryKey = new ModelPrimaryKey(dbItem); }
                            else { _primaryKey.Add(dbItem); }
                        }
                        if (dbItem.IsForeignKey)
                        {
                            if (_foreignKeyCols == null) { _foreignKeyCols = new List<ModelDbItem>(); }
                            _foreignKeyCols.Add(dbItem);
                        }
                        if (dbItem.IsDisplayColumn)
                        {
                            if (_displayCols == null) _displayCols = new List<ModelDbItem>();
                            _displayCols.Add(dbItem);
                        }
                        if (dbItem.IsAutoIdentity)
                        {
                            _autoIdentity = dbItem;
                        }
                        if (dbItem.IsOrderColumn)
                        {
                            if (_orderByCols == null) _orderByCols = new List<ModelDbItem>();
                            _orderByCols.Add(dbItem);
                        }
                        if (dbItem.IsUnqiueKey)
                        {
                            if (_unqiueKeys == null) { _unqiueKeys = new ModelUnqiueKeyCollection(dbItem); }
                            else { _unqiueKeys.Add(dbItem); }
                        }
                    }
                }
            }
            if (items.Count == 0)
            {
                throw new NotDbColumnAsModelException(pri_field_type);
            }
            else
            {
                items.Sort();
            }
        }

        internal void buildType()
        {
            object[] objs = pri_field_type.GetCustomAttributes(true);
            if (objs.Length > 0)
            {
                foreach (object obj in objs)
                {
                    Type type = obj.GetType();
                    if (type == typeof(DbTableAttribute))
                    {
                        this.pri_field_tableName = ((DbTableAttribute)obj).TableName;
                    }
                    else if (type == typeof(ListItemAttribute))
                    {
                        this.ListMember = obj as ListItemAttribute;
                    }
                    else if (type == typeof(ModelEditorAttribute))
                    {
                        this.ModelEditor = obj as ModelEditorAttribute;
                    }
                    else if (type == typeof(ModelTableXml))
                    {
                        this.XmlItemName = ((ModelTableXml)obj).ItemName;
                    }
                }
            }
            if (string.IsNullOrWhiteSpace(pri_field_tableName))
            {
                pri_field_tableName = pri_field_type.Name + "s";
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
                        if ((b.DisplayColumn.Scenes & _currentScenes) == _currentScenes)
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
            //Console.WriteLine(_currentScenes);
            foreach (ModelDbItem b in cols)
            {
                //Console.WriteLine(b.DisplayColumn.Scenes & _currentScenes);
                if ((b.DisplayColumn.Scenes & _currentScenes) == _currentScenes)
                {
                    DataGridViewTextBoxColumn c = new DataGridViewTextBoxColumn();
                    c.HeaderText = b.DisplayColumn.HeaderText;
                    c.FillWeight = b.DisplayColumn.FillWeight;
                    if (!string.IsNullOrWhiteSpace(b.DisplayColumn.Format))
                    {
                        c.DefaultCellStyle.Format = b.DisplayColumn.Format;
                    }
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
            //Console.WriteLine(this.TableName + " Build Columns Completed,Columns.Count=" + grid.Columns.Count.ToString());
            grid.CellFormatting += new DataGridViewCellFormattingEventHandler(cellFormatting);
            //grid.DataError += new DataGridViewDataErrorEventHandler(dataError);
        }

        void cellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.FormattingApplied) return;
            if (e.Value == null) { e.Value = "NULL"; e.FormattingApplied = true; return; }
            if (e.Value.GetType() == typeof(System.DBNull))
            {
                e.FormattingApplied = true; return;
            }
            if (dcols == null) { getDisplayColumns(); }
            //Console.WriteLine("cellFormatting " + dcols[e.ColumnIndex].DisplayColumn.HeaderText + " ValueType=" + e.Value.GetType().ToString() + " Value=" + e.Value.ToString());
            if (e.ColumnIndex < dcols.Count && !e.FormattingApplied && e.Value != null && e.Value.GetType() != typeof(DBNull) && !string.IsNullOrWhiteSpace(e.Value.ToString()))
            {
                //Console.WriteLine(dcols[e.ColumnIndex].DisplayColumn.HeaderText + "," + dcols[e.ColumnIndex].FieldName);
                ModelDbItem dbCol = dcols[e.ColumnIndex];
                DisplayColumnAttribute dspColAttr = dbCol.DisplayColumn;
                if (string.IsNullOrWhiteSpace(dspColAttr.Format))
                {
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
                else
                {
                    e.Value = string.Format(dspColAttr.Format, e.Value);
                    //Console.WriteLine("Formatted");
                }
            }
            e.FormattingApplied = true;
        }

        #endregion

        #region public properties
        /// <summary>
        /// 获取Xml节点属性名和属性数据列特性字典
        /// </summary>
        public Dictionary<string,ModelDbItem> XmlAttrs {
            get {
                Dictionary<string, ModelDbItem> xans = new Dictionary<string, ModelDbItem>();
                for (int i = 0; i < items.Count; i++) {
                    if (!string.IsNullOrWhiteSpace(items[i].AttrName)) {
                        xans.Add(items[i].AttrName,items[i]);
                    }
                }
                return xans;
            }
        }
        /// <summary>
        /// 获取XML节点名
        /// </summary>
        public string XmlItemName { get; private set; }
        public WhereArgs Args { get; set; }
        public ModelTable ModelTableInfo
        {
            get { return _mt; }
        }
        public DisplayScenes CurrentScenes
        {
            get { return _currentScenes; }
            set
            {
                if (_currentScenes != value)
                {
                    _currentScenes = value;
                    buildGridColumns();
                }
            }
        }

        public ModelEditorAttribute ModelEditor
        {
            get;
            internal set;
        }

        /// <summary>
        /// 类型对应的表名称
        /// </summary>
        public string TableName { get { return pri_field_tableName; } }

        /// <summary>
        /// 获取模型中包含DbColumn特性的所有列
        /// </summary>
        public List<ModelDbItem> Columns { get { return items; } }

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
        public ModelPrimaryKey PrimaryKey
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

        public bool ContainField(string fieldName)
        {
            bool r = false;
            foreach (ModelDbItem item in Columns)
            {
                if (item.FieldName == fieldName)
                {
                    r = true;
                    break;
                }
            }
            return r;
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
        /// <summary>
        /// List控件成员特性
        /// </summary>
        public ListItemAttribute ListMember { get; internal set; }
        #endregion

        #region public methods
        public bool CheckData(object obj)
        {
            return true;
        }

        public ModelDbItem GetForeignKey(Type type)
        {
            if (HasForeignKeyColumns)
            {
                foreach (ModelDbItem item in ForeignKeyColumns)
                {
                    if (item.ForeignKey.Type == type)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
        #endregion

        internal ModelDb(Type type)
        {
            if (BaseModel.IsSubclass(type))
            {
                this.pri_field_type = type;
                this.internal_field_Properties = this.pri_field_type.GetProperties();
                buildType();
                buildItems();
                buildGridColumns();
            }
            else
            {
                throw new NotInheritBaseModelException(type);
            }
        }

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
