using System;
using System.Data;

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

        #region private fields
        private Guid _id = Guid.Empty;
        private bool _inServer = false;
        private DateTime _lastModify = DateTime.Now; 
        #endregion

        #region public properties
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
        public bool InServer { get { return _inServer; } set { _inServer = value; } }

        [DbColumn(FN.LastModify, DbType.DateTime, IsAllowNull = false, Index = -1)]
        public DateTime LastModify { get { return _lastModify; } set { _lastModify = value; } } 
        #endregion

        #region static methods
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
        #endregion
    }
}
