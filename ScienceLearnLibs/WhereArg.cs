using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

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
                    return "Convert(" + Field + ",'System.String')='" + ((Guid)Value).ToString() + "'";
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

        public WhereArg() { }
        public WhereArg(string fieldName, object value) {
            this.Field = fieldName;
            this.Value = value;
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

    public class WhereArgs : CollectionBase {
        public void Add(WhereArg arg) {
            this.List.Add(arg);
        }
        public WhereArg this[int index] {
            get {
                if (index < List.Count && index >= 0) {
                    return (WhereArg)List[index];
                }
                return null;
            }
        }

        public string ToWhereString() {
            string r = string.Empty;
            foreach (WhereArg w in List)
            {
                if (string.IsNullOrWhiteSpace(r))
                {
                    r = w.ToWhereString();
                }
                else {
                    r += " AND " + w.ToWhereString();
                }
            }
            return r;
        }

        public string ToRowFilterString() {
            string r = string.Empty;
            foreach (WhereArg w in List)
            {
                if (string.IsNullOrWhiteSpace(r))
                {
                    r = w.ToRowFilterString();
                }
                else {
                    r += " AND " + w.ToRowFilterString();
                }
            }
            return r;
        }
    }
}
