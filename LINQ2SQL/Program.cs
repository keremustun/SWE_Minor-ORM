using System;
using System.Collections.Generic;
using Npgsql;
// using Turkish_ORM;
using System.Linq;
// using System.Linq;

namespace LINQ2SQL
{
    class Program
    {
        public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
        //public static string students = "CREATE TABLE IF NOT EXISTS students (name VARCHAR ( 50 ) NOT NULL, surname VARCHAR ( 50 ) NOT NULL);";
        public static string grades = "CREATE TABLE IF NOT EXISTS grades (value VARCHAR ( 50 ) NOT NULL, courseid int NOT NULL);";
        public static string studentData = "INSERT INTO students VALUES('kerem', 'ustun'), ('emir', 'sarikus'), ('fatih', 'catak');";

        public static List<Student> students = new List<Student>(){};
    
        static void Main(string[] args)
        {
            // var ax = students.Select(s => new{s.Name, s.Surname});
            // foreach(var student in ax ){
            //     Console.WriteLine(student.Name);
            // }
            // var hurrDurr = 5;
            // MyMethod<int>(x => Console.Write(x * hurrDurr));
            // AMethod<int>(s => new { s.Name, s.Surname }));

            // var x = new List<int>();
            // x.Select
            var x = new List<int>();
            x.Add(1);
            x.Add(2);

            var stringx = x.Select(num => new{ num });
            Console.Write(stringx.GetType());
        }

       
        

        // public static void MyMethod<T>(Action<T> lambdaHereLol)
        // {
        //     lambdaHereLol(2);
        // }
        
               

        }

        public class Student{
            public string Name;
            public string Surname;
            
            public Student(string name = "", string surname = ""){
                Name = name;
                Surname = surname;
            }
        }

        //students.Select(s => new { s.Name, s.Surname }).Include(s => s.Grades, q => q.Select(g => new { g.Value, g.CourseId  }));

        
    

}
