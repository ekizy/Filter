using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication5
{
    public partial class GenerateUserWorkout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Generator genarator = new Generator();

            string username = TextBox1.Text.ToString();
            string workoutname= TextBox2.Text.ToString();

            genarator.generateUserWorkout(username, workoutname);

        }
    }
}