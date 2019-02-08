using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace LearnLibs.Enums
{
    public enum JoinType { 
        Left,
        Inner,
        Right
    }
    public enum WhereTarget { 
        SQLite,
        SQLClient,
        DataTable
    }
    /// <summary>
    /// 分支学科
    /// </summary>
    [Flags]
    public enum Branch
    {
        未知 = 0,
        科学 = 1,
        物理 = 2,
        化学 = 4,
        生物 = 8,
        地理 = 16,
        通用 = 31
    }

    /// <summary>
    /// 课程目标等级
    /// </summary>
    [Flags]
    public enum CourseTargetLevel
    {
        [Description("知道、了解、认识、记忆等最低目标要求")]
        知道 = 1,
        [Description("能不假思索的说出相关内容")]
        熟知 = 2,
        [Description("知其然知其所以然，不仅要熟知内容，还要熟知相关内容的来龙去脉")]
        理解 = 4,
        [Description("能举一反三，能将所学知识用于解释日常现象")]
        应用 = 8,
        [Description("建立与自然和谐相处的态度")]
        情感A = 16,
        [Description("对自然充满好奇，有探求真相的欲望")]
        情感B = 32,
        [Description("常常关注科技现状和前沿动态，能以科学解释现象而不是人云亦云不辨真伪")]
        情感C = 64,
        [Description("在探究创新方面不仅有意识、有能力，而且能有实际行动")]
        情感D = 128,
        [Description("知道科学探究等方法是获取科学知识的基本方式")]
        技能1 = 256,
        [Description("了解科学探究的基本步骤")]
        技能2 = 512,
        [Description("能应用科学探究的基本步骤展开预定的探索活动")]
        技能3 = 1024,
        [Description("能灵活应用各种探究方法解决实际遇到的问题")]
        技能4 = 2048
    }

    /// <summary>
    /// 表格列显示场景
    /// </summary>
    [Flags]
    public enum DisplayScenes
    {
        未设置 = 0,
        /// <summary>
        /// 内部管理端
        /// </summary>
        运营端 = 0x1,
        /// <summary>
        /// 用户端
        /// </summary>
        客户端 = 0x2
    }

    /// <summary>
    /// 学校年级
    /// </summary>
    [Flags]
    public enum SchoolGrades
    {
        未设置 = 0,
        一年级 = 1,
        二年级 = 2,
        三年级 = 4,
        四年级 = 8,
        五年级 = 16,
        六年级 = 32,
        七年级 = 64,
        八年级 = 128,
        九年级 = 256,
        高一年级 = 512,
        高二年级 = 1024,
        高三年级 = 2048
    }

    /// <summary>
    /// 学期枚举
    /// </summary>
    [Flags]
    public enum Semesters
    {
        未设置 = 0,
        第一学期 = 1,
        第二学期 = 2,
        全学年 = 3
    }

    /// <summary>
    /// 回答模式
    /// </summary>
    [Flags]
    public enum AnswerMode
    {
        未知 = 0,
        选择 = 1,
        判断 = 2,
        配对 = 4,
        计算 = 8,
        填空 = 16,
        简答 = 32
    }

    /// <summary>
    /// 登录角色
    /// </summary>
    [Flags]
    public enum UserRole
    {
        未知 = 0,
        学生 = 1,
        教师 = 2
    }

    /// <summary>
    /// 学校类型
    /// </summary>
    public enum SchoolType
    {
        全日制学校,
        教学机构
    }

    /// <summary>
    /// 编辑状态
    /// </summary>
    public enum EditState
    {
        草稿 = 0,
        正稿 = 1
    }

    /// <summary>
    /// 发布状态
    /// </summary>
    public enum ReleaseState
    {
        已发布 = 0,
        未发布 = 1
    }

    /// <summary>
    /// 作业状态
    /// </summary>
    [Flags]
    public enum HomeworkState
    {
        教师已布置 = 0x0001,
        学生已收取 = 0x0002,
        学生已提交 = 0x0004,
        教师已收取 = 0x0008,
        教师已批结 = 0x0010,
        超时 = 0x8000
    }

    /// <summary>
    /// 在表格、树形控件中显示的角色
    /// </summary>
    [Flags]
    public enum DisplayTarget
    {
        DataGridColumn = 1,
        TreeNodeText = 2,
        TreeNodeName = 4
    }
}
