using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5
{
    public class Exercise
    {
        public int id;
        public string name;
        public int muscleGroupID;
        public int equipmentID;
        public int exerciseTypeID;
        public static TimeSpan duration = TimeSpan.Parse("00:00:45");
    }
}