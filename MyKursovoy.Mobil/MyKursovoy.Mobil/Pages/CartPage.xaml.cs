using MyKursovoy.Domain.Combiners;
using Newtonsoft.Json;

namespace MyKursovoy.Mobil.Pages;

public partial class CartPage : ContentPage
{
	public CartPage()
	{
		InitializeComponent();
	}

    async void ContentPage_Appearing(System.Object sender, System.EventArgs e)
    {
		using (var client = IHelper.GetHttpClient())
		{
			var response = await client.GetStringAsync($"Product/GetCart/{IHelper.guid}");

			var product = JsonConvert.DeserializeObject<ProductParser>(response);

			if(product !=null && product.Products.Count() != 0)
			{
				Prop.IsVisible = false;

                PopStack.IsVisible = true;

                DeleteCart.IsVisible = true;

				double resultMoney = 0;

				var products = product.Products.ToList();

				products.ForEach(x => resultMoney += Convert.ToDouble(x.Cost));

				ResultMoney.Text = resultMoney.ToString("0.00");

                PopularItems.ItemsSource = products;
			}
		}
    }

    async void ToHomePage_Clicked(System.Object sender, System.EventArgs e)
    {
		await Navigation.PopToRootAsync();
    }

    async void ToProfilePage_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new ProfilePage());
    }

     async void ToCartButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CartPage());
    }

    async void ToCatalogPage_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CatalogPage());
    }

    async void DeleteCart_Clicked(System.Object sender, System.EventArgs e)
    {
        using (var client = IHelper.GetHttpClient())
        {
            var response = await client.DeleteAsync($"Cart/DeleteAllInCart?ClientId={IHelper.guid}");

            var answer = await response.Content.ReadAsStringAsync();

            int exec = Convert.ToInt32(answer);

            if(exec != 0)
            {
                await DisplayAlert("Успешно", "Корзина очищена", "Ok");
            }
            else
            {
                await DisplayAlert("Ошибка", "Серверная ошибка", "Ok");
            }

            PopStack.IsVisible = false;

            Prop.IsVisible = true;
            DeleteCart.IsVisible = false;
        }
    }

    async void PopularItems_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        var item = PopularItems.SelectedItem as ProductCombiner;

        await Navigation.PushAsync(new ProductPage(item));
    }
}
