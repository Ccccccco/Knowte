using SQLite;

namespace Knowte.Data.Contracts
{
    public interface ISQLiteConnectionFactory
    {
        string DatabaseFile { get; }

        SQLiteConnection GetConnection();
    }
}