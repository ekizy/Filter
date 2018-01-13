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
            public UserGenerator userGenerator;
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
            public static int cardioExerciseNumber = 2;

            public static int dumbellQuantity = 3;
            public static int ropeQuantity = 2;
            public static int minQuantity = 1;
            public static int maxQuantity = 2;


            public static int userNumber=1000;
            
            public static DateTime start = new DateTime(2016, 1, 1);
            public static DateTime end = new DateTime(2017, 1, 1);
            public static DateTime error = new DateTime(2013, 1, 1);
            public Generator()
            {
                dbConfig = new SqlDBConfig();
                rnd = new Random();
                userGenerator = new UserGenerator();
            }

        //Bütün fonksiyonlardaki inputlar textboxdan alınacak.Consoledan değil.
            public void generateUsers()
            {
                using (MySqlConnection con = new MySqlConnection(SqlDBConfig.connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;

                    for (int i = 0; i < userNumber; i++)
                    {
                        string username = userGenerator.generateRandomUser();
                        string query = "INSERT INTO USERS (USERNAME) VALUES ('" + username + "');";
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }

                }



            }

            public int createWorkout()
            {
                using (MySqlConnection con = new MySqlConnection(SqlDBConfig.connectionString))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    int workoutNumber = 0;
                    int workoutID = 0;
                    string selectQuery = "SELECT COUNT(ID),MAX(ID) FROM WORKOUTS;";
                    cmd.CommandText = selectQuery;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int numberOfRows = reader.GetInt32(0);
                        if (numberOfRows == 0) workoutNumber = 1;
                        else workoutNumber = reader.GetInt32(1) + 1;
                    }
                    reader.Close();
                    string workoutname = "Workout " + workoutNumber;
                    string query = "INSERT INTO WORKOUTS (NAME) VALUES('" + workoutname + "');";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();

                    string idQuery = "SELECT MAX(ID) FROM WORKOUTS;";
                    cmd.CommandText = idQuery;
                    workoutID = (int)cmd.ExecuteScalar();



                    return workoutID;
                }
            }

            public UserWorkout generateUserWorkout(int userID,int workoutID,DateTime day)
            {
                try
                {
                    TimeSpan start = TimeSpan.FromHours(hourMin);
                    TimeSpan end = TimeSpan.FromHours(hourMax);
                    int maxMinutes = (int)((end - start).TotalMinutes);

                    string Day = convertDateToString(day);

                    int minutes = rnd.Next(maxMinutes);
                    TimeSpan workoutStartTime = start.Add(TimeSpan.FromMinutes(minutes));

                    String randomStartDate = Day + " " + workoutStartTime.ToString();

                    Console.WriteLine("Start Date: " + randomStartDate + "\n");

                    UserWorkout userWorkout = new UserWorkout();
                    userWorkout.workoutID = workoutID;
                    userWorkout.userID = userID;
                    userWorkout.startDate = Day;
                    userWorkout.startTime = workoutStartTime;

                    using(MySqlConnection con=new MySqlConnection(SqlDBConfig.connectionString))
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = con;
                        string query = "INSERT INTO USERWORKOUTS (user,workout,start_date) VALUES ("
                            + userWorkout.userID.ToString() + "," + userWorkout.workoutID.ToString()
                            + ",'" + randomStartDate + "');";

                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                    }

                    return userWorkout;
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine("Null Reference Exception occured.");
                    return null;
                }


            }

            public void generateWorkoutExercises(UserWorkout userWorkout, ref List<int> muscleGroupIDs,List<int> subMuscleGroups)
            {
                List<int> exerciseIDList = dbConfig.getExerciseIDs();
                TimeSpan time = userWorkout.startTime;
                int order = 1;

               
                int exerciseNumber = exerciseNumberMin + rnd.Next(exerciseNumberMax - exerciseNumberMin + 1);

                int i = 0;

                while ( i < exerciseNumber)
                {

                    using(MySqlConnection con=new MySqlConnection(SqlDBConfig.connectionString))
                    {
                        List<int> previousCardioExercises = new List<int>();
                        if (i < cardioExerciseNumber)
                        {
                            con.Open();
                            MySqlCommand cmd = new MySqlCommand();
                            cmd.Connection = con;
                            WorkoutExercise workoutExercise = new WorkoutExercise();
                            workoutExercise.workoutID = userWorkout.workoutID;
                            workoutExercise.startDate = userWorkout.startDate + " " + time.ToString();
                            workoutExercise.startTime = time;
                            workoutExercise.setNumber = 1;
                            workoutExercise.setTime = TimeSpan.FromMinutes(rnd.Next(10, 15));
                            workoutExercise.setTimeInSeconds = Convert.ToInt32(workoutExercise.setTime.TotalSeconds);
                            workoutExercise.endTime = time
                                + calculateExerciseTime(workoutExercise.setNumber, workoutExercise.setTime);
                            workoutExercise.endDate = userWorkout.startDate + " " + workoutExercise.endTime.ToString();
                            workoutExercise.exerciseOrder = order;



                            workoutExercise.exerciseID = chooseCardioExercise();

                            bool found = false;

                            if (previousCardioExercises.Count != 0)
                            {
                                for (int a = 0; a < previousCardioExercises.Count; a++)
                                {
                                    if (workoutExercise.exerciseID == previousCardioExercises[a])
                                        found = true;

                                }
                            }
                            if (found) continue;
                            else previousCardioExercises.Add(workoutExercise.exerciseID);

                            i++;

                            order++;
                            time = workoutExercise.endTime + workoutExercise.restTime;


                            string query = "INSERT INTO WORKOUTEXERCISES" + " (workout,exercise,exercise_order,"
                                + "set_number,set_time,"
                                + "start_date,end_date) VALUES ("
                                + workoutExercise.workoutID + "," + workoutExercise.exerciseID + "," + workoutExercise.exerciseOrder
                                + "," + workoutExercise.setNumber + "," + workoutExercise.setTimeInSeconds + ",'" + workoutExercise.startDate
                                + "','" + workoutExercise.endDate + "');";

                            cmd.CommandText = query;
                            cmd.ExecuteNonQuery();

                        }
                        else
                        {

                            con.Open();
                            int times = minExerciseMuscleGroup + rnd.Next(maxExerciseMuscleGroup - minExerciseMuscleGroup + 1);
                            int j = 0;

                            if (muscleGroupIDs.Count == 0)
                                for (int k = 0; k < subMuscleGroups.Count; k++) muscleGroupIDs.Add(subMuscleGroups[k]);

                            int muscleGroup = muscleGroupIDs[rnd.Next(muscleGroupIDs.Count)];

                            muscleGroupIDs.Remove(muscleGroup);
                            List<int> previousExercises = new List<int>();
                            while (j < times && i < exerciseNumber)
                            {

                               

                                MySqlCommand cmd = new MySqlCommand();
                                cmd.Connection = con;

                                WorkoutExercise workoutExercise = new WorkoutExercise();
                                workoutExercise.workoutID = userWorkout.workoutID;
                                workoutExercise.startDate = userWorkout.startDate + " " + time.ToString();
                                workoutExercise.startTime = time;
                                workoutExercise.endTime = time
                                    + calculateExerciseTime(workoutExercise.setNumber, workoutExercise.setTime);
                                workoutExercise.endDate = userWorkout.startDate + " " + workoutExercise.endTime.ToString();
                                workoutExercise.exerciseOrder = order;

                                workoutExercise.exerciseID = chooseExercise(muscleGroup);

                                bool found = false;

                                if (previousExercises.Count != 0)
                                {
                                    for(int a=0;a<previousExercises.Count;a++)
                                    {
                                        if (workoutExercise.exerciseID == previousExercises[a])
                                            found = true;
                                            
                                    }
                                }
                                if (found) continue;
                                else previousExercises.Add(workoutExercise.exerciseID);

                                i++;
                                j++;

                                order++;
                                time = workoutExercise.endTime + workoutExercise.restTime;


                                string query = "INSERT INTO WORKOUTEXERCISES" + " (workout,exercise,exercise_order,"
                                    + "set_number,set_time,"
                                    + "start_date,end_date) VALUES ("
                                    + workoutExercise.workoutID + "," + workoutExercise.exerciseID + "," + workoutExercise.exerciseOrder
                                    + "," + workoutExercise.setNumber + "," + workoutExercise.setTimeInSeconds + ",'" + workoutExercise.startDate
                                    + "','" + workoutExercise.endDate + "');";

                                cmd.CommandText = query;
                                cmd.ExecuteNonQuery();
                            }
                    }
                    

                       

                    }




                }
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
                int days = dayIntervalLow+ rnd.Next(dayIntervalHigh-dayIntervalLow +1);
                DateTime nextWorkout = lastWorkout.AddDays(days);
                if (DateTime.Compare(nextWorkout, end) > 0) return error;
                else return nextWorkout;
            }

            public string convertDateToString(DateTime Date)
            {
                return Date.ToString("yyyy-MM-dd");
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
                    monthEnd = monthStart.AddDays(30);
                }
                
                return;
             }

            public DateTime generateMonthlyWorkouts(Pattern ptn,int userID,DateTime start,DateTime end)
            {
                DateTime nextDate = nextWorkoutDate(start);
                List<int> muscleGroups = new List<int>();
                List<int> subMuscleGroups = new List<int>();

                DateTime possibleDate=new DateTime();

                for (int i = 0; i < ptn.muscleGroups.Count; i++)
                {
                    muscleGroups.Add(ptn.muscleGroups[i]);
                    subMuscleGroups.Add(ptn.muscleGroups[i]);
                }
                    

                    while (DateTime.Compare(nextDate, error) > 0 && DateTime.Compare(nextDate, end) <= 0)
                    {
                        if (muscleGroups.Count == 0)
                        {
                            for (int j = 0; j < ptn.muscleGroups.Count; j++) 
                                muscleGroups.Add(ptn.muscleGroups[j]);
                        }
                        generateWorkout(nextDate, userID, ref muscleGroups,subMuscleGroups);
                        possibleDate = nextDate;
                        nextDate = nextWorkoutDate(nextDate);
                    }
                return possibleDate;
            }

            public Pattern createPattern()
            {
                List<int> muscleGroups = dbConfig.getMuscleGroupIDs();

                int numberOfMuscleGroups = minMuscleGroup + rnd.Next(maxMuscleGroup - minMuscleGroup + 1);

                muscleGroups.Remove(7);  // whole body elendi.

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
            
            public void generateWorkout(DateTime startDate,int userID,ref List<int> muscleGroupIDs,List<int> subMuscleGroups)
            {
               int workoutID=createWorkout();
               UserWorkout userWorkout = generateUserWorkout(userID, workoutID, startDate);
               generateWorkoutExercises(userWorkout, ref muscleGroupIDs,subMuscleGroups);

                return;
            }

            public int chooseExercise(int muscleGroupID)
            {
                using (MySqlConnection con = new MySqlConnection(SqlDBConfig.connectionString))
                {
                    con.Open();

                    string query = "SELECT exercises.id from exercises join musclegroups on exercises.muscle_group= musclegroups.id where musclegroups.id=" + muscleGroupID + ";";
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = query;

                    List<int> foundExerciseID = new List<int>();

                    MySqlDataReader reader = cmd.ExecuteReader();
                    int counter = 0;
                    while (reader.Read())
                    {
                        counter++;
                        foundExerciseID.Add(reader.GetInt32(0));
                    }
                    reader.Close();
                    if (counter == 0) return -1;
                    int exercise = foundExerciseID[rnd.Next(foundExerciseID.Count)];



                    return exercise; //exercise id dönsün.
                }
            }

            public int chooseCardioExercise()
            {
                using (MySqlConnection con = new MySqlConnection(SqlDBConfig.connectionString))
                {
                    con.Open();

                    string query = "SELECT exercises.id from exercises join exercisetypes on exercises.exercise_type=exercisetypes.id where exercisetypes.name='CARDIO';";
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection =con;
                    cmd.CommandText = query;

                    List<int> foundExerciseID = new List<int>();

                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        foundExerciseID.Add(reader.GetInt32(0));
                    }
                    reader.Close();

                    int exercise = foundExerciseID[rnd.Next(foundExerciseID.Count)];

                    return exercise; //exercise id dönsün.
                }
            }

           public void generateSmallData()
            {
                List<int> userList = dbConfig.getUserIDs();

                for (int i = 0; i < userList.Count; i++)
                {
                    int userID = userList[i];

                    DateTime calendarStart = Generator.start;
                    DateTime calendarEnd = Generator.end;

                    DateTime monthStart = calendarStart;
                    DateTime monthEnd = calendarStart.AddDays(30);

                    Pattern ptn = createPattern();
                    monthStart = generateMonthlyWorkouts(ptn, userID, monthStart, monthEnd);
                }
                return;
            }

          public void generateVerySmallData()
           {
                List<int> userList = dbConfig.getUserIDs();

                for (int i = 0; i < userList.Count; i++)
                {
                    int userID = userList[i];

                    DateTime calendarStart = Generator.start;
                    DateTime calendarEnd = Generator.end;

                    DateTime monthStart = calendarStart;
                    DateTime monthEnd = calendarStart.AddDays(7);

                    Pattern ptn = createPattern();
                    monthStart = generateMonthlyWorkouts(ptn, userID, monthStart, monthEnd);
                }
                return;
            }

          public void generateAllSlots()
          {
              List<Equipment> equipmentList = dbConfig.getEquipments();

              for(int i=0;i<equipmentList.Count;i++)
              {
                  Equipment equipment = equipmentList[i];
                  int times = 0;
                  if (equipment.name == "BOSU" || equipment.name == "UPRIGHT BIKE" || equipment.name == "CROSS TRAINER"
                      || equipment.name == "BATTLE ROPE" || equipment.name == "BODYWEIGHT" || equipment.name ==
                      "ROPE" || equipment.name == "ROWING MACHINE")
                  {
                      times = rnd.Next(3, 6);
                  }

                  else times = rnd.Next(1, 4);

                  //int times = 1;
                  if (equipment.name == "DUMBBELLS")
                      times = rnd.Next(5,8);

                  for(int j=0; j<times ; j++)
                  {
                      generateSlot(equipment);
                  }
             
              }


         }

        public void generateSlot(Equipment equipment)
          {
              using (MySqlConnection con = new MySqlConnection(SqlDBConfig.connectionString))
              {
                  con.Open();
                  MySqlCommand cmd = new MySqlCommand();
                  cmd.Connection = con;
                  string query = "INSERT INTO SLOTS (equipmentid) VALUES ("+equipment.id.ToString()+");";

                  cmd.CommandText = query;
                  cmd.ExecuteNonQuery();
              }

          }
           
    }


    }
