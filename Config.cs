using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StravaGPX
{
    class Config
    {
        private ConfigurationBuilder builder;
        public Config()
        {
            builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        }

        public string ConnectionMySql()
        {
          IConfigurationRoot configurationRoot =  builder.Build();
            IConfigurationSection section = configurationRoot.GetSection("ConnectionMySql");
            var host = section.GetSection("Host").Value;
            var port = section.GetSection("Port").Value;
            var db = section.GetSection("Database").Value;
            var user = section.GetSection("User").Value;
            var pass = section.GetSection("Password").Value;
            return $"Host={host};Port={port};Database={db};Username={user};Password={pass};";
        }

        public string ConnectionPgSql()
        {
            IConfigurationRoot configurationRoot = builder.Build();
            IConfigurationSection section = configurationRoot.GetSection("ConnectionPgSql");
            var host = section.GetSection("Host").Value;
            var port = section.GetSection("Port").Value;
            var db = section.GetSection("Database").Value;
            var user = section.GetSection("User").Value;
            var pass = section.GetSection("Password").Value;
            return $"Host={host};Port={port};Database={db};Username={user};Password={pass};";
        }

        public Proxy GetProxy()
        {
            Proxy proxy;
            proxy.Host = ""; proxy.Port = 0; proxy.User = ""; proxy.Password = "";
            IConfigurationRoot configurationRoot = builder.Build();
            IConfigurationSection section = configurationRoot.GetSection("Proxy");
            proxy.Host = section.GetSection("Host").Value;
            try
            {
                proxy.Port = Convert.ToInt32(section.GetSection("Port").Value);
            }
            catch { proxy.Port = 0; }
            return proxy;
        }
        public int GetDictVolume()
        {
            IConfigurationRoot configurationRoot = builder.Build();
            IConfigurationSection section = configurationRoot.GetSection("Parsing");
            string otvet = section.GetSection("Dict").Value;
            return otvet==""?0:Convert.ToInt32(otvet);
        }
        public string GetUrl()
        {
            IConfigurationRoot configurationRoot = builder.Build();
            IConfigurationSection section = configurationRoot.GetSection("Parsing");
            string otvet = section.GetSection("Url").Value;
            return otvet;
        }

        public struct Proxy
        {
            public string Host;
            public int Port;
            public string User;
            public string Password;
            public Proxy(string host, int port, string user, string password)
            {
                Host = host;
                Port = port;
                User = user;
                Password = password;
            }
        }
    }
}
