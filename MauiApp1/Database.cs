using SQLite;
using MauiApp1.Models;

namespace MauiApp1;

public class Database
{

    private SQLiteAsyncConnection connection;

    public Database()
    {
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

    public async Task<List<Vehicle>> GetAllVehicles()
    {
        await Init();
        return (await connection.Table<Vehicle>().ToListAsync());
    } 

    public async Task<Vehicle?> GetVehicle(int id)
    {
        await Init();
        return (await connection.Table<Vehicle>().ToListAsync())
            .Where(candidate => candidate.Id == id)
            .FirstOrDefault();
    }





}
