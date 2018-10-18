using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ChangLab.Common;
using Microsoft.Office.Interop.Excel;

/*
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * CmC and Branch-Site outputs are ready to be exported to Excel. 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
*/

namespace ChangLab.PAML.CodeML
{
    public class ExportToExcel
    {
        private DataSet ResultsData { get; set; }
        private System.Data.DataTable ResultsTable { get { return this.ResultsData.Tables[0]; } }
        private System.Data.DataTable ValuesTable { get { return this.ResultsData.Tables[1]; } }
        private string SitesTreeForLRT { get; set; }

        private Application App { get; set; }
        private Workbook Book { get; set; }
        private Worksheet ActiveSheet { get; set; }
        private int WorksheetCount { get; set; }
        private Worksheet NextWorksheet
        {
            get
            {
                if (WorksheetCount == 0) // Yes, this looks odd, but it helps with dealing with there always needing to be one worksheet.
                {
                    WorksheetCount = Book.Worksheets.Count;
                    return Book.Worksheets.Cast<Worksheet>().First();
                }
                else
                {
                    WorksheetCount++;
                    return Book.Worksheets.Add(Type.Missing, Book.Worksheets[Book.Worksheets.Count]);
                }
            }
        }

        private int RowIndex { get; set; }
        private int ColumnIndex { get; set; }

        #region Styles
        private Style HeaderStyle { get; set; }
        private Style SubHeaderStyle { get; set; }
        #endregion

        public ExportToExcel(DataSet ResultsData, string SitesTreeForLRT)
        {
            this.ResultsData = ResultsData;
            this.SitesTreeForLRT = SitesTreeForLRT;

            if (!this.ResultsData.Relations.Contains("FK_ResultID"))
            {
                this.ResultsData.Relations.Add(new DataRelation("FK_ResultID", ResultsTable.Columns["ResultID"], ValuesTable.Columns["ResultID"]));
            }
        }

        public void ExportAndOpen()
        {
            App = null;
            try
            {
                App = new Application();
                Book = Excel.ConfigureWorkbook(App);
                SetupStyles();

                if (this.ResultsTable.Select("ModelPresetKey IN ('Model0', 'Model2a', 'Model8a')").Length != 0)
                {
                    SitesSheet();
                }
                if (this.ResultsTable.Select("ModelPresetKey IN ('Branch', 'BranchNull')").Length != 0)
                {
                    BranchSheet();
                }
                if (this.ResultsTable.Select("ModelPresetKey IN ('BranchSite', 'BranchSiteNull')").Length != 0)
                {
                    BranchSiteSheet();
                }
                if (this.ResultsTable.Select("ModelPresetKey IN ('CmC')").Length != 0)
                {
                    CladeModelSheet(false, CladeModels.C);
                }
                if (this.ResultsTable.Select("ModelPresetKey IN ('CmCNull')").Length != 0)
                {
                    CladeModelSheet(true, CladeModels.C);
                }
                if (this.ResultsTable.Select("ModelPresetKey IN ('CmD')").Length != 0)
                {
                    CladeModelSheet(false, CladeModels.D);
                }
                if (this.ResultsTable.Select("ModelPresetKey IN ('CmDNull')").Length != 0)
                {
                    CladeModelSheet(true, CladeModels.D);
                }

                Book.Worksheets.Cast<Worksheet>().First().Select();
                App.WindowState = XlWindowState.xlNormal;
                App.Visible = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (App != null)
                {
                    // We don't call App.Quit() because we want to leave Excel open for the user.
                    App = null;
                }
            }
        }

        public void SetupStyles()
        {
            HeaderStyle = Book.Styles.Add("Header");
            HeaderStyle.Font.Bold = true;
            HeaderStyle.HorizontalAlignment = XlHAlign.xlHAlignCenter;

            SubHeaderStyle = Book.Styles.Add("SubHeader");
            SubHeaderStyle.Font.Bold = true;
            SubHeaderStyle.HorizontalAlignment = XlHAlign.xlHAlignRight;
        }

        public void SitesSheet()
        {
            try
            {
                this.ActiveSheet = NextWorksheet;
                this.ActiveSheet.Name = "Sites";
                Headers(this.ActiveSheet, HeaderPresets.Sites);

                DataRow[] results = this.ResultsTable.Select("ModelPresetKey IN ('Model0', 'Model2a', 'Model8a')");

                RowIndex = 2;
                string treeTitle = string.Empty;
                foreach (var tree in results
                                        .OrderBy(row => ((string)row["Title"] == SitesTreeForLRT ? 0 : (int)row["Rank"]))
                                        .GroupBy(row => (string)row["Title"]))
                {
                    ColumnIndex = 1;
                    SetValue(tree.Key); ColumnIndex++;

                    int m0rowIndex = 0; int m1rowIndex = 0; int m7rowIndex = 0; int m8arowIndex = 0;
                    foreach (DataRow headerRow in tree.OrderBy(row => (string)row["ResultRank"]))
                    {
                        ModelPresets presetKey = ModelPreset.Derive((string)headerRow["ModelPresetKey"]).Key;
                        int nsSite = (int)headerRow["NSSite"];

                        switch (presetKey)
                        {
                            case ModelPresets.Model0:
                                switch (nsSite)
                                {
                                    case 0: SetValue("M0"); m0rowIndex = RowIndex; break;
                                    case 1: SetValue("M1a"); m1rowIndex = RowIndex; break;
                                    case 2: SetValue("M2a"); break;
                                    case 3: SetValue("M3"); break;
                                    case 7: SetValue("M7"); m7rowIndex = RowIndex; break;
                                    case 8: SetValue("M8"); break;
                                }
                                break;
                            case ModelPresets.Model2a: SetValue("M2a_rel"); break;
                            case ModelPresets.Model8a: SetValue("M8a"); m8arowIndex = RowIndex; break;
                        }
                        ColumnIndex++;

                        SetIntegerValue((int)headerRow["np"]); ColumnIndex++;
                        SetDecimalValue((decimal)headerRow["lnL"], Excel.Decimal2NumberFormat); ColumnIndex++;
                        SetDecimalValue((decimal)headerRow["k"], Excel.Decimal2NumberFormat); ColumnIndex++;

                        SitesValues(headerRow, presetKey, nsSite, m0rowIndex, m1rowIndex, m7rowIndex, m8arowIndex);

                        RowIndex += 2;
                        ColumnIndex = 2;
                    }

                    RowIndex++; // Additional line seperator between trees.
                }

                this.ActiveSheet.Range["A:E"].Columns.AutoFit();
                this.ActiveSheet.Range["F:F"].ColumnWidth = 4.86;
                this.ActiveSheet.Range["G:G"].ColumnWidth = 7.29;
                this.ActiveSheet.Range["H:H"].ColumnWidth = 7.29;
                this.ActiveSheet.Range["I:I"].ColumnWidth = 7.29;
                this.ActiveSheet.Range["J:J"].ColumnWidth = 1.71;
                this.ActiveSheet.Range["K:N"].Columns.AutoFit();

                this.ActiveSheet.Range["A1:E" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                this.ActiveSheet.Range["K1:N" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                this.ActiveSheet.Range["A1:N" + RowIndex.ToString()].Font.Size = 11;
                this.ActiveSheet.Range["N1:N" + RowIndex.ToString()].Font.Bold = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SitesValues(DataRow HeaderRow, ModelPresets PresetKey, int NSSite, int M0RowIndex, int M1RowIndex, int M7RowIndex, int M8aRowIndex)
        {
            DataRow[] valueRows = HeaderRow.GetChildRows("FK_ResultID");
            int valueRowIndex1 = 0; int valueRowIndex2 = 0;
            
            #region Sites 1, 2, 3, and 2a rel
            if ((PresetKey == ModelPresets.Model0 && (NSSite >= 1 && NSSite <= 3)) || PresetKey == ModelPresets.Model2a)
            {
                Parameters_Model1Or2(valueRows);

                if ((PresetKey == ModelPresets.Model0 && (NSSite == 1 || NSSite == 3) && M0RowIndex != 0))
                {
                    RowIndex--; ColumnIndex = 11;
                    ComparisonFormulae("M0", M0RowIndex);
                    RowIndex++;
                }
                else if (((PresetKey == ModelPresets.Model0 && NSSite == 2) || PresetKey == ModelPresets.Model2a) && M1RowIndex != 0)
                {
                    RowIndex--; ColumnIndex = 11;
                    ComparisonFormulae("M1a", M1RowIndex);
                    RowIndex++;
                }
            }
            #endregion
            #region Sites 8 and 8a
            else if ((PresetKey == ModelPresets.Model0 && NSSite == 8) || PresetKey == ModelPresets.Model8a)
            {
                valueRowIndex1 = RowIndex;
                SetSubHeader("p:"); ColumnIndex++;
                SetDecimalValue((decimal)valueRows.First(row => (string)row["ValueTypeName"] == "p")["Value"]); ColumnIndex++;

                SetSubHeader("q:"); ColumnIndex++;
                SetDecimalValue((decimal)valueRows.First(row => (string)row["ValueTypeName"] == "q")["Value"]); ColumnIndex++;

                RowIndex++; ColumnIndex -= 4;

                valueRowIndex2 = RowIndex;
                SetSubHeader("p1:"); ColumnIndex++;
                SetDecimalValue((decimal)valueRows.First(row => (string)row["ValueTypeName"] == "p1")["Value"]); ColumnIndex++;

                SetSubHeader("w:"); ColumnIndex++;
                SetDecimalValue((decimal)valueRows.First(row => (string)row["ValueTypeName"] == "w")["Value"]); ColumnIndex++;

                if (PresetKey == ModelPresets.Model8a)
                {
                    RowIndex--; ColumnIndex++;
                    SetValue("n/a");
                    RowIndex++;
                }
                else
                {
                    ColumnIndex++;
                    if (M7RowIndex != 0)
                    {
                        ComparisonFormulae(valueRowIndex1, ColumnIndex, "M7", valueRowIndex1, M7RowIndex);
                    }
                    if (M8aRowIndex != 0)
                    {
                        ComparisonFormulae(valueRowIndex2, ColumnIndex, "M8a", valueRowIndex1, M8aRowIndex);
                    }
                }
            }
            #endregion
            else
            {
                switch (PresetKey)
                {
                    #region Sites 0 and 7
                    case ModelPresets.Model0:
                        switch (NSSite)
                        {
                            case 0:
                                SetDecimalValue((decimal)valueRows[0]["Value"], Excel.Decimal5NumberFormat);
                                this.ActiveSheet.Range["F" + RowIndex.ToString() + ":I" + RowIndex.ToString()].Merge();
                                this.ActiveSheet.Range["F" + RowIndex.ToString() + ":I" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                                ColumnIndex += 5;
                                SetValue("n/a");
                                break;
                            case 7:
                                SetSubHeader("p:"); ColumnIndex++;
                                SetDecimalValue((decimal)valueRows.First(row => (string)row["ValueTypeName"] == "p")["Value"], Excel.Decimal5NumberFormat); ColumnIndex++;
                                SetSubHeader("q:"); ColumnIndex++;
                                SetDecimalValue((decimal)valueRows.First(row => (string)row["ValueTypeName"] == "q")["Value"], Excel.Decimal5NumberFormat); ColumnIndex++;
                                ColumnIndex++;
                                SetValue("n/a");
                                break;
                        }
                        break;
                    #endregion
                }
            }
        }

        private void Headers(Worksheet Sheet, HeaderPresets Preset)
        {
            int col = 65;
            if (Preset == HeaderPresets.Sites) { Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "Tree"; col++; }
            if (Preset == HeaderPresets.Branch || Preset == HeaderPresets.BranchSite) { Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "Branch"; col++; }
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "Model"; col++;
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "np"; col++;
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "lnL"; col++;
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "k"; col++;
            if (Preset != HeaderPresets.Sites)
            {
                Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "AIC"; col++;
                Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "ΔAIC"; col++;
            }
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "Parameters";
            int parameterWidth = (Preset == HeaderPresets.BranchSite ? 5 : 4);
            Sheet.Range[char.ConvertFromUtf32(col) + "1:" + char.ConvertFromUtf32(col + (parameterWidth - 1)) + "1"].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            Sheet.Range[char.ConvertFromUtf32(col) + "1:" + char.ConvertFromUtf32(col + (parameterWidth - 1)) + "1"].Merge(); col += (parameterWidth + 1);
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "Null"; col++;
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "LRT"; col++;
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "df"; col++;
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Value = "P";

            string range = char.ConvertFromUtf32(65) + "1:" + char.ConvertFromUtf32(col) + "1";
            Sheet.Range[range].Style = HeaderStyle.Name;
            Sheet.Range[range].Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
            Sheet.Range[char.ConvertFromUtf32(col) + "1"].Font.Italic = true; // Italicize the P column
        }

        private enum HeaderPresets
        {
            Sites,
            CladeModel,
            BranchSite,
            Branch
        }

        private void CoreValues(DataRow HeaderRow, bool Formulae)
        {
            SetIntegerValue((int)HeaderRow["np"]); ColumnIndex++;
            SetDecimalValue((decimal)HeaderRow["lnL"], Excel.Decimal2NumberFormat); ColumnIndex++;
            SetDecimalValue((decimal)HeaderRow["k"], Excel.Decimal2NumberFormat); ColumnIndex++;

            if (Formulae)
            {
                SetDecimalFormula("=-2*C" + RowIndex.ToString() + "+2*B" + RowIndex.ToString(), Excel.Decimal2NumberFormat); ColumnIndex++;
                SetDecimalFormula("", Excel.Decimal2NumberFormat); ColumnIndex++;
            }
        }

        private void Parameters_Model1Or2(DataRow[] ValueRows)
        {
            ValueRows.GroupBy(row => (string)row["ValueTypeName"]).Select((g, i) => new { Group = g, Index = i }).ToList().ForEach(g =>
            {
                SetSubHeader(g.Group.Key + ":"); ColumnIndex++;
                g.Group.OrderBy(valueRow => (string)valueRow["SiteClass"]).ToList().ForEach(valueRow =>
                {
                    SetDecimalValue((decimal)valueRow["Value"]); ColumnIndex++;
                });
                RowIndex++;
                ColumnIndex -= (g.Group.Count() + 1);
            });
            RowIndex--;
        }

        private void BranchSheet()
        {
            try
            {
                this.ActiveSheet = NextWorksheet;
                this.ActiveSheet.Name = "Branch";
                Headers(this.ActiveSheet, HeaderPresets.Branch);

                DataRow[] results = null;
                RowIndex = 2;
                int m2aRowIndex = 0;
                int coreValuesStartColumnIndex = 3;

                #region Summary
                DataRow m2arow = this.ResultsTable.Select("Title='" + this.SitesTreeForLRT + "' AND ModelPresetKey='Model2a'").FirstOrDefault();
                if (m2arow != null)
                {
                    ColumnIndex = 1; m2aRowIndex = RowIndex;
                    SetValue("None"); ColumnIndex++;
                    SetValue("M2a_rel"); ColumnIndex++;
                    CoreValues(m2arow, true);
                    Parameters_Model1Or2(m2arow.GetChildRows("FK_ResultID"));

                    RowIndex--; ColumnIndex = 14;
                    SetValue("n/a");
                    RowIndex += 3;
                }
                #endregion

                results = this.ResultsTable.Select("ModelPresetKey IN ('Branch', 'BranchNull')");
                foreach (var tree in results.GroupBy(row => (string)row["Title"]))
                {
                    int brNullRowIndex = 0;
                    foreach (DataRow headerRow in tree.OrderBy(row => (string)row["ResultRank"]))
                    {
                        int titleRowIndex = RowIndex;
                        ColumnIndex = 1;
                        SetValue(tree.Key); ColumnIndex++;
                        string model = string.Empty;
                        switch ((string)headerRow["ModelPresetKey"])
                        {
                            case "BranchNull": model = "Br_Null"; brNullRowIndex = RowIndex; break;
                            case "Branch": model = "Br_Alt"; break;
                        }
                        SetValue(model); ColumnIndex++;

                        BranchValues(headerRow, brNullRowIndex, titleRowIndex, coreValuesStartColumnIndex);
                        RowIndex++;
                        ColumnIndex = 1;
                    }

                    RowIndex++; // Additional line seperator between trees.
                }

                this.ActiveSheet.Range["A:G"].Columns.AutoFit();
                this.ActiveSheet.Range["H:K"].ColumnWidth = 9;
                this.ActiveSheet.Range["L:L"].ColumnWidth = 1.71;
                this.ActiveSheet.Range["M:P"].Columns.AutoFit();

                this.ActiveSheet.Range["A1:G" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                this.ActiveSheet.Range["M1:P" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                this.ActiveSheet.Range["A1:P" + RowIndex.ToString()].Font.Size = 11;
                this.ActiveSheet.Range["P1:P" + RowIndex.ToString()].Font.Bold = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BranchValues(DataRow HeaderRow, int BrNullRowIndex, int TitleRowIndex, int CoreValuesStartColumnIndex)
        {
            CoreValues(HeaderRow, true);
            DataRow[] valueRows = HeaderRow.GetChildRows("FK_ResultID");

            int parameterColIndex = ColumnIndex;
            SetSubHeader("background"); ColumnIndex++;
            valueRows.Where(row => (string)row["ValueTypeKey"] == "background_w").ToList().ForEach(row => { SetDecimalValue((decimal)row["Value"]); ColumnIndex++; });
            RowIndex++; ColumnIndex = parameterColIndex;
            SetSubHeader("foreground"); ColumnIndex++;
            valueRows.Where(row => (string)row["ValueTypeKey"] == "foreground_w").ToList().ForEach(row => { SetDecimalValue((decimal)row["Value"]); ColumnIndex++; });
            RowIndex++;

            int endOfResultsRowIndex = (RowIndex - 1);

            if ((string)HeaderRow["ModelPresetKey"] == "Branch" && BrNullRowIndex != 0)
            {
                RowIndex = TitleRowIndex; ColumnIndex = 14;
                ComparisonFormulae("Br_Null", BrNullRowIndex, CoreValuesStartColumnIndex);
                RowIndex = endOfResultsRowIndex;
            }
        }

        private void BranchSiteSheet()
        {
            try
            {
                this.ActiveSheet = NextWorksheet;
                this.ActiveSheet.Name = "Branch-Site";
                Headers(this.ActiveSheet, HeaderPresets.BranchSite);

                DataRow[] results = null;
                RowIndex = 2;
                int m2aRowIndex = 0;
                int coreValuesStartColumnIndex = 3;

                #region Summary
                DataRow m2arow = this.ResultsTable.Select("Title='" + this.SitesTreeForLRT + "' AND ModelPresetKey='Model2a'").FirstOrDefault();
                if (m2arow != null)
                {
                    ColumnIndex = 1; m2aRowIndex = RowIndex;
                    SetValue("None"); ColumnIndex++;
                    SetValue("M2a_rel"); ColumnIndex++;
                    CoreValues(m2arow, true);
                    Parameters_Model1Or2(m2arow.GetChildRows("FK_ResultID"));

                    RowIndex--; ColumnIndex = 14;
                    SetValue("n/a");
                    RowIndex += 3;
                }
                #endregion

                results = this.ResultsTable.Select("ModelPresetKey IN ('BranchSite', 'BranchSiteNull')"); //, 'CmC'
                foreach (var tree in results.GroupBy(row => (string)row["Title"]))
                {
                    int brSNullRowIndex = 0;
                    foreach (DataRow headerRow in tree.OrderBy(row => (string)row["ResultRank"]))
                    {
                        int titleRowIndex = RowIndex;
                        ColumnIndex = 1;
                        SetValue(tree.Key); ColumnIndex++;
                        string model = string.Empty;
                        switch ((string)headerRow["ModelPresetKey"])
                        {
                            case "BranchSiteNull": model = "BrS_Null"; brSNullRowIndex = RowIndex; break;
                            case "BranchSite": model = "BrS_Alt"; break;
                            case "CmC": model = "CmC"; break;
                            case "CmD": model = "CmD"; break;
                        }
                        SetValue(model); ColumnIndex++;

                        //if (model == "CmC")
                        //{
                        //    CmCValues(headerRow, m2aRowIndex, titleRowIndex, 14);
                        //}
                        //else
                        //{
                        BranchSiteValues(headerRow, brSNullRowIndex, titleRowIndex, coreValuesStartColumnIndex);
                        //}
                        RowIndex++;
                        ColumnIndex = 1;
                    }

                    RowIndex++; // Additional line seperator between trees.
                }

                this.ActiveSheet.Range["A:H"].Columns.AutoFit();
                this.ActiveSheet.Range["I:L"].ColumnWidth = 9;
                this.ActiveSheet.Range["M:M"].ColumnWidth = 1.71;
                this.ActiveSheet.Range["N:Q"].Columns.AutoFit();

                this.ActiveSheet.Range["A1:G" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                this.ActiveSheet.Range["N1:Q" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                this.ActiveSheet.Range["A1:Q" + RowIndex.ToString()].Font.Size = 11;
                this.ActiveSheet.Range["Q1:Q" + RowIndex.ToString()].Font.Bold = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BranchSiteValues(DataRow HeaderRow, int BrSNullRowIndex, int TitleRowIndex, int CoreValuesStartColumnIndex)
        {
            CoreValues(HeaderRow, true);
            DataRow[] valueRows = HeaderRow.GetChildRows("FK_ResultID");

            int parameterColIndex = ColumnIndex;
            SetSubHeader("site"); ColumnIndex++;
            valueRows.Select(row => (string)row["SiteClass"]).Distinct().ToList().ForEach(sc => 
                {
                    SetSubHeader(sc);
                    this.ActiveSheet.Cells[RowIndex, ColumnIndex].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                    ColumnIndex++;
                });
            RowIndex++; ColumnIndex = parameterColIndex;
            SetSubHeader("proportion"); ColumnIndex++;
            valueRows.Where(row => (string)row["ValueTypeKey"] == "p_value").ToList().ForEach(row => { SetDecimalValue((decimal)row["Value"]); ColumnIndex++; });
            RowIndex++; ColumnIndex = parameterColIndex;
            SetSubHeader("background"); ColumnIndex++;
            valueRows.Where(row => (string)row["ValueTypeKey"] == "background_w").ToList().ForEach(row => { SetDecimalValue((decimal)row["Value"]); ColumnIndex++; });
            RowIndex++; ColumnIndex = parameterColIndex;
            SetSubHeader("foreground"); ColumnIndex++;
            valueRows.Where(row => (string)row["ValueTypeKey"] == "foreground_w").ToList().ForEach(row => { SetDecimalValue((decimal)row["Value"]); ColumnIndex++; });
            RowIndex++;

            int endOfResultsRowIndex = (RowIndex - 1);

            if ((string)HeaderRow["ModelPresetKey"] == "BranchSite" && BrSNullRowIndex != 0)
            {
                RowIndex = TitleRowIndex; ColumnIndex = 14;
                ComparisonFormulae("BrS_Null", BrSNullRowIndex, CoreValuesStartColumnIndex);
                RowIndex = endOfResultsRowIndex;
            }
        }

        private void CladeModelSheet(bool Null, CladeModels Model)
        {
            try
            {
                this.ActiveSheet = NextWorksheet;
                this.ActiveSheet.Name = "Cm" + Model.ToString() + (Null ? " Res" : string.Empty);
                Headers(this.ActiveSheet, HeaderPresets.CladeModel);

                DataRow[] results = null;
                RowIndex = 2;
                int m1RowIndex = 0; int cladeModelComparisonRowIndex = 0;
                int coreValuesStartColumnIndex = 2;

                #region Summary
                if (this.ResultsTable.Select("Title='" + this.SitesTreeForLRT + "' AND ModelPresetKey IN ('Model0', 'Model2a')").Length != 0)
                {
                    DataRow m1row = this.ResultsTable.Select("Title='" + this.SitesTreeForLRT + "' AND ModelPresetKey='Model0' AND NSSite=1").FirstOrDefault();
                    if (m1row != null)
                    {
                        ColumnIndex = 1; m1RowIndex = RowIndex;
                        SetValue("M1a"); ColumnIndex++; // Model
                        CoreValues(m1row, true);

                        Parameters_Model1Or2(m1row.GetChildRows("FK_ResultID"));
                        RowIndex--; ColumnIndex = 12;
                        SetValue("n/a");
                        RowIndex += 3;
                    }

                    DataRow m2row = this.ResultsTable.Select("Title='" + this.SitesTreeForLRT + "' AND ModelPresetKey='Model0' AND NSSite=2").FirstOrDefault();
                    if (m2row != null)
                    {
                        ColumnIndex = 1;
                        SetValue("M2a"); ColumnIndex++;
                        CoreValues(m2row, true);
                        Parameters_Model1Or2(m2row.GetChildRows("FK_ResultID"));

                        if (m1RowIndex != 0)
                        {
                            RowIndex--; ColumnIndex = 12;
                            ComparisonFormulae("M1a", m1RowIndex, coreValuesStartColumnIndex);
                            RowIndex++;
                        }
                        RowIndex += 2;
                    }

                    DataRow m2arow = this.ResultsTable.Select("Title='" + this.SitesTreeForLRT + "' AND ModelPresetKey='Model2a'").FirstOrDefault();
                    if (m2arow != null)
                    {
                        ColumnIndex = 1; if (Model == CladeModels.C) { cladeModelComparisonRowIndex = RowIndex; }
                        SetValue("M2a_rel"); ColumnIndex++;
                        CoreValues(m2arow, true);
                        Parameters_Model1Or2(m2arow.GetChildRows("FK_ResultID"));

                        if (m1RowIndex != 0)
                        {
                            RowIndex--; ColumnIndex = 12;
                            ComparisonFormulae("M1a", m1RowIndex, coreValuesStartColumnIndex);
                            RowIndex++;
                        }
                        RowIndex += 2;
                    }

                    if (Model == CladeModels.D)
                    {
                        DataRow m3row = this.ResultsTable.Select("Title='" + this.SitesTreeForLRT + "' AND ModelPresetKey='Model0' AND NSSite=3").FirstOrDefault();
                        if (m3row != null)
                        {
                            ColumnIndex = 1; if (Model == CladeModels.D) { cladeModelComparisonRowIndex = RowIndex; }
                            SetValue("M3"); ColumnIndex++;
                            CoreValues(m3row, true);
                            Parameters_Model1Or2(m3row.GetChildRows("FK_ResultID"));

                            if (m1RowIndex != 0)
                            {
                                RowIndex--; ColumnIndex = 12;
                                ComparisonFormulae("M1a", m1RowIndex, coreValuesStartColumnIndex);
                                RowIndex++;
                            }
                            RowIndex += 2;
                        }
                    }
                }
                #endregion

                results = this.ResultsTable.Select("ModelPresetKey='Cm" + Model.ToString() + (Null ? "Null" : string.Empty) + "'");
                foreach (var tree in results.GroupBy(row => (string)row["Title"]))
                {
                    int titleRowIndex = RowIndex;
                    ColumnIndex = 1;
                    SetValue(tree.Key); ColumnIndex++;

                    foreach (DataRow headerRow in tree.OrderBy(row => (string)row["ResultRank"]))
                    {
                        CladeModelValues(Model, headerRow, cladeModelComparisonRowIndex, titleRowIndex, coreValuesStartColumnIndex);
                        RowIndex++;
                        ColumnIndex = 2;
                    }

                    RowIndex++; // Additional line seperator between trees.
                }

                this.ActiveSheet.Range["A:F"].Columns.AutoFit();
                this.ActiveSheet.Range["G:J"].ColumnWidth = 9;
                this.ActiveSheet.Range["K:K"].ColumnWidth = 1.71;
                this.ActiveSheet.Range["L:O"].Columns.AutoFit();

                this.ActiveSheet.Range["A1:F" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                this.ActiveSheet.Range["L1:P" + RowIndex.ToString()].HorizontalAlignment = XlHAlign.xlHAlignCenter;
                this.ActiveSheet.Range["A1:O" + RowIndex.ToString()].Font.Size = 11;
                this.ActiveSheet.Range["O1:O" + RowIndex.ToString()].Font.Bold = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void CladeModelValues(CladeModels Model, DataRow HeaderRow, int CladeModelComparisonRowIndex, int TitleRowIndex, int CoreValuesStartColumnIndex, int NullColumnIndex = 0)
        {
            CoreValues(HeaderRow, true);
            DataRow[] valueRows = HeaderRow.GetChildRows("FK_ResultID");

            int parameterColIndex = ColumnIndex;
            SetSubHeader("site"); ColumnIndex++;
            valueRows.Select(row => (string)row["SiteClass"]).Distinct().ToList().ForEach(sc => 
                {
                    SetSubHeader(sc); 
                    this.ActiveSheet.Cells[RowIndex, ColumnIndex].HorizontalAlignment = XlHAlign.xlHAlignCenter; 
                    ColumnIndex++;
                });
            RowIndex++; ColumnIndex = parameterColIndex;
            SetSubHeader("proportion"); ColumnIndex++;
            valueRows.Where(row => (string)row["ValueTypeName"] == "p").ToList().ForEach(row => { SetDecimalValue((decimal)row["Value"]); ColumnIndex++; });
            RowIndex++; ColumnIndex = parameterColIndex;

            valueRows
                .Where(row => (string)row["ValueTypeName"] == "Branch Type")
                .GroupBy(row => (int)row["ValueRank"])
                .Select((g, i) => new { Group = g, Index = i })
                .ToList()
                .ForEach(g =>
                {
                    SetSubHeader("branch " + g.Index.ToString()); ColumnIndex++;
                    g.Group.OrderBy(valueRow => (string)valueRow["SiteClass"]).ToList().ForEach(valueRow =>
                    { SetDecimalValue((decimal)valueRow["Value"]); ColumnIndex++; });
                    RowIndex++;
                    ColumnIndex = parameterColIndex;
                });
            int endOfResultsRowIndex = (RowIndex - 1);

            if (CladeModelComparisonRowIndex != 0)
            {
                RowIndex = TitleRowIndex; ColumnIndex = (NullColumnIndex == 0 ? 12 : NullColumnIndex);
                string comparisonModel = string.Empty;
                switch (Model)
                {
                    case CladeModels.C: comparisonModel = "M2a_rel"; break;
                    case CladeModels.D: comparisonModel = "M3"; break;
                }
                ComparisonFormulae(comparisonModel, CladeModelComparisonRowIndex, CoreValuesStartColumnIndex);
                RowIndex = endOfResultsRowIndex;
            }
        }

        private enum CladeModels
        {
            C,
            D
        }

        #region Set Range Value
        private void SetValue(string Text, XlHAlign HorizonalAlign = XlHAlign.xlHAlignGeneral, string Style = "")
        {
            SetValue(this.RowIndex, this.ColumnIndex, Text, HorizonalAlign, Style);
        }
        private void SetValue(int RowIndex, int ColumnIndex, string Text, XlHAlign HorizonalAlign = XlHAlign.xlHAlignGeneral, string Style = "")
        {
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].Value = Text;
            if (HorizonalAlign != XlHAlign.xlHAlignGeneral) { this.ActiveSheet.Cells[RowIndex, ColumnIndex].HorizontalAlignment = HorizonalAlign; }
            if (!string.IsNullOrEmpty(Style)) { this.ActiveSheet.Cells[RowIndex, ColumnIndex].Style = Style; }
        }

        private void SetSubHeader(string Text)
        {
            SetSubHeader(this.RowIndex, this.ColumnIndex, Text);
        }
        private void SetSubHeader(int RowIndex, int ColumnIndex, string Text)
        {
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].Value = Text;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].HorizontalAlignment = XlHAlign.xlHAlignRight;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].Style = SubHeaderStyle.Name;
        }

        private void SetDecimalValue(decimal Value, string Format = "")
        {
            SetDecimalValue(this.RowIndex, this.ColumnIndex, Value, Format);
        }
        private void SetDecimalValue(int RowIndex, int ColumnIndex, decimal Value, string Format = "")
        {
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].Value = Value;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].NumberFormat = (string.IsNullOrEmpty(Format) ? Excel.Decimal3NumberFormat : Format);
        }

        private void SetDecimalFormula(string Formula, string Format = "")
        {
            SetDecimalFormula(this.RowIndex, this.ColumnIndex, Formula, Format);
        }
        private void SetDecimalFormula(int RowIndex, int ColumnIndex, string Formula, string Format = "")
        {
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].Value = Formula;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].NumberFormat = (string.IsNullOrEmpty(Format) ? Excel.Decimal3NumberFormat : Format);
        }

        private void SetIntegerValue(int Value)
        {
            SetIntegerValue(this.RowIndex, this.ColumnIndex, Value);
        }
        private void SetIntegerValue(int RowIndex, int ColumnIndex, int Value)
        {
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].Value = Value;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].NumberFormat = "0";
        }

        private void SetIntegerFormula(string Formula)
        {
            SetIntegerFormula(this.RowIndex, this.ColumnIndex, Formula);
        }
        private void SetIntegerFormula(int RowIndex, int ColumnIndex, string Formula)
        {
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].Value = Formula;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            this.ActiveSheet.Cells[RowIndex, ColumnIndex].NumberFormat = "0";
        }

        private void ComparisonFormulae(string ComparisonModel, int ComparisonRowIndex, int CoreValuesStartColumnIndex = 3)
        {
            ComparisonFormulae(this.RowIndex, this.ColumnIndex, ComparisonModel, this.RowIndex, ComparisonRowIndex, CoreValuesStartColumnIndex);
        }
        private void ComparisonFormulae(int RowIndex, int ColumnIndex, string ComparisonModel, int SourceRowIndex, int ComparisonRowIndex, int CoreValuesStartColumnIndex = 3)
        {
            SetValue(RowIndex, ColumnIndex, ComparisonModel); ColumnIndex++;
            SetDecimalFormula(RowIndex, ColumnIndex, "=2*(" + char.ConvertFromUtf32(65 + (CoreValuesStartColumnIndex)) + SourceRowIndex.ToString() + "-" + char.ConvertFromUtf32(65 + (CoreValuesStartColumnIndex)) + ComparisonRowIndex.ToString() + ")", Excel.Decimal3NumberFormat); ColumnIndex++;
            SetIntegerFormula(RowIndex, ColumnIndex, "=" + char.ConvertFromUtf32(65 + (CoreValuesStartColumnIndex - 1)) + SourceRowIndex.ToString() + "-" + char.ConvertFromUtf32(65 + (CoreValuesStartColumnIndex - 1)) + ComparisonRowIndex.ToString()); ColumnIndex++;
            SetDecimalFormula(RowIndex, ColumnIndex, "=CHIDIST(" + char.ConvertFromUtf32(65 + (ColumnIndex - 3)) + RowIndex.ToString() + "," + char.ConvertFromUtf32(65 + (ColumnIndex - 2)) + RowIndex.ToString() + ")", Excel.Decimal4NumberFormat); ColumnIndex++;
        }
        #endregion
    }
}
