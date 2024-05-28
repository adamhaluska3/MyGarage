using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace MyGarage;

public static class Utilities
{
    /// <summary>
    /// Creates a SnackBar with given message.
    /// </summary>
    /// <param name="message">Message in SnackBar</param>
    /// <param name="navigation">Navigation context</param>
    /// <returns>Instance of a SnackBar.</returns>
    public static ISnackbar MakeSnackbar(string message, INavigation navigation)
    {
        var snackbarOptions = new SnackbarOptions
        {
            CornerRadius = new CornerRadius(10),
            Font = Microsoft.Maui.Font.SystemFontOfSize(14),
            ActionButtonFont = Microsoft.Maui.Font.SystemFontOfSize(14),
        };

        var text = message;
        const string actionButtonText = "OK";
        async void Action() => await navigation.PopAsync();
        var duration = TimeSpan.FromSeconds(3);

        var snackbar = Snackbar.Make(text, Action, actionButtonText, duration, snackbarOptions);

        return snackbar;
    }
}
