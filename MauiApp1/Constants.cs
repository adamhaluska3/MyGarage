using SQLite;

namespace MyGarage;

public static class Constants
{
    private const string DatabaseFilename = "MyGarageDatabase.db3";
    public const SQLiteOpenFlags Flags =
        // open the database in read/write mode
        SQLiteOpenFlags.ReadWrite |
        // create the database if it doesn't exist
        SQLiteOpenFlags.Create |
        // enable multi-threaded database access
        SQLiteOpenFlags.SharedCache;

    public static string DatabasePath =>
        Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

    // Input lengths (Changes influence database)
    public const int VehicleNameLength = 100;
    public const int VehicleMakeModelLength = 50;
    public const int VehicleRegNumLength = 20;
    public const int VehicleVinLength = 50;

    public const int NoteNameLength = 100;
    public const int NoteDescriptionLength = 1000;

    // Other
    public const int LowestYearAllowed = 1900;
}
