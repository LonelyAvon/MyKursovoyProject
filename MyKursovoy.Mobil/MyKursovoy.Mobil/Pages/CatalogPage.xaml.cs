using MyKursovoy.Domain.Models;
using Newtonsoft.Json;

namespace MyKursovoy.Mobil.Pages;

public partial class CatalogPage : ContentPage
{
	public CatalogPage()
	{
		InitializeComponent();
	}

    async void ContentPage_Loaded(System.Object sender, System.EventArgs e)
    {
		using (var client = IHelper.GetHttpClient())
		{
			var response = await client.GetStringAsync("Types/GetTypes");

			var types = JsonConvert.DeserializeObject<List<Typeoftechnic>>(response);

			CategoreisCollectionView.ItemsSource = types;
		}
    }

    async void ToHomeButton_Clicked(System.Object sender, System.EventArgs e)
    {
		await Navigation.PopToRootAsync();
    }

    async void ToProfileButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

     async void ToCartButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CartPage());
    }

    async void ToCatalogButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CatalogPage());
    }
}
