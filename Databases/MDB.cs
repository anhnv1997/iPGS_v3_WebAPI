using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace iParking.Database
{
    public class MDB
    {
        int timeout = 0;

        // MS SQL SERVER
        private string SQLServerName = @"DCTHANH\SQLEXPRESS";
        private string SQLDatabaseName = "";
        private string SQLAuthentication = "Windows Authentication";
        private string SQLUserName = "sa";
        private string SQLPassword = "123456";

        // MS ACCESS
        private string MDBFilePath = "";
        private string MDBPassword = "17032008";

        private SqlConnection sqlconn;

        private bool IsMDB = false;

        public MDB()
        {
        }

        public MDB(string sqlservername, string sqldatabasename, string sqlauthentication, string sqlusername, string sqlpassword, string mdbfilepath, string mdbpassword, bool ismdb)
        {
            // MS SQL SERVER
            SQLServerName = sqlservername;
            SQLDatabaseName = sqldatabasename;
            SQLAuthentication = sqlauthentication;
            SQLUserName = sqlusername;
            SQLPassword = sqlpassword;

            // MS ACCESS
            MDBFilePath = mdbfilepath;
            MDBPassword = mdbpassword;

            IsMDB = ismdb;

            //OpenMDB();//ref sqlconn, ref mdbconn);
        }

        // MS SQL SERVER
        public MDB(string sqlservername, string sqldatabasename, string sqlauthentication, string sqlusername, string sqlpassword)
        {
            SQLServerName = sqlservername;
            SQLDatabaseName = sqldatabasename;
            SQLAuthentication = sqlauthentication;
            SQLUserName = sqlusername;
            SQLPassword = sqlpassword;

            IsMDB = false;
            //OpenMDB();//ref sqlconn, ref mdbconn);
        }

        // MS ACCESS
        public MDB(string mdbfilepath, string mdbpassword)
        {
            MDBFilePath = mdbfilepath;
            MDBPassword = mdbpassword;

            IsMDB = true;
        }

        // Mo ket noi den co so du lieu
        public bool OpenMDB()//ref SqlConnection sqlconn, ref OleDbConnection mdbconn)
        {
            if (!IsMDB)
            {
            reconnect:
                try
                {
                    timeout = timeout + 1;
                    // Gan chuoi ket noi vao bien
                    string strConn = "";
                    if (SQLAuthentication == "Windows Authentication")
                    //{
                    //    strConn = "Data Source=" + SQLServerName + ";" +
                    //                     "Initial Catalog=" + SQLDatabaseName + ";" +
                    //                     "Integrated Security=true;Pooling=False";
                    //}
                    //else
                    //{
                    //    strConn = "server=" + SQLServerName + ";database=" + SQLDatabaseName + ";uid=" + SQLUserName + ";pwd=" + SQLPassword;
                    //}
                    {
                        strConn = "Data Source=" + SQLServerName + ";" +
                                         "Initial Catalog=" + SQLDatabaseName + ";" +
                                         "Integrated Security=true;Pooling=False" +
                                         ";MultipleActiveResultSets=True";

                    }
                    else
                    {
                        strConn = "server=" + SQLServerName + ";database=" + SQLDatabaseName + ";uid=" + SQLUserName + ";pwd=" + SQLPassword + ";MultipleActiveResultSets=True";
                    }
                    // Thuc thi chuoi ket noi
                    sqlconn = new SqlConnection(strConn);
                    // Mo ket noi
                    sqlconn.Open();
                    if (sqlconn.State == ConnectionState.Open)
                        return true;
                }
                catch (Exception ex)
                {

                    Thread.Sleep(1000);
                    if (timeout < 3)
                        goto reconnect;
                }
            }
            return false;
        }

        // Dong ket noi den co so du lieu
        public void CloseMDB()
        {
            try
            {
                if (sqlconn != null)
                {
                    if (sqlconn.State == ConnectionState.Open)
                        sqlconn.Close();
                }
            }
            catch
            {
                //nothing
            }
        }

        // Execute Command
        public bool ExecuteCommand(string commandString)
        {
            if (!IsMDB)
            {
                if (State())
                {
                    try
                    {
                        SqlCommand sqlCommand = new SqlCommand(commandString, sqlconn);
                        sqlCommand.ExecuteNonQuery();
                        //sqlconn.Close();
                        return true;
                    }
                    catch
                    {
                        //  SystemUI.SaveLogFile("ExecuteCommand\n" + ex.Message + "\n" + commandString);
                    }
                }
                else if (OpenMDB())
                {
                    try
                    {
                        SqlCommand sqlCommand = new SqlCommand(commandString, sqlconn);
                        sqlCommand.ExecuteNonQuery();
                        //sqlconn.Close();
                        return true;
                    }
                    catch
                    {
                        //  SystemUI.SaveLogFile("ExecuteCommand\n" + ex.Message + "\n" + commandString);
                    }
                }

            }
            return false;
        }

        // Excute command
        public bool ExecuteCommand(string commandString, string parameters, byte[] values)
        {
            if (!IsMDB)
            {
                if (State())
                {
                    SqlCommand sqlCommand = new SqlCommand(commandString, sqlconn);
                    sqlCommand.Parameters.Add(parameters, SqlDbType.Image);
                    if (values != null)
                        sqlCommand.Parameters[parameters].Value = values;
                    else
                        sqlCommand.Parameters[parameters].Value = DBNull.Value;
                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        //sqlconn.Close();
                        return true;
                    }
                    catch 
                    {
                    }
                }
                else if (OpenMDB())
                {
                    SqlCommand sqlCommand = new SqlCommand(commandString, sqlconn);
                    sqlCommand.Parameters.Add(parameters, SqlDbType.Image);
                    if (values != null)
                        sqlCommand.Parameters[parameters].Value = values;
                    else
                        sqlCommand.Parameters[parameters].Value = DBNull.Value;
                    try
                    {
                        sqlCommand.ExecuteNonQuery();
                        //sqlconn.Close();
                        return true;
                    }
                    catch
                    {
                    }
                }
            }
            return false;
        }

        // Lay du lieu tu bang hoac thu tuc
        public DataTable FillData(string commandString)
        {
            if (!IsMDB)
            {
                if (State())
                {
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(commandString, sqlconn);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        dataAdapter.Dispose();
                        //sqlconn.Close();
                        return dataSet.Tables[0];
                    }
                    catch
                    {
                    }
                }
                else if (OpenMDB())
                {
                    try
                    {
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(commandString, sqlconn);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        dataAdapter.Dispose();
                        //sqlconn.Close();
                        return dataSet.Tables[0];
                    }
                    catch
                    {
                    }
                }
            }
            return null;
        }

        //Kiem tra trang thai ket noi SQLServer
        public bool State()
        {
            try
            {
                if (sqlconn != null)
                {
                    if (sqlconn.State == ConnectionState.Open)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
