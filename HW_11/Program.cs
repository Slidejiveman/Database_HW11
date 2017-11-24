using System;
using Dapper;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace HW_11
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connection String to the database as a whole.
            const string CONNECTION_STRING = @"Data Source = .\SQLEXPRESS;" +
                                             @"Initial Catalog = Homework_11;" +
                                             @"Integrated Security = True;" +
                                             @"Connect Timeout = 15;" +
                                             @"Encrypt = False;" +
                                             @"TrustServerCertificate = True;" +
                                             @"ApplicationIntent = ReadWrite;" +
                                             @"MultiSubnetFailover = False";

            using(IDbConnection conn = new SqlConnection(CONNECTION_STRING))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // DB requests go here
                        conn.Execute("DELETE FROM lgcustomer WHERE cust_code = 730", transaction: tran);
                        conn.Execute("DELETE FROM lgbrand WHERE brand_id = 23", transaction: tran);

                        tran.Commit();
                    } catch (Exception Ex)
                    {
                        Console.WriteLine(Ex.Message);
                        try
                        {
                            tran.Rollback(); // If an exception is raised by the FK_Constraint, then enter this block.
                            Console.WriteLine("Transaction rolled back.");
                        }
                        catch (SqlException SqlEx)
                        {
                            Console.WriteLine(SqlEx.Message);
                            Console.WriteLine("The transaction was already rolled back by the DBMS.");
                        
                        }
                    }
                }
            }
        }
    }
}
