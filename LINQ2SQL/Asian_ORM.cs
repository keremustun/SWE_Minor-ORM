using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Dynamic;
namespace Asian_ORM{
    public class DbSet<T>{
        // Manual: first .where() then .select()
        private List<T> lst = new List<T>();
        public string selectString, whereString, orderByString, groupByString; 
        public string tableName = typeof(T).Name;
        public static string connection_string = "UserID=postgres;Password=root;Host=localhost;Port=5432;Database=LINQ2SQL;Pooling=true;";
        
        public DbSet<U> Select<U>(Expression<Func<T,U>> lambd)
        {
            DbSet<U> result = new();
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
            result.whereString = whereString;
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
                result.orderByString = $" order by {columns}";
            }
            else {
                result.orderByString = " order by " + lambd.Body.ToString().Split(".")[1];
            }
            return result;
        }

        public DbSet<T> GroupBy<U>(Expression<Func<T,U>> lambd) 
        {
            DbSet<T> result = new();
            result.selectString = selectString;
            result.whereString = whereString;
            result.orderByString = orderByString;

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
            
            result.groupByString = $" group by {columns}";
            return result;
        }

        public DbSet<T> Where(Expression<Func<T,bool>> predicate)
        {
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
            if (!ReferenceEquals((predicate.Body as BinaryExpression),null))
            {
                string whereLeftOperand  = "" + (predicate.Body as BinaryExpression).Left.ToString().Split(".")[1];
                string whereOperator = getWhereOperator(predicate);
                string whereRightOperand = "" + (predicate.Body as BinaryExpression).Right.ToString().Replace("\"", "");
                //Console.WriteLine(whereLeftOperand);
                result.selectString = selectString;
                result.whereString = $"WHERE {whereLeftOperand} {whereOperator} ";
                if (int.TryParse(whereRightOperand, out int n))
                {
                    result.whereString += whereRightOperand;
                }
                else
                {
                     result.whereString += $"'{whereRightOperand}'";
                }

                result.orderByString = result.whereString;
            }
            return result;
        }
       
        public List<Dictionary<string,object>> ExecuteQuery() 
        {
            
            string sqlstring = selectString + whereString + groupByString + orderByString;
            Console.WriteLine(sqlstring);
            var dataTable = new DataTable();
            NpgsqlConnection conn = new(connection_string);
            conn.Open();
            List<Dictionary<string,object>> res = new();
            using (var cmd = new NpgsqlCommand(sqlstring, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        res.Add(propToDict(reader));
                    }
                }
            }
            return res;
        }

        public static Dictionary<string,object> propToDict(IDataRecord datarec)
        {
            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dict = new ExpandoObject() as IDictionary<string, Object>;
            
            foreach (var prop in props)
            {
                dict.Add(prop.Name, datarec[prop.Name]);
            }
            
            return new Dictionary<string, object>(dict);
        }
    }
}