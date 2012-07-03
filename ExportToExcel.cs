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

namespace Students
{
    public static class ExportToExcel
    {
        public static void Export(GridView gv, string filename)
        {
            Prepare(gv);
            string attachment = "attachment; filename=" + filename + ".xls";
            HttpContext.Current.Response.ClearContent();
            HttpContext.Current.Response.AddHeader("content-disposition", attachment);
            HttpContext.Current.Response.ContentType = "application/ms-excel";
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Write("<html><head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"/></head><body>");
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            //HtmlForm frm = new HtmlForm();
            //gv.Parent.Controls.Add(frm);
            //frm.Attributes["runat"] = "server";
            //frm.Controls.Add(gv);
            //frm.RenderControl(htw);
            gv.RenderControl(htw);
            HttpContext.Current.Response.Write(sw.ToString());
            HttpContext.Current.Response.Write("</body></html>");
            HttpContext.Current.Response.End(); 
        }
        private static void Prepare(GridView gv)
        {
            if (gv.HeaderRow != null)
            {
                PrepareControlForExport(gv.HeaderRow);
            }
            foreach (GridViewRow row in gv.Rows)
            {
                PrepareControlForExport(row);
            }
            if (gv.FooterRow != null)
            {
                PrepareControlForExport(gv.FooterRow);
            }
        }
        private static void PrepareControlForExport(Control control)
        {

            for (int i = 0; i < control.Controls.Count; i++)
            {
                Control current = control.Controls[i];
                if (current is LinkButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                }
                else if (current is ImageButton)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                }
                else if (current is HyperLink)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                }
                else if (current is DropDownList)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                }
                else if (current is CheckBox)
                {
                    control.Controls.Remove(current);
                    control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                }

                if (current.HasControls())
                {
                    PrepareControlForExport(current);
                }
            }

        }
    }

}