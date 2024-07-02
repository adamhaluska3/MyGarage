using MyGarage.Converters;

namespace MyGarage.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();

        ThemeSwitch.IsToggled = BoolAppTheme.AppThemeToBool((AppTheme) Preferences.Get("AppTheme", (int) AppTheme.Light));
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        AppTheme NewTheme = BoolAppTheme.BoolToAppTheme(ThemeSwitch.IsToggled);
        Utilities.SetAppTheme(NewTheme);
    }
}