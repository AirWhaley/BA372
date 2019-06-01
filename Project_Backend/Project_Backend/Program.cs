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
                List<string> sqlViewRequests = new List<string>(); //Initializing list so things can be stored in it
                cmd = new OleDbCommand(SQLquery, Connection);
                try
                {
                    Reader = cmd.ExecuteReader();
                    while (Reader.Read()) // while there is still information to be read, pull that information out and add it to the list
                    {
                        sqlViewRequests.Add(Convert.ToString((Reader[0]))); //Converting reader output to string 
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
        public static List<string> GetReqInfo(int reqID)
        {
            //This Function is used for getting Req+ID info for populating rows. Each item pulls out into a list that can be referenced individually. 
            {
                //make an Oledbcommandobject
                OleDbCommand cmd = null;
                //make an Oledbdatareader
                System.Data.OleDb.OleDbDataReader Reader = null;
                //Making Query
                string query = "SELECT * FROM Requests WHERE Req_ID =" + reqID;
                //connects to the database and catches if there is a problem connecting
                List<string> reqIDList = new List<string>(); //Initializing list so things can be stored in it
                cmd = new OleDbCommand(query, Connection);
                try
                {
                    Reader = cmd.ExecuteReader();
                    while (Reader.Read()) // while there is still information to be read, pull that information out and add it to the list
                    {
                        reqIDList.Add(Convert.ToString(Reader["Travel"]));
                        reqIDList.Add(Convert.ToString(Reader["Est_Total_Cost"]));
                        reqIDList.Add(Convert.ToString(Reader["Emp_ID"]));
                        reqIDList.Add(Convert.ToString(Reader["Description"]));
                        reqIDList.Add(Convert.ToString(Reader["Begin_Date"]));
                        reqIDList.Add(Convert.ToString(Reader["End_Date"]));
                        reqIDList.Add(Convert.ToString(Reader["Decision_Date"]));
                        reqIDList.Add(Convert.ToString(Reader["Status"]));
                        reqIDList.Add(Convert.ToString(Reader["Current_Approval"]));
                    }
                    Reader.Close();
                }
                catch (InvalidOperationException)
                {
                    Connection.Close();
                }
                Reader.Close();
                return reqIDList;
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
                viewInt = int.Parse(Console.ReadLine());
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
            List<int> sqlViewRequests = GetSqlList(query).Select(s => int.Parse(s)).ToList(); //Using GetSqlList function to populate the sqlViewRequests list. The select statement on the end is being used to parse the data into int's
            if (sqlViewRequests != null)// This is writing the SQL requests to the console for testing purposes
            {
                for (int i = 0; i < sqlViewRequests.Count; i++)
                {
                    Console.WriteLine(sqlViewRequests[i]);
                }
            }
            //Testing regID Function
            int testID = 1;
            Console.WriteLine("Testing req ID function");
            List<string> testingReqID = GetReqInfo(testID).ToList(); //This line pulls the info from the "GetReqInfo() function and adds it to another list I just made using the .ToList command. Note you can also clear lists using ListName.Clear();
            for (int i = 0; i < testingReqID.Count; i++)
            {
                Console.WriteLine(testingReqID[i]);
            }
            //Done testing reqID Function

            //These are ints for the console app logic of approving or denying requests. Then used to build SQL Statements. We will have to modify this when we figure out our loop after integration. 
            int approveInt;
            int denyInt;
            
            if (viewInt == 1)//Example of financial manager approval updates
            {
                Console.WriteLine("Finance Manager, approved request");
                approveInt = int.Parse(Console.ReadLine());
                query = "UPDATE Requests, Approval_Chain SET Requests.Current_Approval = Approval_Chain.Next_Approval WHERE Requests.Current_Approval = Approval_Chain.Before_Approval AND Requests.Req_ID =" + approveInt;
                InsertsSqlCommand(query);
                query = "INSERT INTO Request_Transaction ( Req_ID, Trans_Date, Trans_Emp, Approved, Comments )VALUES("+approveInt+", Date(), 1 "/*THIS 1 MUST BE SET TO EMPLOYEE ID OF VIEWER (Finance manager or director)*/ +", 0, \"None\")";
                InsertsSqlCommand(query);


                //Example of Financial manager deny updates
                Console.WriteLine("Finance Manager, deny request");
                denyInt = int.Parse(Console.ReadLine());
                query = "UPDATE Requests SET Status = \"Denied\" WHERE Req_ID ="+denyInt;
                InsertsSqlCommand(query);
                query = "INSERT INTO Request_Transaction ( Req_ID, Trans_Date, Trans_Emp, Approved, Comments )VALUES(" + approveInt + ", Date(), 1 "/*THIS 1 MUST BE SET TO EMPLOYEE ID OF VIEWER (Finance manager or director)*/ + ", 0, \"Denied by financial manager\")";
                InsertsSqlCommand(query);

            }
            if (viewInt == 2)//Example of director approval updates
            {
                Console.WriteLine("Director, approved request");
                approveInt = int.Parse(Console.ReadLine());
                query = "UPDATE Requests SET Status = \"Approved\" WHERE Req_ID =" + approveInt;
                InsertsSqlCommand(query);
                query = "INSERT INTO Request_Transaction ( Req_ID, Trans_Date, Trans_Emp, Approved, Comments )VALUES(" + approveInt + ", Date(), 3 "/*THIS 1 MUST BE SET TO EMPLOYEE ID OF VIEWER (Finance manager or director)*/ + ", 0, \"Approved by director\")";
                InsertsSqlCommand(query);


                //Example of Director manager deny updates
                Console.WriteLine("Director, deny request");
                denyInt = int.Parse(Console.ReadLine());
                query = "UPDATE Requests SET Status = \"Denied\" WHERE Req_ID =" + denyInt;
                InsertsSqlCommand(query);
                query = "INSERT INTO Request_Transaction ( Req_ID, Trans_Date, Trans_Emp, Approved, Comments )VALUES(" + approveInt + ", Date(), 3 "/*THIS 1 MUST BE SET TO EMPLOYEE ID OF VIEWER (Finance manager or director)*/ + ", 0, \"Denied by Director\")";
                InsertsSqlCommand(query);

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
