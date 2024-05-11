using System.Net.Http.Json;
using MyKursovoy.Domain.Combiners;

namespace MyKursovoy.Mobil.Pages;

public partial class ProductPage : ContentPage
{
	private ProductCombiner _product;

	public ProductPage(ProductCombiner product)
	{
		InitializeComponent();
		_product = product;
		BindingContext = _product;
	}

    async void Back_Clicked(System.Object sender, System.EventArgs e)
    {
		await Navigation.PopAsync();
    }

    async void AddToCart_Clicked(System.Object sender, System.EventArgs e)
    {
		using(var client = IHelper.GetHttpClient())
		{
			var productOrder = new CartCreateDTO
			{
				Guid = IHelper.guid,
				IdProduct = _product.Id
			};

			await client.PostAsJsonAsync("Cart/AddToCart", productOrder);

			await DisplayAlert("Успешно", "Товар добален", "Ok");
		}
    }
}
