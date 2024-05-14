using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MyGarage.Models;
using MyGarage.Resources.Languages;

namespace MyGarage.Views;

public partial class NoteDetail
{
	private Note Note;
	private string VehicleName;

	public NoteDetail(string vehicleName, Note note)
	{
		Note = note;
		VehicleName = vehicleName;
		InitializeComponent();
        TitleLabel.Text = $"{LangRes.Notefor} {VehicleName}";

        if (note.Id == -1)
            deleteEntry.IsEnabled = false;

        if (note.Name == null)
        {
            Title = LangRes.NewNote;
        }

        LoadPicker();

    }

    private void LoadPicker()
    {
        var options = new List<string>()
        {
            LangRes.Engine,
            LangRes.Chassis,
            LangRes.Interior,
            LangRes.Body,
            LangRes.Other,
        };

        NoteType.ItemsSource = options;
    }

    private async void UpdateEntry(object sender, EventArgs e)
	{
        if (NoteOdo.Text == null || NoteOdo.Text == "" 
            || !Note.HasRemind)
        {
            Note.HasRemind = false;
            Note.OdoRemind = 0;
        }

        if (Note.Name == null || Note.Name == "")
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.EmptyName, "OK");
            return;
        }

        Note.ImageSource = ChooseImageSource(Note.Type);

        var newNote = (await App.Database.UpdateNote(Note));
        if (newNote == null)
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.UniqRegVIN, "OK");
            return;
        }

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = MakeSnackbar(LangRes.EntryUpdated);

        await snackbar.Show(cancellationTokenSource.Token);

        BindingContext = newNote;
        Note = newNote;
        Title = newNote.Name;


        deleteEntry.IsEnabled = true;
    }
    private async void RemoveEntry(object sender, EventArgs e)
	{
        bool answer = await DisplayAlert(LangRes.Remove, LangRes.ReallyRemove, LangRes.Yes, LangRes.No);
        if (!answer)
            return;

        await App.Database.RemoveNote(Note.Id);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = MakeSnackbar(LangRes.EntryDeleted);

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