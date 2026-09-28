using System.Threading.Tasks;

namespace CalculatorV1.Views;

public partial class RandomNum : ContentPage
{
	Random rand;
	int low = 2147483647;
	public RandomNum()
	{
		InitializeComponent();
		rand = new Random();
	}

	private void Generate(object sender, EventArgs e)
	{
		int num = rand.Next();
		Total.Text = (int.Parse(Total.Text) + 1).ToString();
		Current.Text = $"{num:N0}";
		if (num < low)
		{
			low = num;
			HighScore.Text = $"{low:N0}";
			double chance = (double)low / 2147483647;
			Probability.Text = (chance * 100).ToString("0.000000000000") + "%";
			HighScore.TextColor = (num) switch
			{
				< 10 => Colors.Red,
				< 100 => Colors.Orange,
				< 1000 => Colors.Gold,
				< 10000 => Colors.Coral,
				< 100000 => Colors.Purple,
				< 500000 => Colors.Pink,
				< 1000000 => Colors.LightBlue,
				< 5000000 => Colors.Blue,
				< 10000000 => Colors.Teal,
				< 100000000 => Colors.DarkGreen,
				< 1000000000 => Colors.LightGrey,
				_ => Colors.Black

			};
		}
	}

	private void ToggleAuto(object sender, EventArgs e)
	{
		if ((sender as Button).Text == "x100") ToggleHelper(100);
		else if ((sender as Button).Text == "x1000") ToggleHelper(1000);
		else ToggleHelper(10000);
	}
	private async Task ToggleHelper(int times)
	{
		for (int i = 0; i < times; i++)
		{
			Generate(null, null);
			if (times < 101) await Task.Delay(20);
			if (times < 1001) await Task.Delay(1);
		}
	}
}