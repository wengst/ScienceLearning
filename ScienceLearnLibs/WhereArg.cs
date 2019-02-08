using System;
using System.Collections;
using System.Data;
using System.Collections.Generic;

namespace LearnLibs
{
    public class WhereArg
    {
        private object _value = null;
        public string Field { get; set; }
        public object Value { get { return _value; } set { _value = value; } }

        void convertValue()
        {
            if (_value != null)
            {
                Type vt = _value.GetType();
                if (vt == typeof(DataRow))
                {
                    DataRow row = (DataRow)_value;
                    if (row.Table.Columns.Contains(Field))
                    {
                        _value = row[Field];
                    }
                }
            }
        }

        public string Where
        {
            get
            {
                convertValue();
                if (!string.IsNullOrWhiteSpace(Field) && Value != null)
                {
                    Type t = Value.GetType();
                    if (t == typeof(Guid))
                    {
                        return "UPPER(HEX(a." + Field + "))='" +F.byteToHexStr(((Guid)Value).ToByteArray()) + "'";
                    }
                    else if (t == typeof(string))
                    {
                        return "a." + Field + "='" + Value.ToString() + "'";
                    }
                    else if (t.IsEnum)
                    {
                        return "a." + Field + "=" + ((int)Value).ToString();
                    }
                    else if (t == typeof(bool))
                    {
                        return "a." + Field + "=" + ((bool)Value ? "1" : "0");
                    }
                    else
                    {
                        return "a." + Field + "=" + Value.ToString();
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string RowFilter
        {
            get
            {
                convertValue();
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
                    else if (t == typeof(bool))
                    {
                        return Field + "=" + ((bool)Value ? "1" : "0");
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
        }

        public WhereArg() { }
        public WhereArg(string fieldName, object value)
        {
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

    public class WhereArgs : CollectionBase
    {
        public void Add(WhereArg arg)
        {
            this.List.Add(arg);
        }
        public void Add(string field, object value)
        {
            this.List.Add(new WhereArg(field, value));
        }
        public void Add(string field, DataRow r)
        {
            WhereArg arg = WhereArg.BuildFromDataRow(r, field);
            this.List.Add(arg);
        }

        public WhereArg this[int index]
        {
            get
            {
                if (index < List.Count && index >= 0)
                {
                    return (WhereArg)List[index];
                }
                return null;
            }
        }

        public List<WhereArg> Items
        {
            get
            {
                List<WhereArg> _items = new List<WhereArg>();
                for (int i = 0; i < List.Count; i++)
                {
                    _items.Add((WhereArg)List[i]);
                }
                return _items;
            }
        }

        public string Where
        {
            get
            {
                string r = string.Empty;
                foreach (WhereArg w in List)
                {
                    if (string.IsNullOrWhiteSpace(r))
                    {
                        r = w.Where;
                    }
                    else
                    {
                        r += " AND " + w.Where;
                    }
                }
                return r;
            }
        }

        public string RowFilter
        {
            get
            {
                string r = string.Empty;
                foreach (WhereArg w in List)
                {
                    r = F.JoinString(r, w.RowFilter, " AND ");
                }
                return r;
            }
        }
    }
}
