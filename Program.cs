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

            Repo repo = new Repo();
            repo.CreateBotLog();
            repo.CreateDict();
            repo.CreateDict();
            Console.WriteLine("Tables are created");
            //===========================================================
            repo.SaveLog(1, (int)BotStatus.WORK, "Робот начал работу", (int)MessageType.INFORM);
            var html_link = @"https://yandex.ru/";
            Parser parser = new Parser(html_link, 256);

            HtmlWeb web = new HtmlWeb();

            do
            {
                //очередная ссылка
                repo.SaveLog(1, (int)BotStatus.WORK, "Обработка link", (int)MessageType.INFORM);
                String queue_link = parser.GetLink();
                repo.SaveLog(1, (int)BotStatus.WORK, "link: " + queue_link.Substring(0, 120) + "...", (int)MessageType.SUCCESS);
                Console.WriteLine(queue_link);
                try
                {
                    var htmlDoc = web.Load(queue_link);
                    List<string> hrefTags = new List<string>();

                    foreach (HtmlNode link in htmlDoc.DocumentNode.SelectNodes("//a[@href]"))
                    {
                        HtmlAttribute att = link.Attributes["href"];
                        hrefTags.Add(att.Value);
                    }
                    repo.SaveLog(1, (int)BotStatus.WORK, "Найдено ссылок: " + hrefTags.Count.ToString(), (int)MessageType.INFORM);
                    Console.Write("=============Found links: "); Console.WriteLine(hrefTags.Count);
                    repo.SaveLog(1, (int)BotStatus.WORK, "Добавлено в словарь: " + parser.AddList(hrefTags).ToString(), (int)MessageType.SUCCESS);
                    Console.Write("================ dict value: "); Console.WriteLine(parser.GetVolueDict());
                    Console.Write("================ queue value: "); Console.WriteLine(parser.StackSize());
                }
                catch (Exception e)
                {
                    Console.Write("=ERROR= : "); Console.WriteLine(e.Message);
                    repo.SaveLog(1, (int)BotStatus.WORK, "Error: " + e.Message, (int)MessageType.ERROR);
                }

            } while (parser.StackSize() > 0);
            repo.SaveLog(1, (int)BotStatus.WORK, "Работа по обработке завершена", (int)MessageType.SUCCESS);
            repo.SaveLog(1, (int)BotStatus.WORK, "Записываю словарь", (int)MessageType.INFORM);
            try
            {
                repo.SaveLinks(1, parser.GetDict());
                repo.SaveLog(1, (int)BotStatus.WORK, "Словарь записан", (int)MessageType.SUCCESS);
            }
            catch
            {
                repo.SaveLog(1, (int)BotStatus.WORK, "ERROR - ошибка записи в словарь", (int)MessageType.ERROR);
            }
            repo.SaveLog(1, (int)BotStatus.CLOSE, "Робот выключен", (int)MessageType.INFORM);

            Console.Write("End of programm. Press Enter...");
            Console.ReadLine();
        }

    }
}
