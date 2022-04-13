using System;
using System.Collections.Generic;
using System.Linq;
using static ConsoleApp1.Superdigits;
using static ConsoleApp1.Constants<int>;

using H = ConsoleApp1.Constants<int>.Member<Test<string>.Yulp<int>,float>;
using S = System;

public class C
{

}

namespace ConsoleApp1
{
    

    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> numbers = new List<int> { 1, 3, 5, 7, 11, 21, 34, 41, 56, 63 };

            var p = Whatever2(numbers, (a,b) => b - a).ToList();

            var ints = Superdigit("13756723567");

        }


        static IEnumerable<(int,int)> ToPairs(IEnumerable<int> Input)
        {
            while (Input.Count() > 1)
            {
                yield return (Input.First(), Input.Skip(1).First());
                Input = Input.Skip(1).ToList();
            }
        }
        public static IEnumerable<U> Whatever2<T,U>(IEnumerable<T> list, Func<T,T,U> Function)
        {
            return list.Zip(list.Skip(1), (a,b) => Function(a,b));
        }
    }

    public static class Superdigits
    {
        private static readonly int[] table1 = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        private static readonly int[,] table2 = new int[10, 10]
{
            {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            {1, 2, 3, 4, 5, 6, 7, 8, 9, 0 },
            {2, 3, 4, 5, 6, 7, 8, 9, 0, 1 },
            {3, 4, 5, 6, 7, 8, 9, 0, 1, 2 },
            {4, 5, 6, 7, 8, 9, 0, 1, 2, 3 },
            {5, 6, 7, 8, 9, 0, 1, 2, 3, 4 },
            {6, 7, 8, 9, 0, 1, 2, 3, 4, 5 },
            {7, 8, 9, 0, 1, 2, 3, 4, 5, 6 },
            {8, 9, 0, 1, 2, 3, 4, 5, 6, 7 },
            {9, 0, 1, 2, 3, 4, 5, 6, 7, 8 }
};


        public static IEnumerable<int> ToDigits (string digits)
        {
            return digits.ToCharArray().Select(x => ((int)x) - 48);
        }

        public static int Superdigit(string Digits)
        {
            return Superdigit1(ToDigits(Digits));
        }

        public static int Superdigit1(IEnumerable<int> Digits)
        {
            return Digits.Aggregate((a, b) => table1[a + b]);
        }
    }
} 

public struct S
{

}