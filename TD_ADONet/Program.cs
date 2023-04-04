using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using EmployeDatas.Mysql;
using EmployeDatas.Oracle;


namespace TD_ADONet
{
    class Program
    {
        static void Main(string[] args)
        {
            String host = "freesio.lyc-bonaparte.fr";
            int port = 21521;
            string sid = "slam";
            string login = "montenotado";
            string pwd = "sio";

            //try
            //{

            //    EmployeOracle cs1 = new EmployeOracle(host, port, sid, login, pwd);
            //    cs1.OuvrirOracle();


            //    //Console.WriteLine(cs1.AfficherTousLesCours());
            //    cs1.AfficherNbProjets();
            //    //cs1.AfficherSalaireMoyenParProjet();
            //    //Console.WriteLine(cs1.AugmenterSalaireCurseur());
            //    //cs1.AfficherEmployesSalaire(125000);
            //    cs1.AfficheSalaireEmploye(5);
            //    ////cs1.InsereCours();
            //    ////cs1.SupprimeCours("BR099");
            //    //cs1.AugmenterSalaire(5, "PR2");


            //    cs1.FermerOracle();
            //}
            //catch (OracleException ex)
            //{
            //    Console.WriteLine("Erreur Oracle " + ex.Message);
            //}


            string hostMysql = "127.0.0.1";
            int portMysql = 3306;
            string baseMysql = "dbadonet";
            String uidMysql = "employeado";
            String pwdMysql = "employeado123";
            try
            {
                EmployeMysql cs = new EmployeMysql(hostMysql, portMysql, baseMysql, uidMysql, pwdMysql);
                cs.OuvrirMySql();

                //cs.AfficherTousLesEmployes();
                //cs.AfficherNbSeminaires();
                //cs.AfficherNbInscritsParCours();
                ////cs.AugmenterSalaireCurseur();
                cs.AfficheProjetsNbEmployes(0);
                //cs.SeminairesPosterieurs("2018-12-15");
                //cs.InsereProjet("PR6", "Projet Programmation", "2023-04-04", "2023-05-09", "Monsieur MONTENOT");
                //cs.SupprimerSeminaire("BR0350216");
                //cs.RajouterNbJoursCours(2, 4);


                cs.FermerMySql();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
