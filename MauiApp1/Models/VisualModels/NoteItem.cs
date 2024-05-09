using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiApp1.Models.VisualModels;

public class NoteItem
{
    public Note Note { get; set; }

    public NoteItem(Note note)
    {
        Note = note;
    }
}
