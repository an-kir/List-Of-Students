using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;

namespace Students
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.Text = "Введите логин и пароль для доступа к спискам студентов:";
            
        }
        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            if (IsLiginAndPasswordCorrect())
                Server.Transfer("~/index.aspx");
                //FormsAuthentication.RedirectFromLoginPage(txtLogin.Text, true);
        }

        private bool IsLiginAndPasswordCorrect()
        {
            string login = WebConfigurationManager.AppSettings["login"];
            string password = WebConfigurationManager.AppSettings["password"];
            if (txtLogin.Text == "" && txtPassword.Text == "")
            {
                lblErr.Visible = true;
                lblErr.Text = "Поля логина и пароля не заполнены! Введите логин и пароль";
            }
            else
            {
                if (txtLogin.Text == "" || txtPassword.Text == "")
                {
                    if (txtLogin.Text == "")
                    {
                        lblErr.Visible = true;
                        lblErr.Text = "Поле логина не заполнено! Введите логин";
                    }
                    if (txtPassword.Text == "")
                    {
                        lblErr.Visible = true;
                        lblErr.Text = "Поле пароля не заполнено! Введите пароль";
                    }
                }
                else
                    if (login == txtLogin.Text && password == txtPassword.Text)
                    {
                        return true;
                    }
                    else
                    {
                        lblErr.Visible = true;
                        lblErr.Text = "Логин или пароль введены неверно";
                        
                    }
            }
            return false;
        }
    }
}