using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonAlgorithms.Numerical
{
    public static class MaximumSubArray
    {
        /// <summary>
        /// Solves the problem using brute force (by comparing sums for all possible 2 indexes, C(n,2) possible pairs of indexes). Complexity is at least O(n^2)
        /// </summary>
        /// <returns>The largest sum, start/end indexes</returns>
        public static Tuple<int, int, int> GetMaxSubArrayBruteForce(int[] input)
        {
            // form an array of indexes(0,1,2...input.length-1)
            var set = new List<int>(input.Length);
            for (int i = 0; i < input.Length; i++)
                set.Add(i);

            // generate all C(n,2) combinations ofor indexes of input array(the first index is less than 2nd)
            var combinations = CombinationsAndPermutations<int>.NCombinations(set,2);   //assume O(1)
            int sum, maxSum = 0;
            int startIndex = 0, endIndex = 0;

            // compute the sum from each combination. The biggest wins.
            foreach (var comb in combinations)      //C(n,2)~n^2 elemnts
            {
                sum = 0;
                for (int j = comb[0]; j <= comb[1]; j++)
                    sum += input[j];

                if (sum >= maxSum)
                {
                    maxSum = sum;
                    startIndex = comb[0];
                    endIndex = comb[1];
                }
            }
            return new Tuple<int, int, int>(maxSum, startIndex, endIndex);
        }
        /// <summary>
        /// Returns maximum subarray for given input (continous array with largest sum among all subarrays). Complexity O(N x logN). 
        /// </summary>
        public static Tuple<int, int, int> GetMaxSubArray(int[] input)
        {
            return GetMaxSubArrayInner(input, 0, input.Length-1);
        }
        private static Tuple<int, int, int> GetMaxSubArrayInner(int[] input,int start,int end)
        {
            if (start == end) return new Tuple<int,int,int>(input[start], start, end);  //base step

            //recursively find max subarrays of left,right parts and midpoint crossing max subarray
            int mid = (start+end) / 2;                                              
            var left = GetMaxSubArrayInner(input, start, mid);
            var right = GetMaxSubArrayInner(input, mid+1, end);
            var middle = GetMaxSubArrayCrossingMidpoint(input,start,mid,end);

            //combine step
            if (left.Item1 >= right.Item1 && left.Item1 >= middle.Item1) return left;
            if (right.Item1 >= left.Item1 && right.Item1 >= middle.Item1) return right;
            return middle;
        }
        /// <summary>
        /// Finds the maximum subarray that crosses the midpoint. Complexity is linear
        /// </summary>
        private static Tuple<int, int, int> GetMaxSubArrayCrossingMidpoint(int[] input, int start, int mid, int end)
        {
            // find max sub array in the left part
            int sum = 0, leftSum = int.MinValue, leftIndex = mid;
            for (int i = mid; i >= start; i--)
            {
                sum += input[i];
                if (sum > leftSum)
                {
                    leftSum = sum;
                    leftIndex = i;
                }
            }
            // find max sub array in the right part
            sum = 0;
            int rightIndex = mid+1, rightSum = int.MinValue;
            for (int i = mid+1; i <= end; i++)
            {
                sum += input[i];
                if (sum > rightSum)
                {
                    rightSum = sum;
                    rightIndex = i;
                }
            }
            return new Tuple<int, int, int>(leftSum+rightSum,leftIndex,rightIndex);
        }
    }
}
