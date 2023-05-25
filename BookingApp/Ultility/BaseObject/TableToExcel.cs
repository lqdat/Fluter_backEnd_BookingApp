using HtmlAgilityPack;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace BookingApp.Ultility.BaseObject
{
    public class TableToExcel
    {
        ExcelPackage excel = new ExcelPackage();
        ExcelWorksheet sheet;
        private int maxRow = 0;
        private Dictionary<string, object> cellsOccupied = new Dictionary<string, object>();

        public TableToExcel()
        {
            sheet = excel.Workbook.Worksheets.Add("sheet1");
            // horizontal center
            //sheet.Cells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            // vertical center
            sheet.Cells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            // cells automatically adapt to the size
            sheet.Cells.Style.ShrinkToFit = true;
            sheet.Cells.Style.WrapText = true;
        }

        public byte[] process(string html)
        {
            MemoryStream stream = null;
            try
            {
                process(html, out stream);
                return stream.ToArray();
            }
            finally
            {
                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                    }
                    catch (IOException e)
                    {
                        throw e;
                    }
                }
            }
        }

        public void process(String html, out MemoryStream output)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
            {
                processTable(table);
            }

            try
            {
                output = new MemoryStream();
                excel.SaveAs(output);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void processTable(HtmlNode table)
        {
            int borderSize = table.GetAttributeValue("border", 0);
            string id = table.GetAttributeValue("id", null);

            int rowIndex = 1;
            int colIndex, rowSpan, colSpan, excelColSpan, excelRowSpan;

            if (maxRow > 0)
            {
                // blank row
                // maxRow += 1;
                rowIndex = maxRow;
            }
            // Interate Table Rows.
            var trs = table.Descendants("tr");
            if (trs != null)
            {
                foreach (HtmlNode row in trs)
                {
                    colIndex = 1;
                    // Interate Cols.
                    var tds = row.Descendants("th");
                    if (tds.Count() == 0)
                    {
                        tds = row.Descendants("td");
                    }

                    foreach (HtmlNode td in tds)
                    {
                        // skip occupied cell
                        while (cellsOccupied.ContainsKey(rowIndex + "_" + colIndex))
                        {
                            ++colIndex;
                        }
                        rowSpan = getSpan(td.OuterHtml, 0);
                        colSpan = getSpan(td.OuterHtml, 1);
                        excelColSpan = getExcelColSpan(td.OuterHtml);
                        excelRowSpan = getExcelRowSpan(td.OuterHtml);
                        setValue(rowIndex, colIndex, td);
                        int colIndex1 = colIndex;
                        if (id != "detail")
                        {
                            // col span & row span
                            if (colSpan > 1 && rowSpan > 1)
                            {
                                spanRowAndCol(rowIndex, colIndex, rowSpan, colSpan);
                                colIndex += colSpan;
                            }
                            // col span only
                            else if (colSpan > 1)
                            {
                                spanCol(rowIndex, colIndex, colSpan);
                                colIndex += colSpan;
                            }
                            // row span only
                            else if (rowSpan > 1)
                            {
                                spanRow(rowIndex, colIndex, rowSpan);
                                ++colIndex;
                            }
                            // no span
                            else
                            {
                                ++colIndex;
                            }

                            //Border
                            if (borderSize > 0)
                                border(rowIndex, colIndex);
                        }
                        else
                        {
                            if (excelColSpan > 1)
                            {
                                spanCol(rowIndex, colIndex, excelColSpan);
                                //Border
                                if (borderSize > 0)
                                    border(rowIndex, colIndex, colIndex + excelColSpan - 1);
                                colIndex += excelColSpan - 1;
                            }
                            else if (excelRowSpan > 1)
                            {
                                spanRow(rowIndex, colIndex, excelRowSpan);
                                //Border
                            }
                            else
                            {
                                //Border
                                if (borderSize > 0)
                                    border(rowIndex, colIndex);
                            }
                        }

                        //Format cell
                        //Canh lề
                        string align = td.GetAttributeValue("align", null);
                        if (!string.IsNullOrEmpty(align))
                            alignCell(rowIndex, colIndex1, colIndex, align);

                        //Font size
                        var h1 = td.Descendants("h1");
                        if (h1.Count() > 0)
                        {
                            setFontSize(rowIndex, colIndex1, colIndex, 26);
                        }

                        var bold = td.Descendants().Where(t => t.Name == "b" || t.Name == "strong");
                        if (bold.Count() > 0)
                        {
                            setFontBold(rowIndex, colIndex1, colIndex);
                        }

                        //Width
                        int width = td.GetAttributeValue("width", 0);
                        if (width > 0)
                        {
                            sheet.Column(colIndex).Width = width;
                        }

                        ++colIndex;
                    }
                    ++rowIndex;
                    if (rowIndex > maxRow)
                    {
                        maxRow = rowIndex;
                    }
                }
            }

            //try
            //{

            //}
            //catch (Exception ex) {
            //    File.WriteAllText(HttpContext.Current.Server.MapPath("~/") + "log.txt", ex.Message);
            //}
        }

        private void setFontBold(int rowIndex, int colIndex1, int colIndex2)
        {
            var cell = sheet.Cells[rowIndex, colIndex1, rowIndex, colIndex2];
            cell.Style.Font.Bold = true;
        }

        private void setValue(int rowIndex, int colIndex, HtmlNode td)
        {
            string value = td.InnerText.Trim();

            string data_type = td.GetAttributeValue("data-type", null);
            switch (data_type)
            {
                case "money":
                    value = value.Replace(".", "");
                    int valueInt = 0;
                    try
                    {
                        valueInt = Convert.ToInt32(value);
                    }
                    catch { }

                    sheet.Cells[rowIndex, colIndex].Value = valueInt;
                    sheet.Cells[rowIndex, colIndex].Style.Numberformat.Format = "#,##0;-#,##0;\"-\"??;@";
                    break;
                case "decimal":
                    value = value.Replace(",", ".");
                    decimal valuedecimal = 0.0M;
                    try
                    {
                        valuedecimal = Convert.ToDecimal(value);
                    }
                    catch { }

                    sheet.Cells[rowIndex, colIndex].Value = valuedecimal;
                    sheet.Cells[rowIndex, colIndex].Style.Numberformat.Format = "#,##0.00;-#,##0.00;\"-\"??;@";
                    break;
                case "%":
                    value = value.Replace(",", ".");
                    valuedecimal = 0.0M;
                    try
                    {
                        valuedecimal = Convert.ToDecimal(value) / 100;
                    }
                    catch
                    {
                        //string logPath = HttpContext.Current.Server.MapPath("~/") + "log.txt";
                        //if (File.Exists(logPath))
                        //    File.Delete(logPath);
                        //File.WriteAllText(logPath, ex.Message);
                    }

                    sheet.Cells[rowIndex, colIndex].Value = valuedecimal;
                    sheet.Cells[rowIndex, colIndex].Style.Numberformat.Format = "#,##0.00%;-#,##0.00%;\"-\"??;@";
                    break;
                default:
                    sheet.Cells[rowIndex, colIndex].Value = value;
                    break;
            }
        }

        private void setFontSize(int rowIndex, int colIndex1, int colIndex2, int v)
        {
            var cell = sheet.Cells[rowIndex, colIndex1, rowIndex, colIndex2];
            cell.Style.Font.Size = v;
        }

        private void spanRow(int rowIndex, int colIndex, int rowSpan)
        {
            sheet.Cells[rowIndex, colIndex, rowIndex + rowSpan - 1, colIndex].Merge = true;
            for (int i = 0; i < rowSpan; i++)
            {
                cellsOccupied.Add((rowIndex + i) + "_" + colIndex, true);
            }
            if (rowIndex + rowSpan - 1 > maxRow)
            {
                maxRow = rowIndex + rowSpan - 1;
            }
        }

        private void spanCol(int rowIndex, int colIndex, int colSpan)
        {
            sheet.Cells[rowIndex, colIndex, rowIndex, colIndex + colSpan - 1].Merge = true;
        }

        private void spanRowAndCol(int rowIndex, int colIndex, int rowSpan, int colSpan)
        {
            sheet.Cells[rowIndex, colIndex, rowIndex + rowSpan - 1, colIndex + colSpan - 1].Merge = true;
            for (int i = 0; i < rowSpan; i++)
            {
                for (int j = 0; j < colSpan; j++)
                {
                    cellsOccupied.Add((rowIndex + i) + "_" + (colIndex + j), true);
                }
            }
            if (rowIndex + rowSpan - 1 > maxRow)
            {
                maxRow = rowIndex + rowSpan - 1;
            }
        }

        private int getSpan(string html, int spanType = 0)
        {
            string spanTypeText;
            int span;

            if (spanType == 0)
            {
                spanTypeText = "row";
            }
            else
            {
                spanTypeText = "col";
            }
            string equation = Regex.Match(html.ToLower(), spanTypeText + @"span=.*?\d{1,}").ToString();
            if (!Int32.TryParse(Regex.Match(equation, @"\d{1,}").ToString(), out span))
            {
                span = 1;
            }

            return span;
        }

        private int getExcelColSpan(string html)
        {
            int span;
            string equation = Regex.Match(html.ToLower(), @"excel-colspan=.*?\d{1,}").ToString();
            if (!Int32.TryParse(Regex.Match(equation, @"\d{1,}").ToString(), out span))
            {
                span = 1;
            }

            return span;
        }

        private int getExcelRowSpan(string html)
        {
            int span;
            string equation = Regex.Match(html.ToLower(), @"excel-rowspan=.*?\d{1,}").ToString();
            if (!Int32.TryParse(Regex.Match(equation, @"\d{1,}").ToString(), out span))
            {
                span = 1;
            }

            return span;
        }

        private void border(int rowIndex, int colIndex)
        {
            var cell = sheet.Cells[rowIndex, colIndex];
            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            cell.Style.Border.Top.Color.SetColor(Color.Black);
            cell.Style.Border.Bottom.Color.SetColor(Color.Black);
            cell.Style.Border.Left.Color.SetColor(Color.Black);
            cell.Style.Border.Right.Color.SetColor(Color.Black);
        }

        private void border(int rowIndex, int from, int to)
        {
            var cell = sheet.Cells[rowIndex, from, rowIndex, to];
            cell.Style.Border.Top.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Left.Style = ExcelBorderStyle.Thin;
            cell.Style.Border.Right.Style = ExcelBorderStyle.Thin;

            cell.Style.Border.Top.Color.SetColor(Color.Black);
            cell.Style.Border.Bottom.Color.SetColor(Color.Black);
            cell.Style.Border.Left.Color.SetColor(Color.Black);
            cell.Style.Border.Right.Color.SetColor(Color.Black);
        }

        private void alignCell(int rowIndex, int colIndex1, int colIndex2, string alignValue)
        {
            var cell = sheet.Cells[rowIndex, colIndex1, rowIndex, colIndex2];
            switch (alignValue)
            {
                case "right":
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    break;
                case "center":
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    break;
                default:
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    break;
            }
        }
    }
}