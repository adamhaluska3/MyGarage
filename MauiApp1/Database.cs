#nullable enable
using MyGarage.Models;
using SQLite;

namespace MyGarage;

public class Database : IDisposable
{ 
    private SQLiteAsyncConnection _connection;

    public void Dispose()
    {
        _connection.CloseAsync().Wait();
    }

    private async Task Init()
    {
        if (_connection != null)
            return;

        _connection = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        await _connection.CreateTableAsync<Vehicle>();
        await _connection.CreateTableAsync<Note>();
        await _connection.CreateTableAsync<OdometerState>();
    }
    

    /// <summary>
    /// Removes given vehicle from the database.
    /// </summary>
    /// <param name="id">ID of the vehicle to remove.</param>
    /// <returns>The number of objects deleted.</returns>
    public async Task<int> RemoveVehicle(int id)
    {
        await Init();
        await _connection.Table<Note>().DeleteAsync(note => note.VehicleId == id);

        return await _connection.DeleteAsync<Vehicle>(id);
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
                await _connection.UpdateAsync(updatedEntry);
        }
        catch (Exception)
        {
            return null;
        }

        return await _connection.FindAsync<Vehicle>(updatedEntry.Id);
    }

    /// <summary>
    /// Returns list of all vehicles in the database.
    /// </summary>
    /// <returns>List of vehicles.</returns>
    public async Task<List<Vehicle>> GetAllVehicles()
    {
        await Init();
        return (await _connection.Table<Vehicle>()
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
        return await _connection.FindAsync<Vehicle>(id);
    }
    

    /// <summary>
    /// Removes the note with given ID from the database.
    /// </summary>
    /// <param name="id">ID of the note to remove</param>
    /// <returns>The number of objects deleted.</returns>
    public async Task<int> RemoveNote(int id)
    {
        await Init();
        return await _connection.DeleteAsync<Note>(id);
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
                await _connection.UpdateAsync(updatedEntry);
        }
        catch (Exception)
        {
            return null;
        }

        return await _connection.FindAsync<Note>(updatedEntry.Id);
    }

    /// <summary>
    /// Returns list of all notes related to vehicle with vehicleId.
    /// </summary>
    /// <param name="vehicleId">ID of vehicle</param>
    /// <returns>List of notes.</returns>
    public async Task<List<Note>> GetNotes(int vehicleId)
    {
        await Init();

        return (await _connection.Table<Note>()
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

        if (!string.IsNullOrEmpty(filter))
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

    
    /// <summary>
    /// Removes OdometerState with given ID from the database.
    /// </summary>
    /// <param name="id">ID of OdometerState</param>
    /// <returns>The number of objects deleted.</returns>
    public async Task<int> RemoveOdometerState(int id)
    {
        await Init();

        return await _connection.DeleteAsync<OdometerState>(id);
    }

    /// <summary>
    /// Updates given state in the database.
    /// </summary>
    /// <param name="state">State to update (loaded with new data)</param>
    /// <returns>State with new data.</returns>
    public async Task<OdometerState?> UpdateOdometerState(OdometerState state)
    {
        await Init();

        try
        {
            if (state.Id == -1)
                await AddOdometerState(state);
            else
                await _connection.UpdateAsync(state);
        }
        catch (Exception)
        {
            return null;
        }

        return await _connection.FindAsync<OdometerState>(state.Id);
    }

    /// <summary>
    /// Returns list of OdometerStates related to vehicle with vehicleId.
    /// </summary>
    /// <param name="vehicleId">ID of vehicle</param>
    /// <returns>List of OdometerStates.</returns>
    public async Task<List<OdometerState>> GetOdometerStates(int vehicleId)
    {
        await Init();

        var states = await _connection.Table<OdometerState>().ToListAsync();

        return states
            .Where(e => e.VehicleId == vehicleId)
            .ToList();
    }
    
    
    private async Task<int> AddNewVehicle(Vehicle vehicle)
    {
        await Init();
        return await _connection.InsertAsync(vehicle);
    }
    
    private async Task<int> AddNewNote(Note note)
    {
        await Init();
        return await _connection.InsertAsync(note);
    }
    
    private async Task<int> AddOdometerState(OdometerState state)
    {
        await Init();

        return await _connection.InsertAsync(state);
    }
}
