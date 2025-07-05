using MauiTodo.Models;
using SQLite;

namespace MauiTodo.Data;

public class Database
{
    private readonly SQLiteAsyncConnection connection;

    public Database()
    {
        string? dataDir = FileSystem.AppDataDirectory;
        string databasePath = Path.Combine(dataDir, "MauiTodo.db");

        string? dbEncryptionKey = SecureStorage.GetAsync("dbKey").Result;

        if (!string.IsNullOrEmpty(dbEncryptionKey))
        {
            Guid g = new();
            dbEncryptionKey = g.ToString();
            SecureStorage.SetAsync("dbKey", dbEncryptionKey);
        }

        SQLiteConnectionString dbOptions = new(databasePath, true, key: dbEncryptionKey);

        connection = new SQLiteAsyncConnection(dbOptions);

        _ = Initialize();
    }

    private async Task Initialize() => await connection.CreateTableAsync<TodoItem>();

    public async Task<List<TodoItem>> GetTodos() => await connection.Table<TodoItem>().ToListAsync();

    public async Task<TodoItem> GetTodo(int id)
    {
        AsyncTableQuery<TodoItem> query = connection.Table<TodoItem>().Where(x => x.Id == id);
        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> AddTodo(TodoItem item) => await connection.InsertAsync(item);

    public async Task<int> DeleteTodo(TodoItem item) => await connection.DeleteAsync(item);

    public async Task<int> UpdateTodo(TodoItem item) => await connection.UpdateAsync(item);
}
