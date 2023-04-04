using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;

namespace EmployeDatas.Mysql
{
    class EmployeMysql
    {
        private string host;
        private int port;
        private string db;
        private string login;
        private string pwd;
        private MySqlConnection connexion;

        public EmployeMysql(string host, int port, string db, string login, string pwd)
        {
            this.host = host;
            this.port = port;
            this.db = db;
            this.login = login;
            this.pwd = pwd;
            connexion = new MySqlConnection();
        }

        public void OuvrirMySql()
        {
            String cs = String.Format("Server = {0}; Port={1} ;Database = {2}; Uid = {3}; Pwd = {4}", host, port, db, login, pwd);

            connexion = new MySqlConnection(cs);
            connexion.Open();
            Console.WriteLine("Connecté à MySql");

        }

        public void FermerMySql()
        {
            String cs = String.Format("Data Source= " + "(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))" + "(CONNECT_DATA = (SERVICE_NAME = {2}))); User Id = {3}; Password = {4};", this.host, this.port, this.db, this.login, this.pwd);

            connexion = new MySqlConnection(cs);
            connexion.Close();
            Console.WriteLine("Déconnecté de MySql");
        }


        public void AfficherTousLesEmployes()
        {
            string requete = "SELECT employe.numemp, employe.nomemp, employe.prenomemp FROM employe";
            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                Int16 numero = reader.GetInt16(0);
                string nom = reader.GetString(1);
                string prenom = reader.GetString(2);
                Console.WriteLine("Numéro : {0} | Nom : {1} | Prénom : {2}", numero, nom, prenom);
            }
            reader.Close();
        }

        public void AfficherNbSeminaires()
        {
            string requete = "SELECT count(seminaire.codesemi) FROM seminaire";
            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            object result = mySqlCommand.ExecuteScalar();
            if (result != null)
            {
                int count = Convert.ToInt16(result);
                Console.WriteLine("Nombre de séminaires : {0}", count);
            }
            else
            {
                Console.WriteLine("Il n'existe aucun séminaire");
            }
        }

        public void AfficherNbInscritsParCours()
        {
            string requete = "select seminaire.codecours, count(inscrit.numemp) from seminaire left join inscrit on inscrit.codesemi = seminaire.codesemi group by seminaire.codecours";
            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                string nbemploye = reader.GetString(0);
                Int16 nbcours = reader.GetInt16(1);
                Console.WriteLine("Codecours : {0} | Nombre d'inscrits : {1} \n", nbemploye, nbcours);
            }
            reader.Close();
        }

        public void AugmenterSalaireCurseur()
        {
            string requete = "SELECT * FROM employe WHERE employe.codeprojet = 'PR1'";
            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            int nbLignesAffectees = 0;
            while (reader.Read())
            {
                string requeteupdate = "UPDATE employe SET employe.salaire = employe.salaire * 1.03 WHERE employe.codeprojet = 'PR1'";
                MySqlCommand updateCommand = new MySqlCommand(requeteupdate, connexion);
                nbLignesAffectees += updateCommand.ExecuteNonQuery();

            }
            reader.Close();
            if (nbLignesAffectees > 0)
            {
                Console.WriteLine("Les salaires des employés du projet PR1 ont été augmenté de 3%.");
            }
            else
            {
                Console.WriteLine("Aucun salaire n'a été modifié.");
            }

        }

        public void AfficheProjetsNbEmployes(int nbEmployes)
        {
            string requete = @"SELECT projet.nomprojet, projet.codeprojet FROM projet INNER JOIN employe ON projet.codeprojet = employe.codeprojet GROUP BY projet.nomprojet, projet.codeprojet HAVING COUNT(employe.numemp) > @nbEmployes";

            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            mySqlCommand.Parameters.AddWithValue("@nbEmployes", nbEmployes);

            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine($"Aucun projet ne possède plus de {nbEmployes} employés.");
            }
            else
            {
                while (reader.Read())
                {
                    string nomprojet = reader.GetString(0);
                    string codeprojet = reader.GetString(1);
                    Console.WriteLine("Nom projet : {0} | Codeprojet : {1}", nomprojet, codeprojet);
                }
            }
            reader.Close();
        }

        public void SeminairesPosterieurs(string date)
        {

            string requete = @"SELECT seminaire.codesemi FROM seminaire WHERE seminaire.datedebutsem > @date";
            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            mySqlCommand.Parameters.AddWithValue("@date", date);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();

            if (!reader.HasRows)
            {
                Console.WriteLine($"Aucun seminaire n'existe depuis le {date}.");
            }
            else
            {
                Console.WriteLine($"Liste des séminaires existants depuis le {date} :\n");
                while (reader.Read())
                {
                    string codesemi = reader.GetString(0);
                    Console.WriteLine("Code seminaire : {0}", codesemi);
                }
            }
            reader.Close();
        }

        public void InsereProjet(string codeprojet, string nomprojet, string debutproj, string finprevue, string nomcontact)
        {
            string requete = @"INSERT INTO projet(projet.codeprojet, projet.nomprojet, projet.debutproj, projet.finprevue, projet.nomcontact) VALUES (@codeprojet, @nomprojet, @debutproj, @finprevue, @nomcontact)";
            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            mySqlCommand.Parameters.AddWithValue("@codeprojet", codeprojet);
            mySqlCommand.Parameters.AddWithValue("@nomprojet", nomprojet);
            mySqlCommand.Parameters.AddWithValue("@debutproj", debutproj);
            mySqlCommand.Parameters.AddWithValue("@finprevue", finprevue);
            mySqlCommand.Parameters.AddWithValue("@nomcontact", nomcontact);
            int nbLignesAffectees = mySqlCommand.ExecuteNonQuery();

            if (nbLignesAffectees > 0)
            {
                Console.WriteLine($"Le projet {codeprojet} a bien été ajouté.");
            }
            else
            {
                Console.WriteLine("Aucun projet ajouté.");
            }
        }

        public void SupprimerSeminaire(string codeseminaire)
        {
            string requete = @"DELETE FROM seminaire WHERE codesemi = @codeseminaire";
            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            mySqlCommand.Parameters.AddWithValue("@codeseminaire", codeseminaire);
            int nbLignesAffectees = mySqlCommand.ExecuteNonQuery();

            if(nbLignesAffectees > 0)
            {
                Console.WriteLine($"Le séminaire {codeseminaire} a été supprimé.");
            }
            else
            {
                Console.WriteLine("Aucun séminaire n'a été supprimé.");
            }
        }

        public void RajouterNbJoursCours(int nbJours, int nbJoursCours)
        {
            string requete = @"UPDATE cours SET cours.nbjours = cours.nbjours + @nbJours WHERE cours.nbjours < @nbJoursCours";
            MySqlCommand mySqlCommand = new MySqlCommand(requete, connexion);
            mySqlCommand.Parameters.AddWithValue("@nbJours", nbJours);
            mySqlCommand.Parameters.AddWithValue("@nbJoursCours", nbJoursCours);
            int nbLignesAffectees = mySqlCommand.ExecuteNonQuery();

            if(nbLignesAffectees > 0)
            {
                if(nbJours > 1)
                {
                    Console.WriteLine($"{nbJours} jours ont été ajouté aux cours ayant moins de {nbJoursCours} jour(s).");
                }
                else
                {
                    Console.WriteLine($"{nbJours} jour a été ajouté aux cours ayant moins de {nbJoursCours} jour(s).");
                }
            }
            else
            {
                Console.WriteLine("Aucun jour de cours n'a été ajouté.");
            }
        }

    }
}