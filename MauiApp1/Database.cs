using MyGarage.Models;
using SQLite;

namespace MyGarage;

public class Database : IDisposable
{ 
    private SQLiteAsyncConnection connection;

    public Database()
    {
    }

    public void Dispose()
    {
        connection.CloseAsync().Wait();
    }

    private async Task Init()
    {
        if (connection != null)
            return;

        connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await connection.CreateTableAsync<Vehicle>();
        await connection.CreateTableAsync<Note>();
        await connection.CreateTableAsync<OdometerState>();
    }


    // Vehicle
    /// <summary>
    /// Adds given vehicle into database.
    /// </summary>
    /// <param name="vehicle">Vehicle to add</param>
    /// <returns>The number of rows added to the table.</returns>
    public async Task<int> AddNewVehicle(Vehicle vehicle)
    {
        await Init();
        return await connection.InsertAsync(vehicle);
    }

    /// <summary>
    /// Removes given vehicle from the database.
    /// </summary>
    /// <param name="id">ID of the vehicle to remove.</param>
    /// <returns>The number of objects deleted.</returns>
    public async Task<int> RemoveVehicle(int id)
    {
        await Init();
        await connection.Table<Note>().DeleteAsync(note => note.VehicleId == id);

        return await connection.DeleteAsync<Vehicle>(id);
    }

    /// <summary>
    /// Updates given vehicle in the database.
    /// </summary>
    /// <param name="updatedEntry">Vehicle to update (loadede with new data)</param>
    /// <returns>Vehicle with new data.</returns>
    public async Task<Vehicle?> UpdateVehicle(Vehicle updatedEntry)
    {
        await Init();
        try
        {
            if (updatedEntry.Id == -1)
                await AddNewVehicle(updatedEntry);

            else
                await connection.UpdateAsync(updatedEntry);
        }
        catch (Exception)
        {
            return null;
        }

        return await connection.FindAsync<Vehicle>(updatedEntry.Id);
    }

    /// <summary>
    /// Returns list of all vehicles in the database.
    /// </summary>
    /// <returns>List of vehicles.</returns>
    public async Task<List<Vehicle>> GetAllVehicles()
    {
        await Init();
        return (await connection.Table<Vehicle>()
            .OrderBy(x => x.Name)
            .ToListAsync());
    }

    /// <summary>
    /// Retrieves vehicle with given ID from the database.
    /// </summary>
    /// <param name="id">ID of vehicle to find.</param>
    /// <returns>Instance of found vehicle, null if not found.</returns>
    public async Task<Vehicle?> GetVehicle(int id)
    {
        await Init();
        return await connection.FindAsync<Vehicle>(id);
    }


    // Note
    /// <summary>
    /// Adds given note to the database.
    /// </summary>
    /// <param name="note">New note</param>
    /// <returns>The number of rows added to the table.</returns>
    public async Task<int> AddNewNote(Note note)
    {
        await Init();
        return await connection.InsertAsync(note);
    }

    /// <summary>
    /// Removes the note with given ID from the database.
    /// </summary>
    /// <param name="id">ID of the note to remove</param>
    /// <returns>The number of objects deleted.</returns>
    public async Task<int> RemoveNote(int id)
    {
        await Init();
        return await connection.DeleteAsync<Note>(id);
    }

    /// <summary>
    /// Updates given note in the database.
    /// </summary>
    /// <param name="updatedEntry">Note to update (loaded with new data)</param>
    /// <returns>Note with new data.</returns>
    public async Task<Note?> UpdateNote(Note updatedEntry)
    {
        await Init();
        try
        {
            if (updatedEntry.Id == -1)
                await AddNewNote(updatedEntry);

            else
                await connection.UpdateAsync(updatedEntry);
        }
        catch (Exception)
        {
            return null;
        }

        return await connection.FindAsync<Note>(updatedEntry.Id);
    }

    /// <summary>
    /// Returns list of all notes related to vehicle with vehicleId.
    /// </summary>
    /// <param name="vehicleId">ID of vehicle</param>
    /// <returns>List of notes.</returns>
    public async Task<List<Note>> GetNotes(int vehicleId)
    {
        await Init();

        return (await connection.Table<Note>()
            .Where(note => note.VehicleId == vehicleId)
            .OrderByDescending(x => x.HasRemind)
            .ThenBy(x => x.OdoRemind)
            .ThenBy(x => x.Type)
            .ThenBy(x => x.Name)
            .ToListAsync());
    }

    /// <summary>
    /// Retrieves list of all notes related to vehicle, filtered to contain given string in 
    /// their name or description/ filtered to just the given type.<para/>
    /// 
    /// To ignore string filter, set string to null.<para/>
    /// 
    /// To ignore type filter, set type to value other than valid value of NoteType (e.g. -1).
    /// </summary>
    /// <param name="vehicleId">ID of vehicle</param>
    /// <param name="filter">String to look for</param>
    /// <param name="typeFilter">Type of notes to consider</param>
    /// <returns>List of notes.</returns>
    public async Task<List<Note>> GetNotes(int vehicleId, string? filter, int typeFilter)
    {
        await Init();

        var notes = await GetNotes(vehicleId);

        if (filter != "" && filter != null)
        {
            notes = notes
                .Where(note => (note.Description != null && note.Description.Contains(filter)) ||
                               (note.Name.Contains(filter)))
                .ToList();
        }

        if (Enum.IsDefined(typeof(NoteType), typeFilter))
        {
            notes = notes
                .Where(note => (int)note.Type == typeFilter)
                .ToList();
        }

        return notes;
    }


    // OdometerState
    /// <summary>
    /// Adds given Odometer state to the database.
    /// </summary>
    /// <param name="state">OdometerState to add</param>
    /// <returns>The number of rows added to the table.</returns>
    public async Task<int> AddOdometerState(OdometerState state)
    {
        await Init();

        return await connection.InsertAsync(state);
    }

    /// <summary>
    /// Removes OdometerState with given ID from the database.
    /// </summary>
    /// <param name="id">ID of OdometerState</param>
    /// <returns>The number of objects deleted.</returns>
    public async Task<int> RemoveOdometerState(int id)
    {
        await Init();

        return await connection.DeleteAsync<OdometerState>(id);
    }

    /// <summary>
    /// Updates given state in the database.
    /// </summary>
    /// <param name="state">State to update (loaded with new data)</param>
    /// <returns>State with new data.</returns>
    public async Task<OdometerState> UpdateOdometerState(OdometerState state)
    {
        await Init();

        try
        {
            if (state.Id == -1)
                await AddOdometerState(state);
            else
                await connection.UpdateAsync(state);
        }
        catch (Exception)
        {
            return null;
        }

        return await connection.FindAsync<OdometerState>(state.Id);
    }

    /// <summary>
    /// Returns list of OdometerStates related to vehicle with vehicleId.
    /// </summary>
    /// <param name="vehicleId">ID of vehicle</param>
    /// <returns>List of OdometerStates.</returns>
    public async Task<List<OdometerState>> GetOdometerStates(int vehicleId)
    {
        await Init();

        var states = await connection.Table<OdometerState>().ToListAsync();

        return states
            .Where(e => e.VehicleId == vehicleId)
            .ToList();
    }
}
