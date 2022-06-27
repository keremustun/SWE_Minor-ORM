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
        public static string studentData = "INSERT INTO student VALUES('kerem', 'ustun'), ('emir', 'sarikus'), ('fatih', 'catak');";
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

            // using (var cmd = new NpgsqlCommand(gradesqry, conn))
            // {
            //     cmd.ExecuteNonQuery();
            //    // Console.WriteLine('k');
            // }
            // using (var cmd = new NpgsqlCommand(studentData, conn))
            //             {
            //                 cmd.ExecuteNonQuery();
            //             // Console.WriteLine('k');
            //             }
            // using (var cmd = new NpgsqlCommand(gradesData, conn))
            // {
            //     cmd.ExecuteNonQuery();
            //    // Console.WriteLine('k');
            // }
  

            var ax = grades.Where(g => g.CourseId > 1000).Select(s => new{s.Value}).ExecuteQuery();
            //var axx = students2.Select(s => new{s.Name,s.Surname}).ExecuteQuery();
            foreach(var student in ax ){
                foreach(var kv in student)
                {
                    Console.WriteLine(kv.Key + ":" + kv.Value);
                }
                System.Console.WriteLine("\n");
                
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

        public class Grade{
            public string Value {get; set;}
            public int CourseId {get; set;}
            
            public Grade(string Value = "", int CourseId = 0){
                this.Value = Value;
                this.CourseId = CourseId;
            }
        }

        //students.Select(s => new { s.Name, s.Surname }).Include(s => s.Grades, q => q.Select(g => new { g.Value, g.CourseId  }));

        
    

}
