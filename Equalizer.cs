using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClinicWebApplication
{
    public static class Equalizer
    {
        public static bool CompareChars(char x, char y)
        {
            List<char> eng = new List<char> { 'A', 'B', 'C', 'E', 'H', 'I', 'K', 'M', 'O', 'P', 'T', 'X', 'a', 'c', 'e', 'i', 'o', 'p', 'x', 'y' };
            List<char> ukr = new List<char> { 'А', 'В', 'С', 'Е', 'Н', 'І', 'К', 'М', 'О', 'Р', 'Т', 'Х', 'а', 'с', 'е', 'і', 'о', 'р', 'х', 'у' };
            for (int i = 0; i < eng.Count; i++)
                if ((x == eng[i] && y == ukr[i]) || (x == ukr[i] && y == eng[i]))
                    return true;
            return x == y;
        }

        public static bool CompareStrings(string x, string y)
        {
            if (x.Length != y.Length)
                return false;
            for (int i = 0; i < x.Length; i++)
                if (!CompareChars(x[i], y[i]))
                    return false;
            return true;
        }
    }
}
