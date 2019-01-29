using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.Reflection;

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
        static bool isBaseModel(Type t)
        {
            if (t == null) return false;
            return BaseModel.IsSubclass(t);
        }
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
        static ModelDbItem getForeignKeyItem(Type type, ForeignKeyArg arg)
        {
            ModelDbItem dbItem = null;
            if (isBaseModel(type) && arg != null)
            {
                ModelDb md = ModelDbs[type.Name];
                if (md.HasForeignKeyColumns)
                {
                    foreach (ModelDbItem item in md.ForeignKeyColumns)
                    {
                        if (item.ForeignKey.Name == arg.Key.Name)
                        {
                            dbItem = item;
                            break;
                        }
                    }
                }
            }
            return dbItem;
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
