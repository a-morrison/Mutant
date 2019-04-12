using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Mutant.Deploy.Factory.TestLevels
{
    public class SomeTests : ITestFinder
    {
        public string Level => "RunSpecifiedTests";
        private readonly string ANNOTATION_PATTERN = @"@test [a-zA-Z]*";

        public List<string> FindTests(DirectoryInfo SourceDirectory, List<string> Classes)
        {
            List<string> Tests = new List<string>();
            foreach (string Class in Classes)
            {
                string ClassContents = GetClassContents(SourceDirectory, Class);

                Regex TestAnnotation = new Regex(ANNOTATION_PATTERN, RegexOptions.Singleline);
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

        private string GetClassContents(DirectoryInfo SourceDirectory, string ClassName)
        {
            string FullPath = String.Concat(SourceDirectory.FullName, "\\" + ClassName);
            StreamReader In = new StreamReader(FullPath);

            string ClassAsString = In.ReadToEnd();
            In.Close();

            return ClassAsString;
        }
    }
}
