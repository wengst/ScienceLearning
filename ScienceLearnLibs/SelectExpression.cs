using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LearnLibs
{
    public class SelectExpression
    {
        public string SQLiteWhere { get; set; }
        public string DataTableWhere { get; set; }
        public string SQLiteOrderBy { get; set; }
        public string DataTableSort { get; set; }
        public SelectExpression() {
            this.SQLiteWhere = "";
            this.DataTableWhere = "";
            this.SQLiteOrderBy = "";
            this.DataTableSort = "";
        }
    }
}
