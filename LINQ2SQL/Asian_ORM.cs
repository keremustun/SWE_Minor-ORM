using System;
using System.Collections.Generic;
using Npgsql;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Asian_ORM{
    public class DbSet<T>{
        private List<T> lst = new List<T>();
        public string selectString; 
        public string whereString; 
        public string tableName = typeof(T).Name;
        public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
        
        public DbSet<U> Select<U>(Expression<Func<T,U>> lambd){
            DbSet<U> result = new();
            // var x = lambd(default(T));

            string columns = "";
            if (!ReferenceEquals((lambd.Body as NewExpression),null))
            {
                var members = (lambd.Body as NewExpression).Members;

                for(int i = 0; i < members.Count; i++){
                    string memberName = members[i].Name;
                    columns += memberName;
                    if (i < members.Count - 1)
                        columns += ",";
                }
                
            }
            
            else{
                columns += "*";
            }

            selectString = $"SELECT {columns} FROM {tableName} ";


  // foreach(var item in lst){
            //     result.Add(lambd(item));
            // }
            
            // foreach(var item in result.lst){
            //     Console.WriteLine(item);
            // }
            

            
            // Expression<Func<T,U>> selectBodyExpression = selectBody as Expression<Func<T,U>> ;


            // Expression<Func<T,U>> xpression = selectBody as Expression<Func<T,U>>;
            // var x = xpression.Body as ;
            // // Console.WriteLine(selectBody.Body);
            // Console.WriteLine(selectBody.Body);
            // ParameterExpression paramExpr = Expression.Parameter(typeof(T));
            // var x = selectBody.ToString().Split(",");
            // Console.WriteLine(x[0]);
            // Console.WriteLine(x[1]);
            //Console.WriteLine(x.Type);

            // var objectMember = Expression.Convert(me, typeof(object));
            // var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            // var getter = getterLambda.Compile();
            // selectBody.Body as MemberExpression ?? ((UnaryExpression)selectBody.Body).Operand as MemberExpression).Member.Name
            // MemberExpression body = selectBody.Body as MemberExpression;

            // if (body == null) {
            // UnaryExpression ubody = (UnaryExpression)selectBody.Body;
            // body = ubody.Operand as MemberExpression;
            // }

            // var propInfo = me.Member as PropertyInfo;
            // var myval = propInfo.GetCustomAttributes();
            // var d = paramType.GetMember("Name").Member.Name)[0].Name;
            // Console.WriteLine(body.Member.Name);

            // var nameExpression = (MemberExpression) selectBody.Body;
            // string name = nameExpression.Member.Name;
             
            return result;
            }
          



            // students.Select(x => new {x.Name, x.Surname})

            // foreach(var x in lst){
            //     result.Add(selectBody(x));
            // }

        

        public DbSet<T> Where(Expression<Func<T,bool>> predicate){
            string getWhereOperator(Expression<Func<T,bool>> predicate)
            {
                string whereOperator = "";
                switch (predicate.Body.NodeType.ToString())
                {
                    case "Equal": 
                        whereOperator += "= ";
                        break;
                    case "GreaterThan":
                        whereOperator += "> ";
                        break;
                    case "LessThan":
                        whereOperator += "< ";
                        break;
                    case "GreaterThanOrEqual":
                        whereOperator += ">= ";
                        break;
                    case "LessThanOrEqual":
                        whereOperator += "<= ";
                        break;
                    case "NotEqual":
                        whereOperator += "!= ";
                        break;
                    // asd;lasdjl;kasd;lasdasd;l    
                    case "Between":
                        whereOperator += "... ";
                        break;
                    case "Like":
                        whereOperator += "... ";
                        break;
                    case "In":
                        whereOperator += "... ";
                        break;
                }
                return whereOperator;
            }
            
            Console.WriteLine(predicate.Body.GetType());
            
            if (!ReferenceEquals((predicate.Body as BinaryExpression),null))
            {
                string whereLeftOperand  = "" + (predicate.Body as BinaryExpression).Left;
                string whereOperator = getWhereOperator(predicate);
                string whereRightOperand = "" + (predicate.Body as BinaryExpression).Right;

                // Example:  "WHERE Age > 3   
                whereString = $"WHERE {whereLeftOperand} {whereOperator} {whereRightOperand}";
            }
            
            DbSet<T> result = new();
            return result;
        }

        public DataTable ExecuteQuery()
        {
            string sqlstring = selectString + whereString;
            var dataTable = new DataTable();
            NpgsqlConnection conn = new(connection_string);
            conn.Open();
            using (var cmd = new NpgsqlCommand(sqlstring, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    
                    while (reader.Read())
                    {
                        dataTable.Load(reader);
                        //string row_as_json = JsonSerializer.Serialize(dataTable);
                        
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
            
            return dataTable;
        }

        public List<T> ToList() => lst;


        // public void Add(T listItem){
        //     lst.Add(listItem);
        // }

        public string ToSql()
        {
            return null;
        }

        // List<T> Run(){
        //     var sql = ToSql();
            
        //     var conn = new NpgsqlConnection(connection_string);
        //     using (var cmd = new NpgsqlCommand(sql, conn))
        //     {
        //         using (var reader = cmd.ExecuteReader())
        //         {
                    
        //             while (reader.Read())
        //             {
        //                 var dataTable = new DataTable();
        //                 dataTable.Load(reader);
        //                 string row_as_json = JsonSerializer.Serialize(dataTable);
                        
        //                 //result.Add(actie(JsonSerializer.Deserialize<T>(row_as_json)));
        //                 // int columnCount = reader.FieldCount;
        //                 // string[] row = new string[columnCount]; 

        //                 // for(int i = 0; i < row.Length; i++){
        //                 //     row[i] = reader.GetName(i) + ":" + reader.GetValue(i).ToString();
        //                 // }
                        
        //                 // reader.GetValues(row);
        //                 // string row_as_json = JsonSerializer.Serialize(row);


        //                 // result.Add(actie(reader.GetValue()))
        //                 // reader.getva
        //                 // T student = new T
        //                 // list.Add(new T(reader.GetValue(0));
        //             }
        //         }
        //     }
        // }
    }

    // public static class Extensions{
    //     public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
    //     public static List<U> Select<T,U> (this List<T> list, Func<T,U> actie){
    //         List<U> result = new();
            
    //         var conn = new NpgsqlConnection(connection_string);
    //         conn.Open();
    //         using (var cmd = new NpgsqlCommand("SELECT Name, Surname FROM Students WHERE Name = 'Emir'", conn))
    //         {
    //             using (var reader = cmd.ExecuteReader())
    //             {
                    
    //                 while (reader.Read())
    //                 {
    //                     var dataTable = new DataTable();
    //                     dataTable.Load(reader);
    //                     string row_as_json = JsonSerializer.Serialize(dataTable);
                        
    //                     //result.Add(actie(JsonSerializer.Deserialize<T>(row_as_json)));
    //                     // int columnCount = reader.FieldCount;
    //                     // string[] row = new string[columnCount]; 

    //                     // for(int i = 0; i < row.Length; i++){
    //                     //     row[i] = reader.GetName(i) + ":" + reader.GetValue(i).ToString();
    //                     // }
                        
    //                     // reader.GetValues(row);
    //                     // string row_as_json = JsonSerializer.Serialize(row);


    //                     // result.Add(actie(reader.GetValue()))
    //                     // reader.getva
    //                     // T student = new T
    //                     // list.Add(new T(reader.GetValue(0));
    //                 }
    //             }
    //         }
            
    //         return result;

            
    //         // Retrieve all rows
    //         // Insert some data
    //         // using (var cmd = new NpgsqlCommand(students, conn))
    //         // {
    //         //     cmd.ExecuteNonQuery();
    //         //     //Console.WriteLine('k');
    //         // }

    //         // using (var cmd = new NpgsqlCommand(grades, conn))
    //         // {
    //         //     cmd.ExecuteNonQuery();
    //         //     //Console.WriteLine('k');
    //         // }

    //         // using (var cmd = new NpgsqlCommand(studentData, conn))
    //         // {
    //         //     cmd.ExecuteNonQuery();
    //         //    // Console.WriteLine('k');
    //         // }
    //     }
    // }
}