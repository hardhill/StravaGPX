using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace StravaGPX
{
    class RepoMy
    {
        private MySqlConnection conn;
        public RepoMy(string connString = "Host=localhost;Username=mysql;Password=My5ql;Database=infocenter3_test")
        {
            conn = new MySqlConnection(connString);
        }
        public void CreateBotLog()
        {

            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            conn.Open();
            String sql = "DROP TABLE IF EXISTS protocol_bot;" +
            "CREATE TABLE IF NOT EXISTS protocol_bot(" +
            "id_key int(11) NOT NULL AUTO_INCREMENT," +
            "id_bot int(11) NOT NULL DEFAULT '0'," +
            "status_bot int(11) DEFAULT '0'," +
            "datatime timestamp NULL," +
            "message varchar(200) ," +
            "work_result int(11) DEFAULT '0', KEY id_key (id_key)) ENGINE = InnoDB AUTO_INCREMENT =0 DEFAULT CHARSET = utf8; ";


            try
            {
                var comm = new MySqlCommand(sql, conn);
                int n = comm.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

        }
        public void CreateDict()
        {
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            conn.Open();
            String sql = "DROP TABLE IF EXISTS protocol_dict;" +
            "CREATE TABLE IF NOT EXISTS protocol_dict(" +
            "id int(11) NOT NULL AUTO_INCREMENT," +
            "id_bot int(11) NOT NULL," +
            "datatime timestamp NULL," +
            "link varchar(255), KEY id (id)) ENGINE = InnoDB AUTO_INCREMENT =0 DEFAULT CHARSET = utf8;";
            try
            {
                var comm = new MySqlCommand(sql, conn);
                int n = comm.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

        }
        public void SaveLog(int bot, int bot_status, string message, int messagetype)
        {
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            conn.Open();
            using (var cmd = new MySqlCommand("INSERT INTO protocol_bot (id_bot,status_bot,datatime,message,work_result)" +
             " VALUES (@pbot,@pstatus,@pdt,@pmsg,@pmsgtype)", conn))
            {
                cmd.Parameters.AddWithValue("pbot", bot);
                cmd.Parameters.AddWithValue("pstatus", bot_status);
                cmd.Parameters.AddWithValue("pdt", DateTime.Now);
                cmd.Parameters.AddWithValue("pmsg", message);
                cmd.Parameters.AddWithValue("pmsgtype", messagetype);
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {

                }
            }
        }
        public void SaveLinks(int bot, List<string> links)
        {
            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            conn.Open();
            foreach (string link in links)
            {
                using (var cmd = new MySqlCommand("INSERT INTO protocol_botdict (id_bot,datatime,link)" +
                     " VALUES (@pbot,@pdt,@plink)", conn))
                {
                    cmd.Parameters.AddWithValue("pbot", bot);
                    cmd.Parameters.AddWithValue("pdt", DateTime.Now);
                    cmd.Parameters.AddWithValue("plink", link);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch
                    {

                    }
                }
            }

        }
        private double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}
