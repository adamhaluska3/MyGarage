using CommunityToolkit.Mvvm.ComponentModel;

namespace MyGarage.Models.VisualModels;

public class NoteItem
{
    public Note Note { get; set; }

    public NoteItem(Note note)
    {
        Note = note;
    }
}
