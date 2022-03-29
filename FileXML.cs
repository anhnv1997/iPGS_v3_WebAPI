using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PGS_WEBAPI
{
    public class FileXML
    {
        public FileXML()
        {
        }

        public static void ReadXMLRecList(string filename, ref Rectangle[] rects)
        {
            try
            {
                if (File.Exists(filename))
                {
                    rects = null;
                    System.Xml.Serialization.XmlSerializer reader =
                        new System.Xml.Serialization.XmlSerializer(typeof(Rectangle[]));
                    System.IO.StreamReader file = new System.IO.StreamReader(filename); // @"c:\temp\SerializationOverview.xml");
                    rects = (Rectangle[])reader.Deserialize(file);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void WriteXMLRecList(string filename, Rectangle[] rects)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(Rectangle[]));

                System.IO.StreamWriter file = new System.IO.StreamWriter(filename); // @"c:\temp\SerializationOverview.xml");
                writer.Serialize(file, rects);
                file.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public static void ReadXMLSQLConn(string filename, ref SQLConn[] sqlconns)
        {
            try
            {
                if (File.Exists(filename))
                {
                    sqlconns = null;
                    System.Xml.Serialization.XmlSerializer reader =
                        new System.Xml.Serialization.XmlSerializer(typeof(SQLConn[]));
                    System.IO.StreamReader file = new System.IO.StreamReader(filename); // @"c:\temp\SerializationOverview.xml");
                    sqlconns = (SQLConn[])reader.Deserialize(file);
                    file.Close();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void WriteXMLSQLConn(string filename, SQLConn[] sqlconns)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(SQLConn[]));

                System.IO.StreamWriter file = new System.IO.StreamWriter(filename); // @"c:\temp\SerializationOverview.xml");
                writer.Serialize(file, sqlconns);
                file.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public static T[] ReadXML<T>(string filename) where T : class
        {
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            try
            {

                if (File.Exists(filename))
                {
                    System.Xml.Serialization.XmlSerializer reader =
                        new System.Xml.Serialization.XmlSerializer(typeof(T[]));
                    //file = new System.IO.StreamReader(filename); // @"c:\temp\SerializationOverview.xml");
                    var conns = (T[])reader.Deserialize(file);
                    file.Close();
                    return conns;
                }
            }
            catch (Exception ex)
            {
                // System.Windows.Forms.MessageBox.Show(ex.Message);
                file.Close();
                File.Delete(filename);
            }
            return null;
        }

        public static void WriteXML<T>(string filename, List<T> conns) where T : class
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            try
            {
                System.Xml.Serialization.XmlSerializer writer =
                    new System.Xml.Serialization.XmlSerializer(typeof(List<T>));

                //System.IO.StreamWriter file = new System.IO.StreamWriter(filename); // @"c:\temp\SerializationOverview.xml");
                writer.Serialize(file, conns);
                file.Close();
            }
            catch (Exception ex)
            {
                // System.Windows.Forms.MessageBox.Show(ex.Message);
                file.Close();
                File.Delete(filename);
            }
        }

    }
}
