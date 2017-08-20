using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
namespace WebApplication5
{
    public partial class Equipments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlDBConfig dbConfig = new SqlDBConfig();
            dbConfig.connectToDB();
            MySqlCommand cmd = dbConfig.con.CreateCommand();
            cmd.CommandText = "SELECT * FROM EQUIPMENTS";
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            GridView1.DataSource = ds.Tables[0].DefaultView;
            GridView1.DataBind();
            dbConfig.breakConnection();

        }
    }
}