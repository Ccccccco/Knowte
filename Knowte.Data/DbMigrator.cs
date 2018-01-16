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
        protected const int CURRENT_VERSION = 0;
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
                //conn.Execute("CREATE TABLE Bank (" +
                //             "Id                 INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                //             "Name	             TEXT NOT NULL," +
                //             "Phone	             TEXT," +
                //             "Comment	         TEXT," +
                //             "Website            TEXT);");

                //conn.Execute("CREATE TABLE AccountType (" +
                //             "Id                 INTEGER NOT NULL," +
                //             "Name      	     TEXT NOT NULL);");

                //conn.Execute("CREATE TABLE Account (" +
                //             "Id                 INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                //             "Name	             TEXT NOT NULL," +
                //             "AccountNumber      TEXT NULL," +
                //             "CurrencyNumber	 TEXT NOT NULL," +
                //             "StartDate   	     TEXT NOT NULL," +
                //             "StartBalance	     REAL NOT NULL," +
                //             "IsActive	         INTEGER NOT NULL," +
                //             "IsArchived         INTEGER NOT NULL," +
                //             "BankId	         INTEGER NOT NULL," +
                //             "AccountTypeId      INTEGER NOT NULL);");
            }
        }

        private void AddDefaultData()
        {
            using (var conn = this.factory.GetConnection())
            {
                //conn.Execute("BEGIN TRANSACTION;");
                //conn.Execute("INSERT INTO AccountType(Id, Name) VALUES(1,'Checking account');");
                //conn.Execute("INSERT INTO AccountType(Id, Name) VALUES(2,'Savings account');");
                //conn.Execute("COMMIT;");
            }
        }

        //private void Migrate1()
        //{
        //    using (var conn = this.factory.GetConnection())
        //    {
        //        conn.Execute("ALTER TABLE Bank ADD TestColumn INTEGER;");
        //    }
        //}

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
            this.AddDefaultData();

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
