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
            if (!IsPostBack)
            {
                Label1.Visible = false;
                GridView1.Visible = false;
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(SqlDBConfig.connectionString))
            {
                con.Open();
                string start = "29.04.2017 " + tbstart.Text + ":00";
                string end = "29.04.2017 " + tbend.Text + ":00";
                MySqlCommand cmd =con.CreateCommand();
                cmd.CommandText = "SELECT users.username,T2.workoutnumber,T2.beginning,T2.finish " +
                "FROM USERS JOIN (SELECT T1.user,workoutexercises.workout as workoutnumber,T1.start_date as beginning,max(end_date) as finish " +
                "FROM workoutexercises JOIN (SELECT userworkouts.id as uswid,workouts.id as wid,userworkouts.user,userworkouts.workout,userworkouts.start_date,workouts.name " +
                "FROM userworkouts join workouts on workouts.id=userworkouts.workout where userworkouts.start_date>='" + start + "' and userworkouts.start_date<='" + end + "' ) AS T1 ON workoutexercises.workout=T1.workout GROUP BY T1.user,workoutexercises.workout,T1.start_date) AS T2 ON T2.user=users.id";
                string command = cmd.CommandText;
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds);
                GridView1.DataSource = ds.Tables[0].DefaultView;
                GridView1.DataBind();
                GridView1.Visible = true;
            }
        }
    }
}