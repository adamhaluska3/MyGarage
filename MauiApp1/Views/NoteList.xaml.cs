using MyGarage.Models;
using MyGarage.Models.VisualModels;
using MyGarage.Resources.Languages;
using System.Globalization;

namespace MyGarage.Views;

public partial class NoteList : ContentPage
{
	public int vehicleId;
	public Vehicle currentVehicle;

	public NoteList(int vehicleId, Vehicle vehicle)
	{
		this.vehicleId = vehicleId;
		currentVehicle = vehicle;

		LoadNotes();

		BindingContext = this;
		InitializeComponent();

		SetTitle();
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
			LangRes.Any,
		};

		noteTypeFilter.ItemsSource = options;
    }

    private void SetTitle()
	{
		var currentLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

		if (currentLanguage == "sk")
			Title = $"Poznámky vozidla {currentVehicle.Name}";

		else
			Title = $"{currentVehicle.Name}'s notes";
    }

	private async Task LoadNotes()
	{
		var notes = await App.Database.GetNotes(vehicleId);
		SetNotesSource(notes);
	}

	private async void AddNote(object sender, EventArgs e)
    {
		var newNote = new Note(currentVehicle.Id);
        var noteDetailPage = new NoteDetail(currentVehicle.Name, newNote);

        await Navigation.PushAsync(noteDetailPage, true);

        await LoadNotes();
	}

	private async void FilterNotes(object sender, EventArgs e)
	{
		var notes = await App.Database.GetNotes(vehicleId, stringFilter.Text, noteTypeFilter.SelectedIndex);
		SetNotesSource(notes);
	}

	private async void NavigateNoteDetail(object sender, TappedEventArgs e)
	{
		var note = e.Parameter as Note;
		var noteDetailPage = new NoteDetail(currentVehicle.Name, note);

		await Navigation.PushAsync(noteDetailPage, true);
	}

	private void SetNotesSource(List<Note> notes)
	{
        var items = new List<NoteItem>();
        foreach (var note in notes)
        {
            items.Add(new NoteItem(note));
        }
        notesCollection.ItemsSource = items;
    }

	protected override async void OnAppearing()
	{
		await LoadNotes();
		base.OnAppearing();
	}
}