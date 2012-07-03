using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Students
{
    public class ShowFoto:ITemplate
    {
        //System.Web.UI.WebControls.ListItemType templateType;
        private string fotoUrl;
        public ShowFoto(string fotoUrl)
        {
            this.fotoUrl = fotoUrl;
            //templateType = type;
        }
        public ShowFoto()
        {
            this.fotoUrl = "";
            //templateType = type;
        }
        public void InstantiateIn(Control c)
        {
            PlaceHolder ph = new PlaceHolder();
            if (fotoUrl != "")
            {
                Image img = new Image();
                img.ImageUrl = fotoUrl;
                img.Width = 200;
                c.Controls.Add(img);
                ph.Controls.Add(img);
            }
            else
            {
                ph.Controls.Add(new LiteralControl("No Foto"));
            }
            c.Controls.Add(ph);
        }
    }
}