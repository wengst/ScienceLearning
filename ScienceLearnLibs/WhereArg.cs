using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LearnLibs
{
    public class WhereArg
    {
        public string Field { get; set; }
        public object Value { get; set; }

        public string ToWhereString()
        {
            if (!string.IsNullOrWhiteSpace(Field) && Value != null)
            {
                Type t = Value.GetType();
                if (t == typeof(Guid))
                {
                    return "UPPER(HEX(" + Field + "))='" + F.byteToHexStr(((Guid)Value).ToByteArray()) + "'";
                }
                else if (t == typeof(string))
                {
                    return Field + "='" + Value.ToString() + "'";
                }
                else if (t.IsEnum)
                {
                    return Field + "=" + ((int)Value).ToString();
                }
                else
                {
                    return Field + "=" + Value.ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public string ToRowFilterString()
        {
            if (!string.IsNullOrWhiteSpace(Field) && Value != null)
            {
                Type t = Value.GetType();
                if (t == typeof(Guid))
                {
                    return "Convert(" + Field + ",'System.String'='" + ((Guid)Value).ToString() + "'";
                }
                else if (t.IsEnum)
                {
                    return Field + "=" + ((int)Value).ToString();
                }
                else if (t == typeof(string))
                {
                    return Field + "='" + Value.ToString() + "'";
                }
                else
                {
                    return Field + "=" + Value.ToString();
                }
            }
            else
            {
                return string.Empty;
            }
        }

        public static WhereArg BuildFromDataRow(DataRow row, string field)
        {
            WhereArg arg = null;
            if (row != null && !string.IsNullOrWhiteSpace(field))
            {
                arg = new WhereArg();
                arg.Field = field;
                if (row.Table.Columns.Contains(field))
                {
                    if (!row.IsNull(field))
                    {
                        arg.Value = row[field];
                    }
                    else
                    {
                        arg.Value = null;
                    }
                }
            }
            return arg;
        }
    }
}
