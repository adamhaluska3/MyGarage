using MyGarage.Models;
using MyGarage.Resources.Languages;

namespace MyGarage.Views;

public partial class OdoStateDetail : ContentPage
{
    private OdometerState OdometerState { get; set; }

    private Vehicle relatedVehicle;
    private OdometerState relatedVehicleState;
    public OdoStateDetail(OdometerState odometerState)
    {
        OdometerState = odometerState;

        BindingContext = OdometerState;

        InitializeComponent();

        SetupDatePicker();

        if (odometerState.Id == -1)
            DeleteEntry.IsEnabled = false;
    }

    private async Task SetupDatePicker()
    {
        relatedVehicle = await App.Database.GetVehicle(OdometerState.VehicleId);
        OdoStateDatePicker.MinimumDate = new DateTime(relatedVehicle.Year, 1, 1);
        OdoStateDatePicker.MaximumDate = DateTime.Today;

    }


    public async void UpdateEntry(object sender, EventArgs e)
    {

        if (OdometerState.State == 0 || !await IsValidEntry())
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.InvalidOdoState, "OK");
            return;
        }

        var newEntry = await App.Database.UpdateOdometerState(OdometerState);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = Utilities.MakeSnackbar(LangRes.EntryUpdated, Navigation);

        await snackbar.Show(cancellationTokenSource.Token);

        OdometerState = newEntry;
        DeleteEntry.IsEnabled = true;
    }

    private async Task<bool> IsValidEntry()
    {
        var entries = (await App.Database.GetOdometerStates(OdometerState.VehicleId));

        var predecessor = entries
            .Where(entry => entry.DateTime < OdometerState.DateTime)
            .OrderByDescending(entry => entry.State)
            .FirstOrDefault(defaultValue:null);

        var successor = entries
            .Where(entry => entry.DateTime > OdometerState.DateTime)
            .OrderBy(entry => entry.State)
            .FirstOrDefault(defaultValue: null);

        if (predecessor != null && predecessor.State > OdometerState.State)
            return false;

        if (successor != null && OdometerState.State > successor.State)
            return false;

        return true;
    }

    public async void RemoveEntry(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert(LangRes.Remove, LangRes.ReallyRemove, LangRes.Yes, LangRes.No);
        if (!answer)
            return;

        await App.Database.RemoveOdometerState(OdometerState.Id);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = Utilities.MakeSnackbar(LangRes.EntryDeleted, Navigation);

        await snackbar.Show(cancellationTokenSource.Token);

        await Navigation.PopAsync();
    }

}