using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAlgorithms.String
{
    public class StringAlgorithms
    {
        /// <summary>
        /// Checks if parentheses are nested correctly (e.g. ()) () is incorrect)
        /// </summary>
        public static bool ParenthesesNestedProperly(string text)
        {
            if (text == null) throw new ArgumentNullException();
            int counter = 0;
            foreach (var c in text)
            {
                if (c == '(') counter++;
                if (c == ')') counter--;
                if (counter < 0) return false;
            }
            return counter == 0;
        }
        /// <summary>
        /// Evaluates the arithemtic expression to a double value. Expression can contain addition, subtraction, multiplication, division.
        /// </summary>
        /// <remarks>
        /// Works in following way.
        ///     If the expression is a literal value, parsed as double
        ///     If the expression is of the form (expr), remove the outer parentheses, use recusion to calculate the value
        ///     If the expression is of the form expr1?expr2, use recursion to calculate operand and parse the operator
        /// </remarks>
        public static double EvaluateArithmeticExpression(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression)) throw new ArgumentNullException();
            string originalExpression = expression.Trim().Replace("\n", "").Replace("\t", "").Replace(" ", "");
            return EvaluateInner(originalExpression);
        }
        private static double EvaluateInner(string expression)
        {
            // look for operator
            int count = 0;
            for (int i = 0; i < expression.Length - 1; i++)
            {
                if (expression[i] == '(')
                    count++;
                else if (expression[i] == ')')
                {
                    count--;
                    if (count < 0) throw new Exception(string.Format("Parentheses are not nested properly (at position {0})",i));
                }
                else if (count == 0)
                {
                    // We look for an operator only when we are not in parentheses context
                    char ch = expression[i];
                    if ((ch == '+') || (ch == '-') || (ch == '*') || (ch == '/'))
                    {
                        var expressionLeft = expression.Substring(0, i);
                        var expressionRight = expression.Substring(i + 1);
                        var valueLeft = EvaluateInner(expressionLeft);
                        var valueRight = EvaluateInner(expressionRight);
                        switch (ch)
                        {
                            case '+': return valueLeft + valueRight;                                
                            case '-': return valueLeft - valueRight;
                            case '*': return valueLeft * valueRight;
                            case '/': return valueLeft / valueRight;
                            default: throw new Exception(string.Format("Invalid operator at position {0}",i));
                        }
                    }
                }
            }

            // we did not find an operator
            if (expression[0] == '(' && MatchingParenIndex(expression, 0) == expression.Length - 1)
                return EvaluateInner(expression.Substring(1, expression.Length - 2));

            double result = 0;
            if (double.TryParse(expression, out result)) return result;
            throw new Exception("Invalid expression");
        }


        #region "helpers"
        private static int MatchingParenIndex(string expression, int openParenIndex)
        {
            int count = 1;
            for (int i = openParenIndex + 1; i < expression.Length; i++)
            {
                if (expression[i] == '(') count++;
                else if (expression[i] == ')') count--;
                if (count == 0) return i;
                if (count < 0) return -1;
            }
            return -1;
        }
        #endregion
    }
}
