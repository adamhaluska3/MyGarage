namespace MyGarage
{
    public partial class App : Application
    {

        public static Database Database {  get; private set; }

        public App(Database database)
        {
            InitializeComponent();
            Database = database;

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
