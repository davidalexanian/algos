using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.Numerical
{
    /// <summary>
    /// Contains miscellaneous recurisve algorithms
    /// </summary>
    public static class Recursive
    {
        /// <summary>
        /// Returns nth Fibonacci's number (complexity O(2^n))
        /// </summary>
        public static ulong FibonacciNumber(uint n)
        {
            if (n == 0) return 0;
            if (n == 1) return 1;
            return FibonacciNumber(n - 1) + FibonacciNumber(n - 2);
        }
        /// <summary>
        /// Computes factorial of n (complexity O(n))
        /// </summary>
        public static ulong Factorial(uint n)
        {
            if (n == 0) return 1;
            return Recursive.Factorial(n - 1) * n;
        }
        /// <summary>
        /// Multiplication using addition only (complexity O(n))
        /// </summary>
        public static long Multiplication(uint n, int x)
        {
            if (n == 0) return 0;
            return Multiplication(n - 1, x) + x;
        }
        /// <summary>
        /// Returns the sum of first n positive integers
        /// </summary>
        public static ulong SumOfFirstNIntegers(uint n)
        {
            if (n == 1) return 1;
            return SumOfFirstNIntegers(n - 1) + n;
        }
        /// <summary>
        /// Returns the sum of first n odd positive integers
        /// </summary>
        public static ulong SumOfFirstNOddNumbers(uint n)
        {
            if (n == 1) return 1;
            return SumOfFirstNOddNumbers(n - 1) + 2 * n - 1;
        }
        /// <summary>
        /// Returns largest element from array using recursion (complexity o(n))
        /// </summary>
        public static int MaxFromSetOfIntegers(int[] set, int upToIndex=-1)
        {
            if (upToIndex < 0 || upToIndex > set.Length - 1) upToIndex = set.Length - 1;

            if (upToIndex == 0) return set[0];
            int max = MaxFromSetOfIntegers(set, upToIndex-1);
            if (set[upToIndex] >= max)
                return set[upToIndex];
            else
                return max;
        }
        /// <summary>
        /// Returns the reversal of given string (complexity O(n))
        /// </summary>
        public static string ReversalOfNString(string s)
        {
            if (s.Length == 0) return string.Empty;
            if (s.Length == 1) return s;
            return s[s.Length - 1] + ReversalOfNString(s.Substring(0, s.Length - 1));
        }
        /// <summary>
        /// Draws Koch's curve (4^n continues, 60 degrees roateted curves having same length for given depth n)
        /// </summary>
        /// <param name="n">Number of recursive calls</param>
        /// <param name="start">Start point</param>
        /// <param name="angle">Angle of line made from connecting startPoint/endPoints</param>
        /// <param name="result">IList collection holding lines for later drawing</param>
        public static Point DrawKochCurve(ushort n, Point start, double angle, double length, IList<Line> result)
        {
            if (n == 0)
            {
                Point end = new Point(start.X + Math.Cos(angle) * length, start.Y + Math.Sin(angle) * length);
                result.Add(new Line(start, end));
                return end;
            }
            // generate 4 lines of length initialLength/3 of the form: _/\_ 
            Point p1 = new Point(start.X + Math.Cos(angle) * length / 3, start.Y + Math.Sin(angle) * length / 3);            
            Point p2 = new Point(start.X + Math.Cos(angle) * length / 3 * 2, p2.Y = start.Y + Math.Sin(angle) * length / 3 * 2);
            Point next = DrawKochCurve((ushort)(n - 1), start, angle, length / 3, result);
            next = DrawKochCurve((ushort)(n - 1), next, angle + Math.PI / 3, length / 3, result);
            next = DrawKochCurve((ushort)(n - 1), next, angle - Math.PI / 3, length / 3, result);
            return DrawKochCurve((ushort)(n - 1), p2, angle, length / 3, result);
        }
        /// <summary>
        /// Draws fractial triangles inside the given one (3^n triangles for given depth n)
        /// </summary>
        /// <param name="initial">Initial triangle to draw others inside of </param>
        /// <param name="n">Recursion's depth</param>
        /// <param name="result">IList collection holding rectangles for later drawing</param>
        public static void DrawFractalTriangles(Triangle initialTriangle, ushort n, IList<Triangle> result)
        {
            if (n == 0)
            {
                result.Add(initialTriangle);
                return;                
            }
            // generate 3 trinagles inside initial trinagle and draw all except the central
            Point p1 = new Point((initialTriangle.p1.X + initialTriangle.p2.X) / 2, (initialTriangle.p1.Y + initialTriangle.p2.Y) / 2);
            Point p2 = new Point((initialTriangle.p2.X + initialTriangle.p3.X) / 2, (initialTriangle.p2.Y + initialTriangle.p3.Y) / 2);
            Point p3 = new Point((initialTriangle.p3.X + initialTriangle.p1.X) / 2, (initialTriangle.p3.Y + initialTriangle.p1.Y) / 2);

            Triangle first = new Triangle(initialTriangle.p1, p1, p3);
            Triangle second = new Triangle(initialTriangle.p2, p2,p1);
            Triangle third = new Triangle(initialTriangle.p3, p3,p2);
            DrawFractalTriangles(first, (ushort)(n - 1), result);
            DrawFractalTriangles(second, (ushort)(n - 1), result);
            DrawFractalTriangles(third, (ushort)(n - 1), result);

        }
        /// <summary>
        /// Computes nth Fibonacci's number (complexity O(n))
        /// </summary>
        public static ulong FibonacciNumberInductive(uint n)
        {
            ulong p1 = 0, p2 = 1, p3 = 0;
            if (n == 0) return p1;
            if (n == 1) return p2;

            for (uint i = 2; i <= n; i++)
            {
                p3 = p1 + p2;
                p1 = p2;
                p2 = p3;
            }
            return p3;
        }
        /// <summary>
        /// Computes factorial of n (complexity O(n))
        /// </summary>        
        public static ulong FactorialInductive(uint n)
        {
            if (n == 0) return 1;
            ulong l = 1;
            for (uint i = 1; i <= n; i++)
                l = l * i;
            return l;
        }
    }
    public struct Point
    {
        public Point(double x, double y) { X = x; Y = y; }
        public double X;
        public double Y;
        public override string ToString()
        {
            return "(" + X.ToString() + ", " + Y.ToString() + ")";
        }
    }
    public class Line
    {
        public Line() { }
        public Line(Point p1, Point p2) { this.p1 = p1; this.p2 = p2; }
        public Point p1;
        public Point p2;
    }
    public class Triangle
    {
        public Triangle() { }
        public Triangle(Point p1, Point p2, Point p3) { this.p1 = p1; this.p2 = p2; this.p3 = p3; }
        public Point p1;
        public Point p2;
        public Point p3;
    }
}
