using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

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
            String cs = String.Format("Data Source= " + "(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))" + "(CONNECT_DATA = (SERVICE_NAME = {2}))); User Id = {3}; Password = {4};", this.host, this.port, this.db, this.login, this.pwd);

            connexion = new MySqlConnection(cs);
            connexion.Open();

        }

        public void FermerMySql()
        {
            String cs = String.Format("Data Source= " + "(DESCRIPTION = (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {1}))" + "(CONNECT_DATA = (SERVICE_NAME = {2}))); User Id = {3}; Password = {4};", this.host, this.port, this.db, this.login, this.pwd);

            connexion = new MySqlConnection(cs);
            connexion.Close();
        }

        public string AfficherTousLesEmployes()
        {
            string requete = "SELECT * FROM employe";
            string chaine = "";
            this.OuvrirMySql();
            MySqlCommand command = connexion.CreateCommand();
            command.CommandText = requete;
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                chaine += "ID : " + reader.GetString(0) + " | " + "Nom : " + reader.GetString(1) + "Prénom : " + reader.GetString(3) + "\n";
            }
            reader.Close();
            this.FermerMySql();
            return chaine;
        }
    }
}
