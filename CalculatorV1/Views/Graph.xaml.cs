using CommunityToolkit.Maui.Views;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView.Maui;
using CommunityToolkit.Mvvm;
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

public partial class GraphVM : ObservableObject
{
	[ObservableProperty]
	public partial ObservableCollection<ObservablePoint> Values { get; set; }

	public GraphVM()
	{
		Values = new ObservableCollection<ObservablePoint>();
		Random rand = new();
		for (int i = 0; i < 10; i++)
		{
			Values.Add(new ObservablePoint(i, Math.Log(i + 1) + rand.Next(0, 2) / 100));
		}
	}
}