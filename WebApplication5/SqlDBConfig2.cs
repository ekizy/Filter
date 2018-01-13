using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
namespace WebApplication5
{
    public class SqlDBConfig2
    {
        public static string connectionString = "Server=localhost;Database=smartfitaftersim;Uid=root;Pwd=;";


        public List<int> getUserIDs()
        {
            List<int> userIDs = new List<int>();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;

                string query = "SELECT ID FROM USERS;";
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    userIDs.Add(reader.GetInt32(0));
                }
            }

            return userIDs;
        }

        public List<int> getExerciseIDs()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                List<int> exerciseIDs = new List<int>();
                string query = "SELECT ID FROM EXERCISES;";
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    exerciseIDs.Add(reader.GetInt32(0));
                }

                return exerciseIDs;
            }

        }

        public List<int> getWorkoutIDs()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                List<int> workoutIDs = new List<int>();
                string query = "SELECT ID FROM WORKOUTS;";
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    workoutIDs.Add(reader.GetInt32(0));
                }
                return workoutIDs;
            }

        }

        public List<int> getMuscleGroupIDs()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                List<int> muscleGroupIDs = new List<int>();
                string query = "SELECT ID FROM MUSCLEGROUPS;";
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    muscleGroupIDs.Add(reader.GetInt32(0));
                }

                return muscleGroupIDs;
            }

        }

        public List<Equipment> getEquipments()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                List<Equipment> equipments = new List<Equipment>();
                string query = "SELECT * FROM EQUIPMENTS;";
                cmd.CommandText = query;
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Equipment equipment = new Equipment();
                    equipment.id = reader.GetInt32(0);
                    equipment.name = reader.GetString(1);

                    equipments.Add(equipment);
                }

                return equipments;
            }
        }
    }
}