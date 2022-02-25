using System;
using System.Data.SqlClient;

namespace LINQ2SQL
{
    class Program
    {
        public static string connection_string = @"Data source=(localdb)\MSSQLLocalDB;Initial Catalog=LINQ2SQL;Integrated Security = True";

        static void Main(string[] args)
        {
            GetStudents();
        }


        public static void GetStudents()
        {
            using (SqlConnection con = new(connection_string))
            {
                con.Open();
                SqlCommand cmd = new("SELECT * FROM Students WHERE Name = 'Emir'", con);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    Console.WriteLine(reader.GetValue(0));

                }

            }
        }
    }

}
