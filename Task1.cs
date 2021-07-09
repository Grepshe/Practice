using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;


namespace ConsoleApp4
{
    public static class EnumerableExtensions
    {
        class MyComparator<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y)
            {
                if (x.Equals(y)) return true;
                else return false;
            }
            public int GetHashCode(T obj)
            {
                return obj.GetHashCode();
            }
        }
        static bool CheckEq<T>(IEnumerable<T> source)
        {
            MyComparator<T> comp = new MyComparator<T>();
            for (int i = 0; i < source.Count(); i++)
                for (int j = 0; j < source.Count(); j++)
                    if (i!=j && comp.Equals(source.ElementAt(i), source.ElementAt(j))) return true;

            return false;
        }
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> source)
        {
            if (CheckEq<T>(source)) throw new ArgumentException("Same elems found");
            return permutations(source.ToArray());
        }

        private static IEnumerable<IEnumerable<T>> permutations<T>(IEnumerable<T> source)
        {
            var c = source.Count();
            if (c == 1)
                yield return source;
            else
                for (int i = 0; i < c; i++)
                    foreach (var p in permutations(source.Take(i).Concat(source.Skip(i + 1))))
                        yield return source.Skip(i).Take(1).Concat(p);
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> source)
        {
            if (CheckEq<T>(source)) throw new ArgumentException("Same elems found");
            return combinations(source.ToArray());
        }

        public static IEnumerable<IEnumerable<T>> combinations<T>(IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            T[] data = source.ToArray();

            return Enumerable
              .Range(0, 1 << (data.Length))
              .Select(idx => data
                 .Where((v, i) => (idx & (1 << i)) != 0)
                 .ToArray());
        }

        public static IEnumerable<IEnumerable<T>> KRepeatCombinations<T>(this IEnumerable<T> source, int k)
        {
            if (CheckEq<T>(source)) throw new ArgumentException("Same elems found");
            return kRepeatCombinations(source.ToArray(), k);
        }

        public static IEnumerable<IEnumerable<T>> kRepeatCombinations<T>(IEnumerable<T> source, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              source.SelectMany((e, i) =>
                source.Skip(i).KRepeatCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }


    }
    class Task1
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int> { 1, 2, 3 };
            var a = list.Permutations();
            var b = list.Combinations();
            var c = list.KRepeatCombinations(2);

            return;
        }
    }
}
