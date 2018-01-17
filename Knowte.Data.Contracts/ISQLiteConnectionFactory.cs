using SQLite;

namespace Knowte.Data.Contracts
{
    public interface ISQLiteConnectionFactory
    {
        string DatabaseFile { get; }

        void SetCustomStorageLocation(string customStorageLocation);

        SQLiteConnection GetConnection();
    }
}