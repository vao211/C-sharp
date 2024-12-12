using System.Runtime.CompilerServices;
using System.Xml;

namespace Calculator
{   
    public partial class Main : Form
    {
        static Dictionary<string, int> priority = new Dictionary<string, int>
        {
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
            { "^", 3 },
            { "(", 0 },
            { ")", 0 }
        };
        public Main()
        {
            InitializeComponent();
            InitializeButtons();
            
        }

        private void btnNum_Click(object sender, EventArgs e)
        {
            //Lấy nút được nhấn
            Button button = sender as Button;

            //Kiểm tra xem button có phải là Button không
            if (button != null)
            {
                //Kiểm tra nếu nút nhấn là dấu chấm
                if (button.Text == "." && txtScreen.Text.Contains("."))
                {
                    return; //Không cho phép nhập nhiều hơn một dấu chấm
                }
                txtScreen.Text += button.Text;
            }
        }

        private void InitializeButtons()
        {
            //Gán sự kiện cho các nút từ
            btnNum0.Click += btnNum_Click;
            btnNum1.Click += btnNum_Click;
            btnNum2.Click += btnNum_Click;
            btnNum3.Click += btnNum_Click;
            btnNum4.Click += btnNum_Click;
            btnNum5.Click += btnNum_Click;
            btnNum6.Click += btnNum_Click;
            btnNum7.Click += btnNum_Click;
            btnNum8.Click += btnNum_Click;
            btnNum9.Click += btnNum_Click;
            btnDot.Click += btnNum_Click;
            btnPlus.Click += btnNum_Click;
            btnSubtract.Click += btnNum_Click;
            btnDivide.Click += btnNum_Click;
            btnMulti.Click += btnNum_Click;
            btnPower.Click += btnNum_Click;
            btnRoot.Click += btnNum_Click;
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                txtScreen.Text = txtScreen.Text.Substring(0, txtScreen.Text.Length - 1);
            }
            catch (Exception ex) { };
        }
        
        private void btnCal_Click(object sender, EventArgs e)
        {
            string express = txtScreen.Text.Trim();
            string postfix = change_to_postfix(express, priority);
            double result = calculate(postfix);
            txtScreen.Text = result.ToString();
        }

        private string change_to_postfix(string e, Dictionary<string, int> prio)
        {
            Stack<string> operators = new Stack<string>();
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
                    else if (current == ')')
                    {
                        //pop --> result đến khi gặp (
                        while (operators.Count > 0 && operators.Peek() != "(")
                        {
                            result.Enqueue(operators.Pop());
                        }
                        operators.Pop(); //xóa dấu (
                    }
                    else if (prio.ContainsKey(current.ToString()))
                    {
                        //ktra đỉnh của stack, ưu tiên >= thì pop --> quêu và thêm hiện tại vào stack
                        while (operators.Count > 0 && prio[operators.Peek()] >= prio[current.ToString()])
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
            return string.Join(" ", result);
        }

        private double calculate(string e)
        {
            Stack<Double> stack = new Stack<Double>();
            string[] operators = e.Split(' ');

            foreach (string o in operators)
            {
                if (double.TryParse(o, out double num))
                {
                    stack.Push(num);
                }
                else
                {
                    double right = stack.Pop();
                    double left = stack.Pop();
                    double result = operation(left, right, o);
                    stack.Push(result);
                }

            }
            return stack.Pop();
        }

        private static double operation(double left, double right, string operation)
        {
            double result;
            switch (operation)
            {
                case "+":
                    return result = left + right;
                case "-":
                    return result=left - right;
                case "x":
                    return result=left * right;
                case ":":
                    return result = left / right;
                case "^":
                    return result=Math.Pow(left, right);
                default:
                    throw new InvalidOperationException("Invalid operator: " + operation);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtScreen.Clear();
        }
    }
}
