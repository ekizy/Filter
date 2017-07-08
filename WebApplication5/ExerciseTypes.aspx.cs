﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;
namespace WebApplication5
{
    public partial class ExerciseTypes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string myConnectionString = ConfigurationClass.conString;
            MySqlConnection con = new MySqlConnection(myConnectionString);
            con.Open();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandText = "SELECT * FROM EXERCISETYPES";
            MySqlDataAdapter adap = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adap.Fill(ds);
            GridView1.DataSource = ds.Tables[0].DefaultView;
            GridView1.DataBind();
        }
    }
}