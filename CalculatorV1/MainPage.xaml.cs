namespace CalculatorV1
{
    public partial class MainPage : ContentPage {
        string num = "0"; // Second number entered by user
        string op = "";
        string result = "0"; // First number entered by user
        bool autoclear = false; // Clear display after operation without C
        public MainPage()
        {
            InitializeComponent();
        }
        private void clearDisplay(object sender, EventArgs e)
        {
            result = "0";
            num = "0";
            op = "";
            resultDisplay.Text = "0";
         }

        private void numberPressed(object sender, EventArgs e)
        {
            if (autoclear) // Clear display if it's a new operation
            {
                clearDisplay(sender, e);
                autoclear = false;
            }

            if (resultDisplay.Text == "0" && (sender as Button).Text == "0") return;
            
            if (string.IsNullOrEmpty(op))
            {
                result += (sender as Button).Text;
                if (result[0] == '0') result = result[1..];
                resultDisplay.Text = result;
            } else
            {
                num += (sender as Button).Text;
                if (num[0] == '0') num = num[1..];
                resultDisplay.Text = num;
            }

        }

        private void operatorPressed(object sender, EventArgs e)
        {
            if (autoclear)
            {
                clearDisplay(sender, e);
                autoclear = false;
            }

            if (string.IsNullOrEmpty(op))
            {
                op = (sender as Button).Text;
                resultDisplay.Text = "0";
                return;
            } else
            { try
                {
                    result = op switch // Set result of operation to the first number
                    {
                        "+" => (double.Parse(num) + double.Parse(result)).ToString(),
                        "-" => (double.Parse(result) - double.Parse(num)).ToString(),
                        "*" => (double.Parse(num) * double.Parse(result)).ToString(),
                        "/" => (double.Parse(result) / double.Parse(num)).ToString(),
                        "^" => (Math.Pow(double.Parse(result), double.Parse(num))).ToString()
                    };
                    op = (sender as Button).Text;
                } catch (DivideByZeroException) {
                    clearDisplay(sender, e);
                    resultDisplay.Text = "Error: Division by zero";
                    autoclear = true;
                    }
            }
            // Update display and reset second number and operator
            resultDisplay.Text = result;
            num = "";
        }

        private void unaryOperation(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(op)) // Only one number entered
            {
                result = (sender as Button).Text switch
                {
                    "√" => (Math.Sqrt(double.Parse(result))).ToString(),
                    "ln" => (Math.Log(double.Parse(result))).ToString()
                };
                resultDisplay.Text = result;
            } else // Multiple numbers, take square root of the second/latest number
            {
                num = (sender as Button).Text switch
                {
                    "√" => (Math.Sqrt(double.Parse(num))).ToString(),
                    "ln" => (Math.Log(double.Parse(num))).ToString()
                };
                resultDisplay.Text = num;
            }

        }

        private void decimalPoint(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(op))
            {
                result += ".";
                resultDisplay.Text = result;
            } else
            {
                num += ".";
                resultDisplay.Text = num;
            }
        }

        private void equals(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(op)) return;
            operatorPressed(sender, e);
            autoclear = true;
        }
    }
}
