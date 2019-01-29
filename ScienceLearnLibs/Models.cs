using LearnLibs.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace LearnLibs.Models
{
    #region 客户模型
    /// <summary>
    /// 网站用户
    /// </summary>
    [DbTable("learn_Users")]
    public class User : BaseModel
    {
        [DbColumn(FN.AccountName, DbType.String, 32, IsAllowNull = false, Index = 1)]
        [DisplayColumn("用户名", 1, Scenes = DisplayScenes.运营端)]
        [UnqiueKey("accountName")]
        public string AccountName { get; set; }

        [DbColumn(FN.AccountPassword, DbType.String, 32, IsAllowNull = true, Index = 2)]
        public string AccountPassword { get; set; }

        [DbColumn(FN.Role, DbType.Int32, IsAllowNull = false, Index = 3, DefaultValue = (int)UserRole.未知)]
        [DisplayColumn("角色", 2, DisplayType = typeof(UserRole), Scenes = DisplayScenes.运营端)]
        public UserRole Role { get; set; }

        /// <summary>
        /// 昵称或别名
        /// </summary>
        [DbColumn(FN.Alias, DbType.String, 4, Index = 4, IsAllowNull = true)]
        [DisplayColumn("别名", 3)]
        public string Alias { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(User)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> { 
                    {FN.AccountName,AccountName},
                    {FN.AccountPassword,AccountPassword},
                    {FN.Role,(int)Role},
                    {FN.Alias,Alias}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            User u = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(User)))
            {
                u = base.BuildInstance<User>(r);
                u.AccountName = F.GetValue<string>(r, FN.AccountName, String.Empty);
                u.AccountPassword = F.GetValue<string>(r, FN.AccountPassword, String.Empty);
                u.Role = F.GetValue<UserRole>(r, FN.Role, UserRole.未知);
                u.Alias = F.GetValue<string>(r, FN.Alias, String.Empty);
            }
            return u;
        }
    }

    /// <summary>
    /// 学校
    /// </summary>
    [DbTable("learn_Schools")]
    public class School : BaseModel
    {
        /// <summary>
        /// 地区代码
        /// </summary>
        [DbColumn(FN.CityId, DbType.AnsiStringFixedLength, 6, IsAllowNull = false, Index = 1)]
        [ForeignKey(typeof(Area), FN.Id, FN.Name)]
        public Guid CityId { get; set; }

        /// <summary>
        /// 学校类型
        /// </summary>
        [DbColumn(FN.SchoolType, DbType.Int32, Index = 2, IsAllowNull = false, DefaultValue = (int)SchoolType.全日制学校)]
        [DisplayColumn("类别", 6, typeof(SchoolType))]
        [ForeignKey(typeof(SchoolType))]
        public SchoolType SchoolType { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        [DbColumn(FN.FullName, DbType.String, 16, Index = 3, IsAllowNull = false)]
        [DisplayColumn("全称", 1, Scenes = DisplayScenes.运营端)]
        [TreeNodeColumn(true)]
        public string FullName { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [DbColumn(FN.ShortName, DbType.String, 4, Index = 4, IsAllowNull = false)]
        [DisplayColumn("简称", 2, Scenes = DisplayScenes.运营端)]
        public string ShortName { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(School)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> {
                    {FN.CityId,CityId},
                    {FN.SchoolType,(int)SchoolType},
                    {FN.FullName,FullName},
                    {FN.ShortName,ShortName}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            School s = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(School)))
            {
                s = base.BuildInstance<School>(r);
                s.CityId = F.GetValue<Guid>(r, FN.CityId, Guid.Empty);
                s.SchoolType = F.GetValue<SchoolType>(r, FN.SchoolType, SchoolType.全日制学校);
                s.FullName = F.GetValue<string>(r, FN.FullName, String.Empty);
                s.ShortName = F.GetValue<string>(r, FN.ShortName, String.Empty);
            }
            return s;
        }
    }

    /// <summary>
    /// 学校班级
    /// </summary>
    [DbTable("learn_schoolClasses")]
    public class SchoolClass : BaseModel
    {
        /// <summary>
        /// 学校ID
        /// </summary>
        [DbColumn(FN.SchoolId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(School), FN.Id, FN.ShortName)]
        public Guid SchoolId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DbColumn(FN.IndexNo, DbType.Int32, Index = 2, IsAllowNull = false, DefaultValue = 1)]
        [DisplayColumn("序号", 1)]
        public int Index { get; set; }

        /// <summary>
        /// 班级别名
        /// </summary>
        [DbColumn(FN.Alias, DbType.String, 8, Index = 3, IsAllowNull = true)]
        [DisplayColumn("班级名", 2)]
        [TreeNodeColumn(true)]
        public string Alias { get; set; }

        /// <summary>
        /// 学校类型
        /// </summary>
        [DbColumn(FN.SchoolType, DbType.Int32, Index = 4, IsAllowNull = false, DefaultValue = (int)SchoolType.全日制学校)]
        [DisplayColumn("类别", 6, typeof(SchoolType))]
        [ForeignKey(typeof(SchoolType))]
        public SchoolType SchoolType { get; set; }

        /// <summary>
        /// 届次
        /// </summary>
        [DbColumn(FN.Period, DbType.Int32, Index = 5, IsAllowNull = false, DefaultValue = 2020)]
        [DisplayColumn("届次", 3)]
        public int Period { get; set; }

        /// <summary>
        /// 开始年级
        /// </summary>
        [DbColumn(FN.Admission, DbType.Int32, Index = 6, IsAllowNull = false, DefaultValue = (int)SchoolGrades.七年级)]
        [DisplayColumn("开始年级", 4, Scenes = DisplayScenes.运营端, DisplayType = typeof(SchoolGrades))]
        [ForeignKey(typeof(SchoolGrades))]
        public SchoolGrades Admission { get; set; }

        /// <summary>
        /// 毕业年级
        /// </summary>
        [DbColumn(FN.Graduation, DbType.Int32, Index = 7, IsAllowNull = false, DefaultValue = (int)SchoolGrades.九年级)]
        [DisplayColumn("毕业年级", 5, Scenes = DisplayScenes.运营端, DisplayType = typeof(SchoolGrades))]
        [ForeignKey(typeof(SchoolGrades))]
        public SchoolGrades Graduation { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(SchoolClass)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> { 
                    {FN.SchoolId,SchoolId},
                    {FN.SchoolType,(int)SchoolType},
                    {FN.Alias,Alias},
                    {FN.IndexNo,Index},
                    {FN.Period,Period},
                    {FN.Admission,(int)Admission},
                    {FN.Graduation,(int)Graduation}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            SchoolClass sc = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(SchoolClass)))
            {
                sc = base.BuildInstance<SchoolClass>(r);
                sc.SchoolId = F.GetValue<Guid>(r, FN.SchoolId, Guid.Empty);
                sc.Index = F.GetValue<int>(r, FN.IndexNo, 1);
                sc.SchoolType = F.GetValue<SchoolType>(r, FN.SchoolType, SchoolType.全日制学校);
                sc.Alias = F.GetValue<string>(r, FN.Alias, String.Empty);
                sc.Period = F.GetValue<int>(r, FN.Period, 2020);
                sc.Admission = F.GetValue<SchoolGrades>(r, FN.Admission, SchoolGrades.七年级);
                sc.Graduation = F.GetValue<SchoolGrades>(r, FN.Graduation, SchoolGrades.九年级);
            }
            return sc;
        }
    }

    /// <summary>
    /// 就学信息
    /// </summary>
    [DbTable("learn_Schoolings")]
    public class Schooling : BaseModel
    {
        /// <summary>
        /// 省ID
        /// </summary>
        [DbColumn(FN.ProvinceId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Area), FN.Id, FN.Name)]
        [DisplayColumn("省", 1, DisplayType = typeof(Area), DisplayField = FN.Name, ValueField = FN.Id)]
        public Guid ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// </summary>
        [DbColumn(FN.CityId, DbType.Guid, 16, Index = 2, IsAllowNull = false)]
        [ForeignKey(typeof(Area), FN.Id, FN.Name)]
        [DisplayColumn("市", 2, typeof(Area), FN.Name, ValueField = FN.Id)]
        public Guid CityId { get; set; }

        /// <summary>
        /// 区县ID
        /// </summary>
        [DbColumn(FN.DistrictId, DbType.Guid, 16, Index = 3, IsAllowNull = false)]
        [ForeignKey(typeof(Area), FN.Id, FN.Name)]
        [DisplayColumn("县", 3, typeof(Area), FN.Name, ValueField = FN.Id)]
        public Guid DistrictId { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        [DbColumn(FN.SchoolId, DbType.Guid, 16, Index = 4, IsAllowNull = false)]
        [ForeignKey(typeof(School), FN.Id, FN.ShortName)]
        [DisplayColumn("学校", 4, typeof(School), FN.ShortName, ValueField = FN.Id)]
        public Guid SchoolId { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        [DbColumn(FN.SchoolClassId, DbType.Guid, 16, Index = 5, IsAllowNull = false)]
        [ForeignKey(typeof(SchoolClass), FN.Id, FN.Alias)]
        [DisplayColumn("班级", 5, typeof(SchoolClass), FN.Alias)]
        public Guid SchoolClassId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DbColumn(FN.UserId, DbType.Guid, 16, Index = 6, IsAllowNull = false)]
        [ForeignKey(typeof(User), FN.Id, FN.Alias)]
        [DisplayColumn("用户", 6, typeof(User), FN.AccountName)]
        public Guid UserId { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DbColumn(FN.IndexNo, DbType.Int32, Index = 7, IsAllowNull = false, DefaultValue = 1)]
        [DisplayColumn("序号", 0)]
        public int Index { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        [DbColumn(FN.Alias, DbType.String, 4, Index = 8, IsAllowNull = true)]
        [DisplayColumn("别名", 7)]
        [TreeNodeColumn(true)]
        public string Alias { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Schooling)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> { 
                    {FN.ProvinceId,ProvinceId},
                    {FN.CityId,CityId},
                    {FN.DistrictId,DistrictId},
                    {FN.SchoolId,SchoolId},
                    {FN.SchoolClassId,SchoolClassId},
                    {FN.UserId,UserId},
                    {FN.IndexNo,Index},
                    {FN.Alias,Alias}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Schooling s = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Schooling)))
            {
                s = base.BuildInstance<Schooling>(r);
                s.ProvinceId = F.GetValue<Guid>(r, FN.ProvinceId, Guid.Empty);
                s.CityId = F.GetValue<Guid>(r, FN.CityId, Guid.Empty);
                s.DistrictId = F.GetValue<Guid>(r, FN.DistrictId, Guid.Empty);
                s.SchoolId = F.GetValue<Guid>(r, FN.SchoolId, Guid.Empty);
                s.SchoolClassId = F.GetValue<Guid>(r, FN.SchoolClassId, Guid.Empty);
                s.UserId = F.GetValue<Guid>(r, FN.UserId, Guid.Empty);
                s.Index = F.GetValue<int>(r, FN.IndexNo, 1);
                s.Alias = F.GetValue<string>(r, FN.Alias, String.Empty);
            }
            return s;
        }
    }

    /// <summary>
    /// 教师教学信息
    /// </summary>
    [DbTable("learn_Teachings")]
    public class Teaching : BaseModel
    {
        /// <summary>
        /// 工作学校或机构
        /// </summary>
        [DbColumn(FN.SchoolId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [DisplayColumn("工作单位", 1, typeof(School), FN.ShortName)]
        [ForeignKey(typeof(School), FN.Id)]
        public Guid SchoolId { get; set; }

        /// <summary>
        /// 对应用户
        /// </summary>
        [DbColumn(FN.UserId, DbType.Guid, 16, Index = 2, IsAllowNull = false)]
        [DisplayColumn("用户", 2, typeof(User), FN.AccountName)]
        [ForeignKey(typeof(User), FN.Id)]
        public Guid UserId { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        [DbColumn(FN.Alias, DbType.String, 8, Index = 3, IsAllowNull = true)]
        [DisplayColumn("别名", 3)]
        [TreeNodeColumn(true)]
        public string Alias { get; set; }


        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Teaching)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> { 
                    {FN.SchoolId,SchoolId},
                    {FN.UserId,UserId},
                    {FN.Alias,Alias}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Teaching t = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Teaching)))
            {
                t = base.BuildInstance<Teaching>(r);
                t.SchoolId = F.GetValue<Guid>(r, FN.SchoolId, Guid.Empty);
                t.UserId = F.GetValue<Guid>(r, FN.UserId, Guid.Empty);
                t.Alias = F.GetValue<string>(r, FN.Alias, String.Empty);
            }
            return t;
        }
    }

    /// <summary>
    /// 教师工作明细
    /// </summary>
    [DbTable("learn_TeachingDetails")]
    public class TeachingDetail : BaseModel
    {
        /// <summary>
        /// 教师
        /// </summary>
        [DbColumn(FN.TeachingId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Teaching), "Id")]
        [DisplayColumn("教师", 1, typeof(Teaching), "Alias")]
        public Guid TeachingId { get; set; }

        /// <summary>
        /// 班级
        /// </summary>
        [DbColumn(FN.SchoolClassId, DbType.Guid, 16, Index = 2, IsAllowNull = false)]
        [DisplayColumn("班级", 1, typeof(SchoolClass), "Alias")]
        [ForeignKey(typeof(SchoolClass), "Id")]
        public Guid SchoolClassId { get; set; }

        [DbColumn(FN.Alias, DbType.String, 4, Index = 3, IsAllowNull = true)]
        [DisplayColumn("别名", 2)]
        public string Alias { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(TeachingDetail)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> {
                    {FN.TeachingId,TeachingId},
                    {FN.SchoolClassId,SchoolClassId}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            TeachingDetail t = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(TeachingDetail)))
            {
                t = base.BuildInstance<TeachingDetail>(r);
                t.TeachingId = F.GetValue<Guid>(r, FN.TeachingId, Guid.Empty);
                t.SchoolClassId = F.GetValue<Guid>(r, FN.SchoolClassId, Guid.Empty);
            }
            return t;
        }
    }
    #endregion

    #region 基础资料模型
    /// <summary>
    /// 地区类型
    /// </summary>
    [DbTable("learn_Areas")]
    public class Area : BaseModel
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [DbColumn(FN.ParentId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Area), FN.Id)]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 行政区划代码
        /// </summary>
        [DbColumn(FN.Code, DbType.AnsiStringFixedLength, 6, Index = 2, IsAllowNull = false, DefaultValue = C.Zero6)]
        [DisplayColumn("代码", 1)]
        public string Code { get; set; }

        /// <summary>
        /// 地区名
        /// </summary>
        [DbColumn(FN.Name, DbType.String, 4, Index = 3, IsAllowNull = false)]
        [DisplayColumn("地区名", 2)]
        [TreeNodeColumn(true)]
        public string Name { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [DbColumn(FN.IsValid, DbType.Boolean, Index = 4, IsAllowNull = false, DefaultValue = true)]
        [DisplayColumn("有效", 4)]
        public bool IsValid { get; set; }

        [DbColumn(FN.xPath, DbType.AnsiString, 32, Index = 5, IsAllowNull = true)]
        public string xPath { get; set; }

        [DbColumn(FN.PressId, DbType.Guid, 16, Index = 6, IsAllowNull = true)]
        [DisplayColumn("出版社", 5, typeof(Press), FN.FullName)]
        [ForeignKey(typeof(Press), FN.Id)]
        public Guid PressId
        {
            get;
            set;
        }

        public Area CreateChild()
        {
            Area a = new Area();
            a.Id = Guid.NewGuid();
            a.ParentId = this.Id;
            a.IsValid = true;
            a.InServer = false;
            a.LastModify = DateTime.Now;
            a.PressId = this.PressId;
            a.Code = this.Code;
            a.xPath = this.xPath + this.Code;
            return a;
        }
        public override string TreeNodeTextValue()
        {
            return string.Format("[{0}]{1}", Code, Name);
        }
        public override string TreeNodeNameValue()
        {
            return Id.ToString("x2");
        }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Area)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> {
                    {FN.Code,Code},
                    {FN.ParentId,ParentId},
                    {FN.Name,Name},
                    {FN.IsValid,IsValid},
                    {FN.xPath,xPath}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Area t = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Area)))
            {
                t = base.BuildInstance<Area>(r);
                t.ParentId = F.GetValue<Guid>(r, FN.ParentId, Guid.Empty);
                t.Code = F.GetValue<string>(r, FN.Code, String.Empty);
                t.Name = F.GetValue<string>(r, FN.Name, String.Empty);
                t.IsValid = F.GetValue<bool>(r, FN.IsValid, true);
                t.xPath = F.GetValue<string>(r, FN.xPath, String.Empty);
                t.PressId = F.GetValue<Guid>(r, FN.PressId, Guid.Empty);
            }
            return t;
        }
    }

    /// <summary>
    /// 课程标准类型
    /// </summary>
    [DbTable("learn_Standards")]
    public class Standard : BaseModel
    {
        private Guid _parentId = Guid.Empty;

        [DbColumn(FN.ParentId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Standard), FN.Id)]
        public Guid ParentId { get { return _parentId; } set { _parentId = value; } }

        [DbColumn(FN.Code, DbType.AnsiString, 4, Index = 2, IsAllowNull = false)]
        public string Code { get; set; }

        [DbColumn(FN.Text, DbType.String, 32, Index = 3, IsAllowNull = false)]
        [DisplayColumn("课标", 1)]
        [TreeNodeColumn(true)]
        public string Text { get; set; }

        [DbColumn(FN.IsValid, DbType.Boolean, Index = 4, IsAllowNull = false, DefaultValue = true)]
        [DisplayColumn("有效?", 2)]
        public bool IsValid { get; set; }

        [DbColumn(FN.xPath, DbType.String, 64, Index = 5, IsAllowNull = true)]
        public string xPath { get; set; }

        public Standard CreateNewStandard()
        {
            Standard s = new Standard();
            s.Id = Guid.NewGuid();
            s.InServer = false;
            s.LastModify = DateTime.Now;
            return s;
        }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Standard)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> {
                    {FN.ParentId,ParentId},
                    {FN.Code,Code},
                    {FN.Text,Text},
                    {FN.IsValid,IsValid},
                    {FN.xPath,xPath}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Standard s = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Standard)))
            {
                s = base.BuildInstance<Standard>(r);
                s.ParentId = F.GetValue<Guid>(r, FN.LastModify, Guid.Empty);
                s.Code = F.GetValue<string>(r, FN.Code, String.Empty);
                s.Text = F.GetValue<string>(r, FN.Text, String.Empty);
                s.IsValid = F.GetValue<bool>(r, FN.IsValid, true);
                s.xPath = F.GetValue<string>(r, FN.xPath, String.Empty);
            }
            return s;
        }
    }

    /// <summary>
    /// 出版社
    /// </summary>
    [DbTable("learn_Presses")]
    public class Press : BaseModel
    {
        /// <summary>
        /// 出版社代码
        /// </summary>
        [DbColumn(FN.Code, DbType.AnsiStringFixedLength, 2, Index = 1, IsAllowNull = false, DefaultValue = "01")]
        [DisplayColumn("代码", 0, FillWeight = 20)]
        public string Code { get; set; }

        [DbColumn(FN.FullName, DbType.String, 16, Index = 2, IsAllowNull = false)]
        [DisplayColumn("全名", 1, FillWeight = 50)]
        [TreeNodeColumn(true)]
        public string FullName { get; set; }

        [DbColumn(FN.ShortName, DbType.String, 4, Index = 3, IsAllowNull = false)]
        [DisplayColumn("简称", 2, FillWeight = 30)]
        public string ShortName { get; set; }

        [DbColumn(FN.IsValid, DbType.Boolean, Index = 4, IsAllowNull = false, DefaultValue = true)]
        [DisplayColumn("有效?", 3, DisplayType = typeof(bool))]
        public bool IsValid { get; set; }
        public Press()
        {
            this.IsValid = true;
            this.LastModify = DateTime.Now;
        }
        public TeachBook CreateNewBook()
        {
            TeachBook tb = new TeachBook();
            tb.Id = Guid.NewGuid();
            tb.InServer = false;
            tb.LastModify = DateTime.Now;
            tb.PressId = this.Id;
            tb.SchoolGrade = SchoolGrades.七年级;
            tb.Semester = Semesters.第一学期;
            tb.ImplementDate = DateTime.Parse("2013-1-1");
            tb.IsValid = true;
            return tb;
        }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Press)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> {
                    {FN.Code,Code},
                    {FN.FullName,FullName},
                    {FN.ShortName,ShortName},
                    {FN.IsValid,IsValid}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Press p = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Press)))
            {
                p = base.BuildInstance<Press>(r);
                p.Code = F.GetValue<string>(r, FN.Code, string.Empty);
                p.FullName = F.GetValue<string>(r, FN.FullName, String.Empty);
                p.ShortName = F.GetValue<string>(r, FN.ShortName, String.Empty);
                p.IsValid = F.GetValue<bool>(r, FN.IsValid, true);
            }
            return p;
        }
    }

    /// <summary>
    /// 教材类型
    /// </summary>
    [DbTable("learn_TeachBooks")]
    public class TeachBook : BaseModel
    {
        /// <summary>
        /// 出版社Id
        /// </summary>
        [DbColumn(FN.PressId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Press), FN.Id, FN.ShortName)]
        public Guid PressId { get; set; }

        /// <summary>
        /// 全名
        /// </summary>
        [DbColumn(FN.FullName, DbType.String, 16, Index = 2, IsAllowNull = false)]
        [DisplayColumn("全名", 1)]
        [TreeNodeColumn(true)]
        public string FullName { get; set; }

        /// <summary>
        /// 简称
        /// </summary>
        [DbColumn(FN.ShortName, DbType.String, 6, Index = 3, IsAllowNull = false)]
        [DisplayColumn("简称", 2)]
        public string ShortName { get; set; }

        /// <summary>
        /// 适合的年级
        /// </summary>
        [DbColumn(FN.SchoolGrade, DbType.Int32, Index = 4, IsAllowNull = false, DefaultValue = (int)SchoolGrades.七年级)]
        [DisplayColumn("适应年级", 3, DisplayType = typeof(SchoolGrades))]
        [ForeignKey(typeof(SchoolGrades))]
        public SchoolGrades SchoolGrade { get; set; }

        /// <summary>
        /// 适应学期
        /// </summary>
        [DbColumn(FN.Semester, DbType.Int32, Index = 5, IsAllowNull = false, DefaultValue = (int)Semesters.第一学期)]
        [DisplayColumn("适应学期", 4, DisplayType = typeof(Semesters))]
        [ForeignKey(typeof(Semesters))]
        public Semesters Semester { get; set; }

        /// <summary>
        /// 实施日期
        /// </summary>
        [DbColumn(FN.ImplementDate, DbType.DateTime, Index = 6, IsAllowNull = false, DefaultValue = "2013-09-01 00:00:00")]
        [DisplayColumn("实施日期", 5, Format = "0:D")]
        public DateTime ImplementDate { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [DbColumn(FN.IsValid, DbType.Boolean, Index = 7, IsAllowNull = false, DefaultValue = true)]
        [DisplayColumn("实施中", 6, DisplayType = typeof(bool))]
        [ForeignKey(typeof(bool))]
        public bool IsValid { get; set; }

        public Category CreateNewCategory()
        {
            Category c = new Category();
            c.Id = Guid.NewGuid();
            c.PressId = this.PressId;
            c.TeachBookId = this.Id;
            c.LastModify = DateTime.Now;
            c.Index = 1;
            c.IsValid = this.IsValid;
            return c;
        }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(TeachBook)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> { 
                    {FN.PressId,PressId},
                    {FN.FullName,FullName},
                    {FN.ShortName,ShortName},
                    {FN.SchoolGrade,SchoolGrade},
                    {FN.Semester,Semester},
                    {FN.IsValid,IsValid},
                    {FN.ImplementDate,ImplementDate}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            TeachBook tb = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(TeachBook)))
            {
                tb = base.BuildInstance<TeachBook>(r);
                tb.PressId = F.GetValue<Guid>(r, FN.PressId, Guid.Empty);
                tb.FullName = F.GetValue<string>(r, FN.FullName, String.Empty);
                tb.ShortName = F.GetValue<string>(r, FN.ShortName, String.Empty);
                tb.SchoolGrade = F.GetValue<SchoolGrades>(r, FN.SchoolGrade, SchoolGrades.七年级);
                tb.Semester = F.GetValue<Semesters>(r, FN.Semester, Semesters.第一学期);
                tb.ImplementDate = F.GetValue<DateTime>(r, FN.ImplementDate, DateTime.MinValue);
                tb.IsValid = F.GetValue<bool>(r, FN.IsValid, true);
            }
            return tb;
        }
    }

    /// <summary>
    /// 教材中的单元类
    /// </summary>
    [DbTable("learn_Categorys")]
    public class Category : BaseModel
    {
        private Guid _parentId = Guid.Empty;
        /// <summary>
        /// 出版社ID
        /// </summary>
        [DbColumn(FN.PressId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Press), FN.Id)]
        public Guid PressId { get; set; }

        /// <summary>
        /// 教材ID
        /// </summary>
        [DbColumn(FN.TeachBookId, DbType.Guid, 16, Index = 2, IsAllowNull = false)]
        [ForeignKey(typeof(TeachBook), FN.Id)]
        public Guid TeachBookId { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [DbColumn(FN.ParentId, DbType.Guid, 16, Index = 3, IsAllowNull = false)]
        [ForeignKey(typeof(Category), FN.Id)]
        public Guid ParentId { get { return _parentId; } set { _parentId = value; } }

        /// <summary>
        /// 序号
        /// </summary>
        [DbColumn(FN.IndexNo, DbType.Int16, Index = 4, IsAllowNull = false, DefaultValue = 1)]
        [DisplayColumn("序号", 1)]
        [OrderBy(SortOrder.Ascending)]
        public int Index { get; set; }

        /// <summary>
        /// 单元标题
        /// </summary>
        [DbColumn(FN.Text, DbType.String, 16, Index = 5, IsAllowNull = false)]
        [DisplayColumn("单元", 2)]
        [TreeNodeColumn(true)]
        public string Text { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [DbColumn(FN.IsValid, DbType.Boolean, Index = 6, IsAllowNull = false)]
        [DisplayColumn("有效", 3, DisplayType = typeof(bool))]
        public bool IsValid { get; set; }

        public Category CreateNewCategory()
        {
            Category c = new Category();
            c.Id = Guid.NewGuid();
            c.PressId = this.PressId;
            c.TeachBookId = this.TeachBookId;
            c.ParentId = this.Id;
            c.LastModify = DateTime.Now;
            c.Index = 1;
            c.IsValid = true;
            return c;
        }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Category)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> { 
                    {FN.PressId,PressId},
                    {FN.TeachBookId,TeachBookId},
                    {FN.ParentId,ParentId},
                    {FN.IndexNo,Index},
                    {FN.Text,Text},
                    {FN.IsValid,IsValid}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Category c = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Category)))
            {
                c = base.BuildInstance<Category>(r);
                c.PressId = F.GetValue<Guid>(r, FN.PressId, Guid.Empty);
                c.TeachBookId = F.GetValue<Guid>(r, FN.TeachBookId, Guid.Empty);
                c.ParentId = F.GetValue<Guid>(r, FN.ParentId, Guid.Empty);
                c.Index = F.GetValue<int>(r, FN.IndexNo, 1);
                c.Text = F.GetValue<string>(r, FN.Text, String.Empty);
                c.IsValid = F.GetValue<bool>(r, FN.IsValid, true);
            }
            return c;
        }
    }

    /// <summary>
    /// 专题类型
    /// </summary>
    [DbTable("learn_Topics")]
    public class Topic : BaseModel
    {
        /// <summary>
        /// 父ID
        /// </summary>
        [DbColumn("parentId", DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Topic), "Id")]
        public Guid ParentId { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [DbColumn("Code", DbType.AnsiString, 4, Index = 2, IsAllowNull = false)]
        [DisplayColumn("编号", 1)]
        public string Code { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        [DbColumn("IndexNo", DbType.Int32, Index = 3, IsAllowNull = false, DefaultValue = 1)]
        [DisplayColumn("序号", 0)]
        [OrderBy(SortOrder.Ascending)]
        public int Index { get; set; }

        /// <summary>
        /// 专题名
        /// </summary>
        [DbColumn("text", DbType.String, 16, Index = 4, IsAllowNull = false)]
        [DisplayColumn("专题", 2)]
        [TreeNodeColumn(true)]
        public string Text { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [DbColumn("IsValid", DbType.Boolean, Index = 5, IsAllowNull = false, DefaultValue = true)]
        [DisplayColumn("是否有效", 3)]
        public bool IsValid { get; set; }

        /// <summary>
        /// xPath
        /// </summary>
        [DbColumn("xPath", DbType.AnsiString, 64)]
        public string xPath { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Topic)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r, new Dictionary<string, object> { 
                    {FN.ParentId,ParentId},
                    {FN.Code,Code},
                    {FN.IndexNo,Index},
                    {FN.Text,Text},
                    {FN.IsValid,IsValid},
                    {FN.xPath,xPath}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Topic t = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Topic)))
            {
                t = base.BuildInstance<Topic>(r);
                t.ParentId = F.GetValue<Guid>(r, FN.ParentId, Guid.Empty);
                t.Code = F.GetValue<string>(r, FN.Code, String.Empty);
                t.Index = F.GetValue<int>(r, FN.IndexNo, 1);
                t.Text = F.GetValue<string>(r, FN.Text, String.Empty);
                t.IsValid = F.GetValue<bool>(r, FN.IsValid, true);
                t.xPath = F.GetValue<string>(r, FN.xPath, String.Empty);
            }
            return t;
        }
    }
    #endregion

    #region 业务模型
    /// <summary>
    /// 试题类型
    /// </summary>
    [DbTable("learn_Subjects")]
    public class Subject : BaseModel
    {
        /// <summary>
        /// 所有者
        /// </summary>
        [DbColumn(FN.UserId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [DisplayColumn("提供者", 0, typeof(User), "Alias")]
        [ForeignKey(typeof(User), FN.Id, FN.Alias)]
        public Guid UserId { get; set; }

        /// <summary>
        /// 试题标题
        /// </summary>
        [DbColumn(FN.Title, DbType.String, 32, Index = 2, IsAllowNull = false)]
        [DisplayColumn("标题", 1)]
        public string Title { get; set; }

        /// <summary>
        /// 内容地址
        /// </summary>
        [DbColumn(FN.ContentUrl, DbType.String, 128, Index = 3, IsAllowNull = false)]
        public string ContentUrl { get; set; }

        /// <summary>
        /// 解析地址
        /// </summary>
        [DbColumn(FN.AnalysisUrl, DbType.String, 128, Index = 4, IsAllowNull = false)]
        public string AnalysisUrl { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [DbColumn(FN.CreatedTime, DbType.DateTime, Index = 5, IsAllowNull = false)]
        [DisplayColumn("加入时间", 3, "0:F")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 是否草稿
        /// </summary>
        [DbColumn(FN.EditState, DbType.Int16, Index = 6, IsAllowNull = false, DefaultValue = (Int16)EditState.草稿)]
        [DisplayColumn("状态", 5, typeof(EditState))]
        [ForeignKey(typeof(EditState))]
        public EditState EditState { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        [DbColumn(FN.ReleaseState, DbType.Int16, Index = 7, IsAllowNull = false, DefaultValue = (Int16)ReleaseState.未发布)]
        [DisplayColumn("发布", 6, typeof(ReleaseState))]
        [ForeignKey(typeof(ReleaseState))]
        public ReleaseState ReleaseState { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DbColumn(FN.Price, DbType.Int16, Index = 8, IsAllowNull = false, DefaultValue = 0)]
        [DisplayColumn("价格", 7)]
        public int Price { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null)
            {
                base.SetValues(ref r);
                F.SetValues(ref r,
                    new Dictionary<string, object> { 
                        {FN.UserId,UserId},
                        {FN.Title,Title},
                        {FN.ContentUrl,ContentUrl},
                        {FN.AnalysisUrl,AnalysisUrl},
                        {FN.CreatedTime,CreatedTime},
                        {FN.EditState,(int)EditState},
                        {FN.ReleaseState,(int)ReleaseState},
                        {FN.Price,Price}
                    });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Subject s = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Subject)))
            {
                s = base.BuildInstance<Subject>(r);
                s.UserId = F.GetValue<Guid>(r, FN.UserId, Guid.Empty);
                s.Title = F.GetValue<string>(r, FN.Title, String.Empty);
                s.ContentUrl = F.GetValue<string>(r, FN.ContentUrl, String.Empty);
                s.AnalysisUrl = F.GetValue<string>(r, FN.AnalysisUrl, String.Empty);
                s.CreatedTime = F.GetValue<DateTime>(r, FN.CreatedTime, DateTime.Now);
                s.EditState = F.GetValue<EditState>(r, FN.EditState, EditState.草稿);
                s.ReleaseState = F.GetValue<ReleaseState>(r, FN.ReleaseState, ReleaseState.未发布);
                s.Price = F.GetValue<int>(r, FN.Price, 0);
            }
            return s;
        }
    }

    /// <summary>
    /// 试题设问
    /// </summary>
    [DbTable("learn_Questions")]
    public class Question : BaseModel
    {
        /// <summary>
        /// 试题ID
        /// </summary>
        [DbColumn(FN.SubjectId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [DisplayColumn("试题", 1, typeof(Subject), "Title")]
        [ForeignKey(typeof(Subject), "Id")]
        public Guid SubjectId { get; set; }

        /// <summary>
        /// 回答模式
        /// </summary>
        [DbColumn(FN.AnswerMode, DbType.Int32, Index = 2, IsAllowNull = false, DefaultValue = (int)AnswerMode.选择)]
        [DisplayColumn("题型", 2, typeof(AnswerMode))]
        [ForeignKey(typeof(AnswerMode))]
        public AnswerMode AnswerMode { get; set; }

        /// <summary>
        /// 选项字符
        /// </summary>
        [DbColumn(FN.OptionChars, DbType.String, 256, Index = 3, IsAllowNull = false)]
        [DisplayColumn("选项字符串", 3)]
        public string OptionChars { get; set; }

        /// <summary>
        /// 参考答案
        /// </summary>
        [DbColumn(FN.KeyChars, DbType.String, 256, Index = 4, IsAllowNull = false)]
        [DisplayColumn("参考答案", 4)]
        public string KeyChars { get; set; }

        /// <summary>
        /// 难度系数
        /// </summary>
        [DbColumn(FN.Difficult, DbType.Decimal, Size = 4, Precision = 2, Index = 5, IsAllowNull = false, DefaultValue = 0.7f)]
        [DisplayColumn("难度系数", 5)]
        public float Difficult { get; set; }

        /// <summary>
        /// 分值
        /// </summary>
        [DbColumn(FN.Score, DbType.Decimal, Size = 9, Precision = 1, Index = 6, IsAllowNull = false, DefaultValue = 1f)]
        [DisplayColumn("分值", 6)]
        public float Score { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Question)))
            {
                base.SetValues(ref r);
                F.SetValues(ref r,
                    new Dictionary<string, object> { 
                    {FN.SubjectId,SubjectId},
                    {FN.AnswerMode,AnswerMode},
                    {FN.OptionChars,OptionChars},
                    {FN.KeyChars,KeyChars},
                    {FN.Difficult,Difficult},
                    {FN.Score,Score}
                });
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            Question q = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(Question)))
            {
                q = base.BuildInstance<Question>(r);
                q.SubjectId = F.GetValue<Guid>(r, FN.SubjectId, Guid.Empty);
                q.AnswerMode = F.GetValue<AnswerMode>(r, FN.AnswerMode, AnswerMode.选择);
                q.OptionChars = F.GetValue<string>(r, FN.OptionChars, String.Empty);
                q.KeyChars = F.GetValue<string>(r, FN.KeyChars, String.Empty);
                q.Difficult = F.GetValue<float>(r, FN.Difficult, 0.7f);
                q.Score = F.GetValue<float>(r, FN.Score, 1f);
            }
            return q;
        }
    }

    /// <summary>
    /// 问题涉及的课标
    /// </summary>
    [DbTable("learn_QuestionStandards")]
    public class QuestionStandard : BaseModel
    {
        /// <summary>
        /// 问题ID
        /// </summary>
        [DbColumn(FN.QuestionId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Question), FN.Id)]
        public Guid QuestionId { get; set; }

        /// <summary>
        /// 课标ID
        /// </summary>
        [DbColumn(FN.StandardId, DbType.Guid, 16, Index = 2, IsAllowNull = false)]
        [ForeignKey(typeof(Standard), FN.Id, FN.Text)]
        [DisplayColumn("课标", 1, typeof(Standard), FN.Text)]
        public Guid StandardId { get; set; }

        public override void SetDataRow(ref DataRow r)
        {
            if (r != null)
            {
                base.SetValues(ref r);
                F.SetValue(ref r, FN.QuestionId, QuestionId);
                F.SetValue(ref r, FN.StandardId, StandardId);
            }
        }

        public override BaseModel ToObject(DataRow r)
        {
            QuestionStandard qs = null;
            if (r != null && r.Table != null && r.Table.TableName == F.GetTableName(typeof(QuestionStandard)))
            {
                qs = base.BuildInstance<QuestionStandard>(r);
                qs.QuestionId = F.GetValue<Guid>(r, FN.QuestionId, Guid.Empty);
                qs.StandardId = F.GetValue<Guid>(r, FN.StandardId, Guid.Empty);
            }
            return qs;
        }
    }
    #endregion
}
