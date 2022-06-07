using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Npgsql;
using Asian_ORM;
// using System.Linq;
// using System.Linq;

namespace LINQ2SQL
{
    class Program
    {
        public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
        public static string students = "CREATE TABLE IF NOT EXISTS Student (Name VARCHAR ( 50 ) NOT NULL, Surname VARCHAR ( 50 ) NOT NULL);";
        public static string grades = "CREATE TABLE IF NOT EXISTS grades (value VARCHAR ( 50 ) NOT NULL, courseid int NOT NULL);";
        public static string studentData = "INSERT INTO student VALUES('kerem', 'ustun'), ('emir', 'sarikus'), ('fatih', 'catak');";

        public static DbSet<Student> students2 = new();
        
        static void Main(string[] args)
        {
            // NpgsqlConnection conn = new(connection_string);
            // conn.Open();
            // using (var cmd = new NpgsqlCommand(students, conn))
            // {
            //     cmd.ExecuteNonQuery();
            //     //Console.WriteLine('k');
            // }

            // using (var cmd = new NpgsqlCommand(studentData, conn))
            // {
            //     cmd.ExecuteNonQuery();
            //    // Console.WriteLine('k');
            // }
            // students2.Add(new Student(Name:"aAfriboy", Surname:"a5314"));
            // Console.WriteLine(students3.Count);

            var ax = students2.Select(s => new{s.Name}).OrderBy(s => s.Name).GroupBy(s => new {s.Name}).ExecuteQuery();
            foreach(var student in ax ){
                Console.WriteLine(student["Name"]);
                                //Console.WriteLine(studentcast.Name);
            }
            // var hurrDurr = 5;
            // MyMethod<int>(x => Console.Write(x * hurrDurr));
            // AMethod<int>(s => new { s.Name, s.Surname }));

            // var x = new List<int>();
            // x.Select
            // var x = new List<int>();
            // x.Add(1);
            // x.Add(2);

            // var stringx = x.Select(num => new{ num });
            // Console.Write(stringx.GetType());
        }

       
        

        // public static void MyMethod<T>(Action<T> lambdaHereLol)
        // {
        //     lambdaHereLol(2);
        // }
        
               

        }

        public class Student{
            public string Name {get; set;}
            public string Surname {get; set;}
            
            public Student(string Name = "", string Surname = ""){
                this.Name = Name;
                this.Surname = Surname;
            }
        }

        public class Grades{
            public string Value {get; set;}
            public int CourseId {get; set;}
            
            public Grades(string Value = "", int CourseId = 0){
                this.Value = Value;
                this.CourseId = CourseId;
            }
        }

        //students.Select(s => new { s.Name, s.Surname }).Include(s => s.Grades, q => q.Select(g => new { g.Value, g.CourseId  }));

        
    

}
