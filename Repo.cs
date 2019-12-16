using System;
using System.Collections.Generic;
using Npgsql;

namespace StravaGPX
{
    public class Repo
    {

        private NpgsqlConnection conn;
        public Repo(string connString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres")
        {
            conn = new NpgsqlConnection(connString);
        }
        public void CreateBotLog()
        {

            if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            conn.Open();
            String sql = "DROP TABLE IF EXISTS protocol_bot;" +
            "CREATE TABLE IF NOT EXISTS protocol_bot(" +
            "id_key serial NOT NULL primary KEY," +
            "id_bot integer NOT NULL," +
            "status_bot integer," +
            "datatime timestamp NULL," +
            "message varchar(200) ," +
            "work_result integer) ; ";
            try
            {
                var comm = new NpgsqlCommand(sql, conn);
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
            String sql = "DROP TABLE IF EXISTS protocol_botdict;" +
            "CREATE TABLE IF NOT EXISTS protocol_botdict(" +
            "id serial NOT NULL primary KEY," +
            "id_bot integer NOT NULL," +
            "datatime timestamp NULL," +
            "link varchar)";
            try
            {
                var comm = new NpgsqlCommand(sql, conn);
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
            using (var cmd = new NpgsqlCommand("INSERT INTO protocol_bot (id_bot,status_bot,datatime,message,work_result)" +
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
                using (var cmd = new NpgsqlCommand("INSERT INTO protocol_botdict (id_bot,datatime,link)" +
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