using System;
using System.Collections.Generic;
using Npgsql;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;

namespace Turkish_ORM{
    public static class Extensions{
        public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
        public static List<T> Select<T> (this List<T> list, Func<T,object> actie) where T : class {
            var result = new List<T>();
            
            var conn = new NpgsqlConnection(connection_string);
            conn.Open();
            using (var cmd = new NpgsqlCommand("SELECT name, surname FROM students WHERE name='fatih'", conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine("reader " + reader.GetType());
                        result.Add(propToDict<T>(reader));
                        //var dataTable = new DataTable();
                        //dataTable.Load(reader);
                        //string row_as_json = JsonSerializer.Serialize(dataTable);
                        
                        //result.Add(actie(JsonSerializer.Deserialize<T>(row_as_json)));
                        // int columnCount = reader.FieldCount;
                        // string[] row = new string[columnCount]; 

                        // for(int i = 0; i < row.Length; i++){
                        //     row[i] = reader.GetName(i) + ":" + reader.GetValue(i).ToString();
                        //}
                        
                        // reader.GetValues(row);
                        // string row_as_json = JsonSerializer.Serialize(row);


                        // result.Add(actie(reader.GetValue()))
                        // reader.getva
                        // T student = new T
                        // list.Add(new T(reader.GetValue(0));
                    }
                }
            }
            
            
            result.ForEach(i => Console.WriteLine(i));
            
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
        public static T propToDict<T>(IDataRecord dictionary) where T : class
        {
            var type = typeof(T);
            Console.WriteLine("test123 " + type);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            
            Console.WriteLine("test " + props.GetType());
            foreach (var item in props)
            {
                Console.WriteLine("test1 " + item);
            }

            var student = (T) FormatterServices.GetUninitializedObject(type);

            var newDictionary = new ExpandoObject() as IDictionary<string, Object>;
            
            foreach (var prop in props)
            {
                newDictionary.Add(prop.Name, dictionary[prop.Name]);
                Console.WriteLine("test2 " + prop.Name);
                Console.WriteLine("test2 " + dictionary[prop.Name]);
            }

            return toStudent((ExpandoObject) newDictionary, student);
        }

        public static T toStudent<T>(ExpandoObject dictionary, T student)
            where T : class
        {
            Console.WriteLine("dictionary " + dictionary);
            foreach (var item in dictionary)
            {
                Console.WriteLine("dictionary1 " + item);
            }
            Console.WriteLine("student " + student);
            IDictionary<string, object> newDictionary = dictionary;

            //returns student construconstructorStudent. returns error if more construconstructorStudents
            var constructorStudent = student.GetType().GetConstructors().Single();
            Console.WriteLine("constructorStudent " + constructorStudent);

            var parameters = constructorStudent.GetParameters();

            var parameterValues = new List<Object>();

            foreach (var parameter in parameters)
            {
                parameterValues.Add(newDictionary[parameter.Name]);
            }

            return (T) constructorStudent.Invoke(parameterValues.ToArray());
        }
    }
}