using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
namespace Asian_ORM{
    public class DbSet<T>{
        private List<T> lst = new List<T>();
        public string selectString; 
        public string whereString; 
        public string orderByString; 
        public string groupByString; 
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

            result.selectString = $"SELECT {columns} FROM {tableName} ";
            result.whereString = selectString;


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
            Console.WriteLine(result.selectString);
            return result;
            }
          
        public DbSet<T> OrderBy<U>(Expression<Func<T,U>> lambd) 
        {
            DbSet<T> result = new();
            result.selectString = selectString;
            result.whereString = whereString;
            result.groupByString = groupByString;

            string columns = "";
            if (!ReferenceEquals((lambd.Body as NewExpression),null)) {
                var members = (lambd.Body as NewExpression).Members;
                for(int i = 0; i < members.Count; i++){
                    string memberName = members[i].Name;
                    columns += memberName;
                    if (i < members.Count - 1)
                        columns += ",";
                } 
                result.orderByString = $"order by {columns}";
            }
            else {
                result.orderByString = "order by " + lambd.Body.ToString().Split(".")[1];
            }
            Console.WriteLine(result.orderByString);
            return result;
        }

        public DbSet<T> GroupBy<U>(Expression<Func<T,U>> lambd) 
        {
            DbSet<T> result = new();
            result.selectString = selectString;
            result.whereString = whereString;

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
            
            result.groupByString = $"group by {columns}";
            Console.WriteLine("groupbystr " + result.groupByString);
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
                        whereOperator += "=";
                        break;
                    case "GreaterThan":
                        whereOperator += ">";
                        break;
                    case "LessThan":
                        whereOperator += "<";
                        break;
                    case "GreaterThanOrEqual":
                        whereOperator += ">=";
                        break;
                    case "LessThanOrEqual":
                        whereOperator += "<=";
                        break;
                    case "NotEqual":
                        whereOperator += "!=";
                        break;
                    // asd;lasdjl;kasd;lasdasd;l    
                    case "Between":
                        whereOperator += "...";
                        break;
                    case "Like":
                        whereOperator += "...";
                        break;
                    case "In":
                        whereOperator += "...";
                        break;
                }
                return whereOperator;
            }
            DbSet<T> result = new();
            Console.WriteLine("where " + predicate.Body.GetType());
            if (!ReferenceEquals((predicate.Body as BinaryExpression),null))
            {
                string whereLeftOperand  = "" + (predicate.Body as BinaryExpression).Left.ToString().Split(".")[1];
                
                string whereOperator = getWhereOperator(predicate);
                string whereRightOperand = "" + (predicate.Body as BinaryExpression).Right.ToString().Replace("\"", "");
                Console.WriteLine(whereRightOperand);
                // Example:  "WHERE Age > 3   
                result.selectString = selectString;
                result.whereString = $"WHERE {whereLeftOperand} {whereOperator} '{whereRightOperand}'";
                result.orderByString = result.whereString;
            }
            
            Console.WriteLine(result.whereString);
            return result;
        }
       
        public List<object> ExecuteQuery() 
        {
            
            string sqlstring = selectString + whereString + groupByString + orderByString;
            Console.WriteLine(sqlstring);
            var dataTable = new DataTable();
            NpgsqlConnection conn = new(connection_string);
            conn.Open();
            List<object> res = new();
            using (var cmd = new NpgsqlCommand(sqlstring, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(propToDict(reader));
                        //dataTable.Load(reader);

                        // var xz = dataTable.AsEnumerable();
                        // foreach(var x in xz){
                        //     var y = x.ItemArray;
                        //     foreach(var yy in y){
                        //         Console.WriteLine(yy);
                        //     }
                        //     Console.WriteLine(y);
                        // }
                        // Console.WriteLine(xz.GetType());
                    }
                }
            }


            return res;
        }

        public static T propToDict(IDataRecord dictionary)
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

        public static T toStudent(ExpandoObject dictionary, T student)
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
                Console.WriteLine("param " + parameter);
                Console.WriteLine("111 " + newDictionary[parameter.Name]);
                parameterValues.Add(newDictionary[parameter.Name]);
            }

            return (T) constructorStudent.Invoke(parameterValues.ToArray());
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