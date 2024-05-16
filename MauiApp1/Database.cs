using SQLite;
using MyGarage.Models;

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
    }



    public async Task<int> AddNewVehicle(Vehicle vehicle)
    {
        await Init();
        return await connection.InsertAsync(vehicle);
    }

    public async Task<int> RemoveVehicle(int id)
    {
        await Init();
        await connection.Table<Note>().DeleteAsync(note => note.VehicleId == id);

        return await connection.DeleteAsync<Vehicle>(id);
    }

    public async Task<List<Vehicle>> GetAllVehicles()
    {
        await Init();
        return (await connection.Table<Vehicle>()
            .OrderBy(x => x.Name)
            .ToListAsync());
    }

    public async Task<Vehicle?> GetVehicle(int id)
    {
        await Init();
        return await connection.FindAsync<Vehicle>(id);
    }

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



    public async Task<int> AddNewNote(Note newNote)
    {
        await Init();
        return await connection.InsertAsync(newNote);
    }

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

    public async Task<int> RemoveNote(int id)
    {
        await Init();
        return await connection.DeleteAsync<Note>(id);
    }


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

    public async Task<List<Note>> GetNotes(int vehicleId, string? filter, int typeFilter)
    {
        await Init();

        var notes = (await connection.Table<Note>().ToListAsync())
            .Where(note => note.VehicleId == vehicleId)
            .OrderByDescending(x => x.HasRemind)
            .ThenBy(x => x.OdoRemind)
            .ThenBy(x => x.Type)
            .ThenBy(x => x.Name)
            .ToList();

        if (filter != "" && filter != null)
        {

            notes = notes
                .Where(note => (note.Description != null && note.Description.Contains(filter)) ||
                               (note.Name.Contains(filter)))
                .ToList();
        }

        NoteType type;
        if (Enum.IsDefined(typeof(NoteType), typeFilter))
        {
            notes = notes
                .Where(note => note.Type == typeFilter)
                .ToList();
        }

        return notes;

    }


}
