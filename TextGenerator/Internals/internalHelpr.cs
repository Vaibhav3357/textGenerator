using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace TextGenerator.Internals
{
    internal static class internalHelpr
    {
        public static DataTable getCloneDt(DataTable dt, bool clearRows)
        {
            DataTable _dtClone = dt;
            foreach (System.Data.DataColumn col in _dtClone.Columns) col.ReadOnly = false;
            foreach (System.Data.DataColumn col in _dtClone.Columns) col.AllowDBNull = true;
            _dtClone.PrimaryKey = null;
            if (clearRows)
            {
                _dtClone.Rows.Clear();
            }
            return _dtClone;
        }
        public static DataTable getDatatbleStruct(DataTable dt)
        {
            DataTable _dtClone = new DataTable();
            foreach (var item in dt.Columns)
            {
                _dtClone.Columns.Add(new DataColumn()
                {
                    AllowDBNull = true,
                    AutoIncrement = false,
                    ColumnName = item.ToString(),
                    Caption = item.ToString(),
                    DataType = typeof(string),
                    DefaultValue = "",
                    ReadOnly = false
                });
            }
            return _dtClone;
        }
        public static DataTable RemoveEmptyRowFromDatatable(DataTable dt)
        {
            try
            {
                DataTable dataTable = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field =>
                         field is System.DBNull || string.Compare((field as string).Trim(),
                         string.Empty) == 0)).CopyToDataTable();

                return dataTable;
            }
            catch (Exception)
            {
                DataTable dtClone = dt.Clone();
                return dtClone;
            }
        }
        public static DataTable AddIndexColumnInDatatable(DataTable dt, string Col_Name, int Ordinal)
        {
            DataColumn Col = dt.Columns.Add(Col_Name, System.Type.GetType("System.String"));
            Col.SetOrdinal(Ordinal);

            int index = 0;
            foreach (var item in dt.Rows)
            {
                dt.Rows[index][0] = index + 1;
                index++;
            }
            return dt;
        }
        public static string GetTextExportTableHeadingString(DataTable dt)
        {
            StringBuilder str = new StringBuilder();

            List<string> HeadingArray = new List<string>();
            List<string> HeadingLengthArray = new List<string>();

            foreach (var item in dt.Columns)
            {
                if (item.ToString().Any(char.IsDigit))
                {
                    var dbHeading = item.ToString().Split('_');
                    HeadingLengthArray.Add(dbHeading[0]);
                    HeadingArray.Add(dbHeading[1]);
                }
            }

            StringBuilder sb = new StringBuilder();

            int indexGlobal = 0;

            foreach (var item in HeadingArray)
            {
                var heading = item.PadRight(Convert.ToInt32(HeadingLengthArray[indexGlobal]));
                sb.Append(heading);
                indexGlobal += 1;
            }
            sb.Append(Environment.NewLine);

            return sb.ToString();
        }
        public static string GetTextExportTableHeadingMultilineString(DataTable dt, string sepRatorChar = "*")
        {
            DataTable _dt = dt.Clone();
            List<string> HeadingArray = new List<string>();
            List<string> HeadingLengthArray = new List<string>();

            foreach (var item in _dt.Columns)
            {
                if (item.ToString().Any(char.IsDigit))
                {
                    var dbHeading = item.ToString().Split('_');
                    HeadingLengthArray.Add(dbHeading[0]);
                    HeadingArray.Add(dbHeading[1]);
                }

            }

            DataTable tempData = getCloneDt(_dt, true);
            StringBuilder sb = new StringBuilder();

            int indexGlobal = 0;

            foreach (var item in HeadingArray)
            {
                var _heading = item.ToString();
                if (_heading.Contains(sepRatorChar))
                {
                    string[] _sepHeading = item.Split(sepRatorChar);
                    for (int i = 0; i < _sepHeading.Length; i++)
                    {
                        if (i != 0)
                        {
                            tempData.Rows.Add();
                            var currentVal = _sepHeading[i];
                            tempData.Rows[i - 1][HeadingLengthArray[indexGlobal] + "_" + item.ToString()] = currentVal;
                        }
                        else
                        {
                            var heading = _sepHeading[i].PadRight(Convert.ToInt32(HeadingLengthArray[indexGlobal]));
                            sb.Append(heading);
                        }
                    }
                }
                else
                {
                    var heading = item.PadRight(Convert.ToInt32(HeadingLengthArray[indexGlobal]));
                    sb.Append(heading);
                }

                indexGlobal += 1;
            }
            sb.Append(Environment.NewLine);
            tempData = RemoveEmptyRowFromDatatable(tempData);
            sb.Append(GetTextExportTableRowString(tempData));
            return sb.ToString();
        }
        public static string GetTextExportTableRowString(DataTable dt)
        {
            List<string> HeadingArray = new List<string>();
            List<string> HeadingLengthArray = new List<string>();

            foreach (var item in dt.Columns)
            {
                if (item.ToString().Any(char.IsDigit))
                {
                    var dbHeading = item.ToString().Split('_');
                    HeadingLengthArray.Add(dbHeading[0]);
                    HeadingArray.Add(dbHeading[1]);
                }

            }

            StringBuilder sb = new StringBuilder();

            int indexGlobal = 0;
            int ColumHolder = 0;
            foreach (DataRow dtRows in dt.Rows)
            {
                indexGlobal = 0;
                ColumHolder = 0;
                // On all tables' columns
                string RowString = "";
                foreach (var dc in dtRows.ItemArray)
                {
                    if (dt.Columns[ColumHolder].ColumnName.Any(char.IsDigit))
                    {
                        RowString = RowString + dc.ToString().PadRight(Convert.ToInt32(HeadingLengthArray[indexGlobal]));
                    }
                    indexGlobal += 1;
                    ColumHolder += 1;
                }
                sb.AppendLine(RowString);
                RowString = "";
            }

            return sb.ToString();
        }
        public static string GetTextExportTableRowStringMultiline(DataTable dt, string sepRatorChar = "*")
        {
            List<string> HeadingArray = new List<string>();
            List<string> HeadingLengthArray = new List<string>();

            foreach (var item in dt.Columns)
            {
                if (item.ToString().Any(char.IsDigit))
                {
                    var dbHeading = item.ToString().Split('_');
                    HeadingLengthArray.Add(dbHeading[0]);
                    HeadingArray.Add(dbHeading[1]);
                }

            }
            GC.Collect();

            StringBuilder sb = new StringBuilder();

            DataTable tempData = getDatatbleStruct(dt);
            int indexGlobal = 0;
            int ColumHolder = 0;
            foreach (DataRow dtRows in dt.Rows)
            {
                indexGlobal = 0;
                ColumHolder = 0;
                // On all tables' columns
                string RowString = "";
                foreach (var dc in dtRows.ItemArray)
                {
                    if (dt.Columns[ColumHolder].ColumnName.Any(char.IsDigit))
                    {

                        if (dc.ToString().Contains(sepRatorChar))
                        {
                            string[] _sepColumn = dc.ToString().Split(sepRatorChar);
                            for (int i = 0; i < _sepColumn.Length; i++)
                            {
                                if (i != 0)
                                {
                                    tempData.Rows.Add();
                                    if (_sepColumn[i].Length > 0)
                                    {
                                        tempData.Rows[i - 1][dt.Columns[ColumHolder].ColumnName] = _sepColumn[i].ToString();
                                    }
                                    else
                                    {
                                        tempData.Rows[i - 1][dt.Columns[ColumHolder].ColumnName] = DBNull.Value;
                                    }
                                }
                                else
                                {
                                    var heading = _sepColumn[i].PadRight(Convert.ToInt32(HeadingLengthArray[indexGlobal]));
                                    RowString = RowString + heading;
                                }
                            }

                        }
                        else
                        {
                            RowString = RowString + dc.ToString().PadRight(Convert.ToInt32(HeadingLengthArray[indexGlobal]));
                        }
                    }
                    indexGlobal += 1;
                    ColumHolder += 1;
                }
                sb.AppendLine(RowString);
                if (tempData.Rows.Count > 0)
                {
                    tempData = RemoveEmptyRowFromDatatable(tempData);
                    sb.Append(GetTextExportTableRowString(tempData));
                    tempData.Rows.Clear();
                }
                RowString = "";
            }

            return sb.ToString();
        }
        public static string GetCSVfromDatatable(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            return sb.ToString();
        }
        public static string GetHtmlTable(DataTable dt)
        {
            StringBuilder table = new StringBuilder();
            table.Append(" <table> ");
            table.Append(" <tr> ");
            foreach (var item in dt.Columns)
            {
                table.Append(" <th>" + item.ToString() + "</th>");
            }
            table.Append(" </tr>");

            foreach (DataRow dtRows in dt.Rows)
            {
                table.Append(" <tr>");
                foreach (var dc in dtRows.ItemArray)
                {
                    table.Append("<td>" + dc.ToString() + "</td>");
                }
                table.Append(" </tr>");
            }

            return table.ToString();
        }
    }
}
