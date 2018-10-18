using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

namespace ChangLab.Common
{
    public class Excel
    {
        public static string Decimal2NumberFormat { get { return "0.00"; } }
        public static string Decimal3NumberFormat { get { return "0.000"; } }
        public static string Decimal4NumberFormat { get { return "0.0000"; } }
        public static string Decimal5NumberFormat { get { return "0.00000"; } }

        public static Workbook ConfigureWorkbook(Application App)
        {
            Workbook book = App.Workbooks.Add();

            // Clear out extraneous worksheets...
            while (book.Worksheets.Count > 1)
            {
                Worksheet deleteSheet = book.Worksheets.Cast<Worksheet>().First();
                deleteSheet.Delete();
            }
            // ...but make sure there's at least one.
            if (book.Worksheets.Count == 0)
            {
                book.Worksheets.Add();
            }

            return book;
        }
    }
}
