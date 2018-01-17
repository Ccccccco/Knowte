using Digimezzo.Utilities.Settings;
using Knowte.Core.Base;
using Knowte.Core.IO;
using Knowte.Data.Contracts;
using SQLite;
using System.IO;

namespace Knowte.Data
{
    public class SQLiteConnectionFactory : ISQLiteConnectionFactory
    {
        private string customStorageLocation;

        public string DatabaseFile
        {
            get
            {
                string storageLocation = string.IsNullOrEmpty(this.customStorageLocation) ? ApplicationPaths.CurrentNoteStorageLocation : this.customStorageLocation;
                return System.IO.Path.Combine(storageLocation, "Notes.db"); ;
            }
        }

        public SQLiteConnectionFactory()
        {
        }

        public SQLiteConnectionFactory(string customStorageLocation)
        {
            this.customStorageLocation = customStorageLocation;
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(this.DatabaseFile) { BusyTimeout = new System.TimeSpan(0, 0, 1) };
        }
    }
}
