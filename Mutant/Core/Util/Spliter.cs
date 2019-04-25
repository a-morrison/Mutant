using System;
using System.Linq;

namespace Mutant.Core.Util
{
    public static class Spliter
    {
        public static SplitString Split(string StringToSplit, string SplitLocation)
        {
            SplitString split = new SplitString();
            string[] splited = StringToSplit.Split(new string[] { SplitLocation }, StringSplitOptions.None);
            split.Left = String.Join(".", splited.Take(splited.Length - 1));
            split.Right = splited.Last();
            return split;
        }
    }
}
