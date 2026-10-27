using CommunityToolkit.Mvvm.ComponentModel;
using iText.Html2pdf;
using System.Text;
using CommunityToolkit.Maui.Storage;
namespace CalculatorV1.Views;

public partial class GeneratePage : ContentPage
{

	public GeneratePage(GeneratePageVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}

	public async void GeneratePDF(object sender, EventArgs e)
	{
        GeneratePageData dt = ((GeneratePageVM)BindingContext).Data;
        using var stream = await FileSystem.OpenAppPackageFileAsync("ehsn.png");
        using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);
        var base64String = Convert.ToBase64String(memoryStream.ToArray());
        string HTMLContent = $@"
<html>
    <head>
        <style>
            table, th, td {{
                text-align: center;
                border: 2px solid black;
                border-collapse: collapse;
                empty-cells: show;
                }}

                .StationInfo {{
                    display: flex;
                    justify-content: space-between;
                    align-items: end;
                }}
        </style>
    </head>
    <body style = ""margin:50px 50px"";>
        <h3 style = ""text-align: center;"" >
            <img style = ""float:left; height:70px"" src = ""data:image/png;base64,{base64String}"">
            Hydrometric Survey Note
        </h3>
        <br><br>
        <div class=""StationInfo"">
            <h4>Station Number: <u>{dt.StationNumber}</u> <br><br> Station Name:<u></u></h4>
            <h4 style = ""float:right"">Survey Date:<u>{dt.SurveyDate.ToString("d")}</u></h4>
        </div>
        <table style = ""width:100%; font-size:small"">
            <tr>
                <th> Start Time<br>(hh:mm)</th>
                <th>End Time<br>(hh:mm)</th>
                <th>Air Temp(°C)</th>
                <th>Water Temp</th>
                <th>Width (m)</th>
                <th>Area (m²)</th>
                <th>Mean Velocity<br>(m/s)</th>
                <th>Corrected MGH<br>(m)</th>
                <th>Discharge<br>(m³/s)</th>
                <th>Uncertainty(%)</th>
            </tr>
            <tr>
                <td>&nbsp</td>
                <td></td> 
                <td>{dt.AirTemp}</td>
                <td>{dt.WaterTemp}</td>
                <td>{dt.Width}</td>
                <td>{dt.Area}</td>
                <td></td>
                <td></td>
                <td>{dt.Discharge}</td>
                <td></td>
            </tr>
            <tr>
                <th>Mmt Mean Time<br>(hh:mm)</th>
                <td></td>
                <th colspan = 2 > Calc Shift Base Curve(m)</th>
                <td></td>
                <th>Difference Base<br>Curve (%)</th>
                <td></td>
                <th>Curve #</th>
                <td colspan = ""2"" ></td>
            </tr>
            <tr>
                <th>Control <br> Condition</th>
                <td></td>
                <th colspan=""2"">Control Condition Remarks</th>
                <td colspan = ""6""></td>
            </tr>
            <tr>
                <th colspan=""2"">Discharge Activity Remarks</th>
                <td colspan = ""8"">{dt.Comments}</td>
            </tr>
        </table>
    </body>
</html>";
        try
        {
            byte[] pdfBytes;
            using (var ms = new MemoryStream())
            {
                using (var SourceStream = new MemoryStream(Encoding.UTF8.GetBytes(HTMLContent)))
                {
                    HtmlConverter.ConvertToPdf(SourceStream, ms);
                    pdfBytes = ms.ToArray();
                }
                
            }
            using var saveStream = new MemoryStream(pdfBytes);
            await FileSaver.Default.SaveAsync("Sample.pdf", saveStream, CancellationToken.None);
            await Shell.Current.DisplayAlertAsync("Success", "PDF Saved Successfully", "Ok");
        } catch (Exception ex)
        {
            await Shell.Current.DisplayAlertAsync("Error", ex.Message, "Ok");
        }
    }
}

public class GeneratePageVM : ObservableObject
{
    public GeneratePageData Data { get; } = new();
}

public class GeneratePageData
{
    public string StationNumber { get; set; } = "";
    public DateTime SurveyDate { get; set; } = DateTime.Today;
    public string Name { get; set; } = "";
    public string AirTemp { get; set; } = "";
    public string WaterTemp { get; set; } = "";
    public string Width { get; set; } = "";
    public string Area { get; set; } = "";
    public string Discharge { get; set; } = "";
    public string Comments { get; set; } = "";

}
