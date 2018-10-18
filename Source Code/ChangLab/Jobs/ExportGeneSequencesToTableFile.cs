using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using ChangLab.Common;
using ChangLab.LocalDatabase;
using Microsoft.Office.Interop.Excel;

namespace ChangLab.Jobs
{
    public class ExportGeneSequencesToTableFile : Job
    {
        private List<string> GeneIDs { get; set; }
        private Dictionary<string, string> Columns { get; set;}
        public string FilePath { get; private set; }

        public ExportGeneSequencesToTableFile(IEnumerable<string> GeneIDs, Dictionary<string, string> Columns, string FilePath)
        {
            this.GeneIDs = GeneIDs.ToList();
            this.Columns = Columns;
            this.FilePath = FilePath;
        }

        public void Export()
        {
            Application app = null;
            try
            {
                app = new Application();
                Workbook book = app.Workbooks.Add();

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

                Worksheet sheet = book.Worksheets.Cast<Worksheet>().First();

                var columns = this.Columns
                                .Select((kv, index) => new { ColumnKey = kv.Key, ColumnHeader = kv.Value, Index = (index + 1) })
                                .ToList();
                columns.ForEach(col =>
                        {
                            Range cell = sheet.Cells[1, col.Index];
                            cell.Value = col.ColumnHeader;
                            cell.ColumnWidth = ColumnWidth(col.ColumnKey);
                            cell.Font.Bold = true;
                        });
                
                int rowIndex = 2;
                Common.ProgressUpdateEventArgs progress = new Common.ProgressUpdateEventArgs();
                using (System.Data.DataTable records = Genes.Gene.ForExport(this.GeneIDs))
                {
                    this.OnProgressUpdate(new Common.ProgressUpdateEventArgs() { Setup = true, CurrentMax = records.Rows.Count });

                    for (int i = 0; i < records.Rows.Count; i++)
                    {
                        System.Data.DataRow row = records.Rows[i];

                        columns.ForEach(col =>
                            {
                                Range cell = sheet.Cells[rowIndex + i, col.Index];
                                string value = row[col.ColumnKey].ToString();
                                if (string.IsNullOrEmpty(value)) { value = " "; }
                                cell.Value = value;
                                SetNumberFormat(cell, col.ColumnKey);
                            });

                        progress.CurrentProgress++;
                        this.OnProgressUpdate(progress);
                    }
                }

                progress.CurrentProgress = progress.CurrentMax; progress.StatusMessage = "Saving file...";
                this.OnProgressUpdate(progress);

                book.SaveAs(Filename: this.FilePath, FileFormat: XlFileFormat.xlOpenXMLWorkbook);
            }
            catch (Exception ex)
            {
                throw ex; 
            }
            finally
            {
                if (app != null)
                {
                    try { app.Quit(); }
                    catch { }
                    finally { app = null; }
                }
            }
        }

        private int ColumnWidth(string ColumnKey)
        {
            switch (ColumnKey)
            {
                case "Definition":
                    return 50;
                case "Organism":
                case "Taxonomy":
                    return 30;
                case "Locus":
                case "Accession":
                case "Source":
                    return 13;
                case "Nucleotides":
                    return 100;
                default:
                    return 10;
            }
        }

        private void SetNumberFormat(Range Cell, string ColumnKey)
        {
            switch (ColumnKey)
            {
                case "GenBankID":
                case "Length":
                    Cell.NumberFormat = "0";
                    break;
            }
        }
    }
}
