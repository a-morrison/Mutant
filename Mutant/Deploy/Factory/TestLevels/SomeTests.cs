using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Mutant.Deploy.Factory.TestLevels
{
    public class SomeTests : ITestLevel
    {
        public string Level => "RunSpecifiedTests";
        private const string ANNOTATION_PATTERN = @"@test .*";

        public List<string> FindTests(DirectoryInfo SourceDirectory)
        {
            Console.WriteLine("Here in " + SourceDirectory.ToString());
            List<string> Tests = new List<string>();
            foreach (string Class in Directory.EnumerateFiles(SourceDirectory.ToString(), "*.cls"))
            {
                string ClassContents = GetClassContents(Class);

                Regex TestAnnotation = new Regex(ANNOTATION_PATTERN, RegexOptions.None);
                Match AnnotationMatch = TestAnnotation.Match(ClassContents);
                if (AnnotationMatch.Success)
                {
                    string FirstMatch = AnnotationMatch.Value;
                    FirstMatch = FirstMatch.Replace("@test", "").Trim();
                    Tests.Add(FirstMatch);
                }
            }
            return Tests;
        }

        private string GetClassContents(string ClassName)
        {
            StreamReader In = new StreamReader(ClassName);

            string ClassAsString = In.ReadToEnd();
            In.Close();

            return ClassAsString;
        }
    }
}
