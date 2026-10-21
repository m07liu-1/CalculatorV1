using CommunityToolkit.Maui.Views;
using System.Net.Http.Headers;
using System.Text.Json;
namespace CalculatorV1.Views;


public partial class AQPopup : Popup
{
	private HttpClient _client;
	public AQPopup(HttpClient httpClient)
	{
		InitializeComponent();
		_client = httpClient;
	}

	private async void GetData(object sender, EventArgs e)
	{
		Warning.Text = "";
		if (string.IsNullOrEmpty(StationId.Text) || string.IsNullOrEmpty(Username.Text)
			|| string.IsNullOrEmpty(Password.Text))
		{
			Warning.Text = "Missing Required Fields";
			return;
		}
		string username = Username.Text.Trim();
		string password = Password.Text.Trim();
		string credentials = $"{username}:{password}";
		try
		{
			string base64credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(credentials));
			_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", base64credentials);
			using HttpResponseMessage response = await _client.GetAsync(
				"GetLocationData?LocationIdentifier=" + StationId.Text.Trim());
			response.EnsureSuccessStatusCode();
			string dataString = await response.Content.ReadAsStringAsync();
			LocationData locationData = JsonSerializer.Deserialize<LocationData>(dataString);
			Result.Text = $"Location Name: {locationData.LocationName}\nLocation Id: {locationData.UniqueId}";
		}
		catch (HttpRequestException ex)
		{
			await Shell.Current.DisplayAlertAsync("Error", ex.Message, "Ok");
		}
	}

	private async void ClosePopup(object sender, EventArgs e)
	{
		await CloseAsync();
	}
}

public class LocationData
{
	public string LocationName { get; set; }

	public string UniqueId { get; set; }
};