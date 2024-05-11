using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MauiApp1.Models;

namespace MauiApp1.Views;

public partial class NoteDetail
{
	private Note Note;
	private string VehicleName;

	public NoteDetail(string vehicleName, Note note)
	{
		Note = note;
		VehicleName = vehicleName;
		InitializeComponent();
        TitleLabel.Text = $"Note for {VehicleName}";

        if (note.Id == -1)
            deleteEntry.IsEnabled = false;

        if (note.Name == null)
        {
            Title = "New Note";
        }
    }

	private async void UpdateEntry(object sender, EventArgs e)
	{
        //Note.Type = (NoteType)NoteType.SelectedIndex;
        if (Note.Name == null || Note.Name == "")
        {
            await DisplayAlert("Invalid data", "Empty name.", "OK");
            return;
        }
        Note.ImageSource = ChooseImageSource(Note.Type);
        if (!Note.HasRemind)
            Note.OdoRemind = 0;

        var newNote = (await App.Database.UpdateNote(Note));
        if (newNote == null)
        {
            await DisplayAlert("Invalid data", "Reg. number and VIN need to be unique.", "OK");
            return;
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = MakeSnackbar("Entry updated.");

        await snackbar.Show(cancellationTokenSource.Token);

        BindingContext = newNote;
        Note = newNote;
        Title = newNote.Name;


        deleteEntry.IsEnabled = true;
    }
    private async void RemoveEntry(object sender, EventArgs e)
	{
        bool answer = await DisplayAlert("Remove", "Do you really want to remove this note?", "Yes", "No");
        if (!answer)
            return;

        await App.Database.RemoveNote(Note.Id);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = MakeSnackbar("Entry deleted.");

        await snackbar.Show(cancellationTokenSource.Token);

        await Navigation.PopAsync();
    }

    protected override async void OnAppearing()
    {
        await LoadBindings();
        base.OnAppearing();
    }

    private async Task LoadBindings()
    {
        BindingContext = Note;
    }

    private ISnackbar MakeSnackbar(string message)
    {
        var snackbarOptions = new SnackbarOptions
        {
            CornerRadius = new CornerRadius(10),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
        };

        string text = message;
        string actionButtonText = "OK";
        Action action = async () => await Navigation.PopAsync();
        TimeSpan duration = TimeSpan.FromSeconds(3);

        var snackbar = Snackbar.Make(text, action, actionButtonText, duration, snackbarOptions);

        return snackbar;
    }


    private string ChooseImageSource(int type)
    {
        string output = ((NoteType)type).ToString().ToLowerInvariant() + ".png";

        return output;
    }

    private void HasRemind_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        NoteOdo.IsEnabled = HasRemind.IsChecked;
    }
}