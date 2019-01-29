using System;
using System.Reflection;

namespace LearnLibs
{
    /// <summary>
    /// 类型未设置DbTable特性异常
    /// </summary>
    public class NotDbTableInModelException : Exception
    {
        public NotDbTableInModelException() : base("类型未设置DbTable特性") { }
        public NotDbTableInModelException(Type type) : base(string.Format("类型[{0}]未设置DbTable特性", type == null ? "" : type.Name)) { }
        public NotDbTableInModelException(string message) : base(message) { }
    }
    /// <summary>
    /// 类型不是BaseModel的子类异常
    /// </summary>
    public class NotInheritBaseModelException : Exception
    {
        public NotInheritBaseModelException() : base("类型不是继承自BaseModel的子类") { }
        public NotInheritBaseModelException(string message) : base(message) { }
        public NotInheritBaseModelException(Type type) : base("类型[" + type.Name + "]不是BaseModel的子类") { }
    }

    /// <summary>
    /// 类型不包含指定属性异常
    /// </summary>
    public class NotIncludedPropertyException : Exception
    {
        public NotIncludedPropertyException() : base("类型不含指定属性") { }
        public NotIncludedPropertyException(string message) : base(message) { }
        public NotIncludedPropertyException(Type type) : base(string.Format("类型[{0}]不含指定属性", type == null ? "" : type.Name)) { }
    }

    /// <summary>
    /// 属性不包含DbColumn特性异常
    /// </summary>
    public class NotDbColumnAsPropertyException : Exception
    {
        public NotDbColumnAsPropertyException() : base("属性不包含DbColumn特性") { }
        public NotDbColumnAsPropertyException(string message) : base(message) { }
        public NotDbColumnAsPropertyException(PropertyInfo p) : base(string.Format("属性[{0}]不包含DbColumn特性", p.Name)) { }
    }

    /// <summary>
    /// 模型不包含DbColumn特性异常
    /// </summary>
    public class NotDbColumnAsModelException : Exception
    {
        public NotDbColumnAsModelException() : base("模型的所有公共属性都未设置DbColumn特性") { }
        public NotDbColumnAsModelException(string message) : base(message) { }
        public NotDbColumnAsModelException(Type model) : base(string.Format("模型[{0}]的所有属性都不包含DbColumn特性", model.Name)) { }
    }

    /// <summary>
    /// 主键太多异常
    /// </summary>
    public class PrimaryKeyTooMuchException : Exception
    {
        public PrimaryKeyTooMuchException() : base("类型中只能有一个主键，如果主键涉及多列，每列PrimaryKey特性的Name必须相同") { }
        public PrimaryKeyTooMuchException(string message) : base(message) { }
        public PrimaryKeyTooMuchException(Type type) : base(string.Format("类型[{0}]中只能有一个主键，如果主键涉及多列，每列PrimaryKey特性的Name必须相同", type == null ? "" : type.Name)) { }
    }

    /// <summary>
    /// 类型不包含某项特性异常
    /// </summary>
    public class NotContainAttributeException : Exception
    {
        public NotContainAttributeException() : base("属性未设置指定特性") { }
        public NotContainAttributeException(Type attributeType) : base(string.Format("属性没有设置[{0}]特性", attributeType.Name)) { }
    }

    /// <summary>
    /// 类型不含DisplayColumn特性异常
    /// </summary>
    public class NotDisplayColumnInModelException : Exception
    {
        public NotDisplayColumnInModelException() : base("类型中的全部属性未定义DisplayColumn特性") { }
        public NotDisplayColumnInModelException(Type type) : base(string.Format("类型[{0}]中的全部属性未定义DisplayColumn特性", type == null ? "" : type.Name)) { }
        public NotDisplayColumnInModelException(string message) : base(message) { }
    }

    /// <summary>
    /// 类型中没有配置外键特性
    /// </summary>
    public class NotForeignKeyInModelException : Exception
    {
        public NotForeignKeyInModelException() : base("类型中的属性没有配置外键特性") { }
        public NotForeignKeyInModelException(string message) : base(message) { }
        public NotForeignKeyInModelException(Type type) : base(string.Format("类型[{0}]中的属性没有配置外键特性", type == null ? "" : type.Name)) { }
    }
}
