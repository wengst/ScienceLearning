using System.Collections.Generic;

namespace LearnLibs
{
    /// <summary>
    /// 属性名与表名关系对应类型
    /// </summary>
    public class PropertyTable
    {
        public string PropertyName { get; set; }
        public string TableName { get; set; }
        public PropertyTable(string propertyName, string tableName) {
            if (!string.IsNullOrWhiteSpace(propertyName) && !string.IsNullOrWhiteSpace(tableName)) {
                this.PropertyName = propertyName;
                this.TableName = tableName;
            }
        }
        public PropertyTable(System.Reflection.PropertyInfo property, string tableName) {
            if (property != null && !string.IsNullOrWhiteSpace(tableName)) {
                this.PropertyName = property.Name;
                this.TableName = tableName;
            }
        }
    }
    /// <summary>
    /// 属性名与表名关系实例集合
    /// </summary>
    public class PropertyTableCollection
    {
        string aliases = "bcdefghijklmnopqrstuvwxyz";
        internal List<PropertyTable> List = new List<PropertyTable>();

        public void Add(PropertyTable tt)
        {
            if (tt != null)
            {
                List.Add(tt);
            }
        }

        /// <summary>
        /// 按索引获取TypeTable
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public PropertyTable this[int index]
        {
            get
            {
                if (index < List.Count && index >= 0)
                {
                    return List[index];
                }
                return null;
            }
        }

        /// <summary>
        /// 根据属性名获取表别名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string TableAlias(string propertyName) {
            if (!string.IsNullOrWhiteSpace(propertyName)) {
                for (int i = 0; i < List.Count; i++) {
                    if (List[i].PropertyName == propertyName) {
                        return aliases.Substring(i);
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据属性获取表别名
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string TableAlias(System.Reflection.PropertyInfo property) {
            if (property != null) {
                for (int i = 0; i < List.Count; i++) {
                    if (List[i].PropertyName == property.Name) {
                        return aliases.Substring(i, 1);
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据属性名获取表名
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string TableName(string propertyName) {
            if (!string.IsNullOrWhiteSpace(propertyName)) {
                for (int i = 0; i < List.Count; i++) {
                    if (List[i].PropertyName == propertyName) {
                        return List[i].TableName;
                    }
                }
            }
            return string.Empty;
        }

        public string TableName(System.Reflection.PropertyInfo p) {
            if (p != null) {
                return TableName(p.Name);
            }
            return string.Empty;
        }
    }
}
