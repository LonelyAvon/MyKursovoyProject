using System.Net.Http.Json;
using MyKursovoy.Domain.Combiners;
using MyKursovoy.Domain.Models;
using MyKursovoy.Mobil.Pages;
using Newtonsoft.Json;

namespace MyKursovoy.Mobil;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

    async void ContentPage_Loaded(System.Object sender, System.EventArgs e)
    {
		using (var client  = IHelper.GetHttpClient())
		{
			var response = await client.PostAsJsonAsync($"Client/Create/{IHelper.guid}",IHelper.guid);
			var answer = await response.Content.ReadAsStringAsync();
			var user = JsonConvert.DeserializeObject<Client>(answer);

			IHelper.Client = user;

			var responseString = await client.GetStringAsync("Types/GetTypes");
			var types = JsonConvert.DeserializeObject <List<Typeoftechnic>>(responseString);

			types.ForEach(x => x.PhotoPath = $"Images/CardPhotos/{x.PhotoPath}");
			TypesCarouselView.ItemsSource = types;

			var productsResponse = await client.GetStringAsync("Product/GetQuantity");

			var products = JsonConvert.DeserializeObject<List<Product>>(productsResponse);

			var productCombiner = products.Select(x =>
			{
				var product = new ProductCombiner
				{
					Id = x.Id,
					Name = x.Name,
					Description = x.Description,
					Brandname = x.Brandname,
					Photoname = x.Photoname,
					Typeoftechnic = x.Typeoftechnic,
					Cost = x.Cost,
					ShortDescription = x.ShortDescription,

				};
				if (Convert.ToBoolean(x.Existance))
				{
					product.Existance = "В наличии";
				}
				else
				{
					product.Existance = "Нет в наличии";
				}
				return product;
			});

			PopularItems.ItemsSource = productCombiner;
        }

    }

    async void TypesCarouselView_CurrentItemChanged(System.Object sender, Microsoft.Maui.Controls.CurrentItemChangedEventArgs e)
    {

    }

    async void ToCartButton_Clicked(System.Object sender, System.EventArgs e)
    {

		await Navigation.PushAsync(new CartPage());
    }

    async void ToCatalogButton_Clicked(System.Object sender, System.EventArgs e)
    {
		await Navigation.PushAsync(new CatalogPage());
    }

    async void ToProfileButton_Clicked(System.Object sender, System.EventArgs e)
    {
		await Navigation.PushAsync(new ProfilePage());
    }

    async void PopularItems_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
		var item = PopularItems.SelectedItem as ProductCombiner;

		await Navigation.PushAsync(new ProductPage(item));
    }
}


