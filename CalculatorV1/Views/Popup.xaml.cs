using CommunityToolkit.Maui.Views;
namespace CalculatorV1.Views;

public partial class DataPopup : Popup
{
	public DataPopup()
	{
		InitializeComponent();
	}

	public void FindDet(object sender, EventArgs e)
	{
		if (A00.Text == null) A00.Text = "0";
		if (A01.Text == null) A01.Text = "0";
		if (A10.Text == null) A10.Text = "0";
		if (A11.Text == null) A11.Text = "0";
        double result = double.Parse(A00.Text) * double.Parse(A11.Text) - double.Parse(A01.Text) * double.Parse(A10.Text);
        Result.Text = "det(A) = " + result.ToString();
	}

	public void ChangeColor(object sender, TextChangedEventArgs e)
	{
		if (((Entry)sender).Text == "") {
			((Entry)sender).BackgroundColor = Colors.Red;
		} else
		{
			((Entry)sender).TextColor = Colors.Blue;
			((Entry)sender).BackgroundColor = Colors.White;
		}
	}
}