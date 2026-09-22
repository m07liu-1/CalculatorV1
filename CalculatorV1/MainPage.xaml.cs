namespace CalculatorV1
{
    public partial class MainPage : ContentPage {
        string num = "0"; // Second number entered by user
        string op = "";
        string result = "0"; // First number entered by user
        bool autoclear = false; // Clear display after operation without C
        string expression = "";
        bool lastPressedIsOp = false; // Keep program safe from invalid operator sequences
        public MainPage()
        {
            InitializeComponent();
        }

        private void clearDisplay(object sender, EventArgs e)
        {
            result = "0";
            num = "0";
            op = "";
            expression = "";
            lastPressedIsOp = false;
            resultDisplay.Text = "0";
        }

        private void numberPressed(object sender, EventArgs e)
        {
            lastPressedIsOp = false;

            if (autoclear && string.IsNullOrEmpty(op)) // Clear display if it's a new operation
            {
                clearDisplay(sender, e);
                autoclear = false;
            }

            if (resultDisplay.Text == "0" && (sender as Button).Text == "0") return;
            expression += (sender as Button).Text;

            if (string.IsNullOrEmpty(op))
            {
                result += (sender as Button).Text;
                if (result[0] == '0' && result[1] != '.') result = result[1..];
                resultDisplay.Text = result;
            }
            else
            {
                num += (sender as Button).Text;
                if (num[0] == '0' && num[1] != '.') num = num[1..];
                resultDisplay.Text = $"{result}{op}{num}";
            }
        }

        private void operatorPressed(object sender, EventArgs e)
        {
            if (lastPressedIsOp) return;
            removeEquals();
            bool calconly = true;
            string temp = (sender as Button).Text;
            if (temp == "*" || temp == "/" || temp == "+" || temp == "-" || temp == "^")
            {
                lastPressedIsOp = true;
                calconly = false;
            }
            if ((op == "+" || op == "-" || (op == "" && autoclear)) && (temp == "*" || temp == "/"))
            {
                expression = "(" + expression + ")";
            }
            if (string.IsNullOrEmpty(op))
            {
                if (!calconly) op = temp;
                if (autoclear) resultDisplay.Text = result;
                if (!calconly)
                {
                    expression += op;
                    resultDisplay.Text += op;
                }
                return;
            }
            else
            {
                try
                {
                    result = op switch // Set result of operation to the first number
                    {
                        "+" => round((double.Parse(num) + double.Parse(result)).ToString(), 7),
                        "-" => round((double.Parse(result) - double.Parse(num)).ToString(), 7),
                        "*" => round((double.Parse(num) * double.Parse(result)).ToString(), 7),
                        "/" => round((double.Parse(result) / double.Parse(num)).ToString(), 7),
                        "^" => round((Math.Pow(double.Parse(result), double.Parse(num))).ToString(), 7)
                    };
                    if (!calconly)
                    {
                        op = temp;
                        expression += op;
                    }
                }
                catch (DivideByZeroException)
                {
                    clearDisplay(sender, e);
                    resultDisplay.Text = "Error: Division by zero";
                    autoclear = true;
                }
            }
            // Update display and reset second number and operator
            resultDisplay.Text = result;
            num = "";
            autoclear = false;
        }

        // Helper to put sqrt() and ln() around numbers
        private void ins(ref string s, string ins, int len)
        {
            for (int i = len - 1; i > 0; i--)
            {
                if ((s[i] <= '0' || s[i] >= '9') && s[i] != '.')
                {
                    s = s.Insert(i + 1, ins);
                    s += ")";
                    return;
                }

            }
        }

        // Helper to prevent multiple equals from showing up across multiple operations
        private void removeEquals()
        {
            int len = expression.Length;
            int start = 0;
            for (int i = 0; i < len; i++)
            {
                if (expression[i] == '=')
                {
                    start = i;
                    break;
                }
            }
            if (start > 0) expression = expression[0..start];
        }

        // Helper to truncate doubles
        private string round(string s, int digits)
        {
            int dec = -1;
            bool start = false;
            int l = s.Length;
            for (int i = 0; i < l; i++)
            {
                if (s[i] == '.')
                {
                    dec = i;
                    break;
                }
            }
            if (dec > 0 && l - dec > digits) return s[0..(dec + digits)];
            return s;
        }
        private void unaryOperation(object sender, EventArgs e)
        {
            removeEquals();
            if (string.IsNullOrEmpty(op)) // Only one number entered
            {
                result = (sender as Button).Text switch
                {
                    "√" => round((Math.Sqrt(double.Parse(result))).ToString(), 7),
                    "ln" => round((Math.Log(double.Parse(result))).ToString(), 7)
                };
                switch ((sender as Button).Text)
                {
                    case "√":
                        expression = "sqrt(" + expression + ")";
                        break;
                    case "ln":
                        expression = "ln(" + expression + ")";
                        break;
                }
                ;
                resultDisplay.Text = result;
                expression = expression + $"={result}";
                operationDisplay.Text = expression;
                autoclear = true;
                lastPressedIsOp = false;
            }
            else // Multiple numbers, take square root of the second/latest number
            {
                switch ((sender as Button).Text)
                {
                    case "√":
                        ins(ref expression, "sqrt(", expression.Length);
                        break;
                    case "ln":
                        ins(ref expression, "ln(", expression.Length);
                        break;
                }
                ;
                num = (sender as Button).Text switch
                {
                    "√" => round((Math.Sqrt(double.Parse(num))).ToString(), 7),
                    "ln" => round((Math.Log(double.Parse(num))).ToString(), 7)

                };
                // Do all the calculations and display answer 
                equals(sender, e);
            }

        }

        private void decimalPoint(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(op))
            {
                result += ".";
                resultDisplay.Text += ".";
            }
            else
            {
                num += ".";
                resultDisplay.Text += ".";
            }
            expression += ".";

        }

        private void equals(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(op))
            {
                operationDisplay.Text = expression;
                return;
            }
            operatorPressed(sender, e);
            expression = expression + $"={result}";
            operationDisplay.Text = expression;
            op = "";
            autoclear = true;
            lastPressedIsOp = false;
        }
    }
}
