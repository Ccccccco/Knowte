using SQLite;

namespace Knowte.Data
{
    public interface ISQLiteConnectionFactory
    {
        string DatabaseFile { get; }

        void SetCustomStorageLocation(string customStorageLocation);

        SQLiteConnection GetConnection();
    }
}