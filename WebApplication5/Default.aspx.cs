﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication5
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Generator gn = new Generator();
            Pattern ptn = gn.createPattern();
            Label1.Text = ptn.muscleGroups.ToString();
        }
    }
}