namespace MyGarage;

public partial class App : Application
{

    public static Database Database { get; private set; }

    public App()
    {
        InitializeComponent();
        Database = new Database();

        if (UserAppTheme == AppTheme.Unspecified)
            Utilities.SetAppTheme(AppTheme.Light);

        //MainPage = new NavigationPage(new MainPage());
        MainPage = new AppShell();
    }
}
