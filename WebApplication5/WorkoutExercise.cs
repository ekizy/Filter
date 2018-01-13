using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication5
{
    public class WorkoutExercise
    {
        public int id;
        public int workoutID;
        public int exerciseID;
        public int exerciseOrder;
        public TimeSpan setTime = TimeSpan.Parse("00:02:30");
        public static int setTimeMinSeconds = 150;
        public static int setTimeMaxSeconds = 200;
        public int setTimeInSeconds;

        public string startDate;
        public string endDate;
        public TimeSpan startTime;
        public TimeSpan endTime;
        public static int setNumberMin = 2;
        public static int setNumberMax = 4;
        public int setNumber;

        public static int restTimeMaxSeconds = 60;
        public static int restTimeMinSeconds = 30;
        public int restInSeconds;
        public  TimeSpan restTime ;



        public WorkoutExercise(){
            Random rnd = new Random();
            setNumber = rnd.Next(setNumberMin, setNumberMax);
            restInSeconds = rnd.Next(restTimeMinSeconds,restTimeMaxSeconds+1);
            restTime = TimeSpan.FromSeconds(restInSeconds);
            setTimeInSeconds = rnd.Next(setTimeMinSeconds, setTimeMaxSeconds);
            setTime = TimeSpan.FromSeconds(setTimeInSeconds);

        }
    }
}