using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using LearnLibs;
using LearnLibs.Enums;

namespace LearnLibs
{
    public static class ModelDbSet
    {
        #region private fields
        static SQLiteConnection CON = null;
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
            using (SQLiteCommand cmd = new SQLiteCommand(selectSql, CON))
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
            using (SQLiteCommand cmd = new SQLiteCommand(selectSql, CON))
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
    }
}
