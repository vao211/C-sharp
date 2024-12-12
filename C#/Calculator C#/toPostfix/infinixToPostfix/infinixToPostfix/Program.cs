using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class Program
{
    public static void Main(string[] args)
    {
        Dictionary<string, int> priority = new Dictionary<string, int>
        {
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 },
            { "(", 0 },
            { ")", 0 }
        };
        Console.WriteLine("Enter expression: ");
        string express = Console.ReadLine();
        string postfix = change_to_postfix(express, priority);
        double result = calculate(postfix);
        //Console.WriteLine("Postfix expression: " + postfix);
        Console.WriteLine("Result: " + result);

    }

    private static string change_to_postfix(string e, Dictionary<string,int> prio)
    {
       Stack <string> operators = new Stack<string>();
       Queue<string> result = new Queue<string>();
       for (int i = 0; i < e.Length; i++)
        {
            char current = e[i];
            if (char.IsDigit(current))
            {
                string number = current.ToString();
                while (i + 1 < e.Length && char.IsDigit(e[i + 1]))
                {
                    number += e[++i];
                }
                result.Enqueue(number);
            }
            else
            {
                if (current == '(')
                {
                    operators.Push(current.ToString());
                }
                else if (current ==')')
                {
                    //pop --> result đến khi gặp (
                    while(operators.Count >0 && operators.Peek() != "(")
                    {
                        result.Enqueue(operators.Pop());
                    }
                    operators.Pop(); //xóa dấu (
                }
                else if (prio.ContainsKey(current.ToString()))
                {
                    //ktra đỉnh của stack, ưu tiên >= thì pop --> quêu và thêm hiện tại vào stack
                    while(operators.Count > 0 && prio[operators.Peek()] >= prio[current.ToString()])
                    {
                        result.Enqueue(operators.Pop());
                    }
                    operators.Push(current.ToString());
                }
            }
        }
        //thêm phần còn lại của stack --> queue
        while (operators.Count > 0)
        {
            result.Enqueue(operators.Pop());
        }
        return string.Join(" ",result);
    }

    private static double calculate(string e)
    {
        Stack<Double> stack = new Stack<Double>();
        string[] operators = e.Split(' ');

        foreach(string o in operators){
            if (double.TryParse(o, out double num))
            {
                stack.Push(num);
            }
            else
            {
                double right = stack.Pop();
                double left = stack.Pop();
                double result = operation(left, right,o);
                stack.Push(result);
            }

        }
        return stack.Pop();
    }

    private static double operation(double left, double right, string operation)
    {
        switch (operation)
        {
            case "+":
                return left + right;
            case "-":
                return left - right;
            case "*":
                return left * right;
            case "/":
                return left / right;
             case "^":
                 return Math.Pow(left, right);
            default:
                throw new InvalidOperationException("Invalid operator: " + operation);
        }
    }
}