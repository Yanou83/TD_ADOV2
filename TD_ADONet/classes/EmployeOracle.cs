using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace EmployeDatas.Oracle
{
    class EmployeOracle
    {
        private string host;
        private int port;
        private string db;
        private string login;
        private string pwd;
        private OracleConnection connexion;


        public EmployeOracle(string host, int port, string db, string login, string pwd)
        {
            this.host = host;
            this.port = port;
            this.db = db;
            this.login = login;
            this.pwd = pwd;
            connexion = new OracleConnection();
        }

        public void OuvrirOracle()
        {
            String cs = String.Format("Data Source= " + "(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))" + "(CONNECT_DATA = (SERVICE_NAME = {2}))); User Id = {3}; Password = {4};", this.host, this.port, this.db, this.login, this.pwd);

            connexion = new OracleConnection(cs);
            connexion.Open();
            Console.WriteLine("Connecté Oracle");

        }

        public void FermerOracle()
        {
            String cs = String.Format("Data Source= " + "(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))" + "(CONNECT_DATA = (SERVICE_NAME = {2}))); User Id = {3}; Password = {4};", this.host, this.port, this.db, this.login, this.pwd);

            connexion = new OracleConnection(cs);
            connexion.Close();
            Console.WriteLine("Déconnecté Oracle");
        }

        public string AfficherTousLesCours()
        {
            string requete = "SELECT * FROM cours";
            string chaine = "";
            OracleCommand command = connexion.CreateCommand();
            command.CommandText = requete;
            OracleDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                chaine += "CODECOURS : " + reader.GetString(0) + " | " + "LIBELLECOURS : " + reader.GetString(1) + " | " + "NBJOURS : " + reader.GetString(2) + "\n";
            }
            reader.Close();
            return chaine;
        }

        public void AfficherNbProjets()
        {
            string requete = "SELECT COUNT(projet.codeprojet) AS nb_projets FROM projet";
            OracleCommand oracleCommand = connexion.CreateCommand();
            oracleCommand.CommandText = requete;
            object result = oracleCommand.ExecuteScalar();
            if (result != null)
            {
                int count = Convert.ToInt32(result);
                Console.WriteLine("Nombre de projets : " + count);
            }
            else
            {
                Console.WriteLine("Aucun projet trouvé");
            }
        }


        public void AfficherSalaireMoyenParProjet()
        {
            string requete = "SELECT COALESCE(projet.codeprojet, 'Aucun') AS codeprojet, projet.nomprojet, COUNT(DISTINCT employe.numemp), ROUND(AVG(employe.salaire),2) AS moysalaire FROM employe LEFT JOIN projet ON projet.codeprojet = employe.codeprojet GROUP BY COALESCE(projet.codeprojet, 'Aucun'), projet.nomprojet";
            OracleCommand oracleCommand = connexion.CreateCommand();
            oracleCommand.CommandText = requete;
            OracleDataReader reader = oracleCommand.ExecuteReader();
            while (reader.Read())
            {
                string codeProjet = reader.GetString(0);
                string nomProjet = reader.IsDBNull(1) ? "null" : reader.GetString(1);
                int nbEmployes = reader.GetInt32(2);
                decimal salaireMoyen = reader.GetDecimal(3);
                Console.WriteLine("Code projet : {0} | Nom projet : {1} | Nombre d'employés : {2} | Salaire moyen : {3}", codeProjet, nomProjet, nbEmployes, salaireMoyen);
            }
            reader.Close();
        }


        public void AugmenterSalaireCurseur()
        {
            string r1 = "SELECT * FROM employe WHERE employe.codeprojet = 'PR1'";
            OracleCommand oracleCommand1 = connexion.CreateCommand();
            oracleCommand1.CommandText = r1;
            OracleDataReader reader = oracleCommand1.ExecuteReader();
            int nombreLignesAffectees = oracleCommand1.ExecuteNonQuery();
            while (reader.Read())
            {
                string requeteupdate = "UPDATE employe SET employe.salaire = employe.salaire * 1.03";
                oracleCommand1.CommandText = requeteupdate;

            }
            if (nombreLignesAffectees > 0)
            {
                Console.WriteLine("Les salaires des employés du projet PR1 ont bien été augmenté de 3%");
            }
            else
            {
                Console.WriteLine("Rien n'a été modifié");
            }

            reader.Close();
        }

        public void AfficherEmployesSalaire(int salaireMAX)
        {
            string requete = "SELECT employe.numemp, employe.nomemp, employe.prenomemp, employe.salaire FROM employe WHERE employe.salaire < :salaireMAX";
            OracleCommand oracleCommand = new OracleCommand(requete, connexion);
            oracleCommand.Parameters.Add(new OracleParameter(":salaireMAX", salaireMAX));
            OracleDataReader reader = oracleCommand.ExecuteReader();
            while (reader.Read())
            {
                string numemp = reader.GetString(0);
                string nomemp = reader.GetString(1);
                string prenomemp = reader.GetString(2);
                int salaire = reader.GetInt32(3);
                Console.WriteLine("Numéro employé : {0} | Nom : {1} | Prénom : {2} | Salaire : {3}", numemp, nomemp, prenomemp, salaire);
            }
            reader.Close();
        }

        public void AfficheSalaireEmploye(int numemp)
        {
            string requete = "SELECT employe.numemp, employe.nomemp, employe.prenomemp, employe.salaire FROM employe WHERE employe.numemp = :numemp";
            OracleCommand oracleCommand = new OracleCommand(requete, connexion);
            oracleCommand.Parameters.Add(new OracleParameter(":numemp", numemp));
            OracleDataReader reader = oracleCommand.ExecuteReader();
            while (reader.Read())
            {
                numemp = reader.GetInt16(0);
                string nomemp = reader.GetString(1);
                string prenomemp = reader.GetString(2);
                int salaire = reader.GetInt32(3);
                Console.WriteLine("Numéro employé : {0} | Nom : {1} | Prénom : {2} | Salaire : {3}", numemp, nomemp, prenomemp, salaire);
            }
            reader.Close();
        }

        public void InsereCours()
        {
            string requete = "insert into cours(cours.codecours, cours.libellecours, cours.nbjours) values ('BR099', 'Apprentissage JDBC', 4)";
            OracleCommand oracleCommand = connexion.CreateCommand();
            oracleCommand.CommandText = requete;
            int nbLignesAffectes = oracleCommand.ExecuteNonQuery();
            if (nbLignesAffectes > 0)
            {
                Console.WriteLine("Le cours BR099 a bien été ajouté");
            }
        }

        public void SupprimeCours(string codeCours)
        {
            string requete = "DELETE FROM cours WHERE codecours = :codeCours";
            OracleCommand oracleCommand = new OracleCommand(requete, connexion);
            oracleCommand.Parameters.Add(new OracleParameter(":codeCours", codeCours));
            int nbLignesAffectes = oracleCommand.ExecuteNonQuery();
            if (nbLignesAffectes > 0)
            {
                Console.WriteLine("Le cours " + codeCours + " a bien été supprimé de la liste des cours.");
            }
        }

        public void AugmenterSalaire(double pourcentage, string codeProjet)
        {
            double convertPourc = 1 + (pourcentage / 100);
            string requete = "UPDATE employe SET salaire = salaire * :convertPourc WHERE employe.codeprojet = :codeProjet";
            OracleCommand oracleCommand = new OracleCommand(requete, connexion);
            oracleCommand.Parameters.Add(new OracleParameter(":convertPourc", convertPourc));
            oracleCommand.Parameters.Add(new OracleParameter(":codeProjet", OracleDbType.Char, 4)).Value = codeProjet;
            int nbLignesAffectes = oracleCommand.ExecuteNonQuery();
            Console.WriteLine("{0} lignes ont été mises à jour", nbLignesAffectes);
        }
    }
}