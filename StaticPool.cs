using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Drawing;

using iParking.Database;
using System.Data;
using System.Net.NetworkInformation;
using System.Threading;
using System.Data.SqlClient;

namespace iParking
{
    public static class StaticPool
    {
        // CSDL
        public static MDB mdb = null; // new MDB(Application.StartupPath + "\\iParkingPGS.mdb", "17032008");

        // Thong tin CSDL
        public static string SQLServerName = "171.244.0.161";
        public static string SQLDatabaseName = "iParking_ThienVan";
        public static string SQLAuthentication = "SQL Server Authentication";
        public static string SQLUserName = "adminLogin";
        public static string SQLPassword = "kztek123456";
        // 
        public static bool isChangeCCU = false;
        public static bool isChangeFloor = false;
        public static bool isChangeLed = false;
        public static bool isChangeLedDetail = false;
        public static bool isChangeMap = false;
        public static bool isChangeMapDetail = false;
        public static bool isChangeOutput = false;
        public static bool isChangeOutputDetail = false;
        public static bool isChangeTMA= false;
        public static bool isChangeVehicleType = false;
        public static bool isChangeZoneGroup = false;
        public static bool isChangeZone = false;
        public static bool isChangeZcu = false;

        // Thong tin dang nhap
        public static string FullName = "Anonymous";
        public static string UserCode = "";
        public static int Right1 = -1;


        // Ngon ngu
        public static string Language = "vi";

        public static string Tel = "04 355 27 205";
        public static string Fax = "04 355 27 206";
        public static string Address = "";
        public static int DelayTime = 300;

        public static bool IsConnectIO = false;

        

    }
}
