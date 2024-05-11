namespace MyKursovoy.Mobil;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		if (!Preferences.ContainsKey("guid"))
		{
			var guid = Guid.NewGuid().ToString();
			IHelper.guid = guid;
            Preferences.Set("guid", guid);
		}
		else
		{
			IHelper.guid = Preferences.Get("guid", "error");
		}
		MainPage = new AppShell();
	}
}

