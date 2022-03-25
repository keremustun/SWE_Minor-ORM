using System;
using Npgsql;

namespace LINQ2SQL
{
    class Program
    {
        public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
        public static string students = "CREATE TABLE IF NOT EXISTS students (name VARCHAR ( 50 ) NOT NULL, surname VARCHAR ( 50 ) NOT NULL);";
        public static string grades = "CREATE TABLE IF NOT EXISTS grades (value VARCHAR ( 50 ) NOT NULL, courseid int NOT NULL);";
        public static string studentData = "INSERT INTO students VALUES('kerem', 'ustun'), ('emir', 'sarikus'), ('fatih', 'catak');";

        static void Main(string[] args)
        {
            GetStudents();
        }


        public static void GetStudents()
        {
            var conn = new NpgsqlConnection(connection_string);
            conn.Open();

            // Insert some data
            using (var cmd = new NpgsqlCommand(students, conn))
            {
                cmd.ExecuteNonQuery();
                //Console.WriteLine('k');
            }

            using (var cmd = new NpgsqlCommand(grades, conn))
            {
                cmd.ExecuteNonQuery();
                //Console.WriteLine('k');
            }

            using (var cmd = new NpgsqlCommand(studentData, conn))
            {
                cmd.ExecuteNonQuery();
               // Console.WriteLine('k');
            }

            // Retrieve all rows
            using (var cmd = new NpgsqlCommand("SELECT * FROM students", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(0));
                        Console.WriteLine((reader.GetString(1)).GetType());
                    }
                }
            }
               

        }
    }

}
