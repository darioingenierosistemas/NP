using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;
using SQLite;
using Microsoft.Data.Sqlite;
using NP.BASE_DE_DATOS;


namespace NP
{
    //DARIO JOSE SARMIENTO SANIN

    public partial class MainPage : ContentPage
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        string folder2;
        TABLA_CONSULTA consulta = new TABLA_CONSULTA();
        public int n1, n2, b, s, a;

        public MainPage()
        {
            InitializeComponent();
            CreateBD CrearBD = new CreateBD();
            CrearBD.CreateDataBase();
            btnPerfecto.Clicked += BtnPerfecto_Clicked;
            btnMostrar.Clicked += BtnMostrar_Clicked;
            btnLimpiar.Clicked += BtnLimpiar_Clicked;
       
        }

        private void BtnMostrar_Clicked(object sender, EventArgs e)
        {
           if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
           { 
             var db = new SQLiteConnection(SelectFolder());
             var data = db.Table<TABLA_CONSULTA>();
             List<string> listaConsulta = new List<string>();
             foreach (var lista in data)
             {
                listaConsulta.Add(lista.CONSULTA + " - " + lista.NUMERO_PERFECTO);
             }
             lstMostar.ItemsSource = listaConsulta;
           }
            else
            {
                using (SqliteConnection db = new SqliteConnection("Filename=TABLA_CONSULTA.db3"))
                {
                    db.Open();

                    SqliteCommand selectCommand = new SqliteCommand ("SELECT CONSULTA, NUMERO_PERFECTO from TABLA_CONSULTA", db);

                    SqliteDataReader query = selectCommand.ExecuteReader();
                    List<string> listaConsulta = new List<string>();
                    while (query.Read())
                    {
                        listaConsulta.Add(query.GetString(0)+ " - " + query.GetString(1));
                    }
                    lstMostar.ItemsSource = listaConsulta;

                    db.Close();
                }
            }
            btnLimpiar.IsVisible = true;
        }


        private string SelectFolder()
        {
           
            if (Device.RuntimePlatform == Device.Android)
            {
                folder2 = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "TABLA_CONSULTA.db3");
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {

                var dbName = "TABLA_CONSULTA.db3";
                string personalFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string libraryFolder = Path.Combine(personalFolder, "..", "Library");
                folder2 = Path.Combine(libraryFolder, dbName);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                SqliteConnection db = new SqliteConnection("Filename=TABLA_CONSULTA.db3");

            }
            return folder2;
        }

        private void BtnLimpiar_Clicked(object sender, EventArgs e)
        {
            Limpiar();  
        }

        private async void BtnPerfecto_Clicked(object sender, EventArgs e)
        {
           

            if (String.IsNullOrEmpty(EdtRango1.Text) || String.IsNullOrEmpty(EdtRango2.Text))
            {

                await DisplayAlert("MENSAJE", "Los campos de los numeros NO pueden estar vacios.", "OK");
                Limpiar();
            }
            else
            {
                int numero1 = Convert.ToInt32(EdtRango1.Text.ToString());
                int numero2 = Convert.ToInt32(EdtRango2.Text.ToString());

                if (numero1 > numero2)
                {
                    await DisplayAlert("MENSAJE", "El Primer Numero NO PUEDE SER MAYOR que el Segundo Numero.", "OK");
                    Limpiar();
                }
                else
                {
                Formula();
                }
            }
            btnMostrar.IsVisible = true;

        }

        private void Limpiar()
        {
            EdtRango1.Text = "";
            EdtRango2.Text = "";
            lstMostar.ItemsSource = null;
            btnLimpiar.IsVisible = false;
        }

        private async void Formula()
        {
           
            List<string> lista = new List<string>();
           

            n1 = Convert.ToInt32(EdtRango1.Text.ToString());
            n2 = Convert.ToInt32(EdtRango2.Text.ToString());

            for (int i = n1; i <= n2; i++)
            {
                b = 0;
                s = i / 2;

                for (int j = 1; j <= s; j++)
                {
                    a = i % j;

                    if (a == 0)
                    {
                        b = b + j;
                    }
                }

                if (b == i)
                {
                    lista.Add(i.ToString());
                }
            }

            if (lista.Count != 0)
            {
                lstMostar.IsVisible = true;
                lstMostar.ItemsSource = lista;
                btnLimpiar.IsVisible = true;


                if (Device.RuntimePlatform == Device.Android)
                {
                    using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "TABLA_CONSULTA.db3")))
                    {

                        //var result = conn.Query<TABLA_CONSULTA>("SELECT CONSULTA, NUMERO_PERFECTO FROM TABLA_CONSULTA");
                        consulta.CONSULTA = $"{n1} y {n2}";
                        consulta.NUMERO_PERFECTO = "SI TIENE";

                        conn.Insert(consulta);
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    var dbName = "TABLA_CONSULTA.db3";
                    string personalFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string libraryFolder = Path.Combine(personalFolder, "..", "Library");
                    var path = Path.Combine(libraryFolder, dbName);
                    using (var conn = new SQLiteConnection(path))
                    {
                        consulta.CONSULTA = $"{n1} y {n2}";
                        consulta.NUMERO_PERFECTO = "SI TIENE";
                        conn.Insert(consulta);
                    }

                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    using (SqliteConnection db = new SqliteConnection("Filename=TABLA_CONSULTA.db3"))
                    {
                        db.Open();
                        SqliteCommand insertCommand = new SqliteCommand();
                        insertCommand.Connection = db;

                        string valores = $"'{n1} y {n2}'";
                        // Use parameterized query to prevent SQL injection attacks
                        insertCommand.CommandText = $"INSERT INTO TABLA_CONSULTA (CONSULTA,NUMERO_PERFECTO)  VALUES ({valores}, 'SI TIENE')";
                        insertCommand.ExecuteReader();

                        db.Close();
                    }
                }

            }

            else
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    using (var conn = new SQLiteConnection(System.IO.Path.Combine(folder, "TABLA_CONSULTA.db3")))
                    {
                    //var result = conn.Query<TABLA_CONSULTA>("SELECT CONSULTA, NUMERO_PERFECTO FROM TABLA_CONSULTA");
                    
                    consulta.CONSULTA = $"{n1} y {n2}";
                    consulta.NUMERO_PERFECTO = "NO TIENE";
                    conn.Insert(consulta);
                   
                    }
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    var dbName = "TABLA_CONSULTA.db3";
                    string personalFolder = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    string libraryFolder = Path.Combine(personalFolder, "..", "Library");
                    var path = Path.Combine(libraryFolder, dbName);
                    using (var conn = new SQLiteConnection(path))
                    {
                        consulta.CONSULTA = $"{n1} y {n2}";
                        consulta.NUMERO_PERFECTO = "NO TIENE";
                        conn.Insert(consulta);
                    }


                }
                else if (Device.RuntimePlatform == Device.UWP)
                {
                    using (SqliteConnection db = new SqliteConnection("Filename=TABLA_CONSULTA.db3"))
                    { 
                    db.Open();
                    SqliteCommand insertCommand = new SqliteCommand();
                    insertCommand.Connection = db;

                    string valores = $"'{n1} y {n2}'";
                    // Use parameterized query to prevent SQL injection attacks
                    insertCommand.CommandText = $"INSERT INTO TABLA_CONSULTA (CONSULTA,NUMERO_PERFECTO)  VALUES ({valores}, 'NO TIENE')";
                    insertCommand.ExecuteReader();

                    db.Close();
                    }
                }
                await DisplayAlert("MENSAJE", $"NO HAY NUMEROS PERFECTOS ENTRE {n1} Y {n2}", "OK");
                Limpiar();
            }
        }

     
    }

    //DARIO JOSE SARMIENTO SANIN
}
