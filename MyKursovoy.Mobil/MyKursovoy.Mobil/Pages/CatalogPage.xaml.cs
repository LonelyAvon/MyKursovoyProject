using System.Collections.ObjectModel;
using MyKursovoy.Domain.Combiners;
using MyKursovoy.Domain.Models;
using Newtonsoft.Json;

namespace MyKursovoy.Mobil.Pages;

public partial class CatalogPage : ContentPage
{
    private List<ProductCombiner> combiners = new List<ProductCombiner>();

    private string _filter;
	public CatalogPage(string filter = "")
	{
		InitializeComponent();

        _filter = filter;
	}

    async void ContentPage_Loaded(System.Object sender, System.EventArgs e)
    {
		using (var client = IHelper.GetHttpClient())
		{
			var response = await client.GetStringAsync("Types/GetTypes");

			var types = JsonConvert.DeserializeObject<List<Typeoftechnic>>(response);

			CategoreisCollectionView.ItemsSource = types;

            var productsResponse = await client.GetStringAsync("Product/GetAll");

            var products = JsonConvert.DeserializeObject<List<ProductCombiner>>(productsResponse);
            combiners = products;

            SearchTextBox.Text = _filter;
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
    }

    async void CategoreisCollectionView_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        var item = CategoreisCollectionView.SelectedItem as Typeoftechnic;
        SearchTextBox.Text = item.Name;
    }

    async Task<List<ProductCombiner>> Find(List<ProductCombiner> products, string filter)
    {

        var task = Task.Run(() =>
        {
            var list = products.Where(x => x != null && x.Name !=null &&x.Type!=null && $"{x.Type.ToLower()}{x.Name.ToLower()}".
            Contains(filter.ToLower())).ToList();
            // Ожидание завершения поиска элемента в списке
            return list;
        });
        return await task;
    }   
    async void SearchTextBox_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
    {
        if(SearchTextBox.Text == string.Empty)
        {
            CategoreisCollectionView.IsVisible = true;
            CategoriesLabel.IsVisible = true;
            PopularItems.IsVisible = false;
        }
        else
        {
            var products = await Find(combiners, SearchTextBox.Text);

            PopularItems.ItemsSource = products;

            CategoreisCollectionView.IsVisible = false;
            CategoriesLabel.IsVisible = false;
            PopularItems.IsVisible = true;
        }
    }

    async void PopularItems_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        var item = PopularItems.SelectedItem as ProductCombiner;
        await Navigation.PushAsync(new ProductPage(item));
    }
}
