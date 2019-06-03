using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;

namespace HBR_Port_GUI
{
    public partial class _Default : Page
    {
        static string ConnectString = "provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=Z:\\ba372\\Project\\BackendCode\\BA372.accdb";
        //storing Driver and DB location
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
        public List<string> GetSqlList(String SQLquery)
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

        protected void Page_Load(object sender, EventArgs e)
        {

            int SQLFMORD = 0; 
           
            string po = "";

            //connection
            int number_Rows = 5;
            string PositionView = Request.QueryString["rank"];

            if (PositionView == "fm" || PositionView == "director")
            {
                //the connections is open
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
                if (PositionView == "fm")
                {
                    SQLFMORD = 1;
                }
                if (PositionView == "director")
                {
                    SQLFMORD = 3;
                }
                
                string query = "SELECT REQ_ID FROM Requests WHERE Current_Approval=" + Convert.ToString(SQLFMORD) + " AND Status=\"In-Progress\""; //building SQL query that will run agaisn't database using sqlView variable. 
                List<int> sqlViewRequests = GetSqlList(query).Select(s => int.Parse(s)).ToList(); //Using GetSqlList function to populate the sqlViewRequests list. The select statement on the end is being used to parse the data into int's
                //loops over each ID
                foreach (int i in sqlViewRequests)
                {
                    //
                    string est_Total_cost = "N/A";
                    string travel = "N/A";
                    string emp_ID = "N/A";
                    string description = "N/A";
                    string beg_date = "N/A";
                    string end_date = "N/A";
                    string status = "N/A";
                    string Current_approval = "N/A";
                    List<string> testingReqID = GetReqInfo(i).ToList(); //This line pulls the info from the "GetReqInfo() function and adds it to another list I just made using the .ToList command. Note you can also clear lists using ListName.Clear();


                    travel = testingReqID[0];
                    est_Total_cost = testingReqID[1];
                    emp_ID = testingReqID[2];
                    description = testingReqID[3];
                    beg_date = testingReqID[4];
                    end_date = testingReqID[5];
                    status = testingReqID[6];
                    Current_approval = testingReqID[7];
                     



                    //Puts variables in HTML format and uses index i to give variables distinct identifiers
                        po = po + "<tr>" +
                               "<Form>" +
                              "<th scope=\"row\"><input type=\"radio\" name=\"Decision" + i + "\" value=\"Approve\"></button></th>" +
                              "<td><input type=\"radio\" name=\"Decision" + i + "\" value=\"DENY\"></button></td>" +
                              "</Form>" +
                              "<td>" + sqlViewRequests[i-1] + "</td>" + //subtract 1 because the list starts at 1 but we need to call it by elements that start at 0
                              "<td>" + est_Total_cost + "</td>" +
                              "<td>" + emp_ID + "</td>" +
                              "<td>" + beg_date + "</td>" +
                              "<td>" + end_date + "</td>" +
                              "<td>" + Current_approval + "</td>" +
                              "<td>" + description + "</td>" +
                             "</tr>";
                    
                    
                }
                

                //for (int i = 1; i <= number_Rows; i++)
                //{
                //    // po = purchase order
                //    // string (purchase order) is null, then for loop.
                //    // for int i, as long as in is less than 5 (will change to variable that represents total number of po's)
                //    // will update the 1-7 to be variable for each purchase order variable

                //    po = po + "<tr>" +
                //               "<Form>" +
                //              "<th scope=\"row\"><input type=\"radio\" name=\"Decision" + i + "\" value=\"Approve\"></button></th>" +
                //              "<td><input type=\"radio\" name=\"Decision" + i + "\" value=\"DENY\"></button></td>" +
                //              "</Form>" +
                //              "<td>"+sqlViewRequests[i]+"</td>" +
                //              "<td>Est_Cost</td>" +
                //              "<td>Emp_ID</td>" +
                //              "<td>Beg_Date</td>" +
                //              "<td>End_Date</td>" +
                //              "<td>Decision</td>" +
                //              "<td>Desc</td>" +
                //             "</tr>";

                //}   // 8 total categories (-status)
            }
            else
            {
                
            }

                Somename.Text = po;
            }


        }
    }
