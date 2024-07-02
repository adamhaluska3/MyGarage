namespace MyGarage.Converters;

/// <summary>
/// True = AppTheme.Dark | False = AppTheme.Light
/// </summary>
public static class BoolAppTheme
{
    public static bool AppThemeToBool(AppTheme appTheme)
    {
        return (appTheme == AppTheme.Dark);
    }

    public static AppTheme BoolToAppTheme(bool boolean)
    {
        return (boolean) ? AppTheme.Dark : AppTheme.Light;
    }

}
