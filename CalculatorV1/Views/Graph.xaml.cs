using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
namespace CalculatorV1.Views;

public partial class Graph : Popup
{
	public Graph(GraphVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}

public class Point
{
	public double X { get; set; }

	public string Date { get; set; }
	public double Y { get; set; }

	public Point(double x, double y, DateTime? date = null)
    {
        X = x;
        Y = y;
		Date = date?.ToString("d") ?? string.Empty;
    }


};

public partial class GraphVM
{
	public ObservableCollection<Point> Values { get; set; }

	public ObservableCollection<Point> Scatter { get; set; }

	public ObservableCollection<Point> Step { get; set; }

	public ObservableCollection<Point> Step2 { get; set; }

	public ObservableCollection<Point> InversedArea1 { get; set; } = new ObservableCollection<Point>
	{
		new(1, 0), new(2, 0.6), new(3, 0.7), new(4, 0.68), new(5, 0.56), new(6, 0.8), new(7, 0.5), new(8, 0)
	};

	public ObservableCollection<Point> InversedArea2 { get; set; } = new ObservableCollection<Point>
	{
		new(1, 0), new(1.5, 0.3), new(2.5, 0.2), new(3.5, 0.13), new(4.5, 0.3), new(5.5, 0.23), new(6.5, 0.15), new(7.5, 0)
	};

	
    public GraphVM()
	{
		Values = new();
		Scatter = new();
		Step = new();
		Step2 = new();
		Random rand = new();
		for (int i = 0; i < 10; i++)
		{
			int month = rand.Next(1, 13);
			int date = rand.Next(1, 28);
			Values.Add(new Point(i, rand.Next(0, 2) / 10 + Math.Log(i + 1), new DateTime(2026, month, date)));
			Scatter.Add(new Point(i+0.5, rand.Next(0, 2) / 10 + Math.Log(i + 1.5), new DateTime(2026, month, date)));
			Step.Add(new Point(i, rand.Next(0, 7)));
			Step2.Add(new Point(i, rand.Next(4, 9)));
		}
	}
}