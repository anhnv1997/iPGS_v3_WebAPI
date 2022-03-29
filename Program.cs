using iParking;
using iParking.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace PGS_WEBAPI
{
    class Program
    {
        static SQLConn[] sqls = null;
        public static void Main(string[] args)
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory + "\\SQLConn.xml";
                if (File.Exists(path))
                {
                    FileXML.ReadXMLSQLConn(path, ref sqls);
                }
            }
            catch
            {
            }
            ConnectToSQLServer();
            CreateHostBuilder(args).Build().Run();
        }
        private static void ConnectToSQLServer()
        {
            if (sqls != null && sqls.Length > 0)
            {
                string cbSQLServerName = sqls[0].SQLServerName;
                string cbSQLDatabaseName = sqls[0].SQLDatabase;
                string cbSQLAuthentication = sqls[0].SQLAuthentication;
                string txtSQLUserName = sqls[0].SQLUserName;
                string txtSQLPassword = CryptorEngine.Decrypt(sqls[0].SQLPassword, true);
                StaticPool.mdb = new MDB(cbSQLServerName, cbSQLDatabaseName, cbSQLAuthentication, txtSQLUserName, txtSQLPassword);
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:8082");
                    webBuilder.UseStartup<Startup>();
                }).UseWindowsService();
    }
}
