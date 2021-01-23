using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.Numerical
{
    /// <summary>
    /// Contains miscellaneous numeric algorithms (specified complexities are not expressed in terms of digits counts)
    /// </summary>
    public static class NumericAlgorithms
    {
        /// <summary>
        /// Resolves GCD of 2 integers using the fact that GCD(a,b)=GCD(b,rem(a,b)). Complexity is logarithmic.
        /// </summary>
        public static uint GreatestCommonDivisor(uint a, uint b)
        {
            while (b != 0)
            {
                uint r = a % b;
                a = b;
                b = r;
            }
            return a;
        }
        /// <summary>
        /// Resolves GCD of 2 integers using subtractions method. Complexity is linear.
        /// </summary>
        public static uint GreatestCommonDivisorRecursive(uint a, uint b)
        {
            if (a == b) return a;
            if (a > b) return GreatestCommonDivisorRecursive(a - b, b);
            return GreatestCommonDivisorRecursive(a, b - a);
        }
        /// <summary>
        /// Computes least common multiple(LCM) using LCM = a x b / GCD(a,b) formula. Complexity is logarithmic.
        /// </summary>
        public static uint LeastCommonMultipleByGCD(uint a, uint b)
        {
            return a * b / GreatestCommonDivisor(a, b);
        }
        /// <summary>
        /// Computes least common multiple(LCM) by comparing prime factors of given numbers. Complexity is 
        /// </summary>
        public static decimal LeastCommonMultiple(uint a, uint b)
        {
            var factorsA = PrimeFactors(a);
            var factorsB = PrimeFactors(b);
            if (factorsA.Count == 0 || factorsB.Count == 0) return a * b;   //if one of th numbers is prime lcm = a x b.
            factorsA.Sort();
            factorsB.Sort();

            decimal lcm = 1;
            int i = 0, j = 0, countA, countB;
            ulong factorA=0, factorB=0;
            while (true)
            {
                if (i < factorsA.Count && j < factorsB.Count)
                {
                    factorA = factorsA[i];
                    factorB = factorsB[j];
                    if (factorA == factorB)
                    {
                        countA = 0; countB = 0;
                        while (i < factorsA.Count && factorA == factorsA[i]) { countA++; i++; }  //i,j point to the next factors in A & B
                        while (j < factorsB.Count && factorB == factorsB[j]) { countB++; j++; }
                        lcm = lcm * ExponentBySquaring(factorA, (uint)Math.Max(countA, countB));
                    }
                    else if (factorA < factorB)       //factorsB does not contain factorA
                    {
                        countA = 0;
                        while (i < factorsA.Count && factorA == factorsA[i]) { countA++; i++; }
                        lcm = lcm * ExponentBySquaring(factorA, (uint)countA);
                    }
                    else                              //factorsA does not contain factorB
                    {
                        countB = 0;
                        while (j < factorsB.Count && factorB == factorsB[j]) { countB++; j++; }
                        lcm = lcm * ExponentBySquaring(factorB, (uint)countB);
                    }
                }
                else if (i < factorsA.Count && j >= factorsB.Count)
                {
                    while (i < factorsA.Count)
                    {
                        lcm = lcm * factorsA[i];
                        i++;
                    }
                }
                else if (i >= factorsA.Count && j < factorsB.Count)
                {
                    while (j < factorsB.Count)
                    {
                        lcm = lcm * factorsB[j];
                        j++;
                    }
                }
                else
                    break;
            }

            return lcm;
        }
        /// <summary>
        /// Returns array of integers which are power of 2 and their sum is equal to the given number n (e.g. 7=1+2+4)
        /// </summary>
        public static List<uint> SumOfNumbersOfPower2(uint n)
        {
            var result = new List<uint>();
            if (n == 0) return result;

            //find the 2's degrees up to the number m such that 2^(m+1)>n
            int index = 0;
            List<uint> numbers = new List<uint>((int) Math.Abs(Math.Log10(n) / Math.Log10(2)));
            while (true)
            {
                numbers.Add((uint)Math.Pow(2, index));
                if (Math.Pow(2, index + 1) > n) break;
                index++;
            }

            //compose the combination of numbers such that the sum of elements is equal to n (e.g. 7=4+2+1)
            uint value = n;
            for (int j = numbers.Count - 1; j >= 0; j--)
            {
                if (value >= numbers[j])
                {
                    result.Add(numbers[j]);
                    value -= numbers[j];
                }
                if (value == 0) break;
            }
            return result;
        }
        /// <summary>
        /// Calculates the exponent using the fact that a^n = (a^n/2)^2 (n is even) or a^n = a*(a^(n-1)/2)^2 if n is odd. Complexity is O(logn) (T(n)=T(n/2)+1-even and T(n)=T(n/2)+2-odd)
        /// </summary>
        public static decimal ExponentBySquaring(decimal a, decimal n)
        {
            if (a == 0) return 0;
            if (a == 1) return 1;
            if (n == 0) return 1;
            if (n == 1) return a;
            if (n == 2) return checked(a*a);

            checked
            {
                if (n % 2 == 0)
                    return ExponentBySquaring(ExponentBySquaring(a, n / 2), 2);
                else
                    return a * ExponentBySquaring(ExponentBySquaring(a, (n - 1) / 2), 2);
            }
        }
        /// <summary>
        /// Returns the factors of given number or empty list if the number is prime. Complexity is O(sqrt(N)).
        /// </summary>
        public static List<ulong> PrimeFactors(ulong n)
        {
            var factors = new List<ulong>();
            ulong initialValue = n;
                        
            // check all 2 factors
            while (n % 2 == 0)
            {
                factors.Add(2);
                n = n / 2;
            }

            // check all other odd factors (there are no even factors any more)
            ulong i = 3;
            while (i * i <= n)
            {
                while (n % i == 0)
                {
                    factors.Add(i);
                    n = n / i;
                }
                i = i + 2;  //next odd factor
            }
            if (n > 1 && n!= initialValue) factors.Add(n);  //the remainder is a factor as well
            return factors;
        }
        /// <summary>
        /// Checks if given number is prime using the fact that not prime number has prime divisor less than or equal to sqrt(n)). Complexity is at most O(N x log(log N)) which is almost linear.
        /// </summary>
        /// <remarks>
        /// Complexity could be computed using the expression T(n)=A(n)+B(n), where A(n)=[T(5)+T(7)+...+T(sqrt(n))] and B(n)=c*sqrt(n). There are sqrt(n)/2 summators in A(n) and we can write A(n) as less or equal sqrt(n)*T(sqrt(n)). We can use substitution method to solve that recurrence relation.
        /// In practice the algorithm should be much faster, the worst case complexity O(n*loglogn) should not appear often (because we check i to be a factor of n first and than make the recursive call).
        /// </remarks>
        public static bool IsPrime2(ulong n)
        {
            if (n == 1 || n == 2 || n == 3 || n == 5) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;   //cuts down n/2+n/3-n/6=2n/3 numbers for testing

            ulong i = 5;
            while (i * i <= n)   //total sqrt(n)/2 numbers
            { 
                if (n % i == 0) return !IsPrime2(i);
                i += 2;
            }
            return true;
        }
        /// <summary>
        /// Returns true if n is prime by checking all odd divisors up to sqrt(n). Complexity is O(sqrt(n)) in terms of n (not in terms of numbers of digits of n).
        /// </summary>
        /// <remarks>For a number with n digits sqrt(n) would have approximately n-1 digits (e.g. sqrt(100)=10). For large n, this is stil quite big number.</remarks>
        public static bool IsPrime(ulong n)
        {
            if (n == 1 || n == 2 || n == 3 || n == 5) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;   //cuts down n/2 numbers for testing
            ulong i = 5;
            while (i * i <= n)       //test numbers up to sqrt(n)
            {
                if (n % i == 0) return false;
                i = i + 2;          //next odd factor (assume const time)
            }
            return true;
        }
        /// <summary>
        /// Probabilistic algorithm for primality testing using Fermat's little theorem.
        /// </summary>
        /// <remarks>
        /// Fermat's little teorem states that if p is prime n^(p-1) mod p = 1, where ![CDATA[1 < n < p]]>.Note that this does not mean that for some n and non prime p, n^(p-1) mod p = 1. In this case n is called fermat's liar.
        /// If n^(p-1) mod p != 1 we know that p is not prime (called a Fermat's withness). 
        /// If we randomely pick a number from 1 to p, there is 50% chance that it will be a fermat's liar and we will get false results(p is not prime but module is still 1). 
        /// So repeating the test k times, we get 1/2^k probability that we picked Fermat' liar every time. If for one of test the modulo is not 1, this means that p is not prime.
        /// 
        /// Because of large exponents won't work with fixed length numbers(long,int...) even for small p's.
        /// </remarks>
        public static bool IsPrimeHeuristic(decimal p, ushort testCount)
        {
            if (testCount == 0) testCount = 1;
            for (int j = 1; j <= testCount; j++)
            {
                decimal n = new CongruentalRandomNumberGenerator().Next(2,(int) p-1);   //random number between 1 & p exclusive
                if (ExponentBySquaring(n, p-1) % p != 1) return false;                  //it is not prime
            }
            return true;    //it is prime with probability of 1/2^testCount;
        }
        /// <summary>
        /// Fins prime numbers up to given number n inclusive. The method is known as sieve of Eratosthenes. As with IsPrime2, the complexity is O(N × log(log N)).
        /// </summary>        
        public static long[] FindPrimes(long n)
        {
            if (n <= 0) throw new ArgumentException("Incorrect input (positive long numbers only)");            
            bool[] isComposite = new bool[n+1];     // to count 0 as well

            isComposite[0] = true;

            // cut down all even numbers
            long l = 4;
            for (; l <= isComposite.LongLength-1; l += 2)
                isComposite[l] = true;

            long nextPrime = 3;
            while (nextPrime * nextPrime <= n)          //all numbers from sqrt(n) to n would already be tested.
            {
                // cut down all multiples of current prime number
                for (l = nextPrime * 2; l <= n; l = l + nextPrime)
                    isComposite[l] = true;

                // select next prime number
                for (nextPrime = nextPrime + 2; nextPrime <= n && isComposite[nextPrime]; nextPrime += 2) { }     //skip even numbers
            }

            long primesCount = isComposite.Where(b => !b).LongCount();
            long[] primes = new long[primesCount];      //avoiding List because there could be more primes than int.MaxValue for large numbers

            long i = 0;
            for (l = 0; l < isComposite.LongLength; l++)
            {
                if (!isComposite[l])
                {
                    primes[i] = l;
                    i++;
                }
            }
            return primes;
        }
        /// <summary>
        /// Returns smallest prime number which is bigger than the given number
        /// </summary>
        public static ulong FindNextPrime(ulong n)
        {
            if (n % 2 == 0) n++;    //even numbers are not prime
            while (!IsPrime(n))
                n += 2;
            return n;
        }
        /// <summary>
        /// Computes n degrees root from given number a using Newton's method.
        /// </summary>
        /// <remarks>
        /// Finding the  n root is the same as finding a number x such that x^n-a=0. Newton's method is used to find the root to this equation by 
        /// checking limited amount of numbers computed using the X(i+1)=X(i)-f[x(i)]/f'[x(i)] formula (convergence is not guaranteed by Newton's method)
        /// </remarks>
        public static double NRoot(double a, double n, uint repeatCount = 100, float approximation = 0.000001F)
        {
            approximation = Math.Abs(approximation);
            if (n == 0) return 1;
            if (n == 1) return a;
            if (a == 0) return 0;
            if (a == 1) return 1;
            if (n < 0) return 1 / NRoot(a, Math.Abs(n), repeatCount, approximation);
            if (a < 0 && n % 2 == 0) throw new InvalidOperationException("x must be nonnegative if n is even");
            if (repeatCount == 0) throw new ArgumentException("Repeat count can't be 0");

            double nextX, temp, x = 0;            
            Func<double, double> function = (arg) => Math.Pow(arg, n) - a;
            Func<double, double> derivative = (arg) => n * Math.Pow(arg, n-1);

            x = a;  // start point can be choosen more wisely
            for (uint count = 1; count <= repeatCount; count++)
            {
                temp = derivative(x);
                if (temp == 0) throw new Exception("The trial can't lead to an solution 111.");
                nextX = x - function(x) / temp;
                if (Math.Abs(function(nextX)) <= approximation) return nextX;
                x = nextX;
            }
            throw new Exception("The trial can't lead to an solution.");
        }
        /// <summary>
        /// Converts a number from one base to another
        /// </summary>
        public static string BaseConverter(string s, ushort fromBase, ushort toBase)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            s = s.ToUpperInvariant();
            // validate input
            if (fromBase < 2 || toBase < 2 || fromBase > 36 || toBase > 36) throw new ArgumentException("Invaid base. Converter accepts only numbers with 2 <= base <= 36");
            if (!ValidateNumberOfBaseB(s, fromBase)) throw new ArgumentException(string.Format("{0} number is not valid for base {1}", s, fromBase));
            if (fromBase == toBase) return s;

            ulong decimalValue = ConvertBaseBNumberToDecimal(s, fromBase);
            if (toBase == 10) return decimalValue.ToString();
            return ConvertDecimalToBaseBNumber(decimalValue, toBase);
        }
        /// <summary>
        /// Converts the number with base b to a decimal number
        /// </summary>
        public static ulong ConvertBaseBNumberToDecimal(string number, ushort b)
        {
            if (string.IsNullOrEmpty(number)) throw new ArgumentException("Empty input");
            number = number.ToUpperInvariant();
            if (!ValidateNumberOfBaseB(number, b)) throw new ArgumentException(string.Format("{0} number is not valid for base {1}", number, b));
            if (b < 2 || b > 36) throw new ArgumentException("Invaid base. Converter accepts only numbers with 2 <= base <= 36");
            if (b == 10) return uint.Parse(number);

            uint signDegrees = (uint)number.Length - 1;
            ulong result = 0;
            foreach (char c in number)
            {
                uint code = GetDecimalCodeOfChar(c);
                result = result + code * (ulong) ExponentBySquaring(b, signDegrees);
                signDegrees--;
            }
            return result;
        }
        /// <summary>
        /// Converts decimal value n to a number of base b by divisions method. Complexity is logarithmic O(log(b,N)).
        /// </summary>
        public static string ConvertDecimalToBaseBNumber(ulong n, ushort b)
        {
            if (!ValidateNumberOfBaseB(n.ToString(), 10)) throw new ArgumentException(string.Format("{0} number is not valid for base 10", n));
            if (b < 2 || b > 36 ) throw new ArgumentException("Invaid base. Converter accepts only numbers with 2 <= base <= 36");

            ushort rem;
            ulong factor = n;
            string result = string.Empty;
            while (factor != 0)
            {
                rem = (ushort)(factor % b);
                result = GetCharOfDecimalCode(rem).ToString() + result;
                factor = factor / b;                
            }
            return result;
        }        
        private static bool ValidateNumberOfBaseB(string s, ushort b)
        {
            foreach (var c in s)
            {
                ushort decimalCode = GetDecimalCodeOfChar(c);
                if (decimalCode >= b) return false;
            }
            return true;
        }
        /// <summary>
        /// Returns int code corresponding to the given character (e.g 3=3, A=10, Z=35)
        /// </summary>
        private static ushort GetDecimalCodeOfChar(char c)
        {
            ushort number = 0;
            if (ushort.TryParse(c.ToString(), out number))
            {
                if (number >= 0 || number <= 9) return number;
            }
            int ASCIICode = (int)c;
            if (ASCIICode > 90 || ASCIICode < 65) throw new ArgumentException("Invalid character: " + c.ToString());
            return (ushort) (ASCIICode - 55);
        }
        /// <summary>
        /// Returns character corresponding to the given decimal code (e.g. 3=3, 10=A, 35=Z)
        /// </summary>
        private static char GetCharOfDecimalCode(ushort code)
        {
            if (code < 10) return code.ToString()[0];
            return (char)(code + 55);
        }
    }
}