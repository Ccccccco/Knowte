using Digimezzo.Utilities.Log;
using Knowte.Data.Contracts;
using System;
using System.IO;
using System.Reflection;

namespace Knowte.Data
{
    public class DbMigrator
    {
        protected sealed class DatabaseVersionAttribute : Attribute
        {
            private int version;

            public DatabaseVersionAttribute(int version)
            {
                this.version = version;
            }

            public int Version
            {
                get { return this.version; }
            }
        }

        // NOTE: whenever there is a change in the database schema,
        // this version MUST be incremented and a migration method
        // MUST be supplied to match the new version number
        protected const int CURRENT_VERSION = 1;
        private ISQLiteConnectionFactory factory;
        private int userDatabaseVersion;

        public DbMigrator(ISQLiteConnectionFactory factory)
        {
            this.factory = factory;
        }

        private void CreateConfiguration()
        {
            using (var conn = this.factory.GetConnection())
            {
                conn.Execute("CREATE TABLE Configuration (" +
                             "Id                 INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                             "Key                TEXT NOT NULL," +
                             "Value              TEXT NOT NULL);");

                conn.Execute("INSERT INTO Configuration (Key, Value) VALUES ('DatabaseVersion', @versionParam)", CURRENT_VERSION);
            }
        }

        private void CreateTablesAndIndexes()
        {
            using (var conn = this.factory.GetConnection())
            {
                conn.Execute("CREATE TABLE Collection (" +
                             "Id                        TEXT," +
                             "Title     	            TEXT," +
                             "PRIMARY KEY(Id));");

                conn.Execute("CREATE TABLE Notebook (" +
                             "Id                        TEXT," +
                             "CollectionId              TEXT," +
                             "Title     	            TEXT," +
                             "CreationDate              INTEGER," +
                             "PRIMARY KEY(Id));");

                conn.Execute("CREATE TABLE Note (" +
                                     "Id                TEXT," +
                                     "NotebookId     	TEXT," +
                                     "Title             TEXT," +
                                     "Text              TEXT," +
                                     "CreationDate      INTEGER," +
                                     "OpenDate          INTEGER," +
                                     "ModificationDate  INTEGER," +
                                     "Width             INTEGER," +
                                     "Height            INTEGER," +
                                     "Top               INTEGER," +
                                     "Left              INTEGER, " +
                                     "Flagged           INTEGER," +
                                     "Maximized         INTEGER," +
                                     "PRIMARY KEY(Id));");
            }
        }

        private void Migrate1()
        {
            using (var conn = this.factory.GetConnection())
            {
                conn.Execute("BEGIN TRANSACTION;");

                conn.Execute("CREATE TABLE Collection (" +
                            "Id                        TEXT," +
                            "Title     	            TEXT," +
                            "PRIMARY KEY(Id));");

                conn.Execute("INSERT INTO Collection ('ad61ac96-9764-4067-a36c-102f5e160332','Default')");
                conn.Execute("UPDATE Notebook SET CollectionId='ad61ac96-9764-4067-a36c-102f5e160332'");

                conn.Execute("COMMIT;");
            }
        }

        public void Migrate()
        {
            try
            {
                if (!this.IsDatabaseValid())
                {
                    // Create the database if it doesn't exist
                    LogClient.Info("Creating a new database");
                    this.CreateDatabase();
                }
                else
                {
                    // Upgrade the database if it is not the latest version
                    if (this.IsMigrationNeeded())
                    {
                        LogClient.Info("Creating a backup of the database");
                        this.BackupDatabase();
                        LogClient.Info("Upgrading database");
                        this.MigrateDatabase();
                    }
                }
            }
            catch (Exception ex)
            {
                LogClient.Error("There was a problem initializing the database. Exception: {0}", ex.Message);
            }
        }

        private bool IsDatabaseValid()
        {
            int count = 0;

            using (var conn = this.factory.GetConnection())
            {
                count = conn.ExecuteScalar<int>("SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name='Configuration'");
            }

            return count > 0;
        }

        public bool IsMigrationNeeded()
        {
            if (!this.IsDatabaseValid())
            {
                return true;
            }

            using (var conn = this.factory.GetConnection())
            {
                this.userDatabaseVersion = Convert.ToInt32(conn.ExecuteScalar<string>("SELECT Value FROM Configuration WHERE Key = 'DatabaseVersion'"));
            }

            return this.userDatabaseVersion < CURRENT_VERSION;
        }

        private void CreateDatabase()
        {
            this.CreateConfiguration();
            this.CreateTablesAndIndexes();

            LogClient.Info("New database created at {0}", this.factory.DatabaseFile);
        }

        private void MigrateDatabase()
        {
            for (int i = this.userDatabaseVersion + 1; i <= CURRENT_VERSION; i++)
            {
                MethodInfo method = typeof(DbMigrator).GetTypeInfo().GetDeclaredMethod("Migrate" + i);
                if (method != null) method.Invoke(this, null);
            }

            using (var conn = this.factory.GetConnection())
            {
                conn.Execute("UPDATE Configuration SET Value = ? WHERE Key = 'DatabaseVersion'", CURRENT_VERSION);
            }

            LogClient.Info("Upgraded from database version {0} to {1}", this.userDatabaseVersion.ToString(), CURRENT_VERSION.ToString());
        }

        private void BackupDatabase()
        {
            try
            {
                string databaseFileCopy = this.factory.DatabaseFile + ".old";

                if (File.Exists(databaseFileCopy)) File.Delete(databaseFileCopy);
                File.Copy(this.factory.DatabaseFile, databaseFileCopy);
            }
            catch (Exception ex)
            {
                LogClient.Info("Could not create a copy of the database file. Exception: {0}", ex.Message);
            }
        }
    }
}
