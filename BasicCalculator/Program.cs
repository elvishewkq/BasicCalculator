using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;

namespace CalculatorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Basic Calculator");

            MathOperationInput();
        }

        public static void MathOperationInput()
        {
            string mathOperation, tobecontinued;

            Console.WriteLine("----------------------------------------------------------------\n");
            Console.Write("Enter your maths operation: ");
            mathOperation = Console.ReadLine();

            Console.WriteLine("\nCalculating ...");
            Console.Write(mathOperation + " = ");
            Console.WriteLine(Calculate(mathOperation));

            Console.WriteLine("\n\nDo you wish to continue with other maths operation? (y / n)");

            tobecontinued = Console.ReadLine();

            if (tobecontinued.ToLower() == "y")
            {
                MathOperationInput();
                Console.ReadLine();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public static double Calculate(string sum)
        {
            // To perform operation based on priority (BODMAS)
            // Brackets "()", Orders, Division "/", Multiplication "x", Addition "+" and Subtraction "-"
            // e.g.  10 - ( 2 + 3 * ( 7 - 5 ) )
            // {"10", "-", "(", "2", "+", "3", "*", "(", "7", "-", "5", ")", ")"}            

            try
            {
                while (sum.Contains("("))
                {
                    string brackets = string.Concat(sum.Replace(" ", "").Where(symbol => symbol != '/' && symbol != '*' && symbol != '+' && symbol != '-' && symbol != '.' && !char.IsDigit(symbol))); // "()"

                    // Handling Operations in Nested Brackets
                    if (brackets.Contains("(("))
                    {
                        sum = sum.Replace(" ", "");
                        string[] splittedMathOperation = Regex.Split(sum, @"([^0-9\.])|(\()|(\))|(/)|(\*)|(\+)|(-)|(\[)|(\])");
                        string OperationInBracket = "";
                        string OperationInBracket2 = "";
                        int OpenBracketPos = 0;
                        int OpenBracketCount = 0;
                        int CloseBracketPos = 0;
                        int CloseBracketCount = 0;
                        int NumOfBrackets = sum.Length - sum.Replace(" ", "").Replace("(", "").Length;

                        for (int i = 0; i < splittedMathOperation.Length; i++)
                        {
                            if (splittedMathOperation[i] == "(")
                            {
                                OpenBracketCount++;
                                if (OpenBracketCount == NumOfBrackets)
                                {
                                    OpenBracketPos = i;
                                }
                            }
                        }

                        for (int i = OpenBracketPos; i < splittedMathOperation.Length; i++)
                        {
                            if (splittedMathOperation[i] == ")")
                            {
                                CloseBracketCount++;
                                if (CloseBracketCount == 1)
                                {
                                    CloseBracketPos = i;
                                }
                            }
                        }

                        // First operation performed on the innermost brackets
                        for (int i = OpenBracketPos + 1; i < CloseBracketPos; i++)
                        {
                            OperationInBracket = OperationInBracket + splittedMathOperation[i];
                        }
                        OperationInBracket2 = SimpleCalculation(OperationInBracket).ToString();

                        string sum2 = "";

                        for (int i = 0; i < OpenBracketPos; i++)
                        {
                            sum2 = sum2 + splittedMathOperation[i];
                        }

                        string sum3 = "";

                        for (int i = CloseBracketPos + 1; i < splittedMathOperation.Length; i++)
                        {
                            sum3 = sum3 + splittedMathOperation[i];
                        }

                        sum = sum2 + OperationInBracket2 + sum3;

                    }
                    // Handling Operations in Brackets
                    else
                    {
                        sum = sum.Replace(" ", "");
                        string[] splittedMathOperation = Regex.Split(sum, @"([^0-9\.])|(\()|(\))|(/)|(\*)|(\+)|(-)|(\[)|(\])");
                        string OperationInBracket = "";
                        string OperationInBracket2 = "";

                        for (int i = Array.IndexOf(splittedMathOperation, "(") + 1; i < Array.IndexOf(splittedMathOperation, ")"); i++)
                        {
                            OperationInBracket = OperationInBracket + splittedMathOperation[i];
                        }

                        if (Array.IndexOf(splittedMathOperation, "(") == 0)
                        {
                            OperationInBracket2 = OperationInBracket2 + SimpleCalculation(OperationInBracket).ToString();
                        }
                        else if (Array.IndexOf(splittedMathOperation, "(") > 0)
                        {
                            OperationInBracket2 = OperationInBracket2 + splittedMathOperation[Array.IndexOf(splittedMathOperation, "(") - 1] + SimpleCalculation(OperationInBracket).ToString();
                        }

                        string sum2 = "";

                        for (int i = 0; i < Array.IndexOf(splittedMathOperation, "(") - 1; i++)
                        {
                            sum2 = sum2 + splittedMathOperation[i];
                        }

                        string sum3 = "";

                        for (int i = Array.IndexOf(splittedMathOperation, ")") + 1; i < splittedMathOperation.Length; i++)
                        {
                            sum3 = sum3 + splittedMathOperation[i];
                        }

                        sum = sum2 + OperationInBracket2 + sum3;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return SimpleCalculation(sum);
        }

        static double SimpleCalculation(string simpleOperation)
        {
            double result = 0;
            bool FirstNegative = false;
            char[] operators = { '/', '*', '+' };

            try
            {
                // To perform operation based on priority (BODMAS)
                // Brackets "()", Orders, Division "/", Multiplication "x", Addition "+" and Subtraction "-"
                // e.g.  1 + 1 * 3

                while (simpleOperation.IndexOfAny(operators) > 0 || simpleOperation.IndexOf('-') > 0)
                {
                    simpleOperation = simpleOperation.Replace(" ", "");

                    if (simpleOperation[0] == '-')
                    {
                        simpleOperation = simpleOperation.Substring(1);
                        FirstNegative = true;
                    }
                    else
                    {
                        FirstNegative = false;
                    }

                    string[] splittedMathOperation = Regex.Split(simpleOperation, @"([^0-9\.])|(\()|(\))|(/)|(\*)|(\+)|(-)|(\[)|(\])");    // {"1", "+", "1", "*", "3"}


                    if (FirstNegative == true)
                    {
                        splittedMathOperation[0] = "-" + splittedMathOperation[0];
                    }

                    string nonNumeric = string.Concat(simpleOperation.Where(symbol => symbol != '.' && !char.IsDigit(symbol))); // "+*"

                    // Finding the operator with the highest priority
                    int operatorPriorityPos = 0;
                    int highestPriority = 0;

                    for (int i = 0; i < nonNumeric.Length; i++)
                    {
                        if (Priority(nonNumeric[i]) > highestPriority)
                        {
                            highestPriority = Priority(nonNumeric[i]);
                            operatorPriorityPos = Array.IndexOf(splittedMathOperation, nonNumeric[i].ToString());
                        }
                    }

                    simpleOperation = splittedMathOperation[operatorPriorityPos - 1] + splittedMathOperation[operatorPriorityPos] + splittedMathOperation[operatorPriorityPos + 1];

                    string simpleOperation2 = "";

                    for (int i = 0; i < operatorPriorityPos - 1; i++)
                    {
                        simpleOperation2 = simpleOperation2 + splittedMathOperation[i];
                    }

                    string simpleOperation3 = "";

                    for (int i = operatorPriorityPos + 2; i < splittedMathOperation.Length; i++)
                    {
                        simpleOperation3 = simpleOperation3 + splittedMathOperation[i];
                    }

                    result = PerformOperation(simpleOperation, splittedMathOperation[operatorPriorityPos]);

                    simpleOperation = simpleOperation2 + result.ToString() + simpleOperation3;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return result;
        }

        static double PerformOperation(string mathOperationToBePerformed, string mathOperator)
        {
            string[] mathNum = Regex.Split(mathOperationToBePerformed, @"[^0-9\.]");

            switch (mathOperator)
            {
                case "/":
                    if (mathOperationToBePerformed[0] == '-')
                    {
                        return Convert.ToDouble("-" + mathNum[1]) / Convert.ToDouble(mathNum[2]);
                    }
                    else
                    {
                        return Convert.ToDouble(mathNum[0]) / Convert.ToDouble(mathNum[1]);
                    }
                case "*":
                    if (mathOperationToBePerformed[0] == '-')
                    {
                        return Convert.ToDouble("-" + mathNum[1]) * Convert.ToDouble(mathNum[2]);
                    }
                    else
                    {
                        return Convert.ToDouble(mathNum[0]) * Convert.ToDouble(mathNum[1]);
                    }
                case "+":
                    if (mathOperationToBePerformed[0] == '-')
                    {
                        return Convert.ToDouble("-" + mathNum[1]) + Convert.ToDouble(mathNum[2]);
                    }
                    else
                    {
                        return Convert.ToDouble(mathNum[0]) + Convert.ToDouble(mathNum[1]);
                    }
                case "-":
                    if (mathOperationToBePerformed[0] == '-')
                    {
                        return Convert.ToDouble("-" + mathNum[1]) - Convert.ToDouble(mathNum[2]);
                    }
                    else
                    {
                        return Convert.ToDouble(mathNum[0]) - Convert.ToDouble(mathNum[1]);
                    }
                default:
                    return 0;
            }
        }

        static int Priority(char operation)
        {
            switch (operation)
            {
                case '(': return 5;
                case '/': return 4;
                case '*': return 3;
                case '+': return 2;
                case '-': return 2;
            }
            return 0;
        }

    }
}
