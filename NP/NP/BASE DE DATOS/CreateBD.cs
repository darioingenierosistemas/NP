using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

using System.IO;
using Microsoft.Data.Sqlite;
using Xamarin.Forms;

namespace NP.BASE_DE_DATOS
{
    public class CreateBD
    {
        SQLiteConnection connection;

        public SQLiteConnection folderdivice()
        {
            

            if (Device.RuntimePlatform == Device.Android)
            {
                    var dbName = "TABLA_CONSULTA.db3";
                    var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);
                    connection = new SQLiteConnection(path);

            }
            else if(Device.RuntimePlatform == Device.iOS)
            {
                var dbName = "TABLA_CONSULTA.db3";
                string personalFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string libraryFolder = Path.Combine(personalFolder, "..", "Library");
                var path = Path.Combine(libraryFolder, dbName);
                connection = new SQLiteConnection(path);
            }
           

            return connection;
            
        }
       
        
        public bool CreateDataBase()
        {

           
            try
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    using (SqliteConnection db = new SqliteConnection("Filename=TABLA_CONSULTA.db3"))
                    {
                        db.Open();

                        String tableCommand = "CREATE TABLE IF NOT " +
                            "EXISTS TABLA_CONSULTA (CONSULTA NVARCHAR(20), " +
                            "NUMERO_PERFECTO NVARCHAR(20))";

                        SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                        createTable.ExecuteReader();
                        db.Close();
                    }
                }        

                else
                { 
                using (var conn = folderdivice())
                    {
                        Int32 exist = conn.ExecuteScalar<Int32>("SELECT count(*) FROM sqlite_master WHERE type = 'table' AND name = 'TABLA_CONSULTA'");
                        if (exist == 0)
                        {
                            conn.CreateTable<TABLA_CONSULTA>();

                        }

                    }
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }

        }

    }
}
