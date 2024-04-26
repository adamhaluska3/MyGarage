namespace MauiApp1
{
    public partial class App : Application
    {

        public static Database Database {  get; private set; }

        public App(Database database)
        {
            InitializeComponent();

            MainPage = new AppShell();
            Database = database;
        }
    }
}
