using System;
using System.Linq;
using System.Collections.Generic;

namespace Calculator
{
    static class CommonData
    {
        public static Dictionary<char, double> variables;
    }

    static class Class
    {
        public static int LastIndexOfOutsideBrackets(this string source, char value)
        {
            int result = 0;
            int bracketsBalance = 0;
            //Console.WriteLine($"Input of LastIndexOfOutsideBrackets: {source}");
            for (int i = source.Length - 1; i >= 0; i--)
            {
                switch (source[i])
                {
                    case '(':
                        bracketsBalance++;
                        break;
                    case ')':
                        bracketsBalance--;
                        break;
                    case char c when c == value && bracketsBalance == 0:
                        //Console.WriteLine($"Index {i} in {source}");
                        result = i;
                        break;
                }
            }
            //Console.WriteLine($"Result of LastIndexOfOutsideBrackets: {result}");
            return result;
        }

        public static bool ContainsOutsideBrackets(this string source, char value)
        {
            bool result = false;
            int bracketsBalance = 0;
            for (int i = 0; i < source.Length; i++)
            {
                switch (source[i])
                {
                    case '(':
                        bracketsBalance++;
                        break;
                    case ')':
                        bracketsBalance--;
                        break;
                    case char c when c == value && bracketsBalance == 0:
                        //Console.WriteLine($"Contain on index {i} in {source}");
                        result = true;
                        break;
                }
            }
            return result;
        }
    }

    enum Operators : byte
    {
        Add,
        Substract,
        Multiply,
        Divide,
        Power
    }

    class Value
    {
        Value operand1;
        Value operand2;
        bool haveValue;
        double value;
        Operators currentOperator;

        public double Result()
        {
            if (this.haveValue) return this.value;

            double result = 0;
            switch (currentOperator)
            {
                case Operators.Add:
                    result = operand1.Result() + operand2.Result();
                    //Console.WriteLine($"{operand1.Result()}+{operand2.Result()}={result}");
                    break;

                case Operators.Divide:
                    result = operand1.Result() / operand2.Result();
                    //Console.WriteLine($"{operand1.Result()}/{operand2.Result()}={result}");
                    break;

                case Operators.Multiply:
                    result = operand1.Result() * operand2.Result();
                    //Console.WriteLine($"{operand1.Result()}*{operand2.Result()}={result}");
                    break;

                case Operators.Power:
                    result = Math.Pow(operand1.Result(), operand2.Result());
                    //Console.WriteLine($"{operand1.Result()}^{operand2.Result()}={result}");
                    break;

                case Operators.Substract:
                    result = operand1.Result() - operand2.Result();
                    //Console.WriteLine($"{operand1.Result()}-{operand2.Result()}={result}");
                    break;
            }
            return result;
        }

        public Value(string input/*, ref Dictionary<char, double> variables*/)
        {
            if (input.StartsWith('(') && input.EndsWith(')'))
            {
                bool isAlwaysOpen = true;
                while (isAlwaysOpen)
                {
                    int open = 0;
                    int close = 0;
                    for (int i = 0; i < input.Length - 1; i++)
                    {
                        if (input[i] == '(') open++;
                        if (input[i] == ')') close++;
                        if (close >= open)
                        {
                            isAlwaysOpen = false;
                            break;
                        }
                    }
                    if (isAlwaysOpen) input = string.Join("", input.Skip(1).SkipLast(1));
                }
            }

            //Console.WriteLine($"Input: {input}");
            this.haveValue = false;
            switch (input)
            {
                case string expression when expression.ContainsOutsideBrackets('-'):
                    {
                        Operation('-', expression);
                    }
                    break;

                case string expression when expression.ContainsOutsideBrackets('+'):
                    {
                        Operation('+', expression);
                    }
                    break;

                case string expression when expression.ContainsOutsideBrackets('/'):
                    {
                        Operation('/', expression);
                    }
                    break;

                case string expression when expression.ContainsOutsideBrackets('*'):
                    {
                        Operation('*', expression);
                    }
                    break;

                case string expression when expression.ContainsOutsideBrackets('^'):
                    {
                        Operation('^', expression);
                    }
                    break;

                default:
                    if (input.Length == 1 && char.IsLetter(char.Parse(input)))
                    {
                        this.value = CommonData.variables[char.Parse(input)];
                    }
                    else
                    {
                        //Console.WriteLine($"String to parse:{input.Trim('(', ')')}");
                        this.value = double.Parse(input.Trim('(', ')'));
                    }
                    this.haveValue = true;
                    break;
            }
        }

        private void Operation(char operation, string expression)
        {
            int index = expression.LastIndexOfOutsideBrackets(operation);
            this.operand1 = new Value(string.Join("", expression.Take(index)));
            this.operand2 = new Value(string.Join("", expression.Skip(index + 1)));
            switch (operation)
            {
                case '^':
                    this.currentOperator = Operators.Power;
                    break;

                case '*':
                    this.currentOperator = Operators.Multiply;
                    break;

                case '/':
                    this.currentOperator = Operators.Divide;
                    break;

                case '+':
                    this.currentOperator = Operators.Add;
                    break;

                case '-':
                    this.currentOperator = Operators.Substract;
                    break;
            }
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            string input = Console.ReadLine();
            CommonData.variables = new Dictionary<char, double>();
            foreach (var item in input)
            {
                if (char.IsLetter(item))
                {
                    CommonData.variables.Add(item, 0);
                }
            }
            for (int i = 0; i < CommonData.variables.Count; i++)
            {
                string[] tempInput = Console.ReadLine().Split(' ');
                CommonData.variables[char.Parse(tempInput[0])] = double.Parse(tempInput[1]);
            }
            Console.Write(new Value(input).Result());
            //Console.Write(new Value(Console.ReadLine()).Result());
        }
    }
}
