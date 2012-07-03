using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp;
using System.Web.Script.Serialization;

namespace Students
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string typeOfDB = WebConfigurationManager.AppSettings["keyDB"];
            string typeOfDB = ddlListDB.SelectedValue.ToString();
            switch (typeOfDB.ToUpper())
            {
                case "SQL":
                    odsAllStudents.TypeName = "Students.StudentDB";
                    odsGetOneStudent.TypeName = "Students.StudentDB";
                    break;
                case "XML":
                    odsAllStudents.TypeName = "Students.XmlStudentDB";
                    odsGetOneStudent.TypeName = "Students.XmlStudentDB";
                    break;
                case "JSON":
                    odsAllStudents.TypeName = "Students.JsonStudentDB";
                    odsGetOneStudent.TypeName = "Students.JsonStudentDB";
                    break;
            }
            //if (GridView2.SelectedIndex == -1)
            //    ViewState["RowIndex"] = 0;
        }
        protected void ddlListDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideControls();
            GridView2.SelectedIndex = -1;
            GridView2.DataBind();
        }
        protected void DetailsView2_OnItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            FileUpload fu = (FileUpload)DetailsView2.FindControl("uploadFoto");
            if (fu.PostedFile.ContentLength != 0)
            {
                string path = Server.MapPath(@"\Foto");
                path += "\\Temp.jpg";
                fu.PostedFile.SaveAs(path);
            }
            //ClientScript.RegisterStartupScript(
        }


        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {

            dvGetStudent.Visible = true;
            btnExportToPdf.Visible = true;
            string id = (string)GridView2.SelectedDataKey.Values["StudentID"].ToString();
            int studentID = Convert.ToInt32(id);
            odsGetOneStudent.SelectParameters.Clear();
            odsGetOneStudent.SelectParameters.Add("StudentID", studentID.ToString());

            dvGetStudent.Fields.Clear();
            //dvGetStudent.DataSourceID = "odsGetOneStudent";
            dvGetStudent.AutoGenerateRows = false;

            #region создание полей dvGetStudent
            string fotoUrlFromDB = "";
            try
            {
                fotoUrlFromDB = (string)GridView2.SelectedDataKey.Values["Foto"];
            }
            catch
            {
                //попадает в catch, если foto = null
            }
            if (fotoUrlFromDB != "")
            {
                fotoUrlFromDB = fotoUrlFromDB.Trim();
                if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + fotoUrlFromDB))
                {
                    TemplateField foto = new TemplateField();
                    foto.ItemTemplate = new ShowFoto(fotoUrlFromDB);
                    dvGetStudent.Fields.Add(foto);
                }
            }
            BoundField firstName = new BoundField();
            firstName.DataField = "FirstName";
            firstName.HeaderText = "First Name";
            dvGetStudent.Fields.Add(firstName);
            BoundField secondName = new BoundField();
            secondName.DataField = "SecondName";
            secondName.HeaderText = "Second Name";
            dvGetStudent.Fields.Add(secondName);
            BoundField dateOfBirth = new BoundField();
            dateOfBirth.DataField = "DateOfBirth";
            dateOfBirth.HeaderText = "Date Of Birth";
            dvGetStudent.Fields.Add(dateOfBirth);
            #endregion

            #region мусор
            //form1.Controls.Add(dv);
            //dvGetStudent.DataBind();

            //доделать экспорт в pdf без клика по кнопке
            //CreateChildControls();

            //Button btnExportToPdf = new Button();
            //btnExportToPdf.Text = "Export To PDF";
            //btnExportToPdf.Click += new EventHandler(btnExportToPdf_Click);
            //btnExportToPdf.Attributes["runat"] = "server";
            ////btnExportToPdf.Attributes["onclick"] = "btnExportToPdf_Click";
            //form1.Controls.Add(btnExportToPdf);
            ////PlaceHolder.Controls.Add(btnExport);
            ////ViewState["btnExportToPdf"] = btnExportToPdf;
            ////if(btnExport.Click)

            //btnExport_Click(btnExport, null);
            #endregion

            fuFoto.Visible = false;
            int prevIndex = 0;
            try
            {
                prevIndex = (int)ViewState["RowIndex"];
            }
            catch
            {
            }
            if (prevIndex == GridView2.SelectedIndex)
            {
                GridView2.SelectedIndex = -1;
                HideControls();
            }
            ViewState["RowIndex"] = GridView2.SelectedIndex;
            //Response.Write("</br>" + GridView2.SelectedIndex);
        }
        protected void btnExportToPdf_Click(object sender, EventArgs e)
        {
            Response.Write("----btnExportToPdf_Click----");
            int rows = dvGetStudent.Rows.Count;
            int columns = dvGetStudent.Rows[0].Cells.Count;
            int pdfTableRows = rows + 3;
            iTextSharp.text.Table PdfTable = new iTextSharp.text.Table(2, pdfTableRows);
            PdfTable.BorderWidth = 1;
            //PdfTable.BorderColor = new Color(0, 0, 255);
            PdfTable.Cellpadding = 5;
            PdfTable.Cellspacing = 5;
            Cell c1 = new Cell("Export Or Create PDF From DetailsView In Asp.Net");
            c1.Header = true;
            c1.Colspan = 2;
            PdfTable.AddCell(c1);
            Cell c2 = new Cell("By CsharpAspNetArticles.com");
            c2.Colspan = 2;
            PdfTable.AddCell(c2);
            for (int rowCounter = 0; rowCounter < rows; rowCounter++)
            {
                for (int columnCounter = 0; columnCounter < columns; columnCounter++)
                {
                    string strValue = dvGetStudent.Rows[rowCounter].Cells[columnCounter].Text;
                    PdfTable.AddCell(strValue);
                }
            }
            Document Doc = new Document();
            PdfWriter.GetInstance(Doc, Response.OutputStream);
            Doc.Open();
            Doc.Add(PdfTable);
            Doc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=CsharpAspNetArticles.pdf");
            Response.End();
            
        }
        protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
        {
            HideControls();
            fuFoto.Visible = true;
            GridView2.SelectedIndex = e.NewEditIndex;
        }
        protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (fuFoto.PostedFile.ContentLength != 0)
            {
                string path = Server.MapPath(@"\Foto");
                path += "\\Temp.jpg";
                fuFoto.PostedFile.SaveAs(path);
            }
            fuFoto.Visible = false;
        }
        protected void GridView2_RowUpdated(object sender, EventArgs e)
        {
            GridView2.SelectedIndex = -1;
        }
        protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView2.SelectedIndex = -1;
            fuFoto.Visible = false;
        }
        protected void GridView2_RowDeleting(object sebder, EventArgs e)
        {
            HideControls();
            // добавить подтверждение удаления
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            HideControls();
            GridView2.SelectedIndex = -1;
            GridView gv = GridView2;
            gv.Columns[0].Visible = false;
            gv.Columns[1].Visible = false;
            gv.Columns[2].Visible = false;
            ExportToExcel.Export(gv,"List Of Students");
        }
        private void HideControls()
        {
            dvGetStudent.Visible = false;
            btnExportToPdf.Visible = false;
        }
        public override void VerifyRenderingInServerForm(Control control)
        { }
    }
}



