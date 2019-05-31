using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Data.OleDb;

namespace Project_Backend
{
    class Program
    {
        //storing Driver and DB location
        static string ConnectString = "provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=Z:\\ba372\\Project\\BackendCode\\BA372.accdb";
        static System.Data.OleDb.OleDbConnection Connection = new System.Data.OleDb.OleDbConnection(ConnectString);
        //method to return id value
        public static string GetID(String SQLquery)
        {
            //make an Oledbcommandobject
            OleDbCommand cmd = null;
            //make an Oledbdatareader
            System.Data.OleDb.OleDbDataReader Reader = null;
            string PrimaryKeyID = null;
            //connects to the database and catches if there is a problem connecting
            cmd = new OleDbCommand(SQLquery, Connection);
            try
            {
                Reader = cmd.ExecuteReader();
            }
            catch (InvalidOperationException)
            {
                Connection.Close();
            }
            Reader.Close();
            return PrimaryKeyID;
        }

        public static void InsertsSqlCommand(string SQLquery)
        {
            //make an Oledbcommandobject
            OleDbCommand cmd = null;
            cmd = new OleDbCommand(SQLquery, Connection);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (InvalidOperationException)
            {
                Connection.Close();
            }

        }

        public static List<string> GetSqlList(String SQLquery)
        {
            //This Function is used for running a SQL query, and returning the data in a list<string> variable
            {
                //make an Oledbcommandobject
                OleDbCommand cmd = null;
                //make an Oledbdatareader
                System.Data.OleDb.OleDbDataReader Reader = null;
                //connects to the database and catches if there is a problem connecting
                int sqlReaderThing = 0;
                List<string> sqlViewRequests = new List<string>(); //Initializing list so things can be stored in it
                cmd = new OleDbCommand(SQLquery, Connection);
                try
                {
                    Reader = cmd.ExecuteReader();
                    while (Reader.Read()) // while there is still information to be read, pull that information out and add it to the list
                    {
                        sqlViewRequests.Add(Convert.ToString((Reader[sqlReaderThing]))); //Converting reader output to string 
                        sqlReaderThing += 0; //Counter to know at what stage in the SQL reader we are at
                    }
                    Reader.Close();
                }
                catch (InvalidOperationException)
                {
                    Connection.Close();
                }
                Reader.Close();
                return sqlViewRequests;
            }
        }

        static void Main(string[] args)
        {
            //Testing database connection 
            try
            {
                Console.WriteLine(ConnectString);
                Connection.Open();
                Console.WriteLine("Database connection open...");
            }
            catch (System.Exception er) //if opening the connection failed
            {
                Console.WriteLine("Problems opening..." + er.Message);
                Console.WriteLine("Connection aborted");
                return;
            }
            //Selecting view, this won't be necessary after integration. During integration, the sqlView variable should be determined by the view being used. 
            //sqlView is the employee ID for either the director or financial manager. Reference Database if confused.
            Console.WriteLine("What is your view? Type 1 for Fin Mng 2 for Director");
            bool view = false;//bool for user input while loop. Not important after integration
            int viewInt = 0; //User input int, not important after integration
            int sqlView = 0; //intializing sqlView int
            while (!view)
            {
                viewInt = Convert.ToInt32(Console.ReadLine());
                if (viewInt == 1)
                {
                    Console.WriteLine("Financial Manager view selected");
                    view = true;
                    sqlView = 1;
                    break;
                }
                if (viewInt == 2)
                {
                    Console.WriteLine("Director View selected");
                    view = true;
                    sqlView = 3;
                    break;
                }
                else
                {
                    Console.WriteLine("Sorry, wrong input");
                }
            }

            string query = "SELECT REQ_ID FROM Requests WHERE Current_Approval=" + sqlView+ " AND Status=\"In-Progress\""; //building SQL query that will run agaisn't database using sqlView variable. 
            List<int> sqlViewRequests = new List<int>(); //Initializing sqlViewRequests list. This will determine the length of the for loop that builds the rows in our GUI. 
            sqlViewRequests = GetSqlList(query).Select(s => int.Parse(s)).ToList(); //Using GetSqlList function to populate the sqlViewRequests list. The select statement on the end is being used to parse the data into int's
            if (sqlViewRequests != null)// This is writing the SQL requests to the console for testing purposes
            {
                for (int i = 0; i < sqlViewRequests.Count; i++)
                {
                    Console.WriteLine(sqlViewRequests[i]);
                }
            }

            if (viewInt == 1)//Example of financial manager approval updates
            {
                Console.WriteLine("Finance Manager, approved request");


            }



            Console.ReadLine();





            //close connection to database
            try
            {
                Connection.Close();
                Console.WriteLine("Data collection complete");
                Console.WriteLine("Connection is closed...");
            }
            catch (System.Exception er) //error if connection close failed
            {
                Console.WriteLine("Error:" + er.Message);
                return;
            }
        }
    }
}
