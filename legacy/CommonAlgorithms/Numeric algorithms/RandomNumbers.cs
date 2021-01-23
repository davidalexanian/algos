using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.Numerical
{
    /// <summary>
    /// Pseudo number generator which uses "X(n)=[AX(n-1)+B] mod M" recursive formula. It produces numbers between 0 and M-1 (0,uint.MaxValue-1)
    /// </summary>
    /// <remarks>Because modulo M is used to generate the random numbers, after at most m numbers generator produces duplicate numbers. </remarks>
    public class CongruentalRandomNumberGenerator
    {
        private ulong A;
        private ulong B;
        private ulong seed;
        private uint M;
        private Nullable<uint> lastGenVariable;

        /// <summary>
        /// Uses custom values to initialize A,B,seed,M variables
        /// </summary>
        public CongruentalRandomNumberGenerator()
        {
            this.A = 16807;
            this.B = (ulong)new Random().Next();
            this.seed = 11;
            this.M = int.MaxValue;
        }
        /// <summary>
        /// Random numbers are generated using "X(n+1)=[A*X(n)+B] mod M" formula. It generates variable from (0,M-1) range
        /// </summary>
        /// <param name="A">Constant linear coefficient</param>
        /// <param name="B">Constant sum</param>
        /// <param name="seed">Initial value X(0) other than 0. Small diviations of seeds' values do not affect randomness.</param>
        /// <param name="M">Modulo. Largest value is ulong.MaxValue</param>
        public CongruentalRandomNumberGenerator(ushort A, uint B, uint seed, uint M)
        {
            if (A == 0) throw new ArgumentException("A must not be 0");
            if (seed == 0) throw new ArgumentException("Seed must not be 0");
            if (M <= 1) throw new ArgumentException("M must not be 0 or 1");
            this.A = A; this.B = B; this.seed = seed; this.M = M;
        }
        /// <summary>
        /// Generates a random variable from (0,M-1) range
        /// </summary>
        public uint Next()
        {
            checked
            {
                ulong l;
                if (lastGenVariable.HasValue)
                    l = A * lastGenVariable.Value + B;
                else
                    l = A * seed + B;
                lastGenVariable = (uint)(l % M);  //guaranted to be < uint.MaxValue                
            }
            return lastGenVariable.Value;
        }
        /// <summary>
        /// Generates a random variable in (0,1) range
        /// </summary>
        public double NextDouble()
        {
            this.Next();
            checked
            {
                int charsCount = lastGenVariable.Value.ToString().Length;
                double d = lastGenVariable.Value;
                return d / M;
            }
        }
        /// <summary>
        /// Returns a pseudo random variable from specified range. Uses min + <generated_rnd_var between 0 & 1> * (max-min) formula.
        /// </summary>
        public int Next(int minValue, int maxValue)
        {
            checked
            {
                if (minValue == maxValue) throw new InvalidOperationException("min and max values are the same");
                if (minValue > maxValue) throw new InvalidOperationException("minValue is bigger than maxValue");
                this.Next();
                double d = lastGenVariable.Value;
                return (int)(minValue + d / M * (maxValue - minValue));
            }
        }
        /// <summary>
        /// Generates sequence of n nonnegative pseudo random variables. If m is less than n, sequence contains duplicates.
        /// </summary>
        public uint[] GenerateSequence(int n)
        {
            n = System.Math.Abs(n);
            uint[] seq = new uint[n];
            for (int i = 1; i <= n; i++) {
                seq[i - 1] = this.Next();
            }
            return seq;
        }
        /// <summary>
        /// Generates sequence of n pseudo random variables from the given range. Duplications are possible if n >= m or maxValue-minValue is close to n.
        /// </summary>
        public int[] GenerateSequence(int n, int minValue, int maxValue)
        {
            n = System.Math.Abs(n);
            int[] seq = new int[n];
            for (int i = 1; i <= n; i++)
            {
                seq[i - 1] = this.Next(minValue, maxValue);
            }
            return seq;
        }
        /// <summary>
        /// Resets generator as if there were no generated random variables 
        /// </summary>
        public void Reset()
        {
            lastGenVariable = null;
        }
        /// <summary>
        /// Returns last generated random variable or null if no variable has been generated yet.
        /// </summary>
        public uint? LastGeneratedVariable
        {
            get { return lastGenVariable; }
        }
        public static void RandomizeInput<T>(IList<T> collection)
        {
            if (collection == null || collection.Count==0) return;
            Random r = new Random();
            for (int i = 0; i <= collection.Count - 1; i++)
            {
                int j = r.Next(0, collection.Count - 1);    // rnd number from [0,length-1] range
                T item = collection[i];                             // swap values of i,j indices
                collection[i] = collection[j];
                collection[j] = item;
            }
        }
    }
}
