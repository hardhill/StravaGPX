using HtmlAgilityPack;
using Npgsql;
using System;
using System.Collections.Generic;

namespace StravaGPX
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("STRAVA GPX loader =========================================== v. 0.1");
            var connString = "Host=localhost;Username=postgres;Password=;Database=postgres";
            NpgsqlConnection conn = new NpgsqlConnection(connString);
            conn.Open();
            if(conn.State != System.Data.ConnectionState.Open)
            {
                Console.WriteLine("Database is not opened");
                Console.WriteLine("Good bye!");
                return;
            }
            else
            {
                Console.WriteLine("Database is opened");
                
            }
            
                String sql = "DROP TABLE IF EXISTS protocol_bot;" +
                    "CREATE TABLE IF NOT EXISTS protocol_bot(" +
                    "id_key integer NOT NULL primary KEY," +
                    "id_bot integer NOT NULL," +
                    "status_bot integer," +
                    "datatime timestamp NULL," +
                    "message varchar(200) ," +
                    "work_result integer) ; ";
                var comm = new NpgsqlCommand(sql, conn);
            int n = comm.ExecuteNonQuery();
            //===========================================================
            var html = @"https://yandex.ru/";

            HtmlWeb web = new HtmlWeb();

            var htmlDoc = web.Load(html);
            List<string> hrefTags = new List<string>();

            foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
            {
                HtmlAttribute att = link.Attributes["href"];
                hrefTags.Add(att.Value);
            }

            foreach(string line in hrefTags)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine(hrefTags.Count.ToString());
           


            conn.Close();
            Console.Write("End of programm. Press Enter...");
            Console.ReadLine();
        }
        
    }
}
