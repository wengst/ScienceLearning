using LearnLibs.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using LearnLibs.Controls;

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
        [DisplayColumn("角色", 2, FromType = typeof(UserRole), Scenes = DisplayScenes.运营端)]
        public UserRole Role { get; set; }

        /// <summary>
        /// 昵称或别名
        /// </summary>
        [DbColumn(FN.Alias, DbType.String, 4, Index = 4, IsAllowNull = true)]
        [DisplayColumn("别名", 3)]
        public string Alias { get; set; }
    }

    /// <summary>
    /// 学校
    /// </summary>
    [DbTable("learn_Schools")]
    [ListItem(FN.Id, FN.ShortName)]
    [ModelEditor(typeof(Controls.FormDialog))]
    public class School : BaseModel
    {
        /// <summary>
        /// 地区代码
        /// </summary>
        [DbColumn(FN.CityId, DbType.AnsiStringFixedLength, 6, IsAllowNull = false, Index = 1)]
        [ForeignKey(typeof(Area), FN.Id)]
        public Guid CityId { get; set; }

        /// <summary>
        /// 学校类型
        /// </summary>
        [DbColumn(FN.SchoolType, DbType.Int32, Index = 2, IsAllowNull = false, DefaultValue = (int)SchoolType.全日制学校)]
        [DisplayColumn("类别", 6, typeof(SchoolType))]
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
    }

    /// <summary>
    /// 学校班级
    /// </summary>
    [DbTable("learn_schoolClasses")]
    [ListItem(FN.Id, FN.Alias)]
    public class SchoolClass : BaseModel
    {
        /// <summary>
        /// 学校ID
        /// </summary>
        [DbColumn(FN.SchoolId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(School), FN.Id)]
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
        [DisplayColumn("开始年级", 4, Scenes = DisplayScenes.运营端, FromType = typeof(SchoolGrades))]
        public SchoolGrades Admission { get; set; }

        /// <summary>
        /// 毕业年级
        /// </summary>
        [DbColumn(FN.Graduation, DbType.Int32, Index = 7, IsAllowNull = false, DefaultValue = (int)SchoolGrades.九年级)]
        [DisplayColumn("毕业年级", 5, Scenes = DisplayScenes.运营端, FromType = typeof(SchoolGrades))]
        public SchoolGrades Graduation { get; set; }
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
        [ForeignKey(typeof(Area), FN.Id)]
        [DisplayColumn("省", 1, typeof(Area), FN.Name)]
        public Guid ProvinceId { get; set; }

        /// <summary>
        /// 市ID
        /// </summary>
        [DbColumn(FN.CityId, DbType.Guid, 16, Index = 2, IsAllowNull = false)]
        [ForeignKey(typeof(Area), FN.Id)]
        [DisplayColumn("市", 2, typeof(Area), FN.Name)]
        public Guid CityId { get; set; }

        /// <summary>
        /// 区县ID
        /// </summary>
        [DbColumn(FN.DistrictId, DbType.Guid, 16, Index = 3, IsAllowNull = false)]
        [ForeignKey(typeof(Area), FN.Id)]
        [DisplayColumn("县", 3, typeof(Area), FN.Name)]
        public Guid DistrictId { get; set; }

        /// <summary>
        /// 学校ID
        /// </summary>
        [DbColumn(FN.SchoolId, DbType.Guid, 16, Index = 4, IsAllowNull = false)]
        [ForeignKey(typeof(School), FN.Id)]
        [DisplayColumn("学校", 4, typeof(School), FN.ShortName)]
        public Guid SchoolId { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        [DbColumn(FN.SchoolClassId, DbType.Guid, 16, Index = 5, IsAllowNull = false)]
        [ForeignKey(typeof(SchoolClass), FN.Id)]
        [DisplayColumn("班级", 5, typeof(SchoolClass), FN.Alias)]
        public Guid SchoolClassId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [DbColumn(FN.UserId, DbType.Guid, 16, Index = 6, IsAllowNull = false)]
        [ForeignKey(typeof(User), FN.Id)]
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
    }
    #endregion

    #region 基础资料模型
    /// <summary>
    /// 地区类型
    /// </summary>
    [DbTable("learn_Areas")]
    [ListItem(FN.Id, FN.Name)]
    [ModelEditor(typeof(frmArea))]
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

        public Area()
        {
            this.Id = Guid.Empty;
            this.ParentId = Guid.Empty;
        }
        public Area CreateChild()
        {
            Area a = new Area();
            a.Id = Guid.Empty;
            a.ParentId = this.Id;
            a.IsValid = this.IsValid;
            a.InServer = false;
            a.LastModify = DateTime.Now;
            a.PressId = this.PressId;
            a.Code = this.Code;
            a.xPath = this.xPath + this.Code;
            return a;
        }
    }

    /// <summary>
    /// 课程标准类型
    /// </summary>
    [DbTable("learn_Standards")]
    [ListItem(FN.Id, FN.Text)]
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

        public Standard CreateChild()
        {
            Standard s = new Standard();
            s.Id = Guid.Empty;
            s.InServer = false;
            s.LastModify = DateTime.Now;
            s.ParentId = this.Id;
            s.IsValid = this.IsValid;
            s.xPath = this.xPath + "_" + this.Code;
            return s;
        }
    }

    /// <summary>
    /// 出版社
    /// </summary>
    [DbTable("learn_Presses")]
    [ListItem(FN.Id, FN.FullName)]
    [ModelEditor(typeof(frmPress))]
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
        [DisplayColumn("有效?", 3, FromType = typeof(bool))]
        public bool IsValid { get; set; }

        public Press()
        {
            this.IsValid = true;
            this.LastModify = DateTime.Now;
        }
        public TeachBook CreateNewBook()
        {
            TeachBook tb = new TeachBook();
            
            tb.PressId = this.Id;
            tb.SchoolGrade = SchoolGrades.七年级;
            tb.Semester = Semesters.第一学期;
            tb.ImplementDate = DateTime.Parse("2013-1-1");
            tb.IsValid = true;
            return tb;
        }
    }

    /// <summary>
    /// 教材类型
    /// </summary>
    [DbTable("learn_TeachBooks")]
    [ListItem(FN.Id, FN.ShortName)]
    [ModelEditor(typeof(frmTeachBook))]
    public class TeachBook : BaseModel
    {
        /// <summary>
        /// 出版社Id
        /// </summary>
        [DbColumn(FN.PressId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [ForeignKey(typeof(Press), FN.Id)]
        [DisplayColumn("出版社", 7, typeof(Press), BaseModel.FN.ShortName, FillWeight = 30)]
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
        [DisplayColumn("适应年级", 3, FromType = typeof(SchoolGrades))]
        public SchoolGrades SchoolGrade { get; set; }

        /// <summary>
        /// 适应学期
        /// </summary>
        [DbColumn(FN.Semester, DbType.Int32, Index = 5, IsAllowNull = false, DefaultValue = (int)Semesters.第一学期)]
        [DisplayColumn("适应学期", 4, FromType = typeof(Semesters))]
        public Semesters Semester { get; set; }

        /// <summary>
        /// 实施日期
        /// </summary>
        [DbColumn(FN.ImplementDate, DbType.DateTime, Index = 6, IsAllowNull = false, DefaultValue = "2013-09-01 00:00:00")]
        [DisplayColumn("实施日期", 5, Format = "{0:yyyy-MM-dd}")]
        public DateTime ImplementDate { get; set; }

        public TeachBook()
        {
            this.ImplementDate = DateTime.Now;
            FullName = string.Empty;
            ShortName = string.Empty;
            SchoolGrade = SchoolGrades.七年级;
            Semester = Semesters.第一学期;
        }
        /// <summary>
        /// 是否有效
        /// </summary>
        [DbColumn(FN.IsValid, DbType.Boolean, Index = 7, IsAllowNull = false, DefaultValue = true)]
        [DisplayColumn("实施中", 6, FromType = typeof(bool))]
        public bool IsValid { get; set; }

        public Category CreateNewCategory()
        {
            Category c = new Category();
            c.PressId = this.PressId;
            c.TeachBookId = this.Id;
            c.Index = 1;
            c.IsValid = this.IsValid;
            return c;
        }
    }

    /// <summary>
    /// 教材中的单元类
    /// </summary>
    [DbTable("learn_Categorys")]
    [ListItem(FN.Id, FN.Text)]
    [ModelEditor(typeof(frmCategory))]
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
        [DisplayColumn("有效", 3, FromType = typeof(bool))]
        public bool IsValid { get; set; }

        public Category CreateNewCategory()
        {
            Category c = new Category();
            c.PressId = this.PressId;
            c.TeachBookId = this.TeachBookId;
            c.ParentId = this.Id;
            c.Index = 1;
            c.IsValid = this.IsValid;
            return c;
        }
    }

    /// <summary>
    /// 专题类型
    /// </summary>
    [DbTable("learn_Topics")]
    [ListItem(FN.Id, FN.Text)]
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
    }
    #endregion

    #region 业务模型
    /// <summary>
    /// 试题类型
    /// </summary>
    [DbTable("learn_Subjects")]
    [ListItem(FN.Id, FN.Title)]
    public class Subject : BaseModel
    {
        /// <summary>
        /// 所有者
        /// </summary>
        [DbColumn(FN.UserId, DbType.Guid, 16, Index = 1, IsAllowNull = false)]
        [DisplayColumn("提供者", 0, typeof(User), "Alias")]
        [ForeignKey(typeof(User), FN.Id)]
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
        public EditState EditState { get; set; }

        /// <summary>
        /// 发布状态
        /// </summary>
        [DbColumn(FN.ReleaseState, DbType.Int16, Index = 7, IsAllowNull = false, DefaultValue = (Int16)ReleaseState.未发布)]
        [DisplayColumn("发布", 6, typeof(ReleaseState))]
        public ReleaseState ReleaseState { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [DbColumn(FN.Price, DbType.Int16, Index = 8, IsAllowNull = false, DefaultValue = 0)]
        [DisplayColumn("价格", 7)]
        public int Price { get; set; }
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
        [ForeignKey(typeof(Standard), FN.Id)]
        [DisplayColumn("课标", 1, typeof(Standard), FN.Text)]
        public Guid StandardId { get; set; }
    }
    #endregion
}
