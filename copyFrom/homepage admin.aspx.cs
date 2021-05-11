using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Online_grades.webpages
{
    public partial class WebForm3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Label1.Text = "Accounting Grades";
            ShowYearButtons();
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            Label1.Text = "Industrial Engineering";
            ShowYearButtons();
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
        {
            Label1.Text = "Accountancy";
            ShowYearButtons();
        }

        protected void LinkButton4_Click(object sender, EventArgs e)
        {
            Label1.Text = "Electronics Engineering";
            ShowYearButtons();
        }

        protected void LinkButton5_Click(object sender, EventArgs e)
        {
            Label1.Text = "Admin Information Table";
            HideYearButtons();
        }

        protected void LinkButton6_Click(object sender, EventArgs e)
        {
            Label1.Text = "Student Information Table";
            HideYearButtons();
        }



        private void ShowYearButtons()
        {
            Button1.Visible = true;
            Button2.Visible = true;
            Button3.Visible = true;
            Button4.Visible = true;
            Label2.Visible = true;
            TextBox1.Visible = true;
            Button5.Visible = true;
        }



        private void HideYearButtons()
        {
            Button1.Visible = false;
            Button2.Visible = false;
            Button3.Visible = false;
            Button4.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            Button5.Visible = false;
        }

       
    }
}