using MyGarage.Models.VisualModels;
using MyGarage.Views;

namespace MyGarage
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(VehicleDetail), typeof(VehicleDetail));
            Routing.RegisterRoute(nameof(NoteList), typeof(NoteList));
        }
    }
}
