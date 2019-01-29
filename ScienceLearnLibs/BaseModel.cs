using System;
using System.Data;
using System.Reflection;

namespace LearnLibs
{
    public abstract class BaseModel
    {
        /// <summary>
        /// 表字段名-FieldName
        /// </summary>
        public class FN
        {
            public const string Id = "Id";
            public const string InServer = "InServer";
            public const string LastModify = "lastModify";
            public const string ParentId = "ParentId";
            public const string Code = "Code";
            public const string Name = "Name";
            public const string IsValid = "IsValid";
            public const string PressId = "PressId";
            public const string FullName = "FullName";
            public const string ShortName = "ShortName";
            public const string ImplementDate = "ImplementDate";
            public const string SchoolGrade = "SchoolGradeId";
            public const string Semester = "SemesterId";
            public const string Text = "Text";
            public const string IndexNo = "IndexNo";
            public const string TeachBookId = "TeachBookId";
            public const string xPath = "xPath";
            public const string AccountName = "AccountName";
            public const string AccountPassword = "AccountPassword";
            public const string Role = "UserRole";
            public const string Alias = "Alias";
            public const string AreaCode = "AreaCode";
            public const string SchoolId = "SchoolId";
            public const string SchoolClassId = "SchoolClassId";
            public const string SchoolType = "SchoolType";
            public const string Period = "Period";
            /// <summary>
            /// 开始年级
            /// </summary>
            public const string Admission = "Admission";
            /// <summary>
            /// 毕业年级
            /// </summary>
            public const string Graduation = "Graduation";
            public const string ProvinceId = "ProvinceId";
            public const string CityId = "CityId";
            public const string DistrictId = "DistrictId";
            public const string UserId = "UserId";
            public const string TeachingId = "TeachingId";
            public const string ContentUrl = "ContentUrl";
            public const string AnalysisUrl = "AnalysisUrl";
            public const string CreatedTime = "CreatedTime";
            public const string EditState = "EditState";
            public const string ReleaseState = "ReleaseState";
            public const string Price = "Price";
            public const string Title = "Title";
            public const string SubjectId = "SubjectId";
            public const string QuestionId = "QuestionId";
            public const string StandardId = "StandardId";
            public const string AnswerMode = "AnswerMode";
            public const string OptionChars = "OptionChars";
            public const string KeyChars = "KeyChars";
            public const string Difficult = "Difficult";
            public const string Score = "Score";
        }
        /// <summary>
        /// 简单TreeViewName绑定字段
        /// </summary>
        public string TreeNodeNameField = "Id";
        /// <summary>
        /// 简单TreeViewText绑定字段
        /// </summary>
        public string TreeNodeTextField = "";

        private Guid _id = Guid.Empty;
        /// <summary>
        /// 本地ID
        /// </summary>
        [DbColumn(FN.Id, DbType.Guid, 16, IsAllowNull = false, Index = -3)]
        [PrimaryKey()]
        [TreeNodeColumn()]
        public Guid Id { get { return _id; } set { _id = value; } }

        /// <summary>
        /// 是否已经存入服务器
        /// </summary>
        [DbColumn(FN.InServer, DbType.Boolean, IsAllowNull = false, DefaultValue = false, Index = -2)]
        public bool InServer { get; set; }

        [DbColumn(FN.LastModify, DbType.DateTime, IsAllowNull = false, Index = -1)]
        public DateTime LastModify { get; set; }
        public virtual string TreeNodeNameValue()
        {
            return Id.ToString("x2");
        }
        public virtual string TreeNodeTextValue()
        {
            return "";
        }
        public abstract void SetDataRow(ref DataRow r);
        protected void SetValues(ref DataRow r)
        {
            F.SetValue(ref r, FN.Id, Id);
            F.SetValue(ref r, FN.InServer, InServer);
            F.SetValue(ref r, FN.LastModify, LastModify);
        }
        public abstract BaseModel ToObject(DataRow r);
        protected T BuildInstance<T>(DataRow r) where T : BaseModel, new()
        {
            T obj = new T();
            obj.Id = F.GetValue<Guid>(r, FN.Id, Guid.Empty);
            obj.InServer = F.GetValue<bool>(r, FN.InServer, false);
            obj.LastModify = F.GetValue<DateTime>(r, FN.LastModify, DateTime.Now);
            return obj;
        }
        /// <summary>
        /// 是否继承自BaseModel
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsSubclass(Type type)
        {
            if (type == null) return false;
            return type.IsSubclassOf(typeof(BaseModel));
        }

        public static bool IsFieldInType(Type type, string field) {
            if (type == null || string.IsNullOrWhiteSpace(field)) return false;
            if (IsSubclass(type)) {
                PropertyInfo[] pis = type.GetProperties();
                foreach (PropertyInfo p in pis)
                {
                    object[] ats = p.GetCustomAttributes(typeof(DbColumnAttribute),true);
                    foreach (object at in ats) {
                        if (at.GetType() == typeof(DbColumnAttribute)) {
                            DbColumnAttribute c = at as DbColumnAttribute;
                            if (c.FieldName == field) {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
