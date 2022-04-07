using System;
using System.Collections.Generic;
using Npgsql;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Data;

namespace Turkish_ORM{
    public static class Extensions{
        public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
        public static List<T> Select<T> (this List<T> list, Func<T,object> actie){
            List<T> result = new();
            
            var conn = new NpgsqlConnection(connection_string);
            conn.Open();
            using (var cmd = new NpgsqlCommand("SELECT * FROM students", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dataTable = new DataTable();
                        dataTable.Load(reader);
                        string row_as_json = JsonSerializer.Serialize(dataTable);
                        
                        //result.Add(actie(JsonSerializer.Deserialize<T>(row_as_json)));
                        // int columnCount = reader.FieldCount;
                        // string[] row = new string[columnCount]; 

                        // for(int i = 0; i < row.Length; i++){
                        //     row[i] = reader.GetName(i) + ":" + reader.GetValue(i).ToString();
                        // }
                        
                        // reader.GetValues(row);
                        // string row_as_json = JsonSerializer.Serialize(row);


                        // result.Add(actie(reader.GetValue()))
                        // reader.getva
                        // T student = new T
                        // list.Add(new T(reader.GetValue(0));
                    }
                }
            }
            
            return result;

            
            // Retrieve all rows
            // Insert some data
            // using (var cmd = new NpgsqlCommand(students, conn))
            // {
            //     cmd.ExecuteNonQuery();
            //     //Console.WriteLine('k');
            // }

            // using (var cmd = new NpgsqlCommand(grades, conn))
            // {
            //     cmd.ExecuteNonQuery();
            //     //Console.WriteLine('k');
            // }

            // using (var cmd = new NpgsqlCommand(studentData, conn))
            // {
            //     cmd.ExecuteNonQuery();
            //    // Console.WriteLine('k');
            // }
        }
    }
}