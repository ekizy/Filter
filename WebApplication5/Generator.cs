using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
namespace WebApplication5
{
    public class Generator
    {
            public SqlDBConfig dbConfig;
            public Random rnd;
            public static string beginningDay = "29.04.2017";
            public static  int dayIntervalLow =2;
            public static int dayIntervalHigh = 5;
            public static int hourMin = 9;
            public static int hourMax = 22;
            public static int exerciseNumberMin = 7;
            public static int exerciseNumberMax = 9;
            public static int minMuscleGroup = 5;
            public static int maxMuscleGroup = 6;
            public static int minExerciseMuscleGroup = 2;
            public static int maxExerciseMuscleGroup = 3;
            
            public static DateTime start = new DateTime(2016, 1, 1);
            public static DateTime end = new DateTime(2017, 1, 1);
            public static DateTime error = new DateTime(2013, 1, 1);
            public Generator()
            {
                dbConfig = new SqlDBConfig();
                rnd = new Random();
            }

        //Bütün fonksiyonlardaki inputlar textboxdan alınacak.Consoledan değil.
            public void generateUser(string username)
            {
                dbConfig.connectToDB();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbConfig.con;
                string query = "INSERT INTO USERS (USERNAME) VALUES ('" + username + "');";
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                dbConfig.breakConnection();

            }

            public void generateWorkout(string workoutname)
            {
                dbConfig = new SqlDBConfig();
                dbConfig.connectToDB();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbConfig.con;
                string query = "INSERT INTO WORKOUTS (NAME) VALUES('" + workoutname + "');";
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                dbConfig.breakConnection();
            }

            public void generateUserWorkout(string username,string workoutname)
            {
                try
                {
                    /* List<int> userIDList = dbConfig.getUserIDs();
                     int randomIndex = rnd.Next(userIDList.Count);
                     int randomUserID = userIDList[randomIndex];

                     List<int> workoutIDList = dbConfig.getWorkoutIDs();
                     randomIndex = rnd.Next(workoutIDList.Count);
                     int randomWorkoutID = workoutIDList[randomIndex]; random seçilme senaryosu şimdilik yorum*/

                    generateUser(username);
                    generateWorkout(workoutname);

                    List<int> workoutList = dbConfig.getWorkoutIDs();
                    int workoutID = workoutList.Max();

                    List<int> userList = dbConfig.getUserIDs();
                    int userID = userList.Max();

                    Console.WriteLine("User Id:" + userID + "\nWorkout Id:" + workoutID);

                    TimeSpan start = TimeSpan.FromHours(hourMin);
                    TimeSpan end = TimeSpan.FromHours(hourMax);
                    int maxMinutes = (int)((end - start).TotalMinutes);

                    int minutes = rnd.Next(maxMinutes);
                    TimeSpan workoutStartTime = start.Add(TimeSpan.FromMinutes(minutes));

                    String randomStartDate = beginningDay + " " + workoutStartTime.ToString();

                    Console.WriteLine("Start Date: " + randomStartDate + "\n");

                    UserWorkout userWorkout = new UserWorkout();
                    userWorkout.workoutID = workoutID;
                    userWorkout.userID = userID;
                    userWorkout.startDate = randomStartDate;
                    userWorkout.startTime = workoutStartTime;

                    dbConfig.connectToDB();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = dbConfig.con;
                    string query = "INSERT INTO USERWORKOUTS (user,workout,start_date) VALUES ("
                        + userWorkout.userID.ToString() + "," + userWorkout.workoutID.ToString()
                        + ",'" + userWorkout.startDate + "');";

                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();

                    dbConfig.breakConnection();

                    generateWorkoutExercises(userWorkout);
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Null Reference Exception occured.");
                }


            }

            public void generateWorkoutExercises(UserWorkout userWorkout)
            {
                List<int> exerciseIDList = dbConfig.getExerciseIDs();
                TimeSpan time = userWorkout.startTime;
                int order = 1;

                dbConfig.connectToDB();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = dbConfig.con;

                for (int i = 0; i < 5; i++)
                {
                    int randomIndex = rnd.Next(exerciseIDList.Count);
                    int randomExerciseID = exerciseIDList[randomIndex];

                    WorkoutExercise workoutExercise = new WorkoutExercise();
                    workoutExercise.exerciseID = randomExerciseID;
                    workoutExercise.workoutID = userWorkout.workoutID;
                    workoutExercise.startDate = beginningDay + " " + time.ToString();
                    workoutExercise.startTime = time;
                    workoutExercise.endTime = time
                        + calculateExerciseTime(WorkoutExercise.setNumber, WorkoutExercise.setTime);
                    workoutExercise.endDate = beginningDay + " " + workoutExercise.endTime.ToString();
                    workoutExercise.exerciseOrder = order;

                    order++;
                    exerciseIDList.Remove(randomExerciseID);
                    time = workoutExercise.endTime + WorkoutExercise.restTime;


                    string query = "INSERT INTO WORKOUTEXERCISES" + " (workout,exercise,exercise_order,"
                        + "set_number,set_time,"
                        + "start_date,end_date) VALUES ("
                        + workoutExercise.workoutID + "," + workoutExercise.exerciseID + "," + workoutExercise.exerciseOrder
                        + "," + WorkoutExercise.setNumber + "," + WorkoutExercise.setTimeSeconds + ",'" + workoutExercise.startDate
                        + "','" + workoutExercise.endDate + "');";

                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();



                }
                dbConfig.breakConnection();
            }

            public TimeSpan calculateExerciseTime(int setNumber, TimeSpan setTime)
            {
                TimeSpan exerciseTime = TimeSpan.Parse("00:00:00");
                for (int i = 0; i < setNumber; i++)
                {
                    exerciseTime = exerciseTime + setTime;
                }

                return exerciseTime;
            }

            public string randomDateTime()
            {
                DateTime start = new DateTime(2016,1,1);
                DateTime end = new DateTime(2017, 1, 1);
                int range = (end - start).Days;
                DateTime randomDate = start.AddDays(rnd.Next(range));
               // DateTime randomDate = start.AddDays(1);
                string date = randomDate.ToString("dd.MM.yyyy");
                return date;
                
            }
             public DateTime nextWorkoutDate(DateTime lastWorkout)
            {
                int days = rnd.Next(dayIntervalLow, dayIntervalHigh);
                DateTime nextWorkout = lastWorkout.AddDays(days);
                if (DateTime.Compare(nextWorkout, end) > 0) return error;
                else return nextWorkout;
            }

            public string convertDateToString(DateTime Date)
            {
                return Date.ToString("dd.MM.yyyy");
            }

            public void generateBigData()
            {
                List<int> userList = dbConfig.getUserIDs();

                for (int i = 0; i < userList.Count;i++ )
                {
                    int userID=userList[i];
                    generateWorkoutsForUser(userID);
                }
                return;

            }
            
            public void generateWorkoutsForUser(int userID)
             {
                 DateTime calendarStart = Generator.start;
                 DateTime calendarEnd = Generator.end;

                 DateTime monthStart = calendarStart;
                 DateTime monthEnd = calendarStart.AddDays(30);

                while(DateTime.Compare(monthEnd,calendarEnd)<0)
                {
                    Pattern ptn =createPattern();
                    monthStart=generateMonthlyWorkouts(ptn, userID, monthStart, monthEnd);
                    monthEnd = calendarStart.AddDays(30);
                }
                
                return;
             }

            public DateTime generateMonthlyWorkouts(Pattern ptn,int userID,DateTime start,DateTime end)
            {
                DateTime nextDate = nextWorkoutDate(start);
                while (DateTime.Compare(nextDate, error) > 0 && DateTime.Compare(nextDate, end) <= 0)
                {
                    // haftalık workoutlar generate et.
                    generateWorkout("workout1");
                    nextDate = nextWorkoutDate(nextDate);
                }
                return nextDate;
            }

            public Pattern createPattern()
            {
                List<int> muscleGroups = dbConfig.getMuscleGroupIDs();

                int numberOfMuscleGroups = minMuscleGroup + rnd.Next(maxMuscleGroup - minMuscleGroup + 1);

                muscleGroups.Remove(7);

                Pattern ptn = new Pattern();
                List<int> tempList = new List<int>();

                for (int i = 0; i < numberOfMuscleGroups;i++ )
                {
                    int index = rnd.Next(muscleGroups.Count);
                    //ptn.muscleGroups.Add(muscleGroups[index]);
                    tempList.Add(muscleGroups[index]);
                    muscleGroups.RemoveAt(index);
                }
                ptn.muscleGroups = tempList;
                return ptn;
            }
            
            public void generateWOut(DateTime startDate,int userID,List<int> muscleGroupIDs)
            {

                return;
            }
    }


    }
