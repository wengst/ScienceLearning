using System.Collections.Generic;
using System.Reflection;
using System;
namespace LearnLibs
{
    /// <summary>
    /// 属性名与表名关系对应类型。仅支持右表为主表的情况。
    /// </summary>
    public class ModelField
    {
        #region private fields
        string _joinTableName = string.Empty;
        string _joinTableAlias = string.Empty;
        string _sourceField = string.Empty;
        string _joinField = string.Empty;
        string _displayField = string.Empty;
        internal ModelTable _mainTable = null;
        #endregion
        public ModelTable Table { get { return _mainTable; } }
        /// <summary>
        /// 获取Join表名称
        /// </summary>
        public string JoinTableName { get { return _joinTableName; } }
        /// <summary>
        /// 获取Join表别名
        /// </summary>
        public string JoinTableAlias { get { return _joinTableAlias; } set { _joinTableAlias = value; } }

        /// <summary>
        /// 获取是否需要JOIN
        /// </summary>
        public bool IsJoin { get { return !string.IsNullOrWhiteSpace(JoinTableName); } }

        /// <summary>
        /// 获取或设置源字段，或用于写在JOIN语句部分的右侧
        /// </summary>
        public string SourceField { get { return _sourceField; } }
        /// <summary>
        /// 获取用于连接的字段.写在JOIN语句部分的左侧。
        /// </summary>
        public string ForeignKeyField { get { return _joinField; } }

        /// <summary>
        /// 获取用于Select语句的字段列表部分
        /// </summary>
        public string DisplayField { get { return _displayField; } }

        public string AsField
        {
            get
            {
                if (IsJoin)
                {
                    return SourceField + "_" + DisplayField;
                }
                else
                {
                    return SourceField;
                }
            }
        }

        public string SelectAsField
        {
            get
            {
                if (IsJoin)
                {
                    return JoinTableAlias + "." + DisplayField + " as " + AsField;
                }
                else
                {
                    return "a." + SourceField + " as " + AsField;
                }
            }
        }

        /// <summary>
        /// 左连接字符串，Ex：LEFT JOIN x.FieldName=a.FieldName
        /// </summary>
        public string LeftJoin
        {
            get
            {
                if (IsJoin)
                {
                    return "LEFT JOIN " + JoinTableName + " " + JoinTableAlias + " ON " + JoinTableAlias + "." + ForeignKeyField + "=a." + SourceField;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 内连接字符串，Ex：INNER JOIN x.FieldName=a.FieldName
        /// </summary>
        public string InnerJoin
        {
            get
            {
                if (IsJoin)
                {
                    return "INNER JOIN " + JoinTableName + " " + JoinTableAlias + " ON " + JoinTableAlias + "." + ForeignKeyField + "=a." + SourceField;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 右连接字符串，Ex：RIGHT JOIN x.FieldName=a.FieldName
        /// </summary>
        public string RightJoin
        {
            get
            {
                if (IsJoin)
                {
                    return "RIGHT JOIN " + JoinTableName + " " + JoinTableAlias + " ON " + JoinTableAlias + "." + ForeignKeyField + "=a." + SourceField;
                }
                return string.Empty;
            }
        }

        public ModelField(string sourceField)
        {
            this._sourceField = sourceField;
        }

        public ModelField(string sourceField, string joinTableName, string joinTableAlias, string joinField, string displayField)
        {
            this._sourceField = sourceField;
            this._joinTableName = joinTableName;
            this.JoinTableAlias = joinTableAlias;
            this._joinField = joinField;
            this._displayField = displayField;
        }
    }

    /// <summary>
    /// 属性名与表名关系实例集合
    /// </summary>
    public class ModelTable
    {
        string aliases = "bcdefghijklmnopqrstuvwxyz";
        internal List<ModelField> Fields = new List<ModelField>();
        internal int tables = 0;
        /// <summary>
        /// 模型对应的表名称
        /// </summary>
        public string MainTable { get; set; }

        public void Add(string sourceField)
        {
            if (!string.IsNullOrWhiteSpace(sourceField))
            {
                ModelField mf = new ModelField(sourceField);
                mf._mainTable = this;
                this.Fields.Add(mf);
            }
        }

        public void Add(string mainTableField, string joinTable, string foreignKeyField, string displayField)
        {
            if (!string.IsNullOrWhiteSpace(mainTableField) &&
                !string.IsNullOrWhiteSpace(joinTable) &&
                !string.IsNullOrWhiteSpace(foreignKeyField) &&
                !string.IsNullOrWhiteSpace(displayField)
                )
            {
                string alias = aliases.Substring(tables, 1);
                tables++;
                ModelField mf = new ModelField(mainTableField, joinTable, alias, foreignKeyField, displayField);
                mf._mainTable = this;
                this.Fields.Add(mf);
            }
        }

        /// <summary>
        /// 按索引获取TypeTable
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ModelField this[int index]
        {
            get
            {
                if (index < Fields.Count && index >= 0)
                {
                    return Fields[index];
                }
                return null;
            }
        }

        /// <summary>
        /// 根据asField获取ModelField
        /// </summary>
        /// <param name="asField"></param>
        /// <returns></returns>
        public ModelField this[string asField]
        {
            get
            {
                foreach (ModelField mf in Fields)
                {
                    if (mf.AsField == asField)
                    {
                        return mf;
                    }
                }
                return null;
            }
        }

        public string SelectFields
        {
            get
            {
                string str = string.Empty;
                foreach (ModelField f in Fields)
                {
                    str = F.JoinString(str, f.SelectAsField, ",");
                }
                return str;
            }
        }

        /// <summary>
        /// 获取形如 Select a.Id as Id,a.Name as Name,b.Title as PressId_Title From table1 a Left Join table2 b on b.Id=a.PressId这样的SQL语句
        /// </summary>
        public string LeftJoinSelectSql
        {
            get
            {
                string sql = "SELECT {0} FROM " + MainTable + " a {1}";
                string fields = string.Empty;
                string joins = string.Empty;
                foreach (ModelField f in Fields)
                {
                    fields = F.JoinString(fields, f.SelectAsField, ",");
                    if (f.IsJoin)
                    {
                        joins = F.JoinString(joins, f.LeftJoin, " ");
                    }
                }
                return string.Format(sql, fields, joins);
            }
        }

        /// <summary>
        /// 获取形如 Select a.Id as Id,a.Name as Name,b.Title as PressId_Title From table1 a Inner Join table2 b on b.Id=a.PressId这样的SQL语句
        /// </summary>
        public string InnerJoinSelectSql
        {
            get
            {
                string sql = "SELECT {0} FROM " + MainTable + " a {1}";
                string fields = string.Empty;
                string joins = string.Empty;
                foreach (ModelField f in Fields)
                {
                    fields = F.JoinString(fields, f.SelectAsField, ",");
                    if (f.IsJoin)
                    {
                        joins = F.JoinString(joins, f.InnerJoin, " ");
                    }
                }
                return string.Format(sql, fields, joins);
            }
        }

        /// <summary>
        /// 获取形如 Select a.Id as Id,a.Name as Name,b.Title as PressId_Title From table1 a Right Join table2 b on b.Id=a.PressId这样的SQL语句
        /// </summary>
        public string RightJoinSelectSql
        {
            get
            {
                string sql = "SELECT {0} FROM " + MainTable + " a {1}";
                string fields = string.Empty;
                string joins = string.Empty;
                foreach (ModelField f in Fields)
                {
                    fields = F.JoinString(fields, f.SelectAsField, ",");
                    if (f.IsJoin)
                    {
                        joins = F.JoinString(joins, f.RightJoin, " ");
                    }
                }
                return string.Format(sql, fields, joins);
            }
        }
    }
}
