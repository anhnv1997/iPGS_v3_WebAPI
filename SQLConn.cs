using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PGS_WEBAPI
{
    public class SQLConn
    {
        public SQLConn()
        {

        }
        public SQLConn(string _dbtype, string _servername, string _databasename, string _user, string _pass, string _authentication)
        {
            dbtype = _dbtype;
            sqlservername = _servername;
            sqldatabase = _databasename;
            sqlusername = _user;
            sqlpassword = _pass;
            authentication = _authentication;
        }

        private string dbtype = "";
        public string DBType
        {
            get { return dbtype; }
            set { dbtype = value; }
        }

        private string sqlservername = "";
        public string SQLServerName
        {
            get { return sqlservername; }
            set { sqlservername = value; }
        }

        private string sqldatabase = "";
        public string SQLDatabase
        {
            get { return sqldatabase; }
            set { sqldatabase = value; }
        }

        private string sqlusername = "";
        public string SQLUserName
        {
            get { return sqlusername; }
            set { sqlusername = value; }
        }

        private string sqlpassword = "";
        public string SQLPassword
        {
            get { return sqlpassword; }
            set { sqlpassword = value; }
        }

        private string authentication = "";
        public string SQLAuthentication
        {
            get { return authentication; }
            set { authentication = value; }
        }
    }
}
