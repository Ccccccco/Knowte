using Digimezzo.Utilities.Settings;
using Knowte.Core.Base;
using Knowte.Data.Contracts;
using SQLite;
using System.IO;

namespace Knowte.Data
{
    public class SQLiteConnectionFactory : ISQLiteConnectionFactory
    {
        public string DatabaseFile => Path.Combine(SettingsClient.ApplicationFolder(), ProductInformation.ApplicationName + ".db");

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(this.DatabaseFile);
        }
    }
}
