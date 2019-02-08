using System;
using System.Data;
using System.Data.SQLite;
using System.Reflection;
using System.Windows.Forms;

namespace LearnLibs
{
    /// <summary>
    /// 类型数据列类型
    /// </summary>
    public class ModelDbItem : IComparable, IComparable<ModelDbItem>
    {
        #region private fields
        internal PropertyInfo _property = null;
        internal DisplayColumnAttribute _displayCol = null;
        internal DbColumnAttribute _col = null;
        internal PrimaryKeyAttribute _primaryKey = null;
        internal ForeignKeyAttribute _foreignKey = null;
        internal UnqiueKeyAttribute _unqiueKey = null;
        internal AutoIdentityAttribute _autoIdentity = null;
        internal OrderByAttribute _orderBy = null;
        internal TreeNodeColumnAttribute _treenodeColumn = null;
        #endregion

        #region private methods

        #endregion

        #region public properties
        /// <summary>
        /// 数据列关联的属性
        /// </summary>
        public PropertyInfo Property { get { return _property; } internal set { _property = value; } }

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

        public string AsField
        {
            get
            {
                if (this.IsDisplayColumn && BaseModel.IsSubclass(DisplayColumn.FromType))
                {
                    return FieldName + "_" + DisplayColumn.Field;
                }
                else
                {
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
                        _foreignKey.Name = _property.Name + "_" + _foreignKey.Field;
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

        public ModelDbItem()
        {
            // TODO: Complete member initialization
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
}
