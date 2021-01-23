using System;
using System.Collections;
using System.Collections.Generic;

namespace CommonAlgorithms.Numerical
{
    public static class CombinationsAndPermutations<T>
    {
        /// <summary>
        /// Returns all k combinations of given set
        /// </summary>
        public static List<List<T>> NCombinations(List<T> set, uint k)
        {
            int cap = (int)Recursive.Factorial((uint)set.Count) / (int)Recursive.Factorial(k) * (int)Recursive.Factorial((uint)set.Count - k);
            var results = new List<List<T>>(cap);
            NCombinationsInner(0, new int[k], set, results);
            return results;
        }
        private static void NCombinationsInner(int index, int[] selections, List<T> set, List<List<T>> results)
        {
            if (index == selections.Length)
            {
                List<T> combination = new List<T>();
                for (int i = 0; i < selections.Length; i++)
                    combination.Add(set[selections[i]]);
                results.Add(combination);
            }
            else
            {
                int start = 0;
                if (index > 0) start = selections[index - 1] + 1;
                for (int i = start; i < set.Count; i++)
                {
                    selections[index] = i;
                    NCombinationsInner(index + 1, selections, set, results);
                }
            }
        }
        /// <summary>
        /// Generates all n permutations of given set (complexity O(N^r), N-elements of set, r-permutations)
        /// </summary>
        public static List<List<T>> NPermutations(List<T> set, uint k, bool duplicationsAllowed)
        {
            if (set.Count < k) throw new ArgumentException("n is bigger than the number of set elements");
            if (k==0) throw new ArgumentException("Choose n between [1,n]");

            List<List<T>> result;
            if (k == 1)
            {
                result = new List<List<T>>();
                for (int i = 0; i < set.Count; i++)
                {
                    var comb = new List<T>();
                    comb.Add(set[i]);
                    result.Add(comb);
                }
                return result;
            }

            result = new List<List<T>>();
            var NMinusOneCombinations = NPermutations(set, k - 1, duplicationsAllowed);
            foreach (var comb in NMinusOneCombinations)
            { 
                for (int i = 0; i < set.Count; i++)
                {
                    if (duplicationsAllowed)
                    {
                        List<T> c = new List<T>(comb);              //assume constant time
                        c.Add(set[i]);
                        result.Add(c);
                    }
                    else
                    {
                        if (!comb.Contains(set[i]))                 //assume constant time
                        {
                            List<T> c = new List<T>(comb);
                            c.Add(set[i]);
                            result.Add(c);
                        }
                    }
                }
            }
            return result;
        }
        public static List<string> ExampleOf3CombinationWithLoops(string[] set)
        {
            var result = new List<string>();
            for (int i = 0; i < set.Length; i++)
                for (int j = i + 1; j < set.Length; j++)
                    for (int k = j + 1; k < set.Length; k++)
                        result.Add("(" + set[i] + "," + set[j] + "," + set[k] + ")");
            return result;
        }
    }

}
