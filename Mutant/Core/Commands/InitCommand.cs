﻿using System;
using System.IO;
using ManyConsole;
using Newtonsoft.Json;

namespace Mutant.Core.Commands
{
    public class InitCommand : ConsoleCommand
    {
        private struct Info
        {
            private string _workingDirectory;

            public string URL;
            public string Username;
            public string Password;
            public string WorkingDirectory {
                get
                {
                    return _workingDirectory;
                }
                set
                {
                    if (value.EndsWith(@"\"))
                    {
                        _workingDirectory = value.Remove(value.LastIndexOf(@"\"));
                    } else
                    {
                        _workingDirectory = value;
                    }
                }
            }
        }

        Info MutantInfo = new Info();

        public InitCommand()
        {
            this.IsCommand("Init", "Initializes process with common parameters like the required username and password. Should be called first.");

            this.HasRequiredOption("t|target-url=", "Required. URL of target org.", v => MutantInfo.URL = v);
            this.HasRequiredOption("u|username=", "Required. Username", v => MutantInfo.Username = v);
            this.HasRequiredOption("p|password=", "Required. Password", v => MutantInfo.Password = v);
            this.HasRequiredOption("d|working-directory=", "Required. Full path of working directory.", v => MutantInfo.WorkingDirectory = v);
        }

        public override int Run(string[] remainingArguments)
        {
            Console.WriteLine(MutantInfo.URL);
            Console.WriteLine(MutantInfo.WorkingDirectory);

            try
            {
                Directory.SetCurrentDirectory(MutantInfo.WorkingDirectory);
            }
            catch(DirectoryNotFoundException)
            {
                Console.WriteLine(MutantInfo.WorkingDirectory + " is not a valid directory!");
                return 1;
            }

            using (StreamWriter file = File.CreateText(MutantInfo.WorkingDirectory + @"\.credentials"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, MutantInfo);
            }

            return 0;
        }
    }
}
