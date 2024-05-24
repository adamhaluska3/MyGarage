using MyGarage.Models;
using MyGarage.Resources.Languages;

namespace MyGarage.Views;

public partial class NoteDetail
{
    private Note _note;
    private string _vehicleName;

    public NoteDetail(string vehicleName, Note note)
    {
        _note = note;
        _vehicleName = vehicleName;

        InitializeComponent();

        TitleLabel.Text = $"{LangRes.Notefor} {_vehicleName}";

        if (note.Name == null)
            Title = LangRes.NewNote;
        else

        if (note.Id == -1)
            deleteEntry.IsEnabled = false;

        LoadPicker();
    }

    protected override void OnAppearing()
    {
        BindingContext = _note;
        base.OnAppearing();
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

    private string ChooseImageSource(int type)
    {
        string output = ((NoteType)type).ToString().ToLowerInvariant() + ".png";

        return output;
    }


    // Event handlers
    private async void UpdateEntry(object sender, EventArgs e)
    {
        if (!_note.HasRemind || _note.OdoRemind == 0)
        {
            _note.HasRemind = false;
            _note.OdoRemind = 0;
        }

        if (_note.Name == null || _note.Name == "")
        {
            await DisplayAlert(LangRes.InvalidData, LangRes.EmptyName, "OK");
            return;
        }

        _note.ImageSource = ChooseImageSource((int)_note.Type);

        var newNote = (await App.Database.UpdateNote(_note));

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = Utilities.MakeSnackbar(LangRes.EntryUpdated, Navigation);

        await snackbar.Show(cancellationTokenSource.Token);

        BindingContext = newNote;
        _note = newNote;
        Title = newNote.Name;

        deleteEntry.IsEnabled = true;
    }

    private async void RemoveEntry(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert(LangRes.Remove, LangRes.ReallyRemove, LangRes.Yes, LangRes.No);
        if (!answer)
            return;

        await App.Database.RemoveNote(_note.Id);

        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var snackbar = Utilities.MakeSnackbar(LangRes.EntryDeleted, Navigation);

        await snackbar.Show(cancellationTokenSource.Token);

        await Navigation.PopAsync();
    }

    private void HasRemind_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        NoteOdo.IsEnabled = HasRemind.IsChecked;
    }
}