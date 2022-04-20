using System.Data;
using TextGenerator.Internals;

namespace TextGenerator
{
    public static class Helper
    {
        public static DataTable getCloneDt(DataTable dt, bool clearRows)
        {
            return internalHelpr.getCloneDt(dt, clearRows);
        }
        public static DataTable getDatatbleStruct(DataTable dt)
        {
            return internalHelpr.getDatatbleStruct(dt);
        }
        public static DataTable RemoveEmptyRowFromDatatable(DataTable dt)
        {
            return internalHelpr.RemoveEmptyRowFromDatatable(dt);
        }
        public static DataTable AddIndexColumnInDatatable(DataTable dt, string Col_Name, int Ordinal)
        {
            return internalHelpr.AddIndexColumnInDatatable(dt, Col_Name, Ordinal);
        }
        public static string GetTextExportTableHeadingString(DataTable dt)
        {
            return internalHelpr.GetTextExportTableHeadingString(dt);
        }
        public static string GetTextExportTableHeadingMultilineString(DataTable dt, string sepRatorChar = "*")
        {
            return internalHelpr.GetTextExportTableHeadingMultilineString(dt, sepRatorChar);
        }
        public static string GetTextExportTableRowString(DataTable dt)
        {
            return internalHelpr.GetTextExportTableRowString(dt);
        }
        public static string GetTextExportTableRowStringMultiline(DataTable dt, string sepRatorChar = "*")
        {
            return internalHelpr.GetTextExportTableRowStringMultiline(dt, sepRatorChar);
        }
        public static string GetCSVfromDatatable(DataTable dt)
        {
            return internalHelpr.GetCSVfromDatatable(dt);
        }
        public static string GetHtmlTable(DataTable dt)
        {
            return internalHelpr.GetHtmlTable(dt);
        }
    }
}
