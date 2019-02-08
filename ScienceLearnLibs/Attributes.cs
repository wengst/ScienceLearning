using LearnLibs.Enums;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace LearnLibs
{
    #region Attributes
    /// <summary>
    /// 数据表特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DbTableAttribute : Attribute
    {
        #region public properties
        public string TableName { get; set; }
        #endregion

        public DbTableAttribute(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException("tableName");
            this.TableName = tableName;
        }
    }

    /// <summary>
    /// 当该类型的DataTable被用于ComboBoxList，CheckBoxList,TreeView之类控件的数据源时，指定ValueMember和DispalyMember
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ListItemAttribute : Attribute {
        public string ValueMember { get; set; }
        /// <summary>
        /// ValueMember对应的类型属性
        /// </summary>
        public PropertyInfo ValueProperty { get;internal set; }
        public string DisplayMember { get; set; }
        /// <summary>
        /// DisplayMember对应的类型属性
        /// </summary>
        public PropertyInfo DisplayProperty { get; internal set; }
        public ListItemAttribute(string valueMember,string displayMember) {
            this.ValueMember = valueMember;
            this.DisplayMember = displayMember;
        }
    }

    /// <summary>
    /// 模型编辑器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModelEditorAttribute : Attribute {
        public Type Editor { get; set; }
        public ModelEditorAttribute() { }
        public ModelEditorAttribute(Type type) { this.Editor = type; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class ItemEditorAttribute : Attribute {
        public Type Editor { get; set; }
        public ItemEditorAttribute() { }
        public ItemEditorAttribute(Type type) { this.Editor = type; }
    }

    /// <summary>
    /// 表格显示特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DisplayColumnAttribute : Attribute
    {
        #region private methods
        private void init()
        {
            this.HeaderText = "";
            this.Format = "";
            this.Index = 1;
            this.Scenes = DisplayScenes.运营端;
            this.FromType = typeof(string);
            this.FillWeight = 100;
        }
        #endregion

        #region public properties
        /// <summary>
        /// 显示类型是否是枚举类型
        /// </summary>
        public bool IsEnum {
            get {
                if (FromType != null) {
                    return FromType.IsEnum;
                }
                return false;
            }
        }

        /// <summary>
        /// 表格列标题文本
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// 单元格文本格式化字符串
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 列顺序(数字越小越靠左)
        /// </summary>
        public int Index { get; set; }

        public int FillWeight { get; set; }

        /// <summary>
        /// 显示类型，如枚举,字符串
        /// </summary>
        public Type FromType { get; set; }

        /// <summary>
        /// 显示内容的表字段
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 表格列显示场景
        /// </summary>
        public Enums.DisplayScenes Scenes { get; set; }
        #endregion

        #region 构造函数
        public DisplayColumnAttribute(string title)
        {
            init();
            this.HeaderText = title;
        }

        public DisplayColumnAttribute(string title, int index)
        {
            init();
            this.HeaderText = title;
            this.Index = index;
        }

        public DisplayColumnAttribute(string title, int index, string format)
        {
            init();
            this.HeaderText = title;
            this.Index = index;
            this.Format = format;
        }

        public DisplayColumnAttribute(string title, int index, Type type) {
            init();
            this.HeaderText = title;
            this.Index = index;
            this.FromType = type;
        }

        public DisplayColumnAttribute(string title, int index, Type type, string fieldName)
        {
            init();
            this.HeaderText = title;
            this.Index = index;
            this.FromType = type;
            this.Field = fieldName;
        } 
        #endregion
    }

    public class TreeNodeColumnAttribute : Attribute
    {
        public bool IsTextColumn { get; set; }
        public TreeNodeColumnAttribute() { }
        public TreeNodeColumnAttribute(bool textColumn)
        {
            IsTextColumn = textColumn;
        }
    }

    /// <summary>
    /// 数据列特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbColumnAttribute : Attribute, IComparable, IComparable<DbColumnAttribute>
    {
        #region public properties
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DataType { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public int Size { get; set; }
        /// <summary>
        /// 精度
        /// </summary>
        public int Precision { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        public object DefaultValue { get; set; }
        /// <summary>
        /// 是否允许空值
        /// </summary>
        public bool IsAllowNull { get; set; }
        public int Index { get; set; }
        #endregion

        #region private methods
        private void init()
        {
            this.DefaultValue = null;
            this.Precision = 0;
            this.IsAllowNull = true;
        }
        #endregion

        #region 构造函数
        public DbColumnAttribute(string fieldName, DbType type)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) throw new ArgumentNullException("fieldName");
            this.FieldName = fieldName;
            this.DataType = type;
            switch (type)
            {
                case DbType.Byte:
                    this.Size = 1;
                    break;
                case DbType.Int16:
                case DbType.UInt16:
                    this.Size = 2;
                    break;
                case DbType.UInt32:
                case DbType.Int32:
                    this.Size = 4;
                    break;
                case DbType.Int64:
                case DbType.UInt64:
                    this.Size = 8;
                    break;
            }
            init();
        }
        public DbColumnAttribute(string fieldName, DbType type, int size)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) throw new ArgumentNullException("fieldName");
            this.FieldName = fieldName;
            this.DataType = type;
            this.Size = size < 0 ? 1 : size;
            init();
        }
        public DbColumnAttribute(string fieldName, DbType type, int size, int precision)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) throw new ArgumentNullException("fieldName");
            this.FieldName = fieldName;
            this.DataType = type;
            this.Size = size < 0 ? 1 : size;
            init();
            this.Precision = precision > (size - 1) ? 0 : precision;
        }
        #endregion

        public int CompareTo(object obj)
        {
            if (this.Index > ((DbColumnAttribute)obj).Index)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public int CompareTo(DbColumnAttribute other)
        {
            return this.Index > other.Index ? 1 : 0;
        }
    }

    /// <summary>
    /// 主键特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrimaryKeyAttribute : Attribute
    {
        #region public properties
        /// <summary>
        /// 主键名称
        /// </summary>
        public string Name { get; set; }
        #endregion

        public PrimaryKeyAttribute() { this.Name = "primaryKey1"; }
        public PrimaryKeyAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            this.Name = name;
        }
    }

    /// <summary>
    /// 外键特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ForeignKeyAttribute : Attribute
    {
        #region public properties
        /// <summary>
        /// 外键名
        /// </summary>
        public string Name { get; internal set; }
        /// <summary>
        /// 获取外键特性的对应类型
        /// </summary>
        public Type Type { get; internal set; }
        /// <summary>
        /// 外键表的值字段
        /// </summary>
        public string Field { get; set; }
        #endregion

        #region public methods
        /// <summary>
        /// 外键是否为指定名称的外键
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Equal(ForeignKeyAttribute obj)
        {
            return obj.Name == this.Name;
        }
        #endregion

        /// <summary>
        /// 以BaseModel的派生类类型和属性名实例化外键特性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        public ForeignKeyAttribute(Type type, string field)
        {
            this.Type = type;
            this.Field = field;
            this.Name = field;
        }
    }

    /// <summary>
    /// 自增特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIdentityAttribute : Attribute
    {
        #region private fields
        int _start = 1;
        int _length = 1;
        #endregion

        #region pubic properties
        /// <summary>
        /// 自增开始数字
        /// </summary>
        public int Start { get { return _start; } }
        /// <summary>
        /// 自增步长
        /// </summary>
        public int Length { get { return _length; } }
        #endregion

        public AutoIdentityAttribute(int start, int length)
        {
            if (start <= 0) { start = 1; }
            if (length <= 0) { length = 1; }
            this._start = start;
            this._length = length;
        }
    }

    /// <summary>
    /// 唯一性约束特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UnqiueKeyAttribute : Attribute
    {
        #region public properties
        /// <summary>
        /// 唯一约束特性名
        /// </summary>
        public string Name { get; set; }
        #endregion
        public UnqiueKeyAttribute() { this.Name = "unqiueKey1"; }
        public UnqiueKeyAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException("name");
            this.Name = name;
        }
    }

    /// <summary>
    /// 排序特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OrderByAttribute : Attribute
    {
        #region public priperties
        public SortOrder OrderType { get; set; }
        #endregion
        public OrderByAttribute() { this.OrderType = SortOrder.Ascending; }
        public OrderByAttribute(SortOrder orderBy)
        {
            this.OrderType = orderBy;
        }
    }

    /// <summary>
    /// 模型表的XML特性
    /// </summary>
    public class ModelTableXml : Attribute {
        public string ItemName { get; set; }
        public string ListName { get; set; }
        public ModelTableXml() { }
        /// <summary>
        /// 以列表名和单项名实例化ModelTable特性
        /// </summary>
        /// <param name="collName"></param>
        /// <param name="itemName"></param>
        public ModelTableXml(string collName, string itemName) {
            this.ListName = collName;
            this.ItemName = itemName;
        }
    }

    /// <summary>
    /// 模型属性的XML特性
    /// </summary>
    public class ModelFieldXml : Attribute {
        public string AttrName { get; set; }
        public ModelFieldXml() { }
        public ModelFieldXml(string attrname) {
            this.AttrName = attrname;
        }
    }
    #endregion
}
