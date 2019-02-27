using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Mutant.Core
{
    public class Credentials
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string URL { get; private set; }
        public string WorkingDirectory { get; private set; }

        public Credentials()
        {
            Load();
        }

        private void Load()
        {
            string CurrentDirectory = Directory.GetCurrentDirectory();
            try
            {
                JObject JsonFile = JObject.Parse(File.ReadAllText(CurrentDirectory + @"\.credentials"));

                this.Username = (string)JsonFile["Username"];
                this.Password = (string)JsonFile["Password"];
                this.URL = (string)JsonFile["URL"];
                this.WorkingDirectory = (string)JsonFile["WorkingDirectory"];
            } catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Credentails file not found! Please run Mutant Init to load required credentials.");
            }
            
        }
    }
}
