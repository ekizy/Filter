using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;

namespace WebApplication5
{
    public partial class UserFilter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime datetime = new DateTime(2016,1,1);
            Calendar1.VisibleDate = datetime;
            if (!IsPostBack)
            {
                Label1.Visible = false;
                GridView1.Visible = false;

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            DateTime selected = Calendar1.SelectedDate;
            DateTime nextDay = selected.AddDays(1);
            Calendar1.Visible = false;
            Button1.Visible = false;
            Button3.Visible = false;

            Label2.Text = "Choosen Date is " + convertDateToDay(selected);
            Label2.Visible = true;
            Label2.Font.Size = FontUnit.XLarge;

            using (MySqlConnection con = new MySqlConnection(SqlDBConfig.connectionString))
            {
                con.Open();
                MySqlCommand cmd =con.CreateCommand();

                string query2 = "SELECT T.username as Person,T.start_date as Start,max(workoutexercises.end_date) as End, TIMESTAMPDIFF(MINUTE,T.start_date,max(workoutexercises.end_date)) as Length FROM (SELECT users.username,userworkouts.start_date,userworkouts.workout FROM userworkouts join users on users.id=userworkouts.user where userworkouts.start_date >='" + convertDateToString(selected) + "' and userworkouts.start_date <='" + convertDateToString(nextDay) + "') AS T join workoutexercises on workoutexercises.workout=T.workout GROUP by T.workout, T.username,T.start_date";
                
                cmd.CommandText = query2;

                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);

                GridView1.DataSource = ds.Tables[0];
                GridView1.DataBind();

                GridView1.HeaderRow.Cells[3].Text = "Length(Minutes)";

                foreach (GridViewRow row in GridView1.Rows)
                {
                    row.Cells[1].Text = convertDateToTime(DateTime.Parse(row.Cells[1].Text.ToString()));
                    row.Cells[2].Text = convertDateToTime(DateTime.Parse(row.Cells[2].Text.ToString()));
                }

                GridView1.Font.Size = FontUnit.XLarge;
                GridView1.Visible = true;
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            Calendar1.Visible = true;
        }

        public string convertDateToString(DateTime Date)
        {
            string a = Date.ToString("yyyy-MM-dd");

            return a + " 00:00:00";
        }

        public string convertDateToDay(DateTime Date)
        {
            return Date.ToString("dd/MM/yyyy");
        }



        public string convertDateToTime(DateTime date)
        {
            return date.ToString("HH:mm:ss");
        }

       private int getColIndex(GridView grid,string name) {
          foreach (DataControlField col in grid.Columns){
            if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim()){
                return grid.Columns.IndexOf(col);
            }
          }
          return -1;
      }


      
    }
}