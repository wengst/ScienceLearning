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
            this.DisplayType = typeof(string);
            this.Property = null;
            FillWeight = 100;
            this.ValueField = BaseModel.FN.Id;
        }
        #endregion
        #region public properties
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
        public Type DisplayType { get; set; }
        public string DisplayField { get; set; }
        public string ValueField { get; set; }
        /// <summary>
        /// 获取派生类属性，如果DisplayType是BaseModel的派生类，则Property表示派生类中的属性，否则为NULL
        /// </summary>
        public PropertyInfo Property { get; private set; }
        /// <summary>
        /// 表格列显示场景
        /// </summary>
        public Enums.DisplayScenes Scenes { get; set; }
        #endregion

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
        public DisplayColumnAttribute(string title, int index, Type type)
        {
            init();
            this.HeaderText = title;
            this.Index = index;
            if (BaseModel.IsSubclass(type))
            {
                throw new Exception("必须提供BaseModel派生类的属性");
            }
            else
            {
                this.DisplayType = type;
            }
        }
        public DisplayColumnAttribute(string title, int index, Type type, string fieldName)
        {
            init();
            this.HeaderText = title;
            this.Index = index;
            this.DisplayType = type;
            if (BaseModel.IsSubclass(type))
            {
                this.DisplayField = fieldName;
                this.Property = F.GetProperty(type, fieldName);
            }
        }
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
        /*
         *外键特性既外键约束。表中字段值来自于其他表对应字段的值。
         */
        #region private fields
        string _name = "foreignKey1";
        string _qdStr = "foreignKey_";
        Type _modelType = null;
        PropertyInfo _property = null;
        #endregion

        #region public properties
        public string Name { get { return _name; } }
        /// <summary>
        /// 获取外键特性的对应类型
        /// </summary>
        public Type ModelType { get { return _modelType; } }

        public string ValueField { get; set; }

        public string DisplayField { get; set; }
        #endregion

        #region public methods
        /// <summary>
        /// 外键是否为指定名称的外键
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Equal(string name)
        {
            return (_qdStr + name) == _name;
        }
        #endregion

        /// <summary>
        /// 以BaseModel的派生类类型和属性名实例化外键特性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="valueField"></param>
        public ForeignKeyAttribute(Type type, string valueField)
        {
            this._modelType = type;
            this.ValueField = valueField;
        }

        public ForeignKeyAttribute(Type type, string valueField, string displayField)
        {
            this._modelType = type;
            this.ValueField = valueField;
            this.DisplayField = displayField;
        }

        /// <summary>
        /// 以名称和类型实例化外键特性
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public ForeignKeyAttribute(Type type)
        {
            this._modelType = type;
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
    #endregion
}
