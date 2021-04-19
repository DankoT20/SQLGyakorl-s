using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Timers;
using System.Threading;

namespace MYSQ
{
    class Program
    {
        static string Kapcsolat = "datasource=localhost;port=3306;database=termeles;username=C;password=asd123";
        static MySqlConnection Csatlakozas = new MySqlConnection(Kapcsolat);


        static string adat;
        static int BeEladott;
        static int AdatSorszam;
        static string SQLFelh;
        static string SQLJelsz;
        static string BeFelh;
        static string BeJelszo;
        static int login = 0;
        static string BeID;
        static int probalkozas = 1;
        static int masodikkor;
        static string dbOsszeg;
        static string SQLDarab;
        static int RaktarErtek;
     
        static string SQLNev;

        static void Main(string[] args)
        {
            kezdolap();
          


        }
     

        
        static void kilepes()
        {
            Environment.Exit(0);



        }
        static void kezdolap()
        {



            Console.WriteLine("Ezen kívül még {0} db próbálkozása van.", 3 - probalkozas);
            probalkozas++;
            Console.Write("Kérem adja meg a felhasználónevét: ");


            BeFelh = null;
            while (true)
            {
                var fh = Console.ReadKey(true);
                if (fh.Key == ConsoleKey.Enter)
                    break;
                BeFelh += fh.KeyChar;
            }
            Console.WriteLine();

            Console.Write("Kérem adja meg a jelszavát: ");


            while (true)
            {
                var pw = Console.ReadKey(true);
                if (pw.Key == ConsoleKey.Enter)
                    break;
                BeJelszo += pw.KeyChar;
            }

            Console.WriteLine();
            Console.Write("Kérem adja meg az azonosítóját: ");

            BeID = null;
            while (true)
            {
                var azon = Console.ReadKey(true);
                if (azon.Key == ConsoleKey.Enter)
                    break;
                BeID += azon.KeyChar;

            }
            SQLAzonositas();



        }
        static void Ertek()
        {
            try
            {  
                List<string> BeAzonErtek = new List<string>();
                BeAzonErtek.Clear();
            Console.WriteLine("Kérem adja meg a kívánt termék azonosítóját");
             
                BeAzonErtek.Add(Console.ReadLine());
            string SQLDBcom = "SELECT * FROM termeles.term WHERE sorszam='" + BeAzonErtek[0] + "';";
           
            MySqlCommand SQLDBParancs = new MySqlCommand(SQLDBcom, Csatlakozas);
            MySqlDataReader SQLDBOlvaso;
            Csatlakozas.Open();
            SQLDBOlvaso = SQLDBParancs.ExecuteReader();

            while (SQLDBOlvaso.Read())
            {
                SQLNev = SQLDBOlvaso.GetString(1);
                SQLDarab = SQLDBOlvaso.GetString(2);
                dbOsszeg  = SQLDBOlvaso.GetString(4);
            }

            Csatlakozas.Close();
            RaktarErtek = Convert.ToInt32(SQLDarab) *Convert.ToInt32( dbOsszeg);
            Console.WriteLine("A(z) {0} termékből {1} db van raktáron, aminek az értéke: {2} Ft",SQLNev, SQLDarab, RaktarErtek);
            
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
            loggedIn();


        }
        static void SQLAzonositas()
        {
            try
            {


                Console.WriteLine(BeID);

                string SQL = "SELECT * FROM termeles.azonosito WHERE azon='" + BeID + "';";
                MySqlCommand SQLAzonositoParancs = new MySqlCommand(SQL, Csatlakozas);
                MySqlDataReader SQLAzonositoOlvaso;
                Csatlakozas.Open();
                SQLAzonositoOlvaso = SQLAzonositoParancs.ExecuteReader();

                while (SQLAzonositoOlvaso.Read())
                {
                    SQLFelh = SQLAzonositoOlvaso.GetString(0);
                    SQLJelsz = SQLAzonositoOlvaso.GetString(1);



                }

                Csatlakozas.Close();
                login = 0;
                if (Convert.ToString(BeFelh) == SQLFelh && BeJelszo == SQLJelsz)
                {
                    login = 1;
                    Console.WriteLine("Sikeres belépés!");
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }


            loggedIn();

        }

        static void loggedIn()
        {





            if (login == 1)
            {
                Console.WriteLine("Válassza ki, hogy mit szeretne tenni! \nfeltöltés, javítás, törlés, eladás, érték, kilépés.");
                string lehetoseg = Console.ReadLine();
                if (lehetoseg == "feltöltés")
                {
                    SQL_adat_feltoltes();
                }
                if (lehetoseg == "javítás")
                {
                    SQL_adat_javitas();
                }
                if (lehetoseg == "törlés")
                {
                    SQL_adat_torles();
                }
                if (lehetoseg == "eladás")
                {
                    SQL_adat_eladas_ADAT();
                    SQL_adat_eladas();
                }
                if (lehetoseg == "kilépés")
                {
                    Thread.Sleep(5000);
                    kilepes();
                }
                if (lehetoseg=="érték")
                {
                    Ertek();
                }
                if (lehetoseg != "feltöltés" || lehetoseg != "javítás" || lehetoseg != "törlés" || lehetoseg != "eladás"||lehetoseg!="érték")
                {
                    Console.WriteLine("Kérem válasszon az alábbi listából");
                    loggedIn();
                }
            }

            else
            {
                Console.WriteLine("Rossz felhasználónév vagy jelszó");
                kezdolap();
                probalkozas++;
                masodikkor = 1;


                if (masodikkor == 1)
                {
                    Console.WriteLine("Rossz felhasználónév vagy jelszó");
                    kezdolap();
                    probalkozas++;
                }
                Console.ReadKey();
            }







            static void SQL_adat_feltoltes()
            {

                try
                {

                    Console.Write("Adja meg az azonosítót:  ");
                    int ID = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Adja meg a termék nevét: ");
                    string nev = Console.ReadLine();
                    Console.Write("Adja meg a  darab számot: ");
                    int darab = Convert.ToInt32(Console.ReadLine());
                    // StreamWriter sorszam = new StreamWriter("sorszam.txt");
                    //int Sorszam = 0;
                    //sorszam.Write(Sorszam);
                    //  sorszam.Close();
                    Console.Write("Adja meg a  darabonkénti árat: ");
                    int ar = Convert.ToInt32(Console.ReadLine());
                    StreamReader olvaso = new StreamReader("sorszam.txt");
                    int Ujsorszam = Convert.ToInt32(olvaso.ReadLine()) + 1;
                    olvaso.Close();
                    StreamWriter ujsor = new StreamWriter("sorszam.txt");
                    ujsor.Write(Ujsorszam);

                    ujsor.Close();
                    string Query = "insert into termeles.term(ID,nev,darab,sorszam,dbOsszeg) values('" + ID + "','" + nev + "','" + darab + "','" + Ujsorszam + "','"+ ar +  "');";
                    MySqlCommand SQLParancs = new MySqlCommand(Query, Csatlakozas);
                    MySqlDataReader SQLOlvaso;
                    Csatlakozas.Open();
                    StreamWriter iro = new StreamWriter(DateTime.Now.ToString("HH.mm.ss") + ".txt");

                    iro.WriteLine(ID);
                    iro.WriteLine(nev);
                    iro.WriteLine(darab);
                    iro.Close();
                    SQLOlvaso = SQLParancs.ExecuteReader();
                    while (SQLOlvaso.Read())
                    {
                    }
                    Csatlakozas.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine((ex.Message));
                }

                Console.WriteLine("Szeretne további adatot feltölteni ?(igen/nem)");
                string valasz = Console.ReadLine();
                if (valasz == "igen")
                {
                    SQL_adat_feltoltes();
                }
                else if(valasz=="nem")
                {
 
                    loggedIn();
                }
               






            }

            static void SQL_adat_javitas()
            {
                try
                {
                    MySqlConnection Csatlakozas = new MySqlConnection(Kapcsolat);
                    Console.Write("Adja meg a javítandó termék sorszámát: ");

                    int BeSorszam = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Adja meg, hogy mit szeretne változtatni: ");
                    string BeValt = (Console.ReadLine());
                    string com = "";
                    Console.Write("Adja meg, hogy mire szeretné változtatni: ");
                    if (BeValt == "id")
                    {
                        com = "update termeles.term set ID ='" + Console.ReadLine() + "' where sorszam= '" + BeSorszam + "';";
                    }
                    if (BeValt == "nev")
                    {
                        com = "update termeles.term set nev ='" + Console.ReadLine() + "' where sorszam= '" + BeSorszam + "';";
                    }
                    if (BeValt == "darab")
                    {
                        com = "update termeles.term set darab ='" + Console.ReadLine() + "' where sorszam= '" + BeSorszam + "';";
                    }
                    if (BeValt == "ar")
                    {
                        com = "update termeles.term set dbOsszeg ='" + Console.ReadLine() + "' where sorszam= '" + BeSorszam + "';";
                    }


                    MySqlCommand javitas = new MySqlCommand(com, Csatlakozas);


                    MySqlDataReader SQLOlvaso;
                    Csatlakozas.Open();
                    SQLOlvaso = javitas.ExecuteReader();

                    while (SQLOlvaso.Read())
                    {

                    }
                    Console.WriteLine("A(z) {0}. sor adata frissítve", BeSorszam);
                    Csatlakozas.Close();



                }

                catch (Exception ex)
                {
                    Console.WriteLine((ex.Message));
                }



            }
            static void SQL_adat_torles()
            {
                try
                {
                    string Kapcsolat = "datasource=localhost;port=3306;username=C;password=asd123";
                    MySqlConnection Csatlakozas = new MySqlConnection(Kapcsolat);
                    Console.Write("Adja meg a törölni kívánt sor sorszámát: ");

                    string BekertSorszam = Console.ReadLine();
                    string Parancs = "delete from termeles.term where sorszam='" + BekertSorszam + "';";
                    MySqlCommand torles = new MySqlCommand(Parancs, Csatlakozas);


                    MySqlDataReader SQLTorloOlvaso;
                    Csatlakozas.Open();
                    SQLTorloOlvaso = torles.ExecuteReader();

                    while (SQLTorloOlvaso.Read())
                    {
                    }
                    Csatlakozas.Close();
                    StreamReader olvaso = new StreamReader("sorszam.txt");

                    string sorszamTXT = olvaso.ReadLine();
                    olvaso.Close();

                    StreamWriter iro = new StreamWriter("sorszam.txt");
                    iro.WriteLine(Convert.ToInt32(sorszamTXT) - 1);




                    iro.Close();





                    Console.WriteLine("A(z) {0}. sor törlésre került.", BekertSorszam);

                }


                catch (Exception ex)
                {

                    Console.WriteLine((ex));
                }






            }
            static void SQL_adat_eladas()
            {

                string Kapcsolat = "datasource=localhost;port=3306;username=C;password=asd123";
                MySqlConnection Csatlakozas = new MySqlConnection(Kapcsolat);
                try
                {

                    string com = " ";


                    int AktErtek = Convert.ToInt32(adat) - BeEladott;
                    com = "update termeles.term set darab ='" + AktErtek + "' where sorszam= '" + AdatSorszam + "';";




                    MySqlCommand eladas = new MySqlCommand(com, Csatlakozas);


                    MySqlDataReader SQLOlvaso;
                    Csatlakozas.Open();
                    SQLOlvaso = eladas.ExecuteReader();

                    while (SQLOlvaso.Read())
                    {
                    }
                    Console.WriteLine("A(z) {0}. sor adata frissítve", AdatSorszam);
                    Csatlakozas.Close();





                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);

                }
            }
            static void SQL_adat_eladas_ADAT()
            {
                try
                {
                    string Kapcsolat = "datasource=localhost;port=3306;username=C;password=asd123";
                    MySqlConnection Csatlakozas = new MySqlConnection(Kapcsolat);
                    Console.Write("Adja meg az eladott termék sorszámát: ");
                    AdatSorszam = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Adja meg, hogy mennyit adott el: ");
                    BeEladott = Convert.ToInt32((Console.ReadLine()));
                    string command = "SELECT * FROM termeles.term WHERE sorszam='" + AdatSorszam + "';";

                    MySqlCommand AdatLekeres = new MySqlCommand(command, Csatlakozas);
                    MySqlDataReader SQLADATOlvaso;
                    Csatlakozas.Open();
                    SQLADATOlvaso = AdatLekeres.ExecuteReader();

                    while (SQLADATOlvaso.Read())
                    {
                        adat = SQLADATOlvaso.GetString(2);


                    }

                    Csatlakozas.Close();

                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                }






            }
        }
    }
}





