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
using System.Xml.Linq;
using System.Text;
using System.Text.RegularExpressions;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp;
using System.Web.Script.Serialization;
using System.ComponentModel;

namespace Students
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                ViewState["RowIndex"] = -1;
            //string typeOfDB = WebConfigurationManager.AppSettings["keyDB"];

            //DateTime dt = DateTime.Now;
            
            //string str = new String("{0,-35:D}", dt);
            //lblMyHid.Visible = true;
            //lblMyHid.Text = String.Format("{0:d MM yyyy}", dt);
            //Response.Write(dt.Year + "-" + dt.Month + "-" + dt.Day);
            string typeOfDB = ddlListDB.SelectedValue.ToString();
            switch (typeOfDB.ToUpper())
            {
                case "SQL":
                    odsAllStudents.TypeName = "Students.SqlStudentDB";
                    odsGetOneStudent.TypeName = "Students.SqlStudentDB";
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

        }
        protected void ddlListDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideControls();
            gvAllStudents.SelectedIndex = -1;
            gvAllStudents.DataBind();
        }
        protected void dvInsertStudent_OnItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            if (ValidateOnInsertingDetailsView(e))
            {
                FileUpload fu = (FileUpload)dvInsertStudent.FindControl("uploadFoto");
                if (fu.PostedFile.ContentLength != 0)
                {
                    string path = Server.MapPath(@"\Foto");
                    path += "\\Temp.jpg";
                    fu.PostedFile.SaveAs(path);
                }
            }
            //ClientScript.RegisterStartupScript(
        }
        private bool ValidateOnInsertingDetailsView(DetailsViewInsertEventArgs e)
        {
            Label lblEr = new Label();
            lblEr.ForeColor = System.Drawing.Color.Red;
            
            Regex regName = new Regex("[^a-zA-Zа-яА-Я]");
            Regex regDate = new Regex("[^0-9./-]");
            if (e.Values[0] == null || e.Values[0].ToString() == "")
            {
                e.Cancel = true;
                lblEr.Text += "Вы не ввели имя студента. Введите имя;</br>";
            }
            else
                if (regName.IsMatch(e.Values[0].ToString()))
                {
                    e.Cancel = true;
                    lblEr.Text += "Имя может содержать только буквы русского и латинского алфавитов;</br>";
                }
            if (e.Values[1] == null || e.Values[1].ToString() == "")
            {
                e.Cancel = true;
                lblEr.Text += "Вы не ввели фамилию студента. Введите фамилию;</br>";
            }
            else
                if (regName.IsMatch(e.Values[1].ToString()))
                {
                    e.Cancel = true;
                    lblEr.Text += "Фамилия студента может содержать только буквы русского и латинского алфавитов;</br>";
                }
            if (e.Values[2] != null)
                if (regDate.IsMatch(e.Values[2].ToString()))
                {
                    e.Cancel = true;
                    lblEr.Text += @"Дата рождения может состоять только из цифр и знаков '-', '/' , '.'";
                    lblEr.Text += "</br>";
                }
            if (lblEr.Text != "")
            {
                form1.Controls.AddAt(form1.Controls.IndexOf(dvInsertStudent) + 1, lblEr);
                return false;
            }
            else
                return true;
        }
        
        protected void gvAllStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            dvGetStudent.Visible = true;
            dvGetStudent.DataBind();
            btnExportToPdf.Visible = true;
            string id = (string)gvAllStudents.SelectedDataKey.Values["StudentID"].ToString();
            int studentID = Convert.ToInt32(id);
            odsGetOneStudent.SelectParameters.Clear();
            odsGetOneStudent.SelectParameters.Add("StudentID", studentID.ToString());

            int numberOfFiels = dvInsertStudent.Fields.Count - 1;
            if (dvGetStudent.Fields.Count == numberOfFiels)
                dvGetStudent.Fields.RemoveAt(0);
            //dvGetStudent.Fields.Clear();
            dvGetStudent.AutoGenerateRows = false;

            string fotoUrlFromDB = "";
            try
            {
                fotoUrlFromDB = (string)gvAllStudents.SelectedDataKey.Values["Foto"];
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
                    dvGetStudent.Fields.Insert(0, foto);
                }
            }

            fuFoto.Visible = false;
            DeselectRow();
        }
        private void DeselectRow()
        {

            int prevIndex = 0;
            try
            {
                prevIndex = (int)ViewState["RowIndex"];
            }
            catch
            {
            }
            if (prevIndex == gvAllStudents.SelectedIndex)
            {
                gvAllStudents.SelectedIndex = -1;
                HideControls();
            }

            ViewState["RowIndex"] = gvAllStudents.SelectedIndex;
        }
        protected void btnExportToPdf_Click(object sender, EventArgs e)
        {
            //Response.Write("----btnExportToPdf_Click----");
            string fotoUrlFromDB = "";
            try
            {
                fotoUrlFromDB = (string)gvAllStudents.SelectedDataKey.Values["Foto"];
            }
            catch
            {
                //попадает в catch, если foto = null
            }
            if (fotoUrlFromDB != "")
            {
                fotoUrlFromDB = System.AppDomain.CurrentDomain.BaseDirectory + fotoUrlFromDB.Trim();
                if (File.Exists(fotoUrlFromDB))
                {
                    ExportToPdf.Export(dvGetStudent, fotoUrlFromDB, "StudentsPDF");
                }
            }
            else
                ExportToPdf.Export(dvGetStudent, "StudentsPDF");
            
        }
        protected void gvAllStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            
            HideControls();
            fuFoto.Visible = true;
            gvAllStudents.SelectedIndex = e.NewEditIndex;
        }
        protected void gvAllStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (ValidateOnUpdateGridView(e))
            {
                if (fuFoto.PostedFile != null && fuFoto.PostedFile.ContentLength != 0)
                {
                    string path = Server.MapPath(@"\Foto");
                    path += "\\Temp.jpg";
                    fuFoto.PostedFile.SaveAs(path);
                }
            }
            fuFoto.Visible = false;
        }
        private bool ValidateOnUpdateGridView(GridViewUpdateEventArgs e)
        {
            Label lblEr = new Label();
            lblEr.ForeColor = System.Drawing.Color.Red;
            lblEr.Text = "";

            for (int i = 0; i <= 2; i++)
            {
                if (e.NewValues[i] != null)
                    e.NewValues[i] = e.NewValues[i].ToString().Trim();
            }

            Regex regName = new Regex("[^a-zA-Zа-яА-Я]");
            Regex regDate = new Regex("[^0-9./-]");
            if (e.NewValues[0] == null || e.NewValues[0].ToString() == "")
            {
                e.Cancel = true;
                lblEr.Text += "Вы не ввели имя студента. Введите имя;</br>";
            }
            else
                if (regName.IsMatch(e.NewValues[0].ToString()))
                {
                    e.Cancel = true;
                    lblEr.Text += "Имя может содержать только буквы русского и латинского алфавитов;</br>";
                }
            if (e.NewValues[1] == null || e.NewValues[1].ToString() == "")
            {
                e.Cancel = true;
                lblEr.Text += "Вы не ввели фамилию студента. Введите фамилию;</br>";
            }
            else
                if (regName.IsMatch(e.NewValues[1].ToString()))
                {
                    e.Cancel = true;
                    lblEr.Text += "Фамилия студента может содержать только буквы русского и латинского алфавитов;</br>";
                }
            if (e.NewValues[2] != null)
                if (regDate.IsMatch(e.NewValues[2].ToString()))
                {
                    e.Cancel = true;
                    lblEr.Text += @"Дата рождения может состоять только из цифр и знаков '-', '/' , '.'";
                    lblEr.Text += "</br>";
                }
            if (lblEr.Text != "")
            {
                form1.Controls.AddAt(form1.Controls.IndexOf(gvAllStudents) - 1, lblEr);
                return false;
            }
            else 
                return true;
        }
        protected void gvAllStudents_RowUpdated(object sender, EventArgs e)
        {
            gvAllStudents.SelectedIndex = -1;
        }
        protected void gvAllStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAllStudents.SelectedIndex = -1;
            fuFoto.Visible = false;
        }
        protected void gvAllStudents_RowDeleting(object sebder, EventArgs e)
        {
            HideControls();
            
            // добавить подтверждение удаления
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            HideControls();
            gvAllStudents.SelectedIndex = -1;
            GridView gv = gvAllStudents;
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



