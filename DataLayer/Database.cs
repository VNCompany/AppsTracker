using Npgsql;
using System.IO;
using DataLayer.Repositories;

namespace DataLayer
{
    public partial class DatabaseService : IDisposable
    {
        static bool isInitialized = false;

        NpgsqlConnection connection;
        public DatabaseService(string host, string user, string password, string dbname, string queryFile = "./query.sql")
        {
            connection = new NpgsqlConnection($"Host={host};Username={user};Password={password};Database={dbname}");
            connection.Open();
            if (!isInitialized)
            {
                isInitialized = true;
                using (NpgsqlCommand cmd = new NpgsqlCommand() { Connection = connection })
                {
                    cmd.CommandText = File.ReadAllText(queryFile);
                    cmd.ExecuteNonQuery();
                }
            }
            Console.WriteLine("DatabaseService is created");
        }

        public int Execute(string query)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public NpgsqlCommand ExecuteCommand(string query)
        {
            return new NpgsqlCommand(query, connection);
        }

        public object? ExecuteScalar(string query)
        {
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
            {
                return cmd.ExecuteScalar();
            }
        }

        UsersRepository? users;
        public UsersRepository Users
        {
            get
            {
                if (users == null)
                    users = new UsersRepository(this);
                return users;
            }
        }

        AppsRepository? apps;
        public AppsRepository Apps
        {
            get
            {
                if (apps == null)
                    apps = new AppsRepository(this);
                return apps;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("DatabaseService is disposed");
            connection.Dispose();
        }
    }
}