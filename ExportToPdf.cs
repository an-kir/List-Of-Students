using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Web.UI.WebControls;


namespace Students
{
    public class ExportToPdf
    {
        public ExportToPdf(){}
        public static void Export(DetailsView dvGetStudent)
        {
            int rows = dvGetStudent.Rows.Count;
            int columns = dvGetStudent.Rows[0].Cells.Count;
            int pdfTableRows = rows;
            iTextSharp.text.Table PdfTable = new iTextSharp.text.Table(2, pdfTableRows);
            PdfTable.BorderWidth = 1;
            PdfTable.Cellpadding = 0;
            PdfTable.Cellspacing = 0;
            for (int rowCounter = 0; rowCounter < rows; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < columns; columnCounter++)
                {
                    string strValue = dvGetStudent.Rows[rowCounter].Cells[columnCounter].Text;
                    PdfTable.AddCell(strValue);
                }
            }
            Document Doc = new Document();
            PdfWriter.GetInstance(Doc, HttpContext.Current.Response.OutputStream);
            Doc.Open();
            Doc.Add(PdfTable);
            Doc.Close();
            HttpContext.Current.Response.ContentType = "application/pdf";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=StudentDetails.pdf");
            HttpContext.Current.Response.End();
        }
    }
}