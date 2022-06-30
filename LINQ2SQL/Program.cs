using System;
using Asian_ORM;
using Npgsql;

namespace LINQ2SQL
{
    class Program
    {
        public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
        public static string studentsqry = "CREATE TABLE IF NOT EXISTS Student (Name VARCHAR ( 50 ) NOT NULL, Surname VARCHAR ( 50 ) NOT NULL);";
        public static string gradesqry = "CREATE TABLE IF NOT EXISTS Grade (value VARCHAR ( 50 ) NOT NULL, courseid int NOT NULL);";
        public static string studentData = "INSERT INTO student VALUES('John', 'Doe'), ('Peter', 'Hans'), ('Jan', 'Kees');";
        public static string gradesData = "INSERT INTO grade VALUES('8', 1000), ('9', 2000);";

        public static DbSet<Student> students = new();
        public static DbSet<Grade> grades = new();
        
        static void Main(string[] args)
        {
            // NpgsqlConnection conn = new(connection_string);
            // conn.Open();
            // using (var cmd = new NpgsqlCommand(studentsqry, conn))
            // {
            //     cmd.ExecuteNonQuery();
            //     //Console.WriteLine('k');
            // }
  
            var ax = students.Select(s => s.Name).ExecuteQuery();
            //var ax = students.Select(s => new{s.Name,s.Surname}).OrderBy(s => s.Name).GroupBy(s => new {s.Name, s.Surname}).ExecuteQuery();
            //var ax = grades.Where(g => g.CourseId > 100).Select(s => s.Value).ExecuteQuery();
            //var ax = grades.Where(g => g.CourseId > 100).Select(s => s.Value).OrderBy(s => s.Value).ExecuteQuery();
            
            foreach(var student in ax ) {
                foreach(var kv in student)
                {
                    Console.WriteLine(kv.Key + ":" + kv.Value);
                }
                System.Console.WriteLine("\n");
            }
        }
    }

    public class Student {
        public string Name {get; set;}
        public string Surname {get; set;}
        
        public Student(string Name = "", string Surname = ""){
            this.Name = Name;
            this.Surname = Surname;
        }
    }

    public class Grade {
        public string Value {get; set;}
        public int CourseId {get; set;}
        
        public Grade(string Value = "", int CourseId = 0){
            this.Value = Value;
            this.CourseId = CourseId;
        }
    }
    //students.Select(s => new { s.Name, s.Surname }).Include(s => s.Grades, q => q.Select(g => new { g.Value, g.CourseId  }));
}
