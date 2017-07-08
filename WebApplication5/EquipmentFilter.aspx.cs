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
    public partial class EquipmentFilter : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                MySqlConnection con = new MySqlConnection(ConfigurationClass.conString);
                con.Open();
                MySqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "SELECT ID,NAME FROM EQUIPMENTS;";
                MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adap.Fill(ds, "equipments");
                DropDownList1.DataSource = ds.Tables["equipments"];
                DropDownList1.DataTextField = "Name";
                DropDownList1.DataValueField = "ID";
                DropDownList1.DataBind();
                Label1.Visible = false;
                GridView1.Visible = false;
            }


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string equipment = DropDownList1.SelectedItem.Text;
            string start = "29.04.2017 "+tbstart.Text+":00";
            string end = "29.04.2017 "+tbend.Text+":00";
            MySqlConnection con = new MySqlConnection(ConfigurationClass.conString);
            con.Open(); //open con
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT users.username as Username,T4.name as Equipment ,T4.start_date as Beginning,T4.end_date as Finish " +
            "FROM users join (SELECT T3.user,equipments.name,T3.start_date,T3.end_date FROM equipments join (SELECT * FROM exercises JOIN " +
            " (SELECT workoutexercises.workout,workoutexercises.exercise,workoutexercises.exercise_order," +
            "workoutexercises.set_number,workoutexercises.set_time,t1.user,t1.start_date,workoutexercises.end_date FROM workoutexercises" +
            " join ( SELECT userworkouts.id as uswid,workouts.id as wid,userworkouts.user,userworkouts.workout,userworkouts.start_date,workouts.name" +
            " FROM userworkouts join workouts on workouts.id=userworkouts.workout) AS t1 on t1.workout=workoutexercises.workout) AS t2 on t2.exercise=exercises.id)" +
            " AS T3 ON T3.equipment=equipments.id) AS T4 ON T4.user=users.id where T4.name='" + equipment + "' and T4.start_date >='" + start + "' and T4.end_date <='" + end + "';";
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