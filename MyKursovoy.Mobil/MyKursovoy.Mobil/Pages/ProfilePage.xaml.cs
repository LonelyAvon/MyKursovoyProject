using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace MyKursovoy.Mobil.Pages;

public partial class ProfilePage : ContentPage
{
	public ProfilePage()
	{
		InitializeComponent();
	}

    async void ToHomePage_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    async void UpdateUserButton_Clicked(System.Object sender, System.EventArgs e)
    {
        if(!await Validate())
        {
            return;
        }

        IHelper.Client.Name = NameTextBox.Text;
        IHelper.Client.Surname = SurnameTextBox.Text;
        IHelper.Client.Patronymic = PatronymicTextBox.Text;
        IHelper.Client.Phone = PhoneTextBox.Text;

        using (var client = IHelper.GetHttpClient())
        {
            await client.PostAsJsonAsync("Client/UpdateUser", IHelper.Client);
            await DisplayAlert("Успешно", "Данные обновлены", "Ок");
        }
    }

    void ContentPage_Appearing(System.Object sender, System.EventArgs e)
    {
        BindingContext = IHelper.Client;
    }

    async Task<bool> Validate()
    {
        if(string.IsNullOrWhiteSpace(SurnameTextBox.Text) && SurnameTextBox.Text.Length < 3)
        {
            await DisplayAlert("Ошибка", "Поле Фамилия заполнено не верно","Ok");
            return false;
        }
        if (string.IsNullOrWhiteSpace(NameTextBox.Text) && NameTextBox.Text.Length < 2)
        {
            await DisplayAlert("Ошибка", "Поле Имя заполнено не верно", "Ok");
            return false;
        }
        if (string.IsNullOrWhiteSpace(PatronymicTextBox.Text) && PatronymicTextBox.Text.Length < 3)
        {
            await DisplayAlert("Ошибка", "Поле Отчество заполнено не верно", "Ok");
            return false;
        }

        string pattern = @"^[78]\d{10}$";

        if (!Regex.IsMatch(PhoneTextBox.Text, pattern))
        {
            await DisplayAlert("Ошибка", "Поле Номер заполнено не верно", "Ok");
            return false;
        }
        return true;
    }

    async void ToCartButton_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CartPage());
    }

    async void ToCatalogPage_Clicked(System.Object sender, System.EventArgs e)
    {
        await Navigation.PushAsync(new CatalogPage());
    }
}
